using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Data;
using Microsoft.VisualBasic.FileIO;
using System.Data.SqlClient;
using System.Collections;
using System.Data.OleDb;

namespace TestWindowsService
{
    public class FileInputMonitor
    {
        private FileSystemWatcher fileSystemWatcher;
        //private string folderToWatchFor  = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "watchFolder";

        //this location comes from internal application folder
        //string folderToWatchFor = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + @"\watchFolder";
        //string folderToMoveFor = System.IO.Directory.GetParent(System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString()).ToString() + @"\moveFolder";

        //this location comes from App.config
        string folderToWatchFor = ConfigurationManager.AppSettings["watchFolderPath"];
        string folderToMoveFor = ConfigurationManager.AppSettings["moveFolderPath"];        


        public FileInputMonitor()
        {
            DirectoryInfo dir = new DirectoryInfo(folderToWatchFor);
            if (dir.Exists)
            {

                fileSystemWatcher = new FileSystemWatcher(folderToWatchFor);

                // Instruct the file system watcher to call the FileCreated method
                // when there are files created at the folder.
                fileSystemWatcher.Created += new FileSystemEventHandler(FileCreated);

                fileSystemWatcher.EnableRaisingEvents = true;
            }
            else
            {
                Library.WriteErrorLog("watch folder directory doesn't exists");
                Library.LogEvent("watch folder directory doesn't exists at " + DateTime.Now.ToString());
            }

        } // end FileInputMonitor()

        private void FileCreated(Object sender, FileSystemEventArgs e)
        {

            //if (e.Name == "report-for-" + System.DateTime.Now.ToString("yyyy-MM-dd"))
            //{
            //    ProcessFile(e.FullPath);
            //} // end if

            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                ProcessFile(e.FullPath, e.Name);
            }

        } // end public void FileCreated(Object sender, FileSystemEventArgs e)

        private void ProcessFile(String filePath, String fileName)
        {

            while (true)
            {
                try
                {
                    string extension = Path.GetExtension(fileName);
                    switch (extension)
                    {
                        case ".csv":
                            ReadCSVAndWriteOnDB(filePath);
                            break;
                        case ".xls": //Excel 97-03.
                            ReadXLAndWriteOnDB(filePath, extension, "Yes");
                            break;
                        case ".xlsx": //Excel 07 or higher.
                            ReadXLAndWriteOnDB(filePath, extension, "Yes");
                            break;

                    }

                    DirectoryInfo dir = new DirectoryInfo(folderToMoveFor);
                    if (dir.Exists)
                    {
                        string destPath = Path.Combine(folderToMoveFor, fileName);
                        if (File.Exists(destPath))
                        {
                            File.Delete(destPath);
                            File.Move(filePath, destPath);
                        }
                        else
                        {
                            File.Move(filePath, destPath);
                        }                        
                    }
                    // Break out from the endless loop
                    break;
                }
                catch (IOException ex)
                {
                    // Sleep for 3 seconds before trying
                    Thread.Sleep(3000);
                } // end try
            } // end while(true)
        } // end private void ProcessFile(String fileName)

        

        private void ReadCSVAndWriteOnDB(String filePath)
        {

            DataTable csvDTable = GetDataTabletFromCSVFile(filePath);
            csvDTable = CheckDataTableWithMappingTable(csvDTable);
            BatchBulkCopy(csvDTable, "CyberPayData");

        }

        private void ReadXLAndWriteOnDB(string filePath,string extension,string isHDR)
        {
            string conStr = string.Empty;
            if (extension.Equals(".xls"))
            {
                conStr = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                conStr = String.Format(conStr, filePath, isHDR);
            }
            else if (extension.Equals(".xlsx"))
            {
                conStr = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                conStr = String.Format(conStr, filePath, isHDR);
            }

            DataSet ds = new DataSet();
            OleDbConnection connExcel = new OleDbConnection(conStr);
            connExcel.Open();
            //OleDbCommand cmdExcel = new OleDbCommand();
            OleDbCommand cmdExcel = new OleDbCommand("SELECT * FROM [Sheet1$]", connExcel);
            OleDbDataAdapter oda = new OleDbDataAdapter(cmdExcel);
            oda.Fill(ds);
            DataTable dTable = ds.Tables[0];
            foreach (DataColumn column in dTable.Columns)
            {
                column.ColumnName = column.ColumnName.Trim();
            } 
            //DataTable dTable =  connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            connExcel.Close();
            dTable = CheckDataTableWithMappingTable(dTable);
            BatchBulkCopy(dTable, "CyberPayData");
        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)

