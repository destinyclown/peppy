using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Peppy.Core.Utils
{
    public static class ExcelHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <param name="isFirstRowColumn"></param>
        /// <param name="columnTemplate"></param>
        /// <param name="requireColumns"></param>
        /// <param name="maxRows"></param>
        /// <returns></returns>
        public static DataTable ExcelToDataTable(string fileName, string sheetName, bool isFirstRowColumn, Dictionary<string, string[]> columnTemplate = null, string[] requireColumns = null, int? maxRows = null)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            IWorkbook workbook = null;
            int startRow = 0;
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    catch
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                }

                if (sheetName != null)
                {
                    if (workbook != null)
                    {
                        sheet = workbook.GetSheet(sheetName);
                        if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                        {
                            sheet = workbook.GetSheetAt(0);
                        }
                    }
                }
                else
                {
                    if (workbook != null) sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            string cellValue = cell?.StringCellValue?.Trim();
                            if (!string.IsNullOrWhiteSpace(cellValue))//列名正确性验证
                            {
                                if (columnTemplate != null && !columnTemplate.First().Value.Contains(cellValue))
                                    throw new Exception($"{columnTemplate.First().Key}不存在列名：{cellValue}！正确列名为：{string.Join(",", columnTemplate.First().Value)}");
                                DataColumn column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    if (maxRows != null)
                    {
                        if (rowCount > maxRows)
                            throw new Exception($"请拆分文件，一次最多支持{maxRows}条数据");
                    }
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null || row.Cells.Count == 0 || row.FirstCellNum == -1 || row.Cells.All(d => d.CellType == CellType.Blank)) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            var cellvalue = row.GetCell(j);
                            if (cellvalue == null || (cellvalue.ToString().Trim() == "0"))
                            {
                                if (requireColumns != null && requireColumns.Contains(data.Columns[j].ColumnName))
                                {
                                    //throw new Exception($"第{i}行，第{j}列,【{data.Columns[j].ColumnName}】不能为空或0，必须填写！");
                                }
                            }
                            if (cellvalue != null) dataRow[j] = cellvalue.ToString().Trim();
                            else
                            {
                                dataRow[j] = ""; //string.Empty;
                            }
                        }
                        data.Rows.Add(dataRow);
                    }
                }
                workbook?.Close();
                return data;
            }
            catch (Exception ex)
            {
                workbook?.Close();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 生成Excel文件(多行头部)
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="strExcelFileName">文件名</param>
        /// <param name="extHeaders"></param>
        public static void GridToExcelByNPOIMultiHeader(DataTable dt, string strExcelFileName, List<List<string>> extHeaders)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            try
            {
                int sheetIndex = 0;
                int dataIndex = 0;
                ICellStyle HeadercellStyle = workbook.CreateCellStyle();
                HeadercellStyle.BorderBottom = BorderStyle.Thin;
                HeadercellStyle.BorderLeft = BorderStyle.Thin;
                HeadercellStyle.BorderRight = BorderStyle.Thin;
                HeadercellStyle.BorderTop = BorderStyle.Thin;
                HeadercellStyle.Alignment = HorizontalAlignment.Center;

                ICellStyle cellStyle = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;

                //字体
                IFont headerfont = workbook.CreateFont();
                headerfont.Boldweight = (short)FontBoldWeight.Bold;
                HeadercellStyle.SetFont(headerfont);

                IFont cellfont = workbook.CreateFont();
                cellfont.Boldweight = (short)FontBoldWeight.Normal;
                cellStyle.SetFont(cellfont);
                var index = extHeaders.Count;

                while (dataIndex < dt.Rows.Count)
                {
                    sheetIndex++;
                    ISheet sheet = workbook.CreateSheet($"Sheet{sheetIndex}");

                    if (index > 0)
                    {
                        for (var i = 0; i < index; i++)
                        {
                            IRow extHeaderRow = sheet.CreateRow(i);

                            if (extHeaders[i].Count == 0)
                            {
                                ICell extCell = extHeaderRow.CreateCell(0);
                                extCell.SetCellValue("");
                                extCell.CellStyle = HeadercellStyle;
                            }
                            else
                            {
                                for (var j = 0; j < extHeaders[i].Count; j++)
                                {
                                    ICell cell = extHeaderRow.CreateCell(j);
                                    cell.SetCellValue(extHeaders[i][j]);
                                    cell.CellStyle = HeadercellStyle;
                                }
                            }
                        }
                    }

                    //用column name 作为列名
                    int icolIndex = 0;
                    IRow headerRow = sheet.CreateRow(index);
                    foreach (DataColumn item in dt.Columns)
                    {
                        ICell cell = headerRow.CreateCell(icolIndex);
                        cell.SetCellValue(item.ColumnName);
                        cell.CellStyle = HeadercellStyle;
                        icolIndex++;
                    }

                    //建立内容行
                    int iRowIndex = 1;
                    int iCellIndex = 0;
                    for (int count = 0; dataIndex < dt.Rows.Count; dataIndex++, count++)
                    {
                        if (count >= 65000)
                            break;
                        DataRow Rowitem = dt.Rows[dataIndex];
                        IRow DataRow = sheet.CreateRow(index + iRowIndex);
                        foreach (DataColumn Colitem in dt.Columns)
                        {
                            ICell cell = DataRow.CreateCell(iCellIndex);
                            cell.SetCellValue(Rowitem[Colitem].ToString());
                            cell.CellStyle = cellStyle;
                            iCellIndex++;
                        }
                        iCellIndex = 0;
                        iRowIndex++;
                    }

                    //自适应列宽度
                    for (int i = 0; i < icolIndex; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                }

                if (System.IO.File.Exists(strExcelFileName))
                    System.IO.File.Delete(strExcelFileName);

                //写Excel
                FileStream file = new FileStream(strExcelFileName, FileMode.Create);
                workbook.Write(file);
                file.Flush();
                file.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { workbook = null; }
        }

        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <param name="dt">数据源</param>
        /// <param name="strExcelFileName">文件名</param>
        public static void GridToExcelByNPOI(DataTable dt, string strExcelFileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            try
            {
                int sheetIndex = 0;
                int dataIndex = 0;
                ICellStyle HeadercellStyle = workbook.CreateCellStyle();
                HeadercellStyle.BorderBottom = BorderStyle.Thin;
                HeadercellStyle.BorderLeft = BorderStyle.Thin;
                HeadercellStyle.BorderRight = BorderStyle.Thin;
                HeadercellStyle.BorderTop = BorderStyle.Thin;
                HeadercellStyle.Alignment = HorizontalAlignment.Center;

                ICellStyle cellStyle = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                cellStyle.BorderTop = BorderStyle.Thin;

                //字体
                IFont headerfont = workbook.CreateFont();
                headerfont.Boldweight = (short)FontBoldWeight.Bold;
                HeadercellStyle.SetFont(headerfont);

                IFont cellfont = workbook.CreateFont();
                cellfont.Boldweight = (short)FontBoldWeight.Normal;
                cellStyle.SetFont(cellfont);
                while (dataIndex < dt.Rows.Count)
                {
                    sheetIndex++;
                    ISheet sheet = workbook.CreateSheet($"Sheet{sheetIndex}");

                    //用column name 作为列名
                    int icolIndex = 0;
                    IRow headerRow = sheet.CreateRow(0);
                    foreach (DataColumn item in dt.Columns)
                    {
                        ICell cell = headerRow.CreateCell(icolIndex);
                        cell.SetCellValue(item.ColumnName);
                        cell.CellStyle = HeadercellStyle;
                        icolIndex++;
                    }

                    //建立内容行
                    int iRowIndex = 1;
                    int iCellIndex = 0;
                    for (int count = 0; dataIndex < dt.Rows.Count; dataIndex++, count++)
                    {
                        if (count >= 65000)
                            break;
                        DataRow Rowitem = dt.Rows[dataIndex];
                        IRow DataRow = sheet.CreateRow(iRowIndex);
                        foreach (DataColumn Colitem in dt.Columns)
                        {
                            ICell cell = DataRow.CreateCell(iCellIndex);
                            cell.SetCellValue(Rowitem[Colitem].ToString());
                            cell.CellStyle = cellStyle;
                            iCellIndex++;
                        }
                        iCellIndex = 0;
                        iRowIndex++;
                    }

                    //自适应列宽度
                    for (int i = 0; i < icolIndex; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                }

                if (File.Exists(strExcelFileName))
                    File.Delete(strExcelFileName);

                //写Excel
                FileStream file = new FileStream(strExcelFileName, FileMode.Create);
                workbook.Write(file);
                file.Flush();
                file.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { workbook = null; }
        }

        /// <summary>
        /// 将DataTable数据导入到excel中
        /// </summary>
        /// <param name="data">要导入的数据</param>
        /// <param name="isColumnWritten">DataTable的列名是否要导入</param>
        /// <param name="fileName">导出文件名全路径</param>
        /// <param name="sheetName">要导入的excel的sheet的名称</param>
        /// <returns>导入数据行数(包含列名那一行)</returns>
        public static int DataTableToExcel(DataTable data, string fileName, bool isColumnWritten, string sheetName = "Sheet1")
        {
            int i = 0;
            int j = 0;
            int count = 0;
            ISheet sheet = null;
            IWorkbook workbook = null;
            string myDir = Path.GetDirectoryName(fileName);
            //判断文件夹是否存在
            if (!Directory.Exists(myDir))
            {
                //文件夹不存在则创建该文件夹
                if (myDir != null)
                    Directory.CreateDirectory(myDir);
            }
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
            {
                if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本
                    workbook = new XSSFWorkbook();
                else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                    workbook = new HSSFWorkbook();

                try
                {
                    if (workbook != null)
                    {
                        sheet = workbook.CreateSheet(sheetName);
                    }
                    else
                    {
                        return -1;
                    }

                    if (isColumnWritten == true) //写入DataTable的列名
                    {
                        IRow row = sheet.CreateRow(0);
                        for (j = 0; j < data.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                        }
                        count = 1;
                    }
                    else
                    {
                        count = 0;
                    }

                    for (i = 0; i < data.Rows.Count; ++i)
                    {
                        IRow row = sheet.CreateRow(count);
                        for (j = 0; j < data.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(data.Rows[i][j].ToString());
                        }
                        ++count;
                    }
                    workbook.Write(fs); //写入到excel
                    workbook.Close();
                    return count;
                }
                catch (Exception ex)
                {
                    workbook?.Close();
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <param name="fileName">第一行是否是DataTable的列名</param>
        /// <param name="startRow">开始行数</param>
        /// <param name="startData">开始收集数据行数</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn, string fileName, int startRow = 0, int startData = 1)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            try
            {
                IWorkbook workbook = null;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0 || fileName.IndexOf(".xlsm") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(startRow);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue.Replace(" ", "").Replace("?", "");
                                if (cellValue != null)
                                {
                                    if (data.Columns[cellValue] != null)
                                    {
                                        cellValue += i;
                                    }
                                    DataColumn column = new DataColumn(cellValue);
                                    data.Columns.Add(column);
                                }
                            }
                        }
                        startRow += sheet.FirstRowNum + startData;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// DataTable转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToDataList<T>(this DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                object v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    v = Convert.ChangeType(item[i], Nullable.GetUnderlyingType(info.PropertyType));
                                }
                                else
                                {
                                    v = Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        /// <summary>
        /// DataTable转成Dto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ToDataDto<T>(this DataTable dt)
        {
            T s = Activator.CreateInstance<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return s;
            }
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                if (info != null)
                {
                    try
                    {
                        if (!Convert.IsDBNull(dt.Rows[0][i]))
                        {
                            object v = null;
                            if (info.PropertyType.ToString().Contains("System.Nullable"))
                            {
                                v = Convert.ChangeType(dt.Rows[0][i], Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                v = Convert.ChangeType(dt.Rows[0][i], info.PropertyType);
                            }
                            info.SetValue(s, v, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// 将excel中的数据，根据sheet分别导入到DataTable中
        /// </summary>
        /// <param name="fileName">文件全路径</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <param name="columnTemplate"></param>
        /// <returns>返回的DataTable</returns>
        public static Dictionary<string, DataTable> ExcelToDataTablesBySheet(string fileName, bool isFirstRowColumn, Dictionary<string, string[]> columnTemplate = null)
        {
            ISheet sheet = null;
            IWorkbook workbook = null;
            int startRow = 0;
            var dtDictionary = new Dictionary<string, DataTable>();
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    if (fileName.IndexOf(".xlsx", StringComparison.Ordinal) > 0) // 2007版本
                        workbook = new XSSFWorkbook(fs);
                    else if (fileName.IndexOf(".xls", StringComparison.Ordinal) > 0) // 2003版本
                        workbook = new HSSFWorkbook(fs);
                }

                if (workbook != null)
                {
                    var sheetCount = workbook.NumberOfSheets;
                    for (int k = 0; k < sheetCount; k++)
                    {
                        var data = new DataTable();
                        sheet = workbook.GetSheetAt(k);
                        if (sheet != null)
                        {
                            var sheetName = sheet.SheetName;
                            IRow firstRow = sheet.GetRow(0);
                            int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                            if (isFirstRowColumn)
                            {
                                for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                {
                                    ICell cell = firstRow.GetCell(i);
                                    string cellValue = cell?.StringCellValue?.Trim();
                                    if (cellValue != null)
                                    {
                                        if (columnTemplate != null && !columnTemplate.First().Value.Contains(cellValue))
                                            throw new Exception($"{columnTemplate.First().Key}不存在列名：{cellValue}！正确列名为：{string.Join(",", columnTemplate.First().Value)}");
                                        DataColumn column = new DataColumn(cellValue);
                                        data.Columns.Add(column);
                                    }
                                }
                                startRow = sheet.FirstRowNum + 1;
                            }
                            else
                            {
                                startRow = sheet.FirstRowNum;
                            }

                            //最后一列的标号
                            //var isAddRow = true;
                            int rowCount = sheet.LastRowNum;
                            for (int i = startRow; i <= rowCount; ++i)
                            {
                                //isAddRow = true;
                                IRow row = sheet.GetRow(i);
                                if (row == null) continue; //没有数据的行默认是null　　　　　　　

                                DataRow dataRow = data.NewRow();
                                for (int j = row.FirstCellNum; j < cellCount; ++j)
                                {
                                    var cellvalue = row.GetCell(j);
                                    if (cellvalue != null)
                                    {
                                        dataRow[j] = cellvalue.ToString().Trim();
                                    }
                                }
                                //if (!isAddRow)
                                //{
                                //    continue;
                                //}
                                data.Rows.Add(dataRow);
                            }
                            if (dtDictionary.ContainsKey(sheetName))
                            {
                                throw new Exception($"sheet名称重复：{sheetName}，请规范命名！");
                            }
                            dtDictionary.Add(sheetName, data);
                        }
                    }
                }

                workbook?.Close();
                return dtDictionary;
            }
            catch (Exception ex)
            {
                workbook?.Close();
                throw new Exception(ex.Message);
            }
        }
    }
}