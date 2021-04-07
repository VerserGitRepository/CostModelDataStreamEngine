using CostModelDataStream.CostModelEntities;
using CostModelDataStream.ServiceImplementations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using CostModelDataStream.ViewModels;
using Newtonsoft.Json;
using System.Linq;
using System.Threading;
using System.Text;


namespace CostModelDataStream.StreamEngine
{
    public class ExcelStreamerService
    {
        public static int ServiceActivitiesID = 0;
        public static void ReadExcel()
        {
            string CostModelFolders = ConfigurationSettings.AppSettings.Get("CostModelFolders");
            var filesvalidate = new FilesValidateService();           
          
            string _FileNames=string.Empty;
            try
            {
                if (!Directory.Exists(CostModelFolders))
                {
                    Environment.Exit(0);
                }
                int _Count = 0;
                foreach (string filename in Directory.GetFiles(CostModelFolders))
                {

                    if (!File.Exists(filename))
                    {
                        Environment.Exit(0);
                    }
                    CostModelLogger.InfoLogger($"{_FileNames} File Opened and initiating to process");
                      _FileNames = _FileNames + $"{filename.Substring(31)}, ";
                    var returnvalidation = filesvalidate.IsFileExists(filename);
                    if (returnvalidation.IsFileProcessSuccess == true && returnvalidation.FileName != null)
                    {
                        continue;
                    }
                    if (filename.Contains("Thumb"))
                    {
                        SendMailnotification(_FileNames, _Count);
                        CostModelLogger.InfoLogger($"Email Sent app will exit.");
                        Environment.Exit(0);
                    }
                    Excel.Application xlApp = new Excel.Application();
                    Thread.Sleep(3000);
                    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filename);
                    //Console.WriteLine($"{filename} Processing...");                

                    Thread.Sleep(3000);
                    Excel._Worksheet xlWorksheet = xlWorkbook.Sheets["Project Details"];
                    Thread.Sleep(3000);
                    Excel.Range xlRange = xlWorksheet.UsedRange;
                    string OpportunityNumber = xlRange.Cells[3, 2].Value2.ToString();
                    var Returnvalidation = AddProject(xlRange, OpportunityNumber);
                   // Console.WriteLine($"{OpportunityNumber} Opportunity Number Processing...");
                    CostModelLogger.InfoLogger($"{OpportunityNumber} Opportunity Number Processing...");
                    if (!Returnvalidation.IsSuccess && Returnvalidation.OpportunityNumberID == 0)
                    {
                        Environment.Exit(0);
                    }
                    int OpportunityNumberID = Returnvalidation.OpportunityNumberID;
                    Thread.Sleep(2000);
                    xlWorksheet = xlWorkbook.Sheets["Pricing Summary"];
                    Thread.Sleep(3000);
                    xlRange = xlWorksheet.UsedRange;
                    int rowCount = xlRange.Rows.Count;
                    try
                    {
                        for (int i = 7; i <= rowCount; i++)
                        {
                            string TempText = xlRange.Cells[i, 2].text as string;

                            if (string.IsNullOrWhiteSpace(TempText))
                            {
                                break;
                            }
                            var _CostRevenueDataModel = ServiceCostRevenueModelBuilder(xlRange, OpportunityNumberID, i);
                            if (_CostRevenueDataModel != null)
                            {

                                CreateServiceActivity(_CostRevenueDataModel.ServiceDescription);
                                _CostRevenueDataModel.ServiceActivityID = ServiceActivitiesID;
                                _CostRevenueDataModel.serviceactivity_ID = ServiceActivitiesID;
                                if (_CostRevenueDataModel.OpportunityNumberID == 0)
                                {
                                    _CostRevenueDataModel.OpportunityNumberID = OpportunityNumberID;
                                }
                                CreateServiceRevenue(_CostRevenueDataModel);
                                CreateServiceCost(_CostRevenueDataModel);
                            }
                        }
                        if (Returnvalidation.IsSuccess == true)
                        {
                            filesvalidate.AddNewFile(filename, OpportunityNumber);
                        }
                        _Count ++;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Marshal.ReleaseComObject(xlRange);
                        Marshal.ReleaseComObject(xlWorksheet);
                        xlWorkbook.Close();
                        Marshal.ReleaseComObject(xlWorkbook);
                        xlApp.Quit();
                        Marshal.ReleaseComObject(xlApp);

                        if (_Count >= Directory.GetFiles(CostModelFolders).Count())
                        {
                            SendMailnotification(_FileNames, _Count);
                        } 
                    }
                    catch (Exception ex)
                    {
                        CostModelLogger.ErrorLogger($"Error Occured:  {ex.Message}");
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Marshal.ReleaseComObject(xlRange);
                        Marshal.ReleaseComObject(xlWorksheet);
                        xlWorkbook.Close();
                        Marshal.ReleaseComObject(xlWorkbook);
                        xlApp.Quit();
                        Marshal.ReleaseComObject(xlApp);
                        Console.WriteLine(ex.Message);
                        Console.ReadKey();
                      //  throw;
                    }
                                    
                }               
            }
            catch (Exception Ex)
            {               
                //  Console.WriteLine(Ex.Message);
                CostModelLogger.ErrorLogger($"Error Occured:  {Ex.Message}");               
               // throw;
            }  
        }
        private static ReturnEntityModel AddProject(Excel.Range xlRange, string OpportunityNumber)
        {
            string CustomerContactName = string.Empty;
            var ReturnValues= new ReturnEntityModel();
            try
            {
                var Customer = xlRange.Cells[2, 2].Value2.ToString() ?? null;
                var SiteAddress = xlRange.Cells[4, 2].Value2.ToString() ?? null;
                //if (!string.IsNullOrEmpty(xlRange.Cells[6, 2].Value2.ToString()))
                //{
                //    CustomerContactName = xlRange.Cells[6, 2].Value2.ToString();
                //}
                var VerserBranch = xlRange.Cells[5, 5].Value2.ToString() ?? null;
                var SalesManager = xlRange.Cells[6, 5].Value2.ToString() ?? null;
                var ProjectManager = xlRange.Cells[7, 5].Value2.ToString() ?? null;
                var Approver = xlRange.Cells[4, 5].Value2.ToString() ?? null;
                var OpportunityName = xlRange.Cells[8, 2].Value2.ToString();

                //create Project Manager, Opp,Project if not exist
                int projectmanagerID = ProjectManagerService.CreateProjectManager(ProjectManager);
                int projectID = 0;
                int OpportunityNumberID = 0;
                int JMSProjectid = ProjectList(OpportunityName).Result;
                int SalesManagerID = salesManagerService.CreateSalesManager(SalesManager);
                if (projectmanagerID > 0)
                {
                    projectID = ProjectService.CreateProject(OpportunityName, JMSProjectid, projectmanagerID, SalesManagerID);

                }
                if (projectID > 0)
                {
                    OpportunityNumberID = OpportunityNumberService.CreateOpportunityNumber(Convert.ToInt32(OpportunityNumber), projectID, projectmanagerID, SalesManagerID);
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

                var ProjectServicemethod = new ProjectDetailsService();

                ReturnValues = ProjectServicemethod.CreateProjectDetails(project);
                ReturnValues.OpportunityNumberID = OpportunityNumberID;
            }
            catch (Exception ex)
            {
                CostModelLogger.ErrorLogger($"Error Occurred , {ex.Message}");
            }           
            return ReturnValues;            
        }
        private static void CreateServiceActivity(string ServiceDescription)
        {         
            ServiceActivitiesID = ServiceActivityHelperService.CreateServiceActivity(ServiceDescription);    
        }        
        private static void CreateServiceRevenue(ServiceCostRevenueViewModel _RevenueModel)
        {
            try
            {
                if (ServiceActivitiesID > 0)
                {
                    var _ServiceRevenue = new ServiceRevenue()
                    {
                        ServiceDescription = _RevenueModel.ServiceDescription,
                        PricePerUnit = _RevenueModel.PricePerUnit,
                        Quantity = _RevenueModel.Quantity,
                        TotalPrice = _RevenueModel.TotalPrice,
                        ServiceActivityID = _RevenueModel.serviceactivity_ID,
                        OpportunityNumberID = _RevenueModel.OpportunityNumberID,
                        CostPerUnit = _RevenueModel.CostPerUnit,
                        TravelCostPerUnit = _RevenueModel.TravelCostPerUnit,
                        LabourCostPerUnit = _RevenueModel.LabourCostPerUnit,
                        VariableCostPerUnit = _RevenueModel.VariableCostPerUnit,
                        PMCostPerUnit = _RevenueModel.PMCostPerUnit,
                        TechnicianHourlyRate = _RevenueModel.TechnicianHourlyRate,
                        TravelCostHoursPerunit = _RevenueModel.TravelCostHoursPerunit,
                        LabourCostHoursPerUnit = _RevenueModel.LabourCostHoursPerUnit,
                        VariableCostPerUnitNA = _RevenueModel.VariableCostPerUnitNA,
                        PMCostHoursPerUnit = _RevenueModel.PMCostHoursPerUnit,
                        TotalCost = _RevenueModel.TotalCost
                    };
                    var servicerevenue = new ServiceRevenueService();
                    servicerevenue.CreateServiceRevenue(_ServiceRevenue);
                }
            }
            catch (Exception ex)
            {
               // Console.WriteLine(ex.Message);
                CostModelLogger.ErrorLogger($"Error Occurred , {ex.Message}");
             //   throw;
            }                  
        }
        private static void CreateServiceCost(ServiceCostRevenueViewModel _RevenueModel)
        {
            if (ServiceActivitiesID > 0)
            {
                var _Servicecost = new ServiceCost()
                {
                    CostCategory = _RevenueModel.CostCategory,
                    CostPerUnit = _RevenueModel.CostPerUnit.ToString(),
                    TravelCostPerUnit = _RevenueModel.TravelCostPerUnit.ToString(),
                    LabourCostPerUnit = _RevenueModel.LabourCostPerUnit.ToString(),
                    VariableCostPerUnit = _RevenueModel.VariableCostPerUnit.ToString(),
                    PMCostPerUnit = _RevenueModel.PMCostPerUnit.ToString(),
                    TechnicianHourlyRate = _RevenueModel.TechnicianHourlyRate.ToString(),
                    TravelCostHoursPerunit = _RevenueModel.TravelCostHoursPerunit.ToString(),
                    LabourCostHoursPerUnit = _RevenueModel.LabourCostHoursPerUnit.ToString(),
                    VariableCostPerUnitNA = _RevenueModel.VariableCostPerUnitNA.ToString(),
                    PMCostHoursPerUnit = _RevenueModel.PMCostHoursPerUnit.ToString(),
                    TotalCost = _RevenueModel.TotalCost.ToString(),
                    ProfitPerUnit = _RevenueModel.ProfitPerUnit.ToString(),
                    TotalProfit = _RevenueModel.TotalProfit.ToString(),
                    ActualMarginOnOverHead = _RevenueModel.ActualMarginOnOverHead.ToString(),
                    serviceactivity_ID = ServiceActivitiesID,
                    OpportunityNumberID = _RevenueModel.OpportunityNumberID
                };
                var costservice = new ServiceCostService();
                costservice.CreateServiceCost(_Servicecost);
            }
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
        private static int GetJMSProjectId(string ProjectName)
        {
            string connectionString = ConfigurationSettings.AppSettings["JMS"].Trim();
            string cmdText = "select id from Projects where LTRIM(RTRIM(lower(summary))) ='" + ProjectName.ToLower().Trim() + "'";
            int num = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(cmdText, connection);
                try
                {
                    connection.Open();
                    num = (int)command.ExecuteScalar();
                }
                catch (Exception exception)
                {
                   // Console.WriteLine(exception.Message);
                    CostModelLogger.ErrorLogger($"Error Occurred , {exception.Message}");
                }
            }
            return num;
        }
        public static async  Task<int> ProjectList(string ProjectName)
        {
            int ReturnID = 0;
            string JMSAPIURL = ConfigurationSettings.AppSettings["JMSAPIURL"].Trim();
            var returnmodel = new List<ListItems>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(JMSAPIURL);
                HttpResponseMessage response = client.GetAsync(string.Format("inventorycontrol/projects/listitems")).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    returnmodel = JsonConvert.DeserializeObject<List<ListItems>>(data);
                    var _Projectdata = returnmodel.Where(p => p.Value.Contains(ProjectName)).FirstOrDefault();
                    if (_Projectdata != null)
                    {
                        ReturnID = _Projectdata.ID;
                    }
                }
            }
            return ReturnID;
        }
        public static void SendMailnotification(string responsemodel, int FeedCount)
        {
            //This filepath needs to be changed
            //string templatePath = Path.Combine(@"C:\VerserSourceCodeGitRepo\MCQFeedImport-new\MCQFeedImport\MailTemplate");
            string workingDirectory = Environment.CurrentDirectory;
            string templatePath = Path.Combine(Directory.GetParent(workingDirectory).Parent.FullName + @"\MailTemplate");
            if (templatePath != null && responsemodel != null)
            {
                Dictionary<string, string> replacements = new Dictionary<string, string>();
                replacements.Add("FileName", responsemodel);
                replacements.Add("FeedCount", FeedCount.ToString());
                replacements.Add("Date", DateTime.Now.ToString());
                try
                {
                    MailNotificationService.SendMail(String.Format("{0}\\{1}", templatePath, "FeedUpdatenotification.htm"), replacements);
                }
                catch (Exception ex)
                {
                    CostModelLogger.ErrorLogger($"Sending email failed, {ex.Message}");                
                }
            }
        }
        private static ServiceCostRevenueViewModel ServiceCostRevenueModelBuilder(Excel.Range xlRange, int OpportunityNumberID, int i)
        {
            var _ServiceCostModel = new ServiceCostRevenueViewModel();
            try
            {
                if (xlRange != null)
                {
                    _ServiceCostModel.ServiceDescription = xlRange.Cells[i, 1].Value2.ToString() ?? null;
                    _ServiceCostModel.serviceactivity_ID = ServiceActivitiesID;
                    _ServiceCostModel.PricePerUnit = xlRange.Cells[i, 2].Value2.ToString() ?? null;
                    _ServiceCostModel.Quantity = xlRange.Cells[i, 3].Value2.ToString() ?? null;
                    _ServiceCostModel.TotalPrice = xlRange.Cells[i, 4].Value2.ToString() ?? null;
                    _ServiceCostModel.CostCategory = xlRange.Cells[i, 6].Value2.ToString() ?? null;
                    _ServiceCostModel.CostPerUnit = Convert.ToDecimal(xlRange.Cells[i, 7].Value2.ToString() ?? null);
                    _ServiceCostModel.TravelCostPerUnit = Convert.ToDecimal(xlRange.Cells[i, 8].Value2.ToString() ?? null);
                    _ServiceCostModel.LabourCostPerUnit = Convert.ToDecimal(xlRange.Cells[i, 9].Value2.ToString() ?? null);
                    _ServiceCostModel.VariableCostPerUnit = Convert.ToDecimal(xlRange.Cells[i, 10].Value2.ToString() ?? null);
                    _ServiceCostModel.PMCostPerUnit = Convert.ToDecimal(xlRange.Cells[i, 11].Value2.ToString() ?? null);
                    _ServiceCostModel.TechnicianHourlyRate = Convert.ToDecimal(xlRange.Cells[i, 12].Value2.ToString() ?? null);
                    _ServiceCostModel.TravelCostHoursPerunit = Convert.ToDecimal(xlRange.Cells[i, 13].Value2.ToString() ?? null);
                    string _LabourCostHoursPerUnit = (xlRange.Cells[i, 14].text as string ?? null);
                    if (!string.IsNullOrWhiteSpace(_LabourCostHoursPerUnit) && _LabourCostHoursPerUnit != "$0.00")
                    {
                        _ServiceCostModel.LabourCostHoursPerUnit = Convert.ToDecimal(_LabourCostHoursPerUnit);
                    }   
                    string _PMCostHoursPerUnit = (xlRange.Cells[i, 17].text as string ?? null);                  
                   string  _PMCostHoursPerUnit2 = (xlRange.Cells[i, 17].Value2.ToString() ?? null);                       
                    if (!string.IsNullOrWhiteSpace(_PMCostHoursPerUnit) && _PMCostHoursPerUnit != "$0.00")
                    {
                        _ServiceCostModel.PMCostHoursPerUnit = Convert.ToDecimal(_PMCostHoursPerUnit);
                    } 
                    _ServiceCostModel.VariableCostPerUnitNA = Convert.ToDecimal(xlRange.Cells[i, 15].Value2.ToString() ?? null);
                    _ServiceCostModel.TotalCost = Convert.ToDecimal(xlRange.Cells[i, 17].Value2.ToString() ?? null);
                    _ServiceCostModel.ProfitPerUnit = Convert.ToDecimal(xlRange.Cells[i, 18].Value2.ToString() ?? null);
                    _ServiceCostModel.TotalProfit = Convert.ToDecimal(xlRange.Cells[i, 19].Value2.ToString() ?? null);
                    _ServiceCostModel.ActualMarginOnOverHead = Convert.ToDecimal(xlRange.Cells[i, 20].Value2.ToString() ?? null);
                    _ServiceCostModel.OpportunityNumberID = OpportunityNumberID;
                }
            }
            catch (Exception ex)
            {
                CostModelLogger.ErrorLogger($"Error Occured:  {ex.Message}");
              //  Console.WriteLine(ex.Message); 
            }           
            return _ServiceCostModel;
        }
    }
}


