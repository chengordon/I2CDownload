namespace I2CDownload
{
    partial class MProgressBar
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar_seting = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // progressBar_seting
            // 
            this.progressBar_seting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar_seting.Location = new System.Drawing.Point(0, 0);
            this.progressBar_seting.Name = "progressBar_seting";
            this.progressBar_seting.Size = new System.Drawing.Size(516, 34);
            this.progressBar_seting.Step = 1;
            this.progressBar_seting.TabIndex = 1;
            // 
            // MProgressBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.progressBar_seting);
            this.Name = "MProgressBar";
            this.Size = new System.Drawing.Size(516, 34);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar_seting;
    }
}
