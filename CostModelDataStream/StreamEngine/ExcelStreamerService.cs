using CostModelDataStream.CostModelEntities;
using CostModelDataStream.ServiceImplementations;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace CostModelDataStream.StreamEngine
{
    public class ExcelStreamerService
    {
        public static void ReadExcel()
        {
            string CostModelFolders = ConfigurationSettings.AppSettings.Get("CostModelFolders");

            FilesValidateService filesvalidate = new FilesValidateService();
            if (!Directory.Exists(CostModelFolders))
            {
                Environment.Exit(0);
            }
            foreach (string filename in Directory.GetFiles(CostModelFolders))
            {               
                if (!File.Exists(filename))
                {
                    Environment.Exit(0);
                }
                var returnvalidation = filesvalidate.IsFileExists(filename);
                if (returnvalidation.IsFileProcessSuccess == true && returnvalidation.FileName != null)
                {
                    continue;
                }
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Project Details"];  
                Excel.Range xlRange = xlWorksheet.UsedRange;

                string OpportunityNumber = xlRange.Cells[3, 2].Value2.ToString(); 
                var Returnvalidation = AddProject(xlRange, OpportunityNumber);

                if (!Returnvalidation.IsSuccess && Returnvalidation.OpportunityNumberID == 0)
                {
                    Environment.Exit(0);
                }
                int OpportunityNumberID = Returnvalidation.OpportunityNumberID;
                xlWorksheet = xlWorkbook.Sheets["Pricing summary"];
                xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;

                for (int i = 7; i <= rowCount; i++)
                {
                    if (xlRange.Cells[i, 2].Value2 == null)
                    {
                        break;
                    }

                    AddServiceRevenue(xlRange, OpportunityNumberID, i);
                }
                for (int i = 7; i <= rowCount; i++)
                {
                    if (xlRange.Cells[i, 8].Value2 == null)
                    {
                        break;
                    }
                    AddServiceCost(xlRange,  OpportunityNumberID, i);
                }
                if (Returnvalidation.IsSuccess == true)
                {
                    filesvalidate.AddNewFile(filename, OpportunityNumber);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(xlRange);
                Marshal.ReleaseComObject(xlWorksheet);
               
                xlWorkbook.Close();
                Marshal.ReleaseComObject(xlWorkbook);
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
        }
        private static ReturnEntityModel AddProject(Excel.Range xlRange, string OpportunityNumber)
        {
           
                ReturnEntityModel ReturnValues;
                var Customer = xlRange.Cells[2, 2].Value2.ToString() ?? null;

                var SiteAddress = xlRange.Cells[4, 2].Value2.ToString() ?? null;
                var CustomerContactName = xlRange.Cells[6, 2].Value2.ToString() ?? null;
                var VerserBranch = xlRange.Cells[5, 5].Value2.ToString() ?? null;
                var SalesManager = xlRange.Cells[6, 5].Value2.ToString() ?? null;
                var ProjectManager = xlRange.Cells[7, 5].Value2.ToString() ?? null;
                var Approver = xlRange.Cells[4, 5].Value2.ToString() ?? null;

            //create Project Manager, Opp,Project if not exist
            int projectmanagerID = ProjectManagerService.CreateProjectManager(ProjectManager);
            int projectID = 0;
            int OpportunityNumberID = 0;           
            if (projectmanagerID >0)
            {
                projectID = ProjectService.CreateProject(Customer);
            }
            if (projectID >0)
            {
                OpportunityNumberID = OpportunityNumberService.CreateOpportunityNumber(ProjectManager, projectID);
            }

                 ProjectDetails project = new ProjectDetails
                {
                    Customer = Customer,
                    OpportunityNumber = OpportunityNumber,
                    SiteAddress = SiteAddress,
                    CustomerContactName = CustomerContactName,
                    VerserBranch = VerserBranch,
                    SalesManager = SalesManager,
                    ProjectManager = ProjectManager,
                   
                    Approver = Approver
                };
                if (xlRange.Cells[2, 5].Value.ToString() != null)
                {
                    DateTime StartDate = Convert.ToDateTime(xlRange.Cells[2, 5].Value.ToString());
                    project.StartDate = StartDate;
                }
                //if (xlRange.Cells[3, 5].Value.ToString() != null)
                //{
                //    DateTime EndDate = Convert.ToDateTime(xlRange.Cells[3, 5].Value.ToString());
                //    project.EndDate = EndDate;
                //}
                // project.EndDate = DateTime.Now;

                ProjectDetailsService ProjectServicemethod = new ProjectDetailsService();

                ReturnValues = ProjectServicemethod.CreateProjectDetails(project);
                ReturnValues.OpportunityNumberID = OpportunityNumberID;
            return ReturnValues;
                       
        }
        private static void AddServiceRevenue(Excel.Range xlRange,  int OpportunityNumberID, int i)
        {           
                var ServiceDescription = xlRange.Cells[i, 1].Value2.ToString() ?? null;
                var PricePerUnit = xlRange.Cells[i, 2].Value2.ToString() ?? null;
                var Quantity = xlRange.Cells[i, 3].Value2.ToString() ?? null;
                var TotalPrice = xlRange.Cells[i, 4].Value2.ToString() ?? null;

            int ServiceActivitiesID= ServiceActivityHelperService.CreateServiceActivity(ServiceDescription);

                ServiceRevenue s = new ServiceRevenue()
                {
                    ServiceDescription = ServiceDescription,
                    PricePerUnit = PricePerUnit,
                    Quantity = Quantity,
                    TotalPrice = TotalPrice,
                    ServiceActivityID = ServiceActivitiesID,
                    OpportunityNumberID = OpportunityNumberID
                };
                ServiceRevenueService servicerevenue = new ServiceRevenueService();
                servicerevenue.CreateServiceCost(s);
           
        }
        private static void AddServiceCost(Excel.Range xlRange, int OpportunityNumberID, int i)
        {
            
                string _CostCategory = xlRange.Cells[i, 6].Value2.ToString() ?? null;
                string _CostPerUnit = xlRange.Cells[i, 7].Value2.ToString() ?? null;
                string _TravelCostPerUnit = xlRange.Cells[i, 8].Value2.ToString() ?? null;
                string _LabourCostPerUnit = xlRange.Cells[i, 9].Value2.ToString() ?? null;
                string _VariableCostPerUnit = xlRange.Cells[i, 10].Value2.ToString() ?? null;
                string _PMCostPerUnit = xlRange.Cells[i, 11].Value2.ToString() ?? null;
                string _TechnicianHourlyRate = xlRange.Cells[i, 12].Value2.ToString() ?? null;
                string _TravelCostHoursPerunit = xlRange.Cells[i, 13].Value2.ToString() ?? null;
                string _LabourCostHoursPerUnit = xlRange.Cells[i, 14].Value2.ToString() ?? null;
                string _VariableCostPerUnitNA = xlRange.Cells[i, 15].Value2.ToString() ?? null;
                string _PMCostHoursPerUnit = xlRange.Cells[i, 16].Value2.ToString() ?? null;
                  string _TotalCost = xlRange.Cells[i, 17].Value2.ToString() ?? null;                 
               string _ProfitPerUnit = xlRange.Cells[i, 18].Value2.ToString() ?? null;                   
                string _TotalProfit = xlRange.Cells[i, 19].Value2.ToString() ?? null;                     
               string _ActualMarginOnOverHead = xlRange.Cells[i, 20].Value2.ToString() ?? null;

                ServiceCost c = new ServiceCost()
                {
                    CostCategory = _CostCategory,
                    CostPerUnit = _CostPerUnit,
                    TravelCostPerUnit = _TravelCostPerUnit,
                    LabourCostPerUnit = _LabourCostPerUnit,
                    VariableCostPerUnit = _VariableCostPerUnit,
                    PMCostPerUnit = _PMCostPerUnit,
                    TechnicianHourlyRate = _TechnicianHourlyRate,
                    TravelCostHoursPerunit = _TravelCostHoursPerunit,
                    LabourCostHoursPerUnit = _LabourCostHoursPerUnit,
                    VariableCostPerUnitNA = _VariableCostPerUnitNA,
                    PMCostHoursPerUnit = _PMCostHoursPerUnit,
                    TotalCost = _TotalCost,
                   ProfitPerUnit = _ProfitPerUnit,
                   TotalProfit =_TotalProfit,
                   ActualMarginOnOverHead = _ActualMarginOnOverHead,
                    OpportunityNumberID_FK = OpportunityNumberID
                };
                ServiceCostService costservice = new ServiceCostService();
                costservice.CreateServiceCost(c);           
                    
        }
        public static void ReleaseFile(Excel.Range xlRange, Excel._Worksheet xlWorksheet, Excel.Workbook xlWorkbook, Excel.Application xlApp)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}

