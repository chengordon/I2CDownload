namespace I2CDownload
{
    partial class FrmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblState = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblAuthor = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_DeviceList = new System.Windows.Forms.ComboBox();
            this.gb_Option = new System.Windows.Forms.GroupBox();
            this.chk_LowPower = new System.Windows.Forms.CheckBox();
            this.gb_TestResult = new System.Windows.Forms.GroupBox();
            this.bt_TestReport = new System.Windows.Forms.Button();
            this.pb_TestResult = new System.Windows.Forms.PictureBox();
            this.txt_Total = new System.Windows.Forms.TextBox();
            this.lab_Total = new System.Windows.Forms.Label();
            this.txt_Pass = new System.Windows.Forms.TextBox();
            this.lab_Pass = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuModifySetupPassword = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSwitch = new System.Windows.Forms.ToolStripMenuItem();
            this.gbCDBinfor = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.msglabel = new System.Windows.Forms.Label();
            this.gbProgressbar = new System.Windows.Forms.GroupBox();
            this.timerMon = new System.Windows.Forms.Timer(this.components);
            this.mProgressBar1 = new I2CDownload.MProgressBar();
            this.statusStrip1.SuspendLayout();
            this.gb_Option.SuspendLayout();
            this.gb_TestResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TestResult)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.gbCDBinfor.SuspendLayout();
            this.gbProgressbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblState,
            this.lblStatus,
            this.lblTime,
            this.lblAuthor});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // lblState
            // 
            this.lblState.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblState.Name = "lblState";
            resources.ApplyResources(this.lblState, "lblState");
            // 
            // lblStatus
            // 
            this.lblStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatus.Name = "lblStatus";
            resources.ApplyResources(this.lblStatus, "lblStatus");
            this.lblStatus.Spring = true;
            // 
            // lblTime
            // 
            this.lblTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblTime.Name = "lblTime";
            resources.ApplyResources(this.lblTime, "lblTime");
            // 
            // lblAuthor
            // 
            this.lblAuthor.Name = "lblAuthor";
            resources.ApplyResources(this.lblAuthor, "lblAuthor");
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmb_DeviceList
            // 
            resources.ApplyResources(this.cmb_DeviceList, "cmb_DeviceList");
            this.cmb_DeviceList.FormattingEnabled = true;
            this.cmb_DeviceList.Items.AddRange(new object[] {
            resources.GetString("cmb_DeviceList.Items"),
            resources.GetString("cmb_DeviceList.Items1"),
            resources.GetString("cmb_DeviceList.Items2")});
            this.cmb_DeviceList.Name = "cmb_DeviceList";
            // 
            // gb_Option
            // 
            this.gb_Option.Controls.Add(this.chk_LowPower);
            this.gb_Option.Controls.Add(this.cmb_DeviceList);
            this.gb_Option.Controls.Add(this.label1);
            resources.ApplyResources(this.gb_Option, "gb_Option");
            this.gb_Option.Name = "gb_Option";
            this.gb_Option.TabStop = false;
            // 
            // chk_LowPower
            // 
            resources.ApplyResources(this.chk_LowPower, "chk_LowPower");
            this.chk_LowPower.Name = "chk_LowPower";
            this.chk_LowPower.UseVisualStyleBackColor = true;
            this.chk_LowPower.CheckedChanged += new System.EventHandler(this.chk_LowPower_CheckedChanged);
            // 
            // gb_TestResult
            // 
            this.gb_TestResult.Controls.Add(this.bt_TestReport);
            this.gb_TestResult.Controls.Add(this.pb_TestResult);
            this.gb_TestResult.Controls.Add(this.txt_Total);
            this.gb_TestResult.Controls.Add(this.lab_Total);
            this.gb_TestResult.Controls.Add(this.txt_Pass);
            this.gb_TestResult.Controls.Add(this.lab_Pass);
            resources.ApplyResources(this.gb_TestResult, "gb_TestResult");
            this.gb_TestResult.Name = "gb_TestResult";
            this.gb_TestResult.TabStop = false;
            // 
            // bt_TestReport
            // 
            resources.ApplyResources(this.bt_TestReport, "bt_TestReport");
            this.bt_TestReport.Name = "bt_TestReport";
            this.bt_TestReport.UseVisualStyleBackColor = true;
            this.bt_TestReport.Click += new System.EventHandler(this.bt_TestReport_Click);
            // 
            // pb_TestResult
            // 
            this.pb_TestResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pb_TestResult, "pb_TestResult");
            this.pb_TestResult.Name = "pb_TestResult";
            this.pb_TestResult.TabStop = false;
            // 
            // txt_Total
            // 
            resources.ApplyResources(this.txt_Total, "txt_Total");
            this.txt_Total.Name = "txt_Total";
            this.txt_Total.ReadOnly = true;
            this.txt_Total.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt_Total_MouseDoubleClick);
            // 
            // lab_Total
            // 
            resources.ApplyResources(this.lab_Total, "lab_Total");
            this.lab_Total.Name = "lab_Total";
            // 
            // txt_Pass
            // 
            resources.ApplyResources(this.txt_Pass, "txt_Pass");
            this.txt_Pass.Name = "txt_Pass";
            this.txt_Pass.ReadOnly = true;
            this.txt_Pass.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txt_Pass_MouseDoubleClick);
            // 
            // lab_Pass
            // 
            resources.ApplyResources(this.lab_Pass, "lab_Pass");
            this.lab_Pass.Name = "lab_Pass";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetup,
            this.menuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // menuSetup
            // 
            this.menuSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSetup,
            this.toolStripSeparator1,
            this.menuModifySetupPassword,
            this.toolStripSeparator2,
            this.MenuOptions});
            this.menuSetup.Name = "menuSetup";
            resources.ApplyResources(this.menuSetup, "menuSetup");
            // 
            // menuItemSetup
            // 
            this.menuItemSetup.Name = "menuItemSetup";
            resources.ApplyResources(this.menuItemSetup, "menuItemSetup");
            this.menuItemSetup.Click += new System.EventHandler(this.menuItemSetup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // menuModifySetupPassword
            // 
            this.menuModifySetupPassword.Name = "menuModifySetupPassword";
            resources.ApplyResources(this.menuModifySetupPassword, "menuModifySetupPassword");
            this.menuModifySetupPassword.Click += new System.EventHandler(this.menuModifySetupPassword_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // MenuOptions
            // 
            this.MenuOptions.Name = "MenuOptions";
            resources.ApplyResources(this.MenuOptions, "MenuOptions");
            this.MenuOptions.Click += new System.EventHandler(this.MenuOptions_Click);
            // 
            // menuItem
            // 
            this.menuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSwitch});
            this.menuItem.Name = "menuItem";
            resources.ApplyResources(this.menuItem, "menuItem");
            // 
            // menuItemSwitch
            // 
            this.menuItemSwitch.Name = "menuItemSwitch";
            resources.ApplyResources(this.menuItemSwitch, "menuItemSwitch");
            this.menuItemSwitch.Click += new System.EventHandler(this.menuItemSwitch_Click);
            // 
            // gbCDBinfor
            // 
            this.gbCDBinfor.Controls.Add(this.label4);
            resources.ApplyResources(this.gbCDBinfor, "gbCDBinfor");
            this.gbCDBinfor.Name = "gbCDBinfor";
            this.gbCDBinfor.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // msglabel
            // 
            this.msglabel.BackColor = System.Drawing.SystemColors.ControlText;
            this.msglabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.msglabel, "msglabel");
            this.msglabel.ForeColor = System.Drawing.Color.Green;
            this.msglabel.Name = "msglabel";
            // 
            // gbProgressbar
            // 
            this.gbProgressbar.Controls.Add(this.mProgressBar1);
            resources.ApplyResources(this.gbProgressbar, "gbProgressbar");
            this.gbProgressbar.Name = "gbProgressbar";
            this.gbProgressbar.TabStop = false;
            // 
            // timerMon
            // 
            this.timerMon.Interval = 600;
            this.timerMon.Tick += new System.EventHandler(this.timerMon_Tick);
            // 
            // mProgressBar1
            // 
            resources.ApplyResources(this.mProgressBar1, "mProgressBar1");
            this.mProgressBar1.Maximum = 100;
            this.mProgressBar1.Minimum = 0;
            this.mProgressBar1.Name = "mProgressBar1";
            this.mProgressBar1.Value = 0;
            // 
            // FrmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.msglabel);
            this.Controls.Add(this.gbProgressbar);
            this.Controls.Add(this.gbCDBinfor);
            this.Controls.Add(this.gb_TestResult);
            this.Controls.Add(this.gb_Option);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gb_Option.ResumeLayout(false);
            this.gb_TestResult.ResumeLayout(false);
            this.gb_TestResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TestResult)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbCDBinfor.ResumeLayout(false);
            this.gbCDBinfor.PerformLayout();
            this.gbProgressbar.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel lblState;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblTime;
        private System.Windows.Forms.ToolStripStatusLabel lblAuthor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_DeviceList;
        private System.Windows.Forms.GroupBox gb_Option;
        private System.Windows.Forms.GroupBox gb_TestResult;
        private System.Windows.Forms.PictureBox pb_TestResult;
        private System.Windows.Forms.Button bt_TestReport;
        private System.Windows.Forms.Label lab_Pass;
        private System.Windows.Forms.TextBox txt_Total;
        private System.Windows.Forms.Label lab_Total;
        private System.Windows.Forms.TextBox txt_Pass;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuSetup;
        private System.Windows.Forms.ToolStripMenuItem menuItemSetup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuModifySetupPassword;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem MenuOptions;
        private System.Windows.Forms.ToolStripMenuItem menuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemSwitch;
        private System.Windows.Forms.GroupBox gbCDBinfor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label msglabel;
        private System.Windows.Forms.GroupBox gbProgressbar;
        private System.Windows.Forms.Timer timerMon;
        private MProgressBar mProgressBar1;
        private System.Windows.Forms.CheckBox chk_LowPower;
    }
}

