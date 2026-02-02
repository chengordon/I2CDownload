using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace I2CDownload
{
    public partial class FrmReport : Form
    {        
        
        //AutoSizeForm asc = new AutoSizeForm();
        ClsImportExportData mclsImportExportData = new ClsImportExportData();        
        public CDatabase cdaba = null;
        public FrmReport()
        {
            InitializeComponent();
        }
        public string GetTime()
        {
            DateTime time = System.DateTime.Now;
            int hour = time.Hour;
            int h1 = 0;
            int h2 = hour;
            if (hour >= 10)
            {
                h1 = hour / 10;
                h2 = hour % 10;
            }

            int minute = time.Minute;
            int m1 = 0;
            int m2 = minute;
            if (minute >= 10)
            {
                m1 = minute / 10;
                m2 = minute % 10;
            }

            int second = time.Second;
            int s1 = 0;
            int s2 = second;
            if (second >= 10)
            {
                s1 = second / 10;
                s2 = second % 10;
            }

            string str = string.Format("{0}{1}:{2}{3}:{4}{5}", h1, h2, m1, m2, s1, s2);
            return str;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            tsTime.Text = "Time: " + GetTime();
        } 
        private void FrmReport_Load(object sender, EventArgs e)
        {
            //asc.controllInitializeSize(this);
            tsTime.Text = "Time: " + GetTime();
            this.Text = System.Windows.Forms.Application.ProductName + " TestReport V" + System.Windows.Forms.Application.ProductVersion;
            dtp_startDate.Value = System.DateTime.Now.Date.AddDays(-1);
            dtp_endDate.Value = System.DateTime.Now.Date;
        }

        //private void FrmReport_SizeChanged(object sender, EventArgs e)
        //{
        //    asc.controlAutoSize(this);
        //} 
        private void cmb_Adapter_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            cmb_Adapter.Items.Clear();
            cmb_Adapter.Items.AddRange(new object[] { "" });

            cdaba.DatabaseParam.SqlStr = "select AdapterSN from " + cdaba.DatabaseParam.TabName;
            DataSet ds = cdaba.getDataSet(cdaba.DatabaseParam.SqlStr, cdaba.DatabaseParam.TabName, cdaba.DatabaseParam.DBName);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!list.Contains(row[0].ToString()))
                {
                    list.Add(row[0].ToString());
                }

            }

            string[] ColumnName = list.ToArray();

            foreach (string str in ColumnName)
            {
                cmb_Adapter.Items.Add(str);
            }
        }
        private void cmb_Type_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            cmb_Type.Items.Clear();
            cmb_Type.Items.AddRange(new object[] { "" });

            cdaba.DatabaseParam.SqlStr = "select Type from " + cdaba.DatabaseParam.TabName;
            DataSet ds = cdaba.getDataSet(cdaba.DatabaseParam.SqlStr, cdaba.DatabaseParam.TabName, cdaba.DatabaseParam.DBName);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!list.Contains(row[0].ToString()))
                {
                    list.Add(row[0].ToString());
                }

            }

            string[] ColumnName = list.ToArray();

            foreach (string str in ColumnName)
            {
                cmb_Type.Items.Add(str);
            }
        }
        private void cmb_State_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            cmb_State.Items.Clear();
            cmb_State.Items.AddRange(new object[] { "" });

            cdaba.DatabaseParam.SqlStr = "select State from " + cdaba.DatabaseParam.TabName;
            DataSet ds = cdaba.getDataSet(cdaba.DatabaseParam.SqlStr, cdaba.DatabaseParam.TabName, cdaba.DatabaseParam.DBName);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!list.Contains(row[0].ToString()))
                {
                    list.Add(row[0].ToString());
                }

            }

            string[] ColumnName = list.ToArray();

            foreach (string str in ColumnName)
            {
                cmb_State.Items.Add(str);
            }
        }
        private void cmb_BinFile_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            cmb_BinFile.Items.Clear();
            cmb_BinFile.Items.AddRange(new object[] { "" });

            cdaba.DatabaseParam.SqlStr = "select BinFile from " + cdaba.DatabaseParam.TabName;
            DataSet ds = cdaba.getDataSet(cdaba.DatabaseParam.SqlStr, cdaba.DatabaseParam.TabName, cdaba.DatabaseParam.DBName);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (!list.Contains(row[0].ToString()))
                {
                    list.Add(row[0].ToString());
                }

            }

            string[] ColumnName = list.ToArray();

            foreach (string str in ColumnName)
            {
                cmb_BinFile.Items.Add(str);
            }
        }
        private void bt_Search_Click(object sender, EventArgs e)
        {
            string strTemp = "";
            dataGridView1.DataSource = null;

            string dateStart = dtp_startDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string dateEnd = dtp_endDate.Value.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss");

            string cmdstr = "select * from " + cdaba.DatabaseParam.TabName + " where Date between '" + dateStart + "' and '" + dateEnd + "'";
            if (cmb_Adapter.SelectedIndex > 0)
            {                
                strTemp = cmb_Adapter.SelectedItem.ToString();
                if (!strTemp.Equals(""))
                {
                    cmdstr += " and AdapterSN ='" + strTemp + "'";
                }
            }

            if (cmb_Type.SelectedIndex > 0)
            {
                strTemp = cmb_Type.SelectedItem.ToString();
                if (!strTemp.Equals(""))
                {
                    cmdstr += " and Type ='" + strTemp + "'";
                }
            }

            if (cmb_State.SelectedIndex > 0)
            {
                strTemp = cmb_State.SelectedItem.ToString();
                if (!strTemp.Equals(""))
                {
                    cmdstr += " and State ='" + strTemp + "'";
                }
            }

            if (cmb_BinFile.SelectedIndex > 0)
            {
                strTemp = cmb_BinFile.SelectedItem.ToString();
                if (!strTemp.Equals(""))
                {
                    cmdstr += " and BinFile ='" + strTemp + "'";
                }
            }           

            cdaba.DatabaseParam.SqlStr = cmdstr;
            DataSet ds = cdaba.getDataSet(cdaba.DatabaseParam.SqlStr, cdaba.DatabaseParam.TabName, cdaba.DatabaseParam.DBName);
            dataGridView1.DataSource = ds.Tables[0];
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            } 
        }
        private void bt_Export_Click(object sender, EventArgs e)
        {
            string strPath = mclsImportExportData.GetSaveFilePath();             
            if (strPath.Equals("")) return;
            if (strPath.Substring(strPath.LastIndexOf('.')).ToLower().Equals(".xlsx"))
            {
                if (mclsImportExportData.ExportToExcel(dataGridView1, strPath, "Records"))
                {
                    lblState.Text = "Successfully Exported(xlsx)";
                }
                else
                {
                    lblState.Text = "Export Failed(xlsx)";
                }
            }
            else if (strPath.Substring(strPath.LastIndexOf('.')).ToLower().Equals(".xls"))
            {
                if (mclsImportExportData.ExportToExcel(dataGridView1, strPath, "Records"))
                {
                    lblState.Text = "Successfully Exported(xls)";
                }
                else
                {
                    lblState.Text = "Export Failed(xls)";
                }
            }            
        }

    }
}
