namespace I2CDownload
{
    partial class FrmModuleSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmModuleSelect));
            this.listModuleType = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // listModuleType
            // 
            resources.ApplyResources(this.listModuleType, "listModuleType");
            this.listModuleType.FormattingEnabled = true;
            this.listModuleType.Name = "listModuleType";
            this.listModuleType.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listICType_MouseDoubleClick);
            // 
            // FrmModuleSelect
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listModuleType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmModuleSelect";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.FrmModuleSelect_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listModuleType;
    }
}