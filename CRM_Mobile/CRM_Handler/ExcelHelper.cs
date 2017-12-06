using CRM_Common;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using System.Text;


namespace CRM_Handler
{
    public class ExcelHelper
    {
        // 获取工作表名称
        public static List<string> GetExcelSheetName(string pPath)
        {
            List<string> list = new List<string>();
            //打开一个Excel应用
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Workbook excelWB;//创建工作簿（WorkBook：即Excel文件主体本身）
            Workbooks excelWBs;
            Worksheet excelWS;//创建工作表（即Excel里的子表sheet）
            Sheets excelSts;
           
            try
            {
             
             
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                if (excelApp == null)
                {
                    throw new Exception("打开Excel应用时发生错误！");
                }
                excelWBs = excelApp.Workbooks;
                //打开一个现有的工作薄
                excelWB = excelWBs.Add(pPath);
                excelSts = excelWB.Sheets;
                int count = excelSts.Count;

                for (int i = 1; i <= count; i++)
                {
                    //选择第一个Sheet页
                    excelWS = (Worksheet)excelSts.get_Item(i);

                    string sheetName = excelWS.Name;

                    list.Add(sheetName);

                    ReleaseCOM(excelWS);
                }

                ReleaseCOM(excelSts);
                ReleaseCOM(excelWB);
                ReleaseCOM(excelWBs);


             
             
            }
            catch (Exception ex)
            {
                
                 LogHelper.Error(ex);
            }
            finally
            {
                if(excelApp!=null)
                {
                    excelApp.DisplayAlerts = false;
                    excelApp.Quit();
                    ReleaseCOM(excelApp);
                }
            }

            return list;
        }


        // 释放资源
        private static void ReleaseCOM(object pObj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(pObj);
            }
            catch
            {
                throw new Exception("释放资源时发生错误！");
            }
            finally
            {
                pObj = null;
            }
        }


