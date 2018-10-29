


CREATE TABLE CyberPayData
(
   [Company Code]                      VARCHAR(25)
  ,[ContractID]                        VARCHAR(10)
  ,[Employer EIN]                      VARCHAR(10)
  ,[First Name]                        VARCHAR(50)
  ,[Middle Name]                       VARCHAR(50)
  ,[Last Name]                         VARCHAR(50)
  ,[Suffix]                            VARCHAR(10)
  ,[Social Security Number]            VARCHAR(11)
  ,[Employee Code]                     VARCHAR(25)
  ,[Gender]                            VARCHAR(1)
  ,[Employee Status]                   VARCHAR(2)
  ,[Marital Status]                    VARCHAR(1)
  ,[Pay Type]                          VARCHAR(1)
  ,[Union Status]                      VARCHAR(1)
  ,[Address 1]                         VARCHAR(50)
  ,[Address 2]                         VARCHAR(50)
  ,[City]                              VARCHAR(25)
  ,[State]                             VARCHAR(2)
  ,[Zip Code]                          VARCHAR(10)
  ,[CellPhone]                         VARCHAR(12)
  ,[Email]                             VARCHAR(60)
  ,[Birth Date]                        DATE 
  ,[Hire Date]                         DATE 
  ,[Rehire Date]                       DATE 
  ,[Adjusted Seniority Date]           DATE 
  ,[Termination Date]                  DATE 
  ,[Leave of Absence Begin Date]       DATE 
  ,[Leave of Absence End Date]         DATE 
  ,[Check Date]                        DATE 
  ,[Payroll Begin Date]                DATE 
  ,[Payroll End Date]                  DATE 
  ,[Payroll Frequency]                 VARCHAR(2)
  ,[Division]                          VARCHAR(10)
  ,[Department]                        VARCHAR(10)
  ,[Gross Earnings]                    NUMERIC(13,2)
  ,[Plan Earnings]                     NUMERIC(13,2)
  ,[Hours]                             NUMERIC(13,2)
  ,[Excluded Earnings]                 NUMERIC(13,2)
  ,[Section 125]                       NUMERIC(13,2)
  ,[Employee Deferral]                 NUMERIC(13,2)
  ,[Employee Roth]                     NUMERIC(13,2)
  ,[Employer Match]                    NUMERIC(13,2)
  ,[Safe Harbor Match]                 NUMERIC(13,2)
  ,[Safe Harbor Non-Elective]           NUMERIC(13,2)
  ,[Profit Sharing]                    NUMERIC(13,2)
  ,[Loan Payment 1]                    NUMERIC(13,2)
  ,[Loan Number 1]                     VARCHAR(10)
  ,[Loan Payment 2]                    NUMERIC(13,2)
  ,[Loan Number 2]                     VARCHAR(10)
  ,[Loan Payment 3]                    NUMERIC(13,2)
  ,[Loan Number 3]                     VARCHAR(10)
  ,[Loan Payment 4]                    NUMERIC(13,2)
  ,[Loan Number 4]                     VARCHAR(10)
  ,[Loan Payment 5]                    NUMERIC(13,2)
  ,[Loan Number 5]                     VARCHAR(10)
  ,[YTD Gross Earnings]                NUMERIC(13,2)
  ,[Plan YTD Earnings]                 NUMERIC(13,2)
  ,[Plan YTD Hours]                    NUMERIC(13,2)
  ,[Plan YTD Excluded Earnings]        NUMERIC(13,2)
  ,[Plan YTD Section 125]              NUMERIC(13,2)
  ,[Plan YTD Employee Deferral]        NUMERIC(13,2)
  ,[Plan YTD Employee Roth]            NUMERIC(13,2)
  ,[Plan YTD Employer Match]           NUMERIC(13,2)
  ,[Plan YTD Safe Harbor Match]        NUMERIC(13,2)
  ,[Plan YTD Safe Harbor Non-Elective]  NUMERIC(13,2)
  ,[Plan YTD Profit Sharing]           NUMERIC(13,2)
)
GO

INSERT INTO CyberPayData 
	VALUES ('GL03', '12345', '10-0000003', 'Wendy', 'A', 'Wenchek', NULL, '200000002', '2', 'F', 'A', 'M', 'H', NULL, '8 A St Apt 7', NULL, 'Coeur d''Alene', 'ID', '83814', NULL, 'wwenchek@email.com', '6/21/1960', '12/1/2015', NULL, NULL, NULL, NULL, NULL, '1/12/2017', '12/1/2016', '12/30/2016', 'M', NULL, NULL, 4119.5, 4119.5, 154, NULL, NULL, 120, NULL, NULL, NULL, NULL, NULL, 30, 'LOAN1', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 49513.87, 49513.87, 2080, NULL, NULL, 1440, NULL, NULL, NULL, NULL, NULL);
