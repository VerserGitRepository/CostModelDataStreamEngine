using CostModelDataStream.CostModelEntities;
using CostModelDataStream.ServiceImplementations;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using CostModelDataStream.Logger;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;
using log4net;
using System.Reflection;
using System.Diagnostics;

namespace CostModelDataStream.StreamEngine
{
    public class ExcelStreamerService
    {
        static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void ReadExcel()
        {
            string CostModelFolders = ConfigurationSettings.AppSettings.Get("CostModelFolders");
           
            FilesValidateService filesvalidate = new FilesValidateService();
            if (!Directory.Exists(CostModelFolders))
            {
                Environment.Exit(0);
            }
            string theFileName = "";
            foreach (string filename in Directory.GetFiles(CostModelFolders))
            {
                try
                {
                    Log.Info("processing data for " + filename);
                    if (!File.Exists(filename))
                    {
                        Environment.Exit(0);
                    }
                    var returnvalidation = filesvalidate.IsFileExists(filename);

                    if (returnvalidation.IsFileProcessSuccess == true && returnvalidation.FileName != null)
                    {
                        Log.Info("This file has already been processed. " + filename);
                        continue;
                    }
                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
                    Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Project Details"];
                    Excel.Range xlRange = xlWorksheet.UsedRange;
                    Log.Info("Adding the opportunity number.");
                    string OpportunityNumber = xlRange.Cells[3, 2].Value2.ToString();
                    Log.Info("The opportunity number is " + OpportunityNumber);
                    var Returnvalidation = AddProject(xlRange, OpportunityNumber);
                    string ServiceDescriptionval = "";
                    if (!Returnvalidation.IsSuccess && Returnvalidation.OpportunityNumberID == 0)
                    {
                        Environment.Exit(0);
                    }
                    int OpportunityNumberID = Returnvalidation.OpportunityNumberID;
                    try
                    {
                        xlWorksheet = xlWorkbook.Sheets["Pricing Summary"];
                    }
                    catch (Exception ex)
                    {
                        Log.Info("The exception has occurred. The details are " + ex.ToString());
                        try
                        {
                            xlWorksheet = xlWorkbook.Sheets["Summary"];
                        }
                        catch (Exception ex1)
                        {
                            Log.Info("Double exception. The file format is incorrect" + ex1.ToString());
                            continue;
                        }
                    }
                    xlRange = xlWorksheet.UsedRange;
                    Range cells = xlRange.Worksheet.Cells;
                    bool ishdn = true;
                    int rowCount = xlRange.Rows.Count;
                    string checkStop = "";
                    for (int i = 6; i <= rowCount; i++)
                    {
                        ishdn = cells.Rows[i].Hidden;
                        if (cells.Rows[i].Hidden)
                        {
                            continue;
                        }
                        checkStop = xlRange.Cells[i, 6].Value2;
                       
                        if (checkStop != null && checkStop.ToLower() == "end")
                        {
                            break;
                        }
                        else if (xlRange.Cells[i, 2].Value2 == null)
                        {
                            continue;
                        }

                        try
                        {
                            ServiceDescriptionval = xlRange.Cells[i, 1].Value2.ToString();
                            if (ServiceDescriptionval.ToLower().Trim() == "service description") { continue; }

                        }
                        catch (Exception)
                        { continue; }
                        AddServiceRevenue(xlRange, OpportunityNumberID, i);
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
                   // Process[] pro = Process.GetProcessesByName("excel");
                    foreach(Process pro in Process.GetProcessesByName("excel"))
                    {
                        pro.Kill();
                        pro.WaitForExit();
                    }
                    
                }
                catch (Exception ex)
                {
                    Log.Info("exception occurred. The details are " + ex.ToString());
                    continue;
                }
            }
        }
        private static int GetJMSProjectId(string ProjectName)
        {
            string connString = ConfigurationSettings.AppSettings["JMS"].Trim();

            string sql = "select id from Projects where LTRIM(RTRIM(lower(summary))) ='" + ProjectName.ToLower().Trim()+"'" ;
            int newProdID = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                try
                {
                    conn.Open();
                    newProdID = (Int32)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return (int)newProdID;
        }

        private static ReturnEntityModel AddProject(Excel.Range xlRange, string OpportunityNumber)
        {
            Log.Info("Inside method AddProject");
            string Approver = "";
            string SiteAddress = "";
            string VerserBranch = "";
            string SalesManager = "";
            string ProjectManager = "";
            ReturnEntityModel ReturnValues;
            Log.Info("Reading values.");
            var Customer = xlRange.Cells[2, 2].Value2.ToString() ?? null;
            var projectName = xlRange.Cells[8, 2].Value2.ToString() ?? null;
            Log.Info("customer values."+ Customer);
            try
            {
                SiteAddress = xlRange.Cells[4, 2].Value2.ToString() ?? null;
            }
            catch (Exception ex)
            {
                Log.Info(ex.ToString());
            }
            try
            {
                // var CustomerContactName = xlRange.Cells[2, 2].Value2.ToString() ?? null;
                VerserBranch = xlRange.Cells[5, 5].Value2.ToString() ?? null;
            }
            catch (Exception)
            {

            }
            try
            {
                SalesManager = xlRange.Cells[6, 5].Value2.ToString() ?? null;
            }
            catch (Exception)
            {

            }
            try
            {
                ProjectManager = xlRange.Cells[7, 5].Value2 ?? null;
            }
            catch (Exception)
            {
                throw;
            }
            try
            {
                Approver = xlRange.Cells[4, 5].Value2.ToString() ?? null;
            }
            catch (Exception)
            {

            }
            int JMSProjectId = GetJMSProjectId(xlRange.Cells[8, 2].Value2.ToString());

      
               ProjectDetails project = new ProjectDetails
                {
                    Customer = Customer,
                    OpportunityNumber = OpportunityNumber,
                    SiteAddress = SiteAddress,
                   // CustomerContactName = CustomerContactName,
                    VerserBranch = VerserBranch,
                    SalesManager = SalesManager,
                    ProjectManager = ProjectManager,
                   
                    Approver = Approver
                };
            try
            {
                if (xlRange.Cells[2, 5].Value.ToString() != null)
                {
                    DateTime StartDate = Convert.ToDateTime(xlRange.Cells[2, 5].Value.ToString());
                    project.StartDate = StartDate;
                }
            }
            catch (Exception ex)
            {
                Log.Info("Exception occurred in project startdate. The details are " + ex.ToString());
            }

            ReturnValues = ProjectDetailsService.CreateProjectDetails(project);
            int projectmanagerID = ProjectManagerService.CreateProjectManager(ProjectManager);
            int projectID = 0;
            int OpportunityNumberID = 0;
            var _customer = ProjectDetailsService.AddCustomer(project);
            int salesmanid = SalesManagerService.AddSalesManager(project.SalesManager);
            if (projectmanagerID > 0)
            {
                projectID = ProjectService.CreateProject(projectName, JMSProjectId);
            }
            if (projectID > 0)
            {
                OpportunityNumberID = OpportunityNumberService.CreateOpportunityNumber(int.Parse(OpportunityNumber), projectID, projectmanagerID, salesmanid);
            }           

            ReturnValues.OpportunityNumberID = OpportunityNumberID;
            return ReturnValues;
                       
        }
        private static void AddServiceRevenue(Excel.Range xlRange,  int OpportunityNumberID, int i)
        {
            string ServiceDescription = "";
            try
            {
                 ServiceDescription = xlRange.Cells[i, 1].Value2.ToString();
               
            }
            catch (Exception)
            { return; }
            if (ServiceDescription.Length >= 500)
            {
                ServiceDescription = ServiceDescription.Substring(0, 450);
            }
            ServiceDescription = ServiceDescription.Replace("'", "");
            decimal PricePerUnit = 0.0M;
            var _CostCategory = "";
            decimal _CostPerUnit = 0.0M;
            decimal _TravelCostPerUnit = 0.0M;
            decimal _LabourCostPerUnit = 0.0M;
            decimal _VariableCostPerUnit = 0.0M;
            decimal _PMCostPerUnit = 0.0M;
            decimal _TechnicianHourlyRate = 0.0M;
            decimal _TravelCostHoursPerunit = 0.0M;
            decimal _LabourCostHoursPerUnit = 0.0M;
            decimal _VariableCostPerUnitNA = 0.0M;
            decimal _PMCostHoursPerUnit = 0.0M;
            decimal _TotalCost = 0.0M;
            decimal _ProfitPerUnit = 0.0M;
            decimal _TotalProfit = 0.0M;
            try
            {
                PricePerUnit = decimal.Parse(xlRange.Cells[i, 2].Value2.ToString().Replace("$", "").Trim()) ?? null;
                PricePerUnit = decimal.Parse(PricePerUnit.ToString().Replace("$", "").Trim());
                PricePerUnit = Math.Round(PricePerUnit, 2, MidpointRounding.ToEven);
               
            }
            catch (Exception ex)
            {
                PricePerUnit = 0.0M;
            }
                int Quantity = 0;
            try
            {
                int.TryParse(xlRange.Cells[i, 3].Value2.ToString(), out Quantity);
            }
            catch (Exception ex)
            {
                Quantity = 0;
            }
                decimal TotalPrice = 0;
            try
            {
                
                TotalPrice = decimal.Parse(xlRange.Cells[i, 4].Value2.ToString().Replace("$", "").Trim()) ?? null;
                TotalPrice = decimal.Parse(TotalPrice.ToString().Replace("$", "").Trim());
                TotalPrice = Math.Round(TotalPrice, 2, MidpointRounding.ToEven);

            }
            catch (Exception ex)
            {
                TotalPrice = 0.0M;
            }

            //service cost merge
            if (xlRange.Cells[i, 6].Value2 != null)
            {
                _CostCategory = xlRange.Cells[i, 6].Value2.ToString() ?? null;
            }
            if (xlRange.Cells[i, 7].Value2 != null)
            {

                try
                {
                    _CostPerUnit = decimal.Parse(xlRange.Cells[i, 7].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _CostPerUnit = decimal.Parse(_CostPerUnit.ToString().Replace("$", "").Trim());
                    _CostPerUnit = Math.Round(_CostPerUnit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _CostPerUnit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 8].Value2 != null)
            {

                try
                {
                    _TravelCostPerUnit = decimal.Parse(xlRange.Cells[i, 8].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _TravelCostPerUnit = decimal.Parse(_TravelCostPerUnit.ToString().Replace("$", "").Trim());
                    _TravelCostPerUnit = Math.Round(_TravelCostPerUnit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _TravelCostPerUnit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 9].Value2 != null)
            {

                try
                {
                    _LabourCostPerUnit = decimal.Parse(xlRange.Cells[i, 9].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _LabourCostPerUnit = decimal.Parse(_LabourCostPerUnit.ToString().Replace("$", "").Trim());
                    _LabourCostPerUnit = Math.Round(_LabourCostPerUnit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _LabourCostPerUnit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 10].Value2 != null)
            {

                try
                {
                    _VariableCostPerUnit = decimal.Parse(xlRange.Cells[i, 10].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _VariableCostPerUnit = decimal.Parse(_VariableCostPerUnit.ToString().Replace("$", "").Trim());
                    _VariableCostPerUnit = Math.Round(_VariableCostPerUnit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _VariableCostPerUnit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 11].Value2 != null)
            {

                try
                {
                    _PMCostPerUnit = decimal.Parse(xlRange.Cells[i, 11].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _PMCostPerUnit = decimal.Parse(_PMCostPerUnit.ToString().Replace("$", "").Trim());
                    _PMCostPerUnit = Math.Round(_PMCostPerUnit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _PMCostPerUnit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 12].Value2 != null)
            {

                try
                {
                    _TechnicianHourlyRate = decimal.Parse(xlRange.Cells[i, 12].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _TechnicianHourlyRate = decimal.Parse(_TechnicianHourlyRate.ToString().Replace("$", "").Trim());
                    _TechnicianHourlyRate = Math.Round(_TechnicianHourlyRate, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _TechnicianHourlyRate = 0.0M;
                }
            }
            if (xlRange.Cells[i, 14].Value2 != null)
            {

                try
                {
                    _TravelCostHoursPerunit = decimal.Parse(xlRange.Cells[i, 14].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _TravelCostHoursPerunit = decimal.Parse(_TravelCostHoursPerunit.ToString().Replace("$", "").Trim());
                    _TravelCostHoursPerunit = Math.Round(_TravelCostHoursPerunit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _TravelCostHoursPerunit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 15].Value2 != null)
            {
                try
                {
                    _LabourCostHoursPerUnit = decimal.Parse(xlRange.Cells[i, 15].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _LabourCostHoursPerUnit = decimal.Parse(_LabourCostHoursPerUnit.ToString().Replace("$", "").Trim());
                    _LabourCostHoursPerUnit = Math.Round(_LabourCostHoursPerUnit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _LabourCostHoursPerUnit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 16].Value2 != null)
            {
                try
                {
                    _VariableCostPerUnitNA = decimal.Parse(xlRange.Cells[i, 16].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _VariableCostPerUnitNA = decimal.Parse(_VariableCostPerUnitNA.ToString().Replace("$", "").Trim());
                    _VariableCostPerUnitNA = Math.Round(_VariableCostPerUnitNA, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _VariableCostPerUnitNA = 0.0M;
                }
            }
            if (xlRange.Cells[i, 17].Value2 != null)
            {
                try
                {
                    _PMCostHoursPerUnit = decimal.Parse(xlRange.Cells[i, 17].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _PMCostHoursPerUnit = decimal.Parse(_PMCostHoursPerUnit.ToString().Replace("$", "").Trim());
                    _PMCostHoursPerUnit = Math.Round(_PMCostHoursPerUnit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _PMCostHoursPerUnit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 18].Value2 != null)
            {

                try
                {
                    _TotalCost = decimal.Parse(xlRange.Cells[i, 18].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _TotalCost = decimal.Parse(_TotalCost.ToString().Replace("$", "").Trim());
                    _TotalCost = Math.Round(_TotalCost, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _TotalCost = 0.0M;
                }
            }
            if (xlRange.Cells[i, 19].Value2 != null)
            {
                try
                {
                    _ProfitPerUnit = decimal.Parse(xlRange.Cells[i, 19].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _ProfitPerUnit = decimal.Parse(_ProfitPerUnit.ToString().Replace("$", "").Trim());
                    _ProfitPerUnit = Math.Round(_ProfitPerUnit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _ProfitPerUnit = 0.0M;
                }
            }
            if (xlRange.Cells[i, 20].Value2 != null)
            {

                try
                {
                    _TotalProfit = decimal.Parse(xlRange.Cells[i, 20].Value2.ToString().Replace("$", "").Trim()) ?? null;
                    _TotalProfit = decimal.Parse(_TotalProfit.ToString().Replace("$", "").Trim());
                    _TotalProfit = Math.Round(_TotalProfit, 2, MidpointRounding.ToEven);
                }
                catch (Exception ex)
                {
                    _TotalProfit = 0.0M;
                }
            }
            int ServiceActivitiesID= ServiceActivityHelperService.CreateServiceActivity(ServiceDescription);

                ServiceRevenue s = new ServiceRevenue()
                {
                    ServiceDescription = ServiceDescription,
                    PricePerUnit = Convert.ToString(PricePerUnit).Trim(),
                    Quantity = Convert.ToString(Quantity),
                    TotalPrice = Convert.ToString(TotalPrice).Trim(),
                    ServiceActivityID = ServiceActivitiesID,
                    OpportunityNumberID = OpportunityNumberID,
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
                    TotalProfit = _TotalProfit,
                   // ActualMarginOnOverHead = _ActualMarginOnOverHead,                    
                };
                ServiceRevenueService servicerevenue = new ServiceRevenueService();
                servicerevenue.CreateServiceCost(s);           
        }
        private static void AddServiceCost(Excel.Range xlRange, int OpportunityNumberID, int i)
        {
            string _ActualMarginOnOverHead = xlRange.Cells[i, 20].Value2.ToString() ?? null;
            ServiceCost c = new ServiceCost()
            {
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