        //读取EXCEL
        public static List<System.Data.DataTable> readExcel(string path)
        {
            List<System.Data.DataTable> list = new List<System.Data.DataTable>();
            OleDbConnection myConn = null;
            try
            {
                string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=NO;IMEX=1;\"";
                // string strCon = " Provider = Microsoft.ACE.OLEDB.12.0 ; Data Source = " + path + ";Extended Properties=Excel 12.0;HDR=Yes;IMEX=1";
                //string strCon = " Provider = Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1";
                myConn = new OleDbConnection(strCon);
                myConn.Open();
                List<string> strTableNamelist = GetExcelSheetName(path);

                foreach (var item in strTableNamelist)
                {
                    string strCom = " SELECT * FROM [" + item + "$] ";
                    OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
                    System.Data.DataSet myDataSet = new DataSet();
                    myCommand.Fill(myDataSet, item);
                    list.Add(myDataSet.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            finally
            {
                if (myConn != null)
                {
                    myConn.Close();
                    myConn.Dispose();
                }
            }






            return list;
        }




        static Microsoft.Office.Interop.Excel.Application _excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbooks _books = null;
        static Microsoft.Office.Interop.Excel._Workbook _book = null;
        static Microsoft.Office.Interop.Excel.Sheets _sheets = null;
        static Microsoft.Office.Interop.Excel._Worksheet _sheet = null;
        static Microsoft.Office.Interop.Excel.Range _range = null;
        static Microsoft.Office.Interop.Excel.Font _font = null;
        // Optional argument variable
        static object _optionalValue = Missing.Value;


        /// <summary>
        /// 保存到Excel
        /// </summary>
        /// <param name="excelName"></param>
        public static void SaveToExcel(string excelName, System.Data.DataTable dataTable)
        {
            try
            {
                if (dataTable != null)
                {
                    if (dataTable.Rows.Count != 0)
                    {

                        CreateExcelRef();
                        FillSheet(dataTable);
                        SaveExcel(excelName);

                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(e);
            }
            finally
            {
                ReleaseCOM(_sheet);
                ReleaseCOM(_sheets);
                ReleaseCOM(_book);
                ReleaseCOM(_books);
                ReleaseCOM(_excelApp);
            }
        }
        /// <summary>
        /// 将数据填充到内存Excel的工作表
        /// </summary>
        /// <param name="dataTable"></param>
        static void FillSheet(System.Data.DataTable dataTable)
        {
            object[] header = CreateHeader(dataTable);
            WriteData(header, dataTable);
        }

        static object[] CreateHeader(System.Data.DataTable dataTable)
        {

            List<object> objHeaders = new List<object>();
            for (int n = 0; n < dataTable.Columns.Count; n++)
            {
                objHeaders.Add(dataTable.Columns[n].ColumnName);
            }

            var headerToAdd = objHeaders.ToArray();
            //工作表的单元是从“A1”开始
            AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
            SetHeaderStyle();

            return headerToAdd;
        }

        static void WriteData(object[] header, System.Data.DataTable dataTable)
        {
            object[,] objData = new object[dataTable.Rows.Count, header.Length];

            for (int j = 0; j < dataTable.Rows.Count; j++)
            {
                var item = dataTable.Rows[j];
                for (int i = 0; i < header.Length; i++)
                {
                    var y = dataTable.Rows[j][i];
                    objData[j, i] = (y == null) ? "" : y.ToString();
                }
            }
            AddExcelRows("A2", dataTable.Rows.Count, header.Length, objData);
            AutoFitColumns("A1", dataTable.Rows.Count + 1, header.Length);
        }

        /// <summary>
        /// 将数据填充到Excel工作表的单元格中
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="values"></param>
        static void AddExcelRows(string startRange, int rowCount, int colCount, object values)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.set_Value(_optionalValue, values);
        }

        static void AutoFitColumns(string startRange, int rowCount, int colCount)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.Columns.AutoFit();
        }

        /// <summary>
        /// 将表头加粗显示
        /// </summary>
        static void SetHeaderStyle()
        {
            _font = _range.Font;
            _font.Bold = true;
        }


        /// <summary>
        /// 创建一个Excel程序实例
        /// </summary>
        private static void CreateExcelRef()
        {
            ////打开一个Excel应用
            //Microsoft.Office.Interop.Excel.Application excelApp;
            //Workbook excelWB;//创建工作簿（WorkBook：即Excel文件主体本身）
            //Workbooks excelWBs;
            //Worksheet excelWS;//创建工作表（即Excel里的子表sheet）
            //Sheets excelSts;
            //excelApp = new Microsoft.Office.Interop.Excel.Application();
            //if (excelApp == null)
            //{
            //    throw new Exception("打开Excel应用时发生错误！");
            //}
            //excelWBs = excelApp.Workbooks;
            ////打开一个现有的工作薄
            //excelWB = excelWBs.Add(pPath);
            //excelSts = excelWB.Sheets;
            //int count = excelSts.Count;

            //for (int i = 1; i <= count; i++)
            //{
            //    //选择第一个Sheet页
            //    excelWS = excelSts.get_Item(i);

            //    string sheetName = excelWS.Name;

            //    list.Add(sheetName);

            //    ReleaseCOM(excelWS);
            //}

            //ReleaseCOM(excelSts);
            //ReleaseCOM(excelWB);
            //ReleaseCOM(excelWBs);


            //excelApp.DisplayAlerts = false;
            //excelApp.Quit();
            //ReleaseCOM(excelApp);

            _excelApp = new Microsoft.Office.Interop.Excel.Application();
            _books = (Microsoft.Office.Interop.Excel.Workbooks)_excelApp.Workbooks;
            _book = (Microsoft.Office.Interop.Excel.Workbook)(_books.Add(_optionalValue));
            _sheets = (Microsoft.Office.Interop.Excel.Sheets)_book.Worksheets;
            _sheet = (Microsoft.Office.Interop.Excel.Worksheet)(_sheets.get_Item(1));
        }

        /// <summary>
        /// 将内存中Excel保存到本地路径
        /// </summary>
        /// <param name="excelName"></param>
        static void SaveExcel(string excelName)
        {
            _excelApp.Visible = false;
            //保存为Office2003和Office2007都兼容的格式
            _book.SaveAs(excelName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel12, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            _excelApp.Quit();

        }
    }
}
