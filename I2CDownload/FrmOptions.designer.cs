namespace I2CDownload
{
    partial class FrmOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOptions));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.gb_languageSettings = new System.Windows.Forms.GroupBox();
            this.cmb_language = new System.Windows.Forms.ComboBox();
            this.lb_selectLanguage = new System.Windows.Forms.Label();
            this.chk_useSystemLanguage = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.gb_languageSettings.SuspendLayout();
            this.flowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.gb_languageSettings);
            resources.ApplyResources(this.tabGeneral, "tabGeneral");
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // gb_languageSettings
            // 
            this.gb_languageSettings.Controls.Add(this.cmb_language);
            this.gb_languageSettings.Controls.Add(this.lb_selectLanguage);
            this.gb_languageSettings.Controls.Add(this.chk_useSystemLanguage);
            resources.ApplyResources(this.gb_languageSettings, "gb_languageSettings");
            this.gb_languageSettings.Name = "gb_languageSettings";
            this.gb_languageSettings.TabStop = false;
            // 
            // cmb_language
            // 
            this.cmb_language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_language.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_language, "cmb_language");
            this.cmb_language.Name = "cmb_language";
            // 
            // lb_selectLanguage
            // 
            resources.ApplyResources(this.lb_selectLanguage, "lb_selectLanguage");
            this.lb_selectLanguage.Name = "lb_selectLanguage";
            // 
            // chk_useSystemLanguage
            // 
            resources.ApplyResources(this.chk_useSystemLanguage, "chk_useSystemLanguage");
            this.chk_useSystemLanguage.Name = "chk_useSystemLanguage";
            this.chk_useSystemLanguage.UseVisualStyleBackColor = true;
            this.chk_useSystemLanguage.CheckedChanged += new System.EventHandler(this.chk_useSystemLanguage_CheckedChanged);
            // 
            // flowLayoutPanel
            // 
            resources.ApplyResources(this.flowLayoutPanel, "flowLayoutPanel");
            this.flowLayoutPanel.Controls.Add(this.btn_Cancel);
            this.flowLayoutPanel.Controls.Add(this.btn_OK);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            // 
            // btn_Cancel
            // 
            resources.ApplyResources(this.btn_Cancel, "btn_Cancel");
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // btn_OK
            // 
            resources.ApplyResources(this.btn_OK, "btn_OK");
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // FrmOptions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel);
            this.Controls.Add(this.tabControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.gb_languageSettings.ResumeLayout(false);
            this.gb_languageSettings.PerformLayout();
            this.flowLayoutPanel.ResumeLayout(false);
            this.flowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.GroupBox gb_languageSettings;
        private System.Windows.Forms.CheckBox chk_useSystemLanguage;
        private System.Windows.Forms.Label lb_selectLanguage;
        private System.Windows.Forms.ComboBox cmb_language;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;
    }
}