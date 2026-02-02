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
    public partial class FrmPassword : Form
    {
        public ClsFlashSetupConfig mclsFlashSetup = null;

        public FrmPassword()
        {
            InitializeComponent();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))//回车键
            {
                if (mclsFlashSetup.CheckPassword(txtPassword.Text.Trim()))
                {
                    mclsFlashSetup.gbAccessPass = true;
                }
                else
                {
                    mclsFlashSetup.gbAccessPass = false;
                }
                this.Close();
            }
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (mclsFlashSetup.CheckPassword(txtPassword.Text.Trim()))
            {
                mclsFlashSetup.gbAccessPass = true;
            }
            else
            {
                mclsFlashSetup.gbAccessPass = false;
            }
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
