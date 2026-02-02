using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace I2CDownload
{
    public partial class FrmPasswordModify : Form
    {
        public ClsFlashSetupConfig clFlashSetup = null;       

        public FrmPasswordModify()
        {
            InitializeComponent();
        }

        private void txtNewPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))//回车键
            {
                clFlashSetup.ModifyPassword(txtNewPassword.Text.Trim());
                this.Close();
            }
        }
        
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(13))//回车键
            {
                if (clFlashSetup.CheckPassword(txtPassword.Text.Trim()))
                {
                    txtNewPassword.Enabled = true;
                    btnOk.Enabled = true;
                }
                else
                {
                    txtNewPassword.Enabled = false;
                    btnOk.Enabled = false;
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            clFlashSetup.ModifyPassword(txtNewPassword.Text.Trim());
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