        {

            DataTable csvData = new DataTable();

            try

            {

                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))

                {

                    csvReader.SetDelimiters(new string[] { "," });

                    //csvReader.HasFieldsEnclosedInQuotes = true;

                    string[] colFields = csvReader.ReadFields();

                    foreach (string column in colFields)

                    {

                        DataColumn datecolumn = new DataColumn(column.Trim());

                        datecolumn.AllowDBNull = true;

                        csvData.Columns.Add(datecolumn);

                    }

                    while (!csvReader.EndOfData)

                    {

                        string[] fieldData = csvReader.ReadFields();

                        //Making empty value as null

                        for (int i = 0; i < fieldData.Length; i++)

                        {

                            if (fieldData[i] == "")

                            {

                                fieldData[i] = null;

                            }

                        }

                        csvData.Rows.Add(fieldData);

                    }

                }

            }

            catch (Exception ex)

            {

            }

            return csvData;

        }



        private DataTable CheckDataTableWithMappingTable(DataTable csvDTable)
        {
            //assume we have default provider for now but it may be different
            //as an example of check other provider with two different column in data table
            List<DataColumn> other1ProviderList = (from DataColumn column in csvDTable.Columns
                         where column.ColumnName.Equals("ComCode") || column.ColumnName.Equals("ConID")
                         select column).ToList();

            if (other1ProviderList.Count > 0)
            {
                //change datatable column header to map default map
                foreach (DataColumn item in other1ProviderList)
                {                   

                    switch (item.ColumnName)
                    {
                        case "ComCode":
                            item.ColumnName = "Company Code";
                            break;
                        case "ConID":
                            item.ColumnName = "ContractID";
                            break;
                        default:
                            break;
                    }
                }
            }

            return csvDTable;
        }

       

        public void BatchBulkCopy(DataTable dataTable, string DestinationTbl)
        {

            if (dataTable.Rows.Count > 0)
            {
                string consString = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name                       
                        sqlBulkCopy.DestinationTableName = DestinationTbl;                        
                        //sqlBulkCopy.ColumnMappings.Add("Id", "CustomerId");                                              
                        // Add your column mappings here
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            sqlBulkCopy.ColumnMappings.Add(
                                new SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName));
                        }

                        con.Open();
                        sqlBulkCopy.WriteToServer(dataTable);
                        con.Close();
                    }
                }
            }
        }

       
        public void OnStopFileMonitoring()
        {
            if (fileSystemWatcher != null)
            {
                fileSystemWatcher.EnableRaisingEvents = false;
                fileSystemWatcher.Dispose();
            }
        }

        //sample datatable parse example :: not needed method
        public DataTable ParseData(DataTable dtExcel)
        {
            var dt = new DataTable("sourceData");
            dt.Columns.Add(new DataColumn("Emp", typeof(String)));
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Shifts", typeof(String)));

            foreach (DataRow row in dtExcel.Rows)
            {
                var firstRowVal = row["Emp"];
                for (var i = 1; i < dtExcel.Columns.Count; i++)
                {
                    var newRow = dt.NewRow();
                    newRow["Emp"] = firstRowVal;
                    var dateVal = DateTime.MinValue;
                    DateTime.TryParse(dtExcel.Columns[i].ColumnName, out dateVal);
                    newRow["Date"] = dateVal;
                    newRow["Shifts"] = row[i].ToString();

                    dt.Rows.Add(newRow);
                }
            }
            return dt;
        }

    }
}
