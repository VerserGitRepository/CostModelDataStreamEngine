using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading.Tasks;

namespace CostModelDataStream
{
    class ExcelDataImport
    {
        public class MEF_GRAExporter
        {
            const int DETAILS_START_ROWINDEX = 24;

            private Excel.Application _xlApp;


            int ssnIndex;
            int brandIndex;
            int modelIndex;
            int serialNoIndex;
            int barcodeIndex;
            int itemTypeIndex;
            int itemNoIndex;
            int de1Index;
            int de2Index;
            int appearanceIndex;
            int completenessIndex;
            int operabilityIndex;
            int servicesIndex;
            int listPriceIndex;
            int priceIndex;
            int gradeIndex;
            int totalDVIndex;
            int conditionIndex;
            int errorIndex;
            int buyPriceIndex;
            int formFactorIndex;
            int colourIndex;
            int ramIndex;
            int videoMemoryIndex;
            int opticalDrivesIndex;
            int dvIndex;
            int hddSizeIndex;
            int conditionReportDateIndex;
            int dateCollectedReturnedIndex;

            Excel.Range loadNoRange;
            Excel.Range supplierNameRange;
            Excel.Range bookinAgentRange;
            Excel.Range connoteRange;
            Excel.Range woRange;
            Excel.Range dateCreatedRange;
            Excel.Range dateReceivedRange;
            Excel.Range referenceNumberRange;

            public MEF_GRAExporter(Excel.Application xlApp)
            {
                _xlApp = xlApp;

                Excel.Workbook activeWorkbook = _xlApp.ActiveWorkbook;

                ssnIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_NUMBER").Column;
                brandIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME1").Column;
                modelIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME2").Column;
                conditionReportDateIndex = activeWorkbook.ActiveSheet.Range("H_DATE_REPORT").Column;
                dateCollectedReturnedIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME7").Column;
                serialNoIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME4").Column;
                barcodeIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME6").Column;
                itemTypeIndex = activeWorkbook.ActiveSheet.Range("H_CATEGORY_NAME").Column;
                itemNoIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_NUMBER").Column;
                de1Index = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME11").Column;
                de2Index = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME12").Column;
                hddSizeIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME13").Column;
                ramIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME14").Column;
                opticalDrivesIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME23").Column;
                videoMemoryIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME15").Column;
                colourIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME10").Column;
                formFactorIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME9").Column;
                appearanceIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE10").Column;
                completenessIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE11").Column;
                dvIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE14").Column;
                buyPriceIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE15").Column;
                operabilityIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE9").Column;
                servicesIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE12").Column;
                listPriceIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE13").Column;
                priceIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE15").Column;
                gradeIndex = activeWorkbook.ActiveSheet.Range("H_ELEMENT_NAME3").Column;
                totalDVIndex = activeWorkbook.ActiveSheet.Range("H_ITEM_ATTRIBUTE14").Column;
                conditionIndex = activeWorkbook.ActiveSheet.Range("H_CONDITION").Column;
                errorIndex = activeWorkbook.ActiveSheet.Range("H_ERROR").Column;

                loadNoRange = activeWorkbook.ActiveSheet.Range("C13");
                supplierNameRange = activeWorkbook.ActiveSheet.Range("C7");
                bookinAgentRange = activeWorkbook.ActiveSheet.Range("C9");
                connoteRange = activeWorkbook.ActiveSheet.Range("H_CONNOTE");
                woRange = activeWorkbook.ActiveSheet.Range("D5");
                dateCreatedRange = activeWorkbook.ActiveSheet.Range("C15");
                dateReceivedRange = activeWorkbook.ActiveSheet.Range("C17");
                referenceNumberRange = activeWorkbook.ActiveSheet.Range("D9");
            }

            //public void ProcessLoadHeader(LoadViewModel load)
            //{
            //    loadNoRange.Value2 = load.LoadNo;

            //    if (!string.IsNullOrEmpty(load.LeasingCustomerName))
            //    {
            //        supplierNameRange.Value2 = load.LeasingCustomerName;
            //    }
            //    else if (string.IsNullOrEmpty(load.LeasingCustomerName) && !string.IsNullOrEmpty(load.ProjectClientRef))
            //    {
            //        supplierNameRange.Value2 = load.ProjectClientRef;
            //    }
            //    else
            //    {
            //        supplierNameRange.Value2 = load.ProjectName;
            //    }

            //    if (load.WarehouseName == "Auckland - Remarkit")
            //    {
            //        bookinAgentRange.Value2 = "Auckland - Remarkit";
            //    }
            //    else if (load.WarehouseName == "Wellington - Remarkit")
            //    {
            //        bookinAgentRange.Value2 = "Wellingtion - Remarkit";
            //    }
            //    else
            //    {
            //        bookinAgentRange.Value2 = "RPC";
            //    }

            //    connoteRange.Value2 = load.Connote;
            //    woRange.Value2 = String.Format("W/O: {0}", load.JobNo);

            //    dateReceivedRange.Value2 = load.DateReceived; 
            //    dateReceivedRange.NumberFormat = "dd/mm/yyyy";

            //    dateCreatedRange.Value2 = load.JobDateRaised; 
            //    dateCreatedRange.NumberFormat = "dd/mm/yyyy";

