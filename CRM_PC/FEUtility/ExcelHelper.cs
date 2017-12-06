namespace FEUtility
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;
    using NPOI;
    using NPOI.HPSF;
    using NPOI.HSSF;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.POIFS;
    using NPOI.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;

    /// <summary>
    /// 导出数据到excel
    /// </summary>
    public class ExcelHelper
    {
        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        /// <param name="sheetName">工作薄名称</param>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName, string sheetName)
        {
            using (MemoryStream ms = Export(dtSource, strHeaderText, sheetName))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="sheetName">工作薄名称</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ///设置工作薄名称
            ISheet sheet = workbook.CreateSheet(sheetName);
            sheet.TabColorIndex = HSSFColor.Red.Index;
            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "北京圣邦天麒科技有限公司";
                dsi.Category = "业务导出";///类别
                workbook.DocumentSummaryInformation = dsi;
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "圣邦天麒技术支持部"; //填加xls文件作者信息
                si.ApplicationName = "代理商服务平台"; //填加xls文件创建程序信息
                si.LastAuthor = "圣邦天麒技术支持部"; //填加xls文件最后保存者信息
                si.Comments = "如有疑问请询问在线客服"; //填加xls文件作者信息
                si.Title = strHeaderText; //填加xls文件标题信息
                si.Subject = "圣邦天麒在线系统业务导出";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion
            ///时间格式化格式
            HSSFCellStyle dateStyle = workbook.CreateCellStyle() as HSSFCellStyle;
            HSSFDataFormat format = workbook.CreateDataFormat() as HSSFDataFormat;
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd HH:mm:ss");
            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet() as HSSFSheet;
                    }
                    #region 表头及样式
                    //{

                    //    HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                    //    headerRow.HeightInPoints = 25;
                    //    headerRow.CreateCell(0).SetCellValue(strHeaderText);
                    //    HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                    //    headStyle.Alignment = HorizontalAlignment.CENTER;
                    //    HSSFFont font = workbook.CreateFont() as HSSFFont;
                    //    font.FontHeightInPoints = 20;
                    //    font.Boldweight = 200;
                    //    headStyle.SetFont(font);
                    //    headerRow.GetCell(0).CellStyle = headStyle;
                    //    sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                    //}
                    #endregion

                    #region 列头及样式
                    {
                        //杨宝帅8.23修改把列头提到第一行
                        //HSSFRow headerRow = sheet.CreateRow(1) as HSSFRow;
                        HSSFRow headerRow = sheet.CreateRow(0) as HSSFRow;
                        HSSFCellStyle headStyle = workbook.CreateCellStyle() as HSSFCellStyle;
                        headStyle.Alignment = HorizontalAlignment.Center;
                        HSSFFont font = workbook.CreateFont() as HSSFFont;
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.IsLocked = true;
                        headStyle.SetFont(font);
                        //设置每列的文字杨宝帅8-15修改
                        string[] header = strHeaderText.Split(',');
                        int i = 0;
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            if (i < header.Length)
                            {
                                //headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);                           
                                headerRow.CreateCell(column.Ordinal).SetCellValue(header[i]);
                                headerRow.GetCell(column.Ordinal).CellStyle = headStyle;
                                //设置列宽
                                sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                            }
                            i++;
                        }
                        //让列头不动
                        //sheet.CreateFreezePane(0, 2, 0, dtSource.Columns.Count - 1);
                        sheet.CreateFreezePane(0, 1, 0, dtSource.Columns.Count - 1);
                    }
                    #endregion
                    //修改把内容提到第二行
                    // rowIndex = 2;
                    rowIndex = 1;
                }
                #endregion

                #region 填充内容
                HSSFRow dataRow = sheet.CreateRow(rowIndex) as HSSFRow;
                foreach (DataColumn column in dtSource.Columns)
                {
                    HSSFCell newCell = dataRow.CreateCell(column.Ordinal) as HSSFCell;
                    string drValue = row[column].ToString();
                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }
                #endregion
                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
                return ms;
            }
        }


        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        /// <param name="sheetName">工作薄名称</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName, string sheetName)
        {
            HttpContext curContext = HttpContext.Current;
            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8) + ".xls");
            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText, sheetName).GetBuffer());
            //curContext.Response.End();
            // 因为（由于代码已经过优化或本机框架位于调用堆栈之上,无法计算表达式的值）异常
            HttpContext.Current.ApplicationInstance.CompleteRequest(); 
        }


        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();
            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
            HSSFSheet sheet = hssfworkbook.GetSheetAt(0) as HSSFSheet;
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            HSSFRow headerRow = sheet.GetRow(0) as HSSFRow;
            int cellCount = headerRow.LastCellNum;
            for (int j = 0; j < cellCount; j++)
            {
                HSSFCell cell = headerRow.GetCell(j) as HSSFCell;
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = sheet.GetRow(i) as HSSFRow;
                DataRow dataRow = dt.NewRow();
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }
                dt.Rows.Add(dataRow);
            }
            return dt;
        }
        /// <summary>
        /// Excel表格转DataTable
        /// </summary>
        /// <param name="FilePath">Excel文件物理路径</param>
        /// <returns>DataTable</returns>
        public DataTable ExcelToDataTable(string FilePath)
        {
            DataTable dt = new DataTable("table1");
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = WorkbookFactory.Create(fs);//使用接口，自动识别excel2003/2007格式
                ISheet sheet = workbook.GetSheetAt(0);//得到里面第一个sheet
                //表头  
                IRow headerRow = sheet.GetRow(0);//获得第一行
                int cellCount = headerRow.LastCellNum - 1;//获得最后一个单元格的列号
                for (int i = 0; i <= cellCount; i++)
                {
                    DataColumn column;
                    if (headerRow.GetCell(i) == null)
                    {
                        column = new DataColumn("");
                    }
                    else
                    {
                        string ColumnName = headerRow.GetCell(i).StringCellValue.ToString().Trim();
                        //判断重名
                        if (dt.Columns.Contains(ColumnName))
                        {
                            int RepeatNum = 1;
                            //加重复数后缀，循环判断
                            while (dt.Columns.Contains(ColumnName + RepeatNum.ToString()))
                            {
                                RepeatNum++;
                            }
                            column = new DataColumn(ColumnName + RepeatNum.ToString());
                        }
                        else
                        {
                            column = new DataColumn(ColumnName);
                        }
                    }
                    dt.Columns.Add(column);
                }
                //内容
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    IRow Row = sheet.GetRow(i);
                    if (Row == null)
                    {
                        continue;
                    }
                    for (int j = 0; j <= cellCount; j++)
                    {
                        if (Row.GetCell(j) == null)
                        {
                            dr[j] = "";
                        }
                        else
                        {
                            dr[j] = Row.GetCell(j).ToString().Trim();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}