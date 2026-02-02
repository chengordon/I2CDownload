using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace I2CDownload
{
    public partial class IDOKMessageBox : Form
    {
        public Button buttonOK;
        private Button buttonNO;
        private Label labelMessage;
        private bool t;       

        //构造函数
        public IDOKMessageBox(string message)
        {
            InitializeComponent();
            //显示消息
            this.labelMessage.Text = message;
            ////初始化按钮的文本
            //this.buttonOK.Text = "OK";
        }        

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDOKMessageBox));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonNO = new System.Windows.Forms.Button();
            this.labelMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonNO
            // 
            resources.ApplyResources(this.buttonNO, "buttonNO");
            this.buttonNO.Name = "buttonNO";
            this.buttonNO.UseVisualStyleBackColor = true;
            this.buttonNO.Click += new System.EventHandler(this.buttonNO_Click);
            // 
            // labelMessage
            // 
            resources.ApplyResources(this.labelMessage, "labelMessage");
            this.labelMessage.Name = "labelMessage";
            // 
            // IDOKMessageBox
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.buttonNO);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IDOKMessageBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public void buttonOK_Click(object sender, EventArgs e)
        {
            //单击确定按钮，关闭对话框
            this.Close();
            t = true;
        }

        public void buttonNO_Click(object sender, EventArgs e)
        {
            //单击确定按钮，关闭对话框
            this.Close();
            t = false;
        }

        public bool GettValue()
        {
            return t;
        }
    }
}
