using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using I2CDownload.Properties;

namespace I2CDownload
{
    public partial class FrmOptions : Form
    {

        bool useSystemLanguage;

        public bool UseSystemLanguage
        {
            get { return useSystemLanguage; }
            set { useSystemLanguage = value; }
        }

        public FrmOptions()
        {
            InitializeComponent();
            useSystemLanguage = Settings.Default.UseSystemLanguage;
            chk_useSystemLanguage.DataBindings.Add("Checked", this, "UseSystemLanguage");

            if (string.IsNullOrEmpty(Settings.Default.SelectedLanguage))
                Settings.Default.SelectedLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));
            dt.Rows.Add(strings.English, "en");
            dt.Rows.Add(strings.SimplifiedChinese, "zh-CN");
            dt.DefaultView.Sort = "Name";

            cmb_language.DataSource = dt.DefaultView;
            cmb_language.DisplayMember = "Name";
            cmb_language.ValueMember = "Value";
            cmb_language.SelectedValue = Settings.Default.SelectedLanguage;
            if (cmb_language.SelectedIndex == -1)
                cmb_language.SelectedIndex = 0;
        }


        private void btn_OK_Click(object sender, EventArgs e)
        {
            bool changed = false;

            if (Settings.Default.UseSystemLanguage != this.useSystemLanguage ||
                Settings.Default.SelectedLanguage != (string)cmb_language.SelectedValue)
            {
                Settings.Default.UseSystemLanguage = this.UseSystemLanguage;
                Settings.Default.SelectedLanguage = (string)cmb_language.SelectedValue;

                Program.ShowMessage(strings.ProgramRestartSettings);

                changed = true;
            }

            if (changed)
                Settings.Default.Save();

            this.DialogResult = DialogResult.OK;
        }

        private void chk_useSystemLanguage_CheckedChanged(object sender, EventArgs e)
        {
            cmb_language.Enabled = lb_selectLanguage.Enabled = !chk_useSystemLanguage.Checked;
        }
    }
}
