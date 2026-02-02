using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;//Regex

namespace I2CDownload
{
    public partial class FrmFlashSetup : Form
    {
        public ClsFlashSetupConfig mclsFlashSetup = null;

        public FrmFlashSetup()
        {
            InitializeComponent();
        }
        private void InitDataGrid(int intDataNum)
        {
            dtGrid.RowCount = intDataNum;
            dtGrid.ColumnCount = 22;
            //
            dtGrid.RowHeadersWidth = 60;//标题列
            dtGrid.ColumnHeadersHeight = 40;//
            for (int i = 0; i < dtGrid.ColumnCount; i++)
            {
                dtGrid.Columns[i].Width = 75;//列宽
            }
            dtGrid.Columns[0].Width = 150;//Name
            dtGrid.Columns[1].Width = 65;//PW DevAddr
            dtGrid.Columns[2].Width = 55;//PW Page
            dtGrid.Columns[3].Width = 55;//PWAddr
            dtGrid.Columns[4].Width = 65;//CDB PW
            dtGrid.Columns[5].Width = 50;//RevPage
            dtGrid.Columns[6].Width = 65;//RevRegAddr
            dtGrid.Columns[7].Width = 65;//RevLen
            dtGrid.Columns[8].Width = 55;//CDBType
            dtGrid.Columns[9].Width = 65;//CDBHeaderLen
            dtGrid.Columns[10].Width = 80;//CDBCMDTrigMode
            dtGrid.Columns[11].Width = 80;//CDBDataModel
            dtGrid.Columns[12].Width = 65;//CDBDataLen
            dtGrid.Columns[13].Width = 65;//MediaInterface
            dtGrid.Columns[14].Width = 50;//AFETrim
            dtGrid.Columns[15].Width = 50;//SCDREN
            dtGrid.Columns[16].Width = 65;//CDRParams
            dtGrid.Columns[17].Width = 80;//SCDRMulSel
            dtGrid.Columns[18].Width = 150;//BinRev
            dtGrid.Columns[19].Width = 150;//ModuleForm
            dtGrid.Columns[20].Width = 400;//APP
            dtGrid.Columns[21].Width = 500;//BinName
            //
            int iCol = 0;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.Name;//0
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.PWDevAddr;//1
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.PWPage;//2
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.PWAddr;//3
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.CDBPW;//4
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.RevPage;//5
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.RevRegAddr;//6
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.RevLen;//7
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.CDBType;//8
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.CDBHeaderLen;//9
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.CDBCMDTrigMode;//10
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.CDBDataModel;//11
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.CDBDataLen;//12
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.MediaInterface;//13
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.AFETrim;//14           
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.SCDREN;//15
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.CDRParams;//16
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.SCDRMulSel;//17
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.BinRev;//18
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.ModuleForm;//19
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.APPCode;//20
            iCol++;
            dtGrid.Columns[iCol].HeaderCell.Value = strings.BinName;//21

            // 禁止用户改变DataGridView1的所有列的列宽   
            dtGrid.AllowUserToResizeColumns = false;
            //禁止用户改变DataGridView1的所有行的行高   
            dtGrid.AllowUserToResizeRows = false;
            // 禁止用户改变列头的高度   
            dtGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            // 禁止用户改变行头的高度  
            dtGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //datagridview 列标题居中 
            dtGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //datagridview 行标题居中 
            dtGrid.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            for (int i = 0; i < dtGrid.RowCount; i++)
            {
                dtGrid.Rows[i].HeaderCell.Value = i.ToString();
                //dtGrid.Rows[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft;
                //dtGrid[0, i].Value = i;
                //dtGrid[1, i].Value = Dec2Hex(i, 2);
            }

            for (int i = 0; i < dtGrid.Columns.Count; i++)
            {
                dtGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;//禁止排列
                dtGrid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;//居中
                //dtGrid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;//自动宽度
            }
            //dtGrid.Columns[1].ReadOnly = true;//只读
        }
        private void RefreshGrid()
        {
            mclsFlashSetup.RefreshData();
            int iRowsNum = mclsFlashSetup.gmstrICType.Length;
            InitDataGrid(iRowsNum);
            mclsFlashSetup.ReadDataFromFile(ref dtGrid);
        }
      
