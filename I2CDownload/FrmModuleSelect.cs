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
    public partial class FrmModuleSelect : Form
    {
        public ClsFlashSetupConfig mclsFlashConfig = null;

        public FrmModuleSelect()
        {
            InitializeComponent();
        }

        private void FrmModuleSelect_Load(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            listModuleType.Items.Clear();

            try
            {
                mclsFlashConfig.RefreshData();
                string[] ColumnName = mclsFlashConfig.gmstrICType;// GetICTypes();

                foreach (string str in ColumnName)
                {
                    listModuleType.Items.Add(str);
                    if (str.Equals(mclsFlashConfig.strModuleTypeSel))
                    {
                        listModuleType.SelectedIndex = listModuleType.Items.Count - 1;
                    }
                }
            }
            catch
            {
                return;
            }           
        }         
       
        private void listICType_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            mclsFlashConfig.strModuleTypeSel = listModuleType.SelectedItem.ToString();
            this.Close();
        }
    }
}
