using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;//Missing

using System.Text.RegularExpressions;//Regex
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace I2CDownload
{
    public class XLSXWrapper : IDisposable
    {
        ExcelPackage package;
        ExcelWorksheet worksheet;
        public XLSXWrapper()
        {
            package = new ExcelPackage();
        }

        public XLSXWrapper(string strPath)
        {
            package = new ExcelPackage(new System.IO.FileInfo(strPath));
        }

        public void Dispose()
        {
            package.Dispose();
        }

        public bool addSheet(string name)
        {
            worksheet = package.Workbook.Worksheets.Add(name);
            return true;
        }
        //public bool copySheet(string srcName, string distName);
        //public bool deleteSheet(string name);
        //public override sealed void Dispose();
        //protected virtual void Dispose(bool __p1);
        //public bool insertSheet(int index, string name);
        //public bool moveSheet(string srcName, int distIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">从1开始</param>
        /// <param name="col">从1开始</param>
        /// <returns></returns>
        public object read(int row, int col)
        {
            if (worksheet.Cells[row, col].Value == null) return "";
            else return worksheet.Cells[row, col].Value;
        }
        //public bool renameSheet(string oldName, string newName);

        public bool saveAs(string strPath)
        {
            package.SaveAs(new System.IO.FileInfo(strPath));
            return true;
        }

        public bool selectSheet(string strSheetName)
        {
            worksheet = package.Workbook.Worksheets[strSheetName];
            return worksheet != null;
        }
        //public bool setColumnHidden(int column, bool hidden);
        //public bool setColumnWidth(int column, double width);
        //public bool setRowHeight(int row, double height);
        //public bool setRowHidden(int row, bool hidden);
        public string[] sheetNames()
        {
            string[] strs = new string[package.Workbook.Worksheets.Count];
            for (int i = 0; i < strs.Length; i++)
            {
                strs[i] = package.Workbook.Worksheets[i].Name;
            }
            return strs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">从1开始</param>
        /// <param name="col">从1开始</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool write(int row, int col, object value)
        {
            worksheet.Cells[row, col].Value = value;
            return true;
        }

        //public bool write(int row, int col, string value, string fontName, int size, int color, bool bold, bool italic, HorizontalAlignment horizontalAlignment, bool strikeOut, FontScript fontScript, FontUnderline fontUnderline, bool outline, bool textWrap, int rotation, int indent, bool shink);
    }


    public class ClsImportExportData
    {
        public bool ExportToExcel(DataGridView dtView, string strPath, string strSheetName)
        {
            return ExportToExcel_base(dtView, strPath, strSheetName);
        }
        public bool ExportToExcel(DataGridView dtView, string strPath)
        {
            return ExportToExcel_base(dtView, strPath, "");
        }
        private bool ExportToExcel_base(DataGridView dtView, string strPath, string strSheetName)
        {
            using (XLSXWrapper xlsx = new XLSXWrapper())
            {
                int intRowsNum = dtView.Rows.Count;
                int intColsNum = dtView.Columns.Count;
                int i, j;

                if (strSheetName.Equals(""))
                {
                    xlsx.addSheet("Sheet4");
                }
                else
                {
                    xlsx.addSheet(strSheetName);
                }

                //获取数据库中的行数，并将其保存到myExcel中     
                for (i = 0; i < intRowsNum; i++)
                {
                    //xlsx.write(i + 2, 1, dtView.Rows[i].HeaderCell.Value);
                    for (j = 0; j < intColsNum; j++)
                    {
                        if (i == 0)
                        {
                            xlsx.write(1, j + 1, dtView.Columns[j].HeaderCell.Value.ToString());
                        }
                        xlsx.write(i + 2, j + 1, dtView[j, i].Value);
                    }
                }

                xlsx.saveAs(strPath);

            }

            return true;
        }
        //
        public bool ImportFromExcel(DataGridView dtView, string strPath, string strSheetName, int intInputType)
        {
            return ImportFromExcel_base(dtView, strPath, strSheetName, intInputType);
        }
        public bool ImportFromExcel(DataGridView dtView, string strPath, int intInputType)
        {
            return ImportFromExcel_base(dtView, strPath, "", intInputType);
        }
        //intInputType=0:数据不强制转换；=1：强制转换为byte
        private bool ImportFromExcel_base(DataGridView dtView, string strPath, string strSheetName, int intInputType)
        {
            using (XLSXWrapper xlsx = new XLSXWrapper(strPath))
            {
                if (xlsx.selectSheet(strSheetName.Equals("") ? "Sheet1" : strSheetName))
                {
                    int intRowsNum = dtView.Rows.Count;
                    int intColsNum = dtView.Columns.Count;
                    int i, j;
                    //获取数据库中的行数，并将其保存到myExcel中  
                    for (i = 0; i < intRowsNum; i++)
                    {
                        dtView.Rows[i].HeaderCell.Value = xlsx.read(i + 2, 1).ToString();
                        for (j = 0; j < intColsNum; j++)
                        {
                            if (i == 0)
                            {
                                dtView.Columns[j].HeaderCell.Value = xlsx.read(1, j + 2).ToString();
                            }
                            if (intInputType == 0)
                            {
                                dtView[j, i].Value = xlsx.read(i + 2, j + 2).ToString();
                            }
                            else
                            {
                                string str = xlsx.read(i + 2, j + 2).ToString();
                                dtView[j, i].Value = convertStr2Byte(str);
                            }
                        }
                    }

                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// 获取打开的文件路径
        /// </summary>
        /// <param name="openFileDialog">对话框</param>
        /// <param name="iFilterIndex">文件对话框中筛选器索引</param>
        /// <returns>文件路径</returns>        
        public string GetOpenFilePath()
        {
            string resultFile = "";

            OpenFileDialog openFile = new OpenFileDialog(); //实例化打开对话框对象
            //openFile.Filter = "xlsx files (*.xlsx)|*.xlsx|txt files (*.txt)|*.txt|All files (*.*)|*.*";//设置打开文件筛选器
            openFile.Filter = "xlsx files (*.xlsx)|*.xlsx|xls files (*.xls)|*.xls|All files (*.*)|*.*";//设置打开文件筛选器
            openFile.Multiselect = false;//设置打开对话框中不能多选
            openFile.RestoreDirectory = true;//控制对话框在关闭之前是否恢复当前目录
            openFile.FilterIndex = 1;//在对话框中选择的文件筛选器的索引，如果选第一项就设为1
            openFile.AddExtension = true;//自动在文件名中添加扩展名
            openFile.CheckFileExists = true;//指示用户指定不存在的文件名
            openFile.CheckPathExists = true;//指示用户指定不存在的路径
            if (openFile.ShowDialog() == DialogResult.OK) //弹出选择框
            {
                resultFile = openFile.FileName;                
            }            
            return resultFile;
        }
        /// <summary>
        /// 获取保存的文件路径
        /// </summary>
        /// <param name="saveFileDialog">对话框</param>
        /// <param name="iFilterIndex">文件对话框中筛选器索引</param>
        /// <returns>文件路径</returns>        
        public string GetSaveFilePath()
        {
            string resultFile = "";

            SaveFileDialog saveFile = new SaveFileDialog(); //实例化打开对话框对象                
            saveFile.Filter = "xlsx files (*.xlsx)|*.xlsx|xls files (*.xls)|*.xls|All files (*.*)|*.*";//设置打开文件筛选器
            saveFile.RestoreDirectory = true;//控制对话框在关闭之前是否恢复当前目录
            saveFile.FilterIndex = 1;//在对话框中选择的文件筛选器的索引，如果选第一项就设为1
            saveFile.AddExtension = true;//自动在文件名中添加扩展名
            //saveFile.CheckFileExists = true;//指示用户指定不存在的文件名
            saveFile.CheckPathExists = true;//指示用户指定不存在的路径

            if (saveFile.ShowDialog() == DialogResult.OK) //弹出选择框
            {
                resultFile = saveFile.FileName;
            }
            return resultFile;
        }        
       
        private byte convertStr2Byte(string strData)
        {
            int intReturn;
            byte bytReturn;
            try
            {
                intReturn = Convert.ToInt32(strData);
                if (intReturn > 255)
                {
                    intReturn = 0;
                }
            }
            catch
            {
                intReturn = 0;
            }
            bytReturn = (byte)intReturn;
            return bytReturn;
        }
        //
    }
}