            //    referenceNumberRange.Value2 = load.ClientRef;
            //}

            //private void ProcessAssets(List<GRAAssetViewModel> assets)
            //{
            //    for (int i = 0; i < assets.Count(); i++)
            //    {
            //        Excel.Range currentRow = _xlApp.ActiveSheet.Rows[(DETAILS_START_ROWINDEX) + i];

            //        currentRow.Cells[Type.Missing, ssnIndex].Value2 = assets[i].SSN;
            //        currentRow.Cells[Type.Missing, brandIndex].Value2 = assets[i].Make;
            //        currentRow.Cells[Type.Missing, modelIndex].Value2 = assets[i].Model;
            //        currentRow.Cells[Type.Missing, serialNoIndex].Value2 = assets[i].SerialNo;
            //        currentRow.Cells[Type.Missing, barcodeIndex].Value2 = assets[i].Barcode;
            //        currentRow.Cells[Type.Missing, itemTypeIndex].Value2 = string.Format("IT-{0}", assets[i].ItemTypeName);

            //        string grade = assets[i].Grade;

            //        if (!string.IsNullOrEmpty(assets[i].Grade))
            //        {
            //            grade = grade.Substring(grade.Length - 1, 1);
            //        }

            //        currentRow.Cells[Type.Missing, gradeIndex].Value2 = !string.IsNullOrEmpty(assets[i].Grade) ? string.Format("CLASS {0}", grade) : string.Empty;

            //        string CPU = assets[i].CPU;

            //        if (CPU != null)
            //        {
            //            if (CPU.Length >= 30)
            //            {
            //                CPU = CPU.Substring(0, 30);
            //            }
            //        }

            //        Excel.Range conditionReportDateRange = currentRow.Cells[Type.Missing, conditionReportDateIndex];
            //        Excel.Range dateReturnedRange = currentRow.Cells[Type.Missing, dateCollectedReturnedIndex];

            //        if (assets[i].ProjectName == ConfigurationManager.AppSettings["MEF_ANZ_ProjectName"])
            //        {
            //            conditionReportDateRange.Value2 = dateCreatedRange.Value2;
            //            currentRow.Cells[Type.Missing, conditionReportDateIndex].Value2 = dateCreatedRange.Value2;

            //            dateReturnedRange.Value2 = dateCreatedRange.Value2;
            //            currentRow.Cells[Type.Missing, dateCollectedReturnedIndex].Value2 = dateCreatedRange.Value2;
            //        }
            //        else
            //        {
            //            conditionReportDateRange.Value2 = dateReceivedRange.Value2;
            //            currentRow.Cells[Type.Missing, conditionReportDateIndex].Value2 = dateReceivedRange.Value2;

            //            dateReturnedRange.Value2 = dateReceivedRange.Value2;
            //            currentRow.Cells[Type.Missing, dateCollectedReturnedIndex].Value2 = dateReceivedRange.Value2;
            //        }

            //        conditionReportDateRange.NumberFormat = "dd/m/yyyy";
            //        dateReturnedRange.NumberFormat = "dd/m/yyyy";

            //        currentRow.Cells[Type.Missing, appearanceIndex].Value2 = assets[i].Appearance;
            //        currentRow.Cells[Type.Missing, completenessIndex].Value2 = assets[i].Completeness;
            //        currentRow.Cells[Type.Missing, operabilityIndex].Value2 = assets[i].Operability;
            //        currentRow.Cells[Type.Missing, servicesIndex].Value2 = assets[i].Services;
            //        currentRow.Cells[Type.Missing, conditionIndex].Value2 = assets[i].Condition;
            //        currentRow.Cells[Type.Missing, formFactorIndex].Value2 = assets[i].ChassisType;
            //        currentRow.Cells[Type.Missing, colourIndex].Value2 = assets[i].Colour;
            //        currentRow.Cells[Type.Missing, ramIndex].Value2 = assets[i].Memory;
            //        currentRow.Cells[Type.Missing, videoMemoryIndex].Value2 = assets[i].VideoMemory;
            //        currentRow.Cells[Type.Missing, opticalDrivesIndex].Value2 = assets[i].OpticalDrives;
            //        currentRow.Cells[Type.Missing, hddSizeIndex].Value2 = assets[i].DiskSize;

            //        string itemType = string.Format("IT-{0}", assets[i].ItemTypeName);

            //        switch (itemType)
            //        {
            //            case "IT-DESKTOP PC":
            //            case "IT-NOTEBOOK PC":
            //            case "IT-SERVER":
            //                currentRow.Cells[Type.Missing, de1Index].Value2 = CPU;
            //                currentRow.Cells[Type.Missing, de2Index].Value2 = assets[i].CPUSpeed;
            //                break;
            //            case "IT-MONITOR":
            //                currentRow.Cells[Type.Missing, de1Index].Value2 = assets[i].ScreenSize;
            //                currentRow.Cells[Type.Missing, de2Index].Value2 = assets[i].ScreenType;
            //                break;
            //        }
            //    }
            //}

            //public void ProcessGRA(LoadViewModel load, List<GRAAssetViewModel> assets)
            //{
            //    ProcessLoadHeader(load);
            //    ProcessAssets(assets);
            //}
        }
    }
}