        private bool GridPaste_Base(DataGridView DGridView, bool PasteSelAreaOnly)
        {
            if (DGridView.CurrentCell == null) return false;  // ' 当前单元格是否选择的判断

            int intStartRow = DGridView.CurrentCell.RowIndex;
            int intStartColumn = DGridView.CurrentCell.ColumnIndex;
            int MaxColumnNum = DGridView.Columns.Count - intStartColumn;// '最大粘贴列数

            if (PasteSelAreaOnly == false)// 'PasteSelAreaOnly=False：粘贴板中所有数据粘贴到单元格中
            {   //' 获取剪切板的内容，并按行分割
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText)) return false;
                pasteText = Regex.Replace(pasteText, @"\r\n", ",");
                string[] lines = pasteText.Split(',');
                foreach (string line in lines)
                {
                    if (intStartRow >= DGridView.Rows.Count) break;
                    string[] vals = line.Split('t');// ' 按 Tab 分割数据
                    if (MaxColumnNum > vals.Length) //'重新计算最大粘贴列数
                        MaxColumnNum = vals.Length;
                    for (int i = 0; i < MaxColumnNum; i++)//'UBound(vals) ' row.Cells.Count - 1
                        DGridView.Rows[intStartRow].Cells[i + intStartColumn].Value = vals[i];
                    intStartRow += 1;
                }
            }
            else //'PasteSelAreaOnly=True：只粘贴到选择的单元格中
            {
                int SelectColumnMax, SelectColumnMin;
                int SelectRowMax, SelectRowMin;
                int SelectNum;
                //'先找出选择的单元格起始终止位置
                SelectColumnMax = DGridView.CurrentCell.ColumnIndex;
                SelectColumnMin = DGridView.CurrentCell.ColumnIndex;
                SelectRowMax = DGridView.CurrentCell.RowIndex;
                SelectRowMin = DGridView.CurrentCell.RowIndex;
                foreach (DataGridViewCell c in DGridView.SelectedCells)
                {
                    if (SelectColumnMax < c.ColumnIndex)
                        SelectColumnMax = c.ColumnIndex;
                    if (SelectColumnMin > c.ColumnIndex)
                        SelectColumnMin = c.ColumnIndex;
                    if (SelectRowMax < c.RowIndex)
                        SelectRowMax = c.RowIndex;
                    if (SelectRowMin > c.RowIndex)
                        SelectRowMin = c.RowIndex;
                }
                SelectNum = DGridView.SelectedCells.Count;

                //' 获取剪切板的内容，并按行分割
                string pasteText = Clipboard.GetText();
                if (string.IsNullOrEmpty(pasteText)) return false;
                if (string.IsNullOrEmpty(pasteText)) return false;
                pasteText = Regex.Replace(pasteText, @"\r\n", ",");
                string[] lines = pasteText.Split(',');
                intStartRow = SelectRowMin;
                foreach (string line in lines)
                {
                    if (intStartRow > SelectRowMax || line.Equals("")) break;
                    string[] vals = line.Split('\t');// ' 按 Tab 分割数据
                    MaxColumnNum = SelectColumnMax - SelectColumnMin + 1;
                    if (MaxColumnNum > vals.Length) //'重新计算最大粘贴列数
                        MaxColumnNum = vals.Length;
                    intStartColumn = SelectColumnMin;
                    for (int i = 0; i < MaxColumnNum; i++) //'UBound(vals) ' row.Cells.Count - 1
                    {
                        DGridView.Rows[intStartRow].Cells[i + intStartColumn].Value = vals[i];
                    }
                    intStartRow += 1;
                }
            }
            return true;
        }
        // '将复制选择单元格区域内的数据
        private bool GridCopy(DataGridView DGridView)
        {
            try
            {
                System.Windows.Forms.Clipboard.Clear();
                DGridView.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
                Clipboard.SetDataObject(DGridView.GetClipboardContent().GetData(DataFormats.UnicodeText));
                return true;
            }
            catch
            {
                return false;
            }
        }
        //'将复制数据粘贴到以选择单元格开始的区域内。
        private bool GridPaste(DataGridView DGridView)
        {
            return GridPaste_Base(DGridView, true);
        }
        private void FrmFlashSetup_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            mclsFlashSetup.SaveDataToFile(dtGrid);
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtInsertRow_KeyPress(object sender, KeyPressEventArgs e)
        {
            int intRowNums = dtGrid.Rows.Count;
            int intColNums = dtGrid.ColumnCount;
            int intRowInsert = Convert.ToInt16(txtInsertRow.Text);
            string[,] strTemp = new string[intRowNums, intColNums];
            if (e.KeyChar == Convert.ToChar(13))//回车键
            {
                if (intRowInsert < 0)
                {
                    intRowInsert = 0;
                    txtInsertRow.Text = intRowInsert.ToString();
                }
                if (intRowInsert > intRowNums)
                {
                    intRowInsert = intRowNums;
                    txtInsertRow.Text = intRowInsert.ToString();
                }
                for (int i = 0; i < intRowNums; i++)
                {
                    for (int j = 0; j < intColNums; j++)
                    {
                        if (dtGrid[j, i].Value == null)
                        {
                            strTemp[i, j] = "0";
                        }
                        else
                        {
                            strTemp[i, j] = dtGrid[j, i].Value.ToString();//保存原数据
                        }
                    }
                }
                intRowNums++;
                string[,] strSetTemp = new string[intRowNums, intColNums];
                for (int i = 0; i < intRowInsert; i++)
                {
                    for (int j = 0; j < intColNums; j++)
                    {
                        strSetTemp[i, j] = strTemp[i, j];
                    }
                }
                for (int j = 0; j < intColNums; j++)
                {
                    strSetTemp[intRowInsert, j] = "0";
                }
                for (int i = intRowInsert; i < intRowNums - 1; i++)
                {
                    for (int j = 0; j < intColNums; j++)
                    {
                        strSetTemp[i + 1, j] = strTemp[i, j];
                    }
                }
                InitDataGrid(intRowNums);
                for (int i = 0; i < intRowNums; i++)
                {
                    for (int j = 0; j < intColNums; j++)
                    {
                        dtGrid[j, i].Value = strSetTemp[i, j];
                    }
                }
            }
        }
        private void txtDeleteRow_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))//回车键
            {
                if (dtGrid.Rows.Count < 1) return;
                int intInsertRow = Convert.ToInt16(txtDeleteRow.Text);
                if (intInsertRow > dtGrid.Rows.Count - 1)
                {
                    intInsertRow = dtGrid.Rows.Count - 1;
                    txtDeleteRow.Text = intInsertRow.ToString();
                }
                dtGrid.Rows.RemoveAt(intInsertRow);
                for (int i = 0; i < dtGrid.Rows.Count; i++)
                {
                    dtGrid.Rows[i].HeaderCell.Value = i.ToString();
                }
            }
        }
        private void dtGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(3))// 'Ctr+C复制数据
            {
                GridCopy(dtGrid);
            }
            else if (e.KeyChar == Convert.ToChar(22)) // 'Ctr+V粘贴数据
            {
                GridPaste(dtGrid);
            }
        }
    }
}
