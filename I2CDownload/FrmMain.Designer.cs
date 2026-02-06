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
            this.tsTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsAuthor = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.comboUSBDriver = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_Connect = new System.Windows.Forms.Button();
            this.gb_Filename = new System.Windows.Forms.GroupBox();
            this.bt_Browse = new System.Windows.Forms.Button();
            this.txt_HexFileName = new System.Windows.Forms.TextBox();
            this.bt_Download = new System.Windows.Forms.Button();
            this.btn_TBPowerOff = new System.Windows.Forms.Button();
            this.btn_TBPowerOn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.CommitImage = new System.Windows.Forms.Label();
            this.RunImage = new System.Windows.Forms.Label();
            this.ImageBVer = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ImageAVer = new System.Windows.Forms.Label();
            this.btn_GetStatus = new System.Windows.Forms.Button();
            this.btn_CopyImageB2A = new System.Windows.Forms.Button();
            this.btn_CopyImageA2B = new System.Windows.Forms.Button();
            this.btn_CommitImage = new System.Windows.Forms.Button();
            this.btn_RunImage = new System.Windows.Forms.Button();
            this.gb_ReadImage = new System.Windows.Forms.GroupBox();
            this.btn_ExportImage = new System.Windows.Forms.Button();
            this.btn_ReadEPL = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btn_ReadLPL = new System.Windows.Forms.Button();
            this.gb_Function = new System.Windows.Forms.GroupBox();
            this.btn_WriteEPL = new System.Windows.Forms.Button();
            this.txt_HexFile = new System.Windows.Forms.TextBox();
            this.btn_WriteLPL = new System.Windows.Forms.Button();
            this.gb_BinFile = new System.Windows.Forms.GroupBox();
            this.txtDeviceAddr = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLPLSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.rTxt_BinFile = new System.Windows.Forms.RichTextBox();
            this.btn_SaveDatBin = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btn_LoadDatBin = new System.Windows.Forms.Button();
            this.cmbBoxDisplayLength = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbBoxDisplayLineLen = new System.Windows.Forms.ComboBox();
            this.btn_AbortDownload = new System.Windows.Forms.Button();
            this.btnReadImage = new System.Windows.Forms.Button();
            this.btnReadSPI = new System.Windows.Forms.Button();
            this.btnWriteSPI = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.gb_Filename.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gb_ReadImage.SuspendLayout();
            this.gb_Function.SuspendLayout();
            this.gb_BinFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblState,
            this.lblStatus,
            this.tsTime,
            this.tsAuthor});
            this.statusStrip1.Location = new System.Drawing.Point(0, 554);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1182, 29);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblState
            // 
            this.lblState.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(69, 24);
            this.lblState.Text = "lblState";
            // 
            // lblStatus
            // 
            this.lblStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(904, 24);
            this.lblStatus.Spring = true;
            // 
            // tsTime
            // 
            this.tsTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsTime.Name = "tsTime";
            this.tsTime.Size = new System.Drawing.Size(125, 24);
            this.tsTime.Text = "Time: hh.mm.ss";
            // 
            // tsAuthor
            // 
            this.tsAuthor.Name = "tsAuthor";
            this.tsAuthor.Size = new System.Drawing.Size(64, 24);
            this.tsAuthor.Text = "by CGN";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // comboUSBDriver
            // 
            this.comboUSBDriver.FormattingEnabled = true;
            this.comboUSBDriver.Location = new System.Drawing.Point(619, 59);
            this.comboUSBDriver.Margin = new System.Windows.Forms.Padding(4);
            this.comboUSBDriver.Name = "comboUSBDriver";
            this.comboUSBDriver.Size = new System.Drawing.Size(121, 23);
            this.comboUSBDriver.TabIndex = 6;
            this.comboUSBDriver.Text = "C8051F340";
            this.comboUSBDriver.SelectedIndexChanged += new System.EventHandler(this.comboUSBDriver_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(619, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "USB Driver DLL";
            // 
            // bt_Connect
            // 
            this.bt_Connect.Location = new System.Drawing.Point(755, 33);
            this.bt_Connect.Margin = new System.Windows.Forms.Padding(4);
            this.bt_Connect.Name = "bt_Connect";
            this.bt_Connect.Size = new System.Drawing.Size(99, 44);
            this.bt_Connect.TabIndex = 4;
            this.bt_Connect.Text = "Connect";
            this.bt_Connect.UseVisualStyleBackColor = true;
            this.bt_Connect.Click += new System.EventHandler(this.bt_Connect_Click);
            // 
            // gb_Filename
            // 
            this.gb_Filename.Controls.Add(this.bt_Browse);
            this.gb_Filename.Controls.Add(this.txt_HexFileName);
            this.gb_Filename.Location = new System.Drawing.Point(0, 6);
            this.gb_Filename.Margin = new System.Windows.Forms.Padding(4);
            this.gb_Filename.Name = "gb_Filename";
            this.gb_Filename.Padding = new System.Windows.Forms.Padding(4);
            this.gb_Filename.Size = new System.Drawing.Size(607, 92);
            this.gb_Filename.TabIndex = 4;
            this.gb_Filename.TabStop = false;
            this.gb_Filename.Text = "CDB Filename";
            // 
            // bt_Browse
            // 
            this.bt_Browse.Location = new System.Drawing.Point(489, 24);
            this.bt_Browse.Margin = new System.Windows.Forms.Padding(4);
            this.bt_Browse.Name = "bt_Browse";
            this.bt_Browse.Size = new System.Drawing.Size(100, 58);
            this.bt_Browse.TabIndex = 1;
            this.bt_Browse.Text = "Browse";
            this.bt_Browse.UseVisualStyleBackColor = true;
            this.bt_Browse.Click += new System.EventHandler(this.bt_Browse_Click);
            // 
            // txt_HexFileName
            // 
            this.txt_HexFileName.Location = new System.Drawing.Point(11, 24);
            this.txt_HexFileName.Margin = new System.Windows.Forms.Padding(4);
            this.txt_HexFileName.Multiline = true;
            this.txt_HexFileName.Name = "txt_HexFileName";
            this.txt_HexFileName.ReadOnly = true;
            this.txt_HexFileName.Size = new System.Drawing.Size(463, 56);
            this.txt_HexFileName.TabIndex = 0;
            // 
            // bt_Download
            // 
            this.bt_Download.Location = new System.Drawing.Point(969, 57);
            this.bt_Download.Margin = new System.Windows.Forms.Padding(4);
            this.bt_Download.Name = "bt_Download";
            this.bt_Download.Size = new System.Drawing.Size(99, 44);
            this.bt_Download.TabIndex = 3;
            this.bt_Download.Text = "WriteEPP";
            this.bt_Download.UseVisualStyleBackColor = true;
            this.bt_Download.Click += new System.EventHandler(this.bt_Download_Click);
            // 
            // btn_TBPowerOff
            // 
            this.btn_TBPowerOff.Location = new System.Drawing.Point(863, 57);
            this.btn_TBPowerOff.Margin = new System.Windows.Forms.Padding(4);
            this.btn_TBPowerOff.Name = "btn_TBPowerOff";
            this.btn_TBPowerOff.Size = new System.Drawing.Size(99, 44);
            this.btn_TBPowerOff.TabIndex = 36;
            this.btn_TBPowerOff.Text = "Power Off";
            this.btn_TBPowerOff.UseVisualStyleBackColor = true;
            this.btn_TBPowerOff.Click += new System.EventHandler(this.btn_TBPowerOff_Click);
            // 
            // btn_TBPowerOn
            // 
            this.btn_TBPowerOn.Location = new System.Drawing.Point(863, 8);
            this.btn_TBPowerOn.Margin = new System.Windows.Forms.Padding(4);
            this.btn_TBPowerOn.Name = "btn_TBPowerOn";
            this.btn_TBPowerOn.Size = new System.Drawing.Size(99, 44);
            this.btn_TBPowerOn.TabIndex = 35;
            this.btn_TBPowerOn.Text = "Power On";
            this.btn_TBPowerOn.UseVisualStyleBackColor = true;
            this.btn_TBPowerOn.Click += new System.EventHandler(this.btn_TBPowerOn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.CommitImage);
            this.groupBox2.Controls.Add(this.RunImage);
            this.groupBox2.Controls.Add(this.ImageBVer);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.ImageAVer);
            this.groupBox2.Controls.Add(this.btn_GetStatus);
            this.groupBox2.Controls.Add(this.btn_CopyImageB2A);
            this.groupBox2.Controls.Add(this.btn_CopyImageA2B);
            this.groupBox2.Controls.Add(this.btn_CommitImage);
            this.groupBox2.Controls.Add(this.btn_RunImage);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(561, 101);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(610, 159);
            this.groupBox2.TabIndex = 39;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "switch to the new firmware image";
            // 
            // CommitImage
            // 
            this.CommitImage.AutoSize = true;
            this.CommitImage.Location = new System.Drawing.Point(518, 137);
            this.CommitImage.Name = "CommitImage";
            this.CommitImage.Size = new System.Drawing.Size(0, 15);
            this.CommitImage.TabIndex = 41;
            // 
            // RunImage
            // 
            this.RunImage.AutoSize = true;
            this.RunImage.Location = new System.Drawing.Point(518, 115);
            this.RunImage.Name = "RunImage";
            this.RunImage.Size = new System.Drawing.Size(0, 15);
            this.RunImage.TabIndex = 40;
            // 
            // ImageBVer
            // 
            this.ImageBVer.AutoSize = true;
            this.ImageBVer.Location = new System.Drawing.Point(502, 93);
            this.ImageBVer.Name = "ImageBVer";
            this.ImageBVer.Size = new System.Drawing.Size(0, 15);
            this.ImageBVer.TabIndex = 39;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(384, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 15);
            this.label6.TabIndex = 38;
            this.label6.Text = "Commit Image:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(384, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 15);
            this.label5.TabIndex = 37;
            this.label5.Text = "Running Image:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(384, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 15);
            this.label4.TabIndex = 36;
            this.label4.Text = "ImageB Ver:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(384, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 15);
            this.label1.TabIndex = 35;
            this.label1.Text = "ImageA Ver:";
            // 
            // ImageAVer
            // 
            this.ImageAVer.AutoSize = true;
            this.ImageAVer.Location = new System.Drawing.Point(501, 74);
            this.ImageAVer.Name = "ImageAVer";
            this.ImageAVer.Size = new System.Drawing.Size(0, 15);
            this.ImageAVer.TabIndex = 34;
            // 
            // btn_GetStatus
            // 
            this.btn_GetStatus.Location = new System.Drawing.Point(387, 23);
            this.btn_GetStatus.Margin = new System.Windows.Forms.Padding(4);
            this.btn_GetStatus.Name = "btn_GetStatus";
            this.btn_GetStatus.Size = new System.Drawing.Size(175, 44);
            this.btn_GetStatus.TabIndex = 33;
            this.btn_GetStatus.Text = "Get Status";
            this.btn_GetStatus.UseVisualStyleBackColor = true;
            this.btn_GetStatus.Click += new System.EventHandler(this.btn_GetStatus_Click);
            // 
            // btn_CopyImageB2A
            // 
            this.btn_CopyImageB2A.Location = new System.Drawing.Point(8, 104);
            this.btn_CopyImageB2A.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CopyImageB2A.Name = "btn_CopyImageB2A";
            this.btn_CopyImageB2A.Size = new System.Drawing.Size(184, 44);
            this.btn_CopyImageB2A.TabIndex = 32;
            this.btn_CopyImageB2A.Text = "Copy Image B to A";
            this.btn_CopyImageB2A.UseVisualStyleBackColor = true;
            this.btn_CopyImageB2A.Click += new System.EventHandler(this.btn_CopyImageB2A_Click);
            // 
            // btn_CopyImageA2B
            // 
            this.btn_CopyImageA2B.Location = new System.Drawing.Point(8, 26);
            this.btn_CopyImageA2B.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CopyImageA2B.Name = "btn_CopyImageA2B";
            this.btn_CopyImageA2B.Size = new System.Drawing.Size(184, 44);
            this.btn_CopyImageA2B.TabIndex = 31;
            this.btn_CopyImageA2B.Text = "Copy Image A to B";
            this.btn_CopyImageA2B.UseVisualStyleBackColor = true;
            this.btn_CopyImageA2B.Click += new System.EventHandler(this.btn_CopyImageA2B_Click);
            // 
            // btn_CommitImage
            // 
            this.btn_CommitImage.Location = new System.Drawing.Point(214, 104);
            this.btn_CommitImage.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CommitImage.Name = "btn_CommitImage";
            this.btn_CommitImage.Size = new System.Drawing.Size(148, 44);
            this.btn_CommitImage.TabIndex = 9;
            this.btn_CommitImage.Text = "Commit Image";
            this.btn_CommitImage.UseVisualStyleBackColor = true;
            this.btn_CommitImage.Click += new System.EventHandler(this.btn_CommitImage_Click);
            // 
            // btn_RunImage
            // 
            this.btn_RunImage.Location = new System.Drawing.Point(214, 26);
            this.btn_RunImage.Margin = new System.Windows.Forms.Padding(4);
            this.btn_RunImage.Name = "btn_RunImage";
            this.btn_RunImage.Size = new System.Drawing.Size(148, 44);
            this.btn_RunImage.TabIndex = 9;
            this.btn_RunImage.Text = "Run Image";
            this.btn_RunImage.UseVisualStyleBackColor = true;
            this.btn_RunImage.Click += new System.EventHandler(this.btn_RunImage_Click);
            // 
            // gb_ReadImage
            // 
            this.gb_ReadImage.Controls.Add(this.btn_ExportImage);
            this.gb_ReadImage.Controls.Add(this.btn_ReadEPL);
            this.gb_ReadImage.Controls.Add(this.textBox2);
            this.gb_ReadImage.Controls.Add(this.btn_ReadLPL);
            this.gb_ReadImage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gb_ReadImage.Location = new System.Drawing.Point(14, 179);
            this.gb_ReadImage.Margin = new System.Windows.Forms.Padding(4);
            this.gb_ReadImage.Name = "gb_ReadImage";
            this.gb_ReadImage.Padding = new System.Windows.Forms.Padding(4);
            this.gb_ReadImage.Size = new System.Drawing.Size(539, 78);
            this.gb_ReadImage.TabIndex = 38;
            this.gb_ReadImage.TabStop = false;
            this.gb_ReadImage.Text = "Read latest downloaded firmware image";
            // 
            // btn_ExportImage
            // 
            this.btn_ExportImage.Location = new System.Drawing.Point(370, 26);
            this.btn_ExportImage.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ExportImage.Name = "btn_ExportImage";
            this.btn_ExportImage.Size = new System.Drawing.Size(153, 44);
            this.btn_ExportImage.TabIndex = 27;
            this.btn_ExportImage.Text = "Export Image";
            this.btn_ExportImage.UseVisualStyleBackColor = true;
            this.btn_ExportImage.Click += new System.EventHandler(this.btn_ExportImage_Click);
            // 
            // btn_ReadEPL
            // 
            this.btn_ReadEPL.Location = new System.Drawing.Point(187, 26);
            this.btn_ReadEPL.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ReadEPL.Name = "btn_ReadEPL";
            this.btn_ReadEPL.Size = new System.Drawing.Size(153, 44);
            this.btn_ReadEPL.TabIndex = 32;
            this.btn_ReadEPL.Text = "Read Image EPL";
            this.btn_ReadEPL.UseVisualStyleBackColor = true;
            this.btn_ReadEPL.Click += new System.EventHandler(this.btn_ReadEPL_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(346, 152);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(232, 140);
            this.textBox2.TabIndex = 7;
            // 
            // btn_ReadLPL
            // 
            this.btn_ReadLPL.Location = new System.Drawing.Point(8, 26);
            this.btn_ReadLPL.Margin = new System.Windows.Forms.Padding(4);
            this.btn_ReadLPL.Name = "btn_ReadLPL";
            this.btn_ReadLPL.Size = new System.Drawing.Size(153, 44);
            this.btn_ReadLPL.TabIndex = 9;
            this.btn_ReadLPL.Text = "Read Image LPL";
            this.btn_ReadLPL.UseVisualStyleBackColor = true;
            this.btn_ReadLPL.Click += new System.EventHandler(this.btn_ReadLPL_Click);
            // 
            // gb_Function
            // 
            this.gb_Function.Controls.Add(this.btn_WriteEPL);
            this.gb_Function.Controls.Add(this.txt_HexFile);
            this.gb_Function.Controls.Add(this.btn_WriteLPL);
            this.gb_Function.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gb_Function.Location = new System.Drawing.Point(14, 101);
            this.gb_Function.Margin = new System.Windows.Forms.Padding(4);
            this.gb_Function.Name = "gb_Function";
            this.gb_Function.Padding = new System.Windows.Forms.Padding(4);
            this.gb_Function.Size = new System.Drawing.Size(355, 76);
            this.gb_Function.TabIndex = 37;
            this.gb_Function.TabStop = false;
            this.gb_Function.Text = "Download new firmware image";
            // 
            // btn_WriteEPL
            // 
            this.btn_WriteEPL.Location = new System.Drawing.Point(187, 26);
            this.btn_WriteEPL.Margin = new System.Windows.Forms.Padding(4);
            this.btn_WriteEPL.Name = "btn_WriteEPL";
            this.btn_WriteEPL.Size = new System.Drawing.Size(156, 44);
            this.btn_WriteEPL.TabIndex = 31;
            this.btn_WriteEPL.Text = "Program EPL";
            this.btn_WriteEPL.UseVisualStyleBackColor = true;
            this.btn_WriteEPL.Click += new System.EventHandler(this.btn_WriteEPL_Click);
            // 
            // txt_HexFile
            // 
            this.txt_HexFile.Location = new System.Drawing.Point(346, 152);
            this.txt_HexFile.Margin = new System.Windows.Forms.Padding(4);
            this.txt_HexFile.Multiline = true;
            this.txt_HexFile.Name = "txt_HexFile";
            this.txt_HexFile.ReadOnly = true;
            this.txt_HexFile.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_HexFile.Size = new System.Drawing.Size(232, 140);
            this.txt_HexFile.TabIndex = 7;
            // 
            // btn_WriteLPL
            // 
            this.btn_WriteLPL.Location = new System.Drawing.Point(8, 26);
            this.btn_WriteLPL.Margin = new System.Windows.Forms.Padding(4);
            this.btn_WriteLPL.Name = "btn_WriteLPL";
            this.btn_WriteLPL.Size = new System.Drawing.Size(153, 44);
            this.btn_WriteLPL.TabIndex = 9;
            this.btn_WriteLPL.Text = "Program LPL";
            this.btn_WriteLPL.UseVisualStyleBackColor = true;
            this.btn_WriteLPL.Click += new System.EventHandler(this.btn_WriteLPL_Click);
            // 
            // gb_BinFile
            // 
            this.gb_BinFile.Controls.Add(this.txtDeviceAddr);
            this.gb_BinFile.Controls.Add(this.label9);
            this.gb_BinFile.Controls.Add(this.txtLPLSize);
            this.gb_BinFile.Controls.Add(this.label8);
            this.gb_BinFile.Controls.Add(this.rTxt_BinFile);
            this.gb_BinFile.Controls.Add(this.btn_SaveDatBin);
            this.gb_BinFile.Controls.Add(this.textBox1);
            this.gb_BinFile.Controls.Add(this.btn_LoadDatBin);
            this.gb_BinFile.Controls.Add(this.cmbBoxDisplayLength);
            this.gb_BinFile.Controls.Add(this.label2);
            this.gb_BinFile.Controls.Add(this.label7);
            this.gb_BinFile.Controls.Add(this.cmbBoxDisplayLineLen);
            this.gb_BinFile.Location = new System.Drawing.Point(16, 270);
            this.gb_BinFile.Name = "gb_BinFile";
            this.gb_BinFile.Size = new System.Drawing.Size(1150, 280);
            this.gb_BinFile.TabIndex = 40;
            this.gb_BinFile.TabStop = false;
            this.gb_BinFile.Text = "Bin";
            // 
            // txtDeviceAddr
            // 
            this.txtDeviceAddr.Location = new System.Drawing.Point(1032, 24);
            this.txtDeviceAddr.Name = "txtDeviceAddr";
            this.txtDeviceAddr.Size = new System.Drawing.Size(87, 25);
            this.txtDeviceAddr.TabIndex = 31;
            this.txtDeviceAddr.Text = "A0";
            this.txtDeviceAddr.TextChanged += new System.EventHandler(this.txtDeviceAddr_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(898, 27);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 15);
            this.label9.TabIndex = 30;
            this.label9.Text = "DeviceAddr(Hex)";
            // 
            // txtLPLSize
            // 
            this.txtLPLSize.Location = new System.Drawing.Point(1032, 61);
            this.txtLPLSize.Name = "txtLPLSize";
            this.txtLPLSize.Size = new System.Drawing.Size(87, 25);
            this.txtLPLSize.TabIndex = 29;
            this.txtLPLSize.Text = "64";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(898, 64);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 15);
            this.label8.TabIndex = 28;
            this.label8.Text = "LPL Size(1-116)";
            // 
            // rTxt_BinFile
            // 
            this.rTxt_BinFile.BackColor = System.Drawing.SystemColors.Control;
            this.rTxt_BinFile.Location = new System.Drawing.Point(10, 45);
            this.rTxt_BinFile.Margin = new System.Windows.Forms.Padding(4);
            this.rTxt_BinFile.Name = "rTxt_BinFile";
            this.rTxt_BinFile.ReadOnly = true;
            this.rTxt_BinFile.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rTxt_BinFile.Size = new System.Drawing.Size(880, 220);
            this.rTxt_BinFile.TabIndex = 14;
            this.rTxt_BinFile.Text = "";
            // 
            // btn_SaveDatBin
            // 
            this.btn_SaveDatBin.Location = new System.Drawing.Point(906, 200);
            this.btn_SaveDatBin.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SaveDatBin.Name = "btn_SaveDatBin";
            this.btn_SaveDatBin.Size = new System.Drawing.Size(87, 44);
            this.btn_SaveDatBin.TabIndex = 25;
            this.btn_SaveDatBin.Text = "Save dat(bin)";
            this.btn_SaveDatBin.UseVisualStyleBackColor = true;
            this.btn_SaveDatBin.Click += new System.EventHandler(this.btn_SaveDatBin_Click);
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(10, 20);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(880, 18);
            this.textBox1.TabIndex = 15;
            this.textBox1.Text = "00000000: 00 01 02 03 04 05 06 07 08 09 0A 0B 0C 0D 0E 0F 10 11 12 13 14 15 16 17" +
    " 18 19 1A 1B 1C 1D 1E 1F";
            // 
            // btn_LoadDatBin
            // 
            this.btn_LoadDatBin.Location = new System.Drawing.Point(1032, 200);
            this.btn_LoadDatBin.Margin = new System.Windows.Forms.Padding(4);
            this.btn_LoadDatBin.Name = "btn_LoadDatBin";
            this.btn_LoadDatBin.Size = new System.Drawing.Size(87, 44);
            this.btn_LoadDatBin.TabIndex = 26;
            this.btn_LoadDatBin.Text = "Load dat(bin)";
            this.btn_LoadDatBin.UseVisualStyleBackColor = true;
            this.btn_LoadDatBin.Click += new System.EventHandler(this.btn_LoadDatBin_Click);
            // 
            // cmbBoxDisplayLength
            // 
            this.cmbBoxDisplayLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxDisplayLength.FormattingEnabled = true;
            this.cmbBoxDisplayLength.Items.AddRange(new object[] {
            "1K",
            "2K",
            "4K",
            "8K",
            "16K",
            "32K",
            "64K",
            "128K",
            "256K",
            "512K",
            "1024K",
            "2048K",
            "4096K",
            "8192K"});
            this.cmbBoxDisplayLength.Location = new System.Drawing.Point(1008, 103);
            this.cmbBoxDisplayLength.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxDisplayLength.Name = "cmbBoxDisplayLength";
            this.cmbBoxDisplayLength.Size = new System.Drawing.Size(111, 23);
            this.cmbBoxDisplayLength.TabIndex = 16;
            this.cmbBoxDisplayLength.SelectedIndexChanged += new System.EventHandler(this.cmbBoxDisplayLength_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(898, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "Flash Size:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(898, 139);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 15);
            this.label7.TabIndex = 19;
            this.label7.Text = "Line   Len:";
            // 
            // cmbBoxDisplayLineLen
            // 
            this.cmbBoxDisplayLineLen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBoxDisplayLineLen.FormattingEnabled = true;
            this.cmbBoxDisplayLineLen.Items.AddRange(new object[] {
            "10Bytes",
            "16Bytes",
            "32Bytes"});
            this.cmbBoxDisplayLineLen.Location = new System.Drawing.Point(1008, 136);
            this.cmbBoxDisplayLineLen.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBoxDisplayLineLen.Name = "cmbBoxDisplayLineLen";
            this.cmbBoxDisplayLineLen.Size = new System.Drawing.Size(111, 23);
            this.cmbBoxDisplayLineLen.TabIndex = 18;
            this.cmbBoxDisplayLineLen.SelectedIndexChanged += new System.EventHandler(this.cmbBoxDisplayLineLen_SelectedIndexChanged);
            // 
            // btn_AbortDownload
            // 
            this.btn_AbortDownload.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_AbortDownload.Location = new System.Drawing.Point(384, 127);
            this.btn_AbortDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btn_AbortDownload.Name = "btn_AbortDownload";
            this.btn_AbortDownload.Size = new System.Drawing.Size(156, 44);
            this.btn_AbortDownload.TabIndex = 41;
            this.btn_AbortDownload.Text = "Abort Firmware";
            this.btn_AbortDownload.UseVisualStyleBackColor = true;
            this.btn_AbortDownload.Click += new System.EventHandler(this.btn_AbortDownload_Click);
            // 
            // btnReadImage
            // 
            this.btnReadImage.Location = new System.Drawing.Point(968, 8);
            this.btnReadImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadImage.Name = "btnReadImage";
            this.btnReadImage.Size = new System.Drawing.Size(99, 44);
            this.btnReadImage.TabIndex = 42;
            this.btnReadImage.Text = "ReadEPP";
            this.btnReadImage.UseVisualStyleBackColor = true;
            this.btnReadImage.Click += new System.EventHandler(this.btnReadImage_Click);
            // 
            // btnReadSPI
            // 
            this.btnReadSPI.Location = new System.Drawing.Point(1075, 8);
            this.btnReadSPI.Margin = new System.Windows.Forms.Padding(4);
            this.btnReadSPI.Name = "btnReadSPI";
            this.btnReadSPI.Size = new System.Drawing.Size(99, 44);
            this.btnReadSPI.TabIndex = 44;
            this.btnReadSPI.Text = "ReadSPI";
            this.btnReadSPI.UseVisualStyleBackColor = true;
            this.btnReadSPI.Click += new System.EventHandler(this.btnReadSPI_Click);
            // 
            // btnWriteSPI
            // 
            this.btnWriteSPI.Location = new System.Drawing.Point(1076, 57);
            this.btnWriteSPI.Margin = new System.Windows.Forms.Padding(4);
            this.btnWriteSPI.Name = "btnWriteSPI";
            this.btnWriteSPI.Size = new System.Drawing.Size(99, 44);
            this.btnWriteSPI.TabIndex = 43;
            this.btnWriteSPI.Text = "WriteSPI";
            this.btnWriteSPI.UseVisualStyleBackColor = true;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1182, 583);
            this.Controls.Add(this.btnReadSPI);
            this.Controls.Add(this.btnWriteSPI);
            this.Controls.Add(this.btnReadImage);
            this.Controls.Add(this.btn_AbortDownload);
            this.Controls.Add(this.gb_BinFile);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.gb_ReadImage);
            this.Controls.Add(this.gb_Function);
            this.Controls.Add(this.btn_TBPowerOff);
            this.Controls.Add(this.btn_TBPowerOn);
            this.Controls.Add(this.bt_Download);
            this.Controls.Add(this.comboUSBDriver);
            this.Controls.Add(this.bt_Connect);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gb_Filename);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CDB Download Tool V1.00";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.gb_Filename.ResumeLayout(false);
            this.gb_Filename.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gb_ReadImage.ResumeLayout(false);
            this.gb_ReadImage.PerformLayout();
            this.gb_Function.ResumeLayout(false);
            this.gb_Function.PerformLayout();
            this.gb_BinFile.ResumeLayout(false);
            this.gb_BinFile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel lblState;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel tsTime;
        private System.Windows.Forms.ToolStripStatusLabel tsAuthor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bt_Connect;
        private System.Windows.Forms.GroupBox gb_Filename;
        private System.Windows.Forms.Button bt_Browse;
        private System.Windows.Forms.TextBox txt_HexFileName;
        private System.Windows.Forms.Button bt_Download;
        private System.Windows.Forms.ComboBox comboUSBDriver;
        private System.Windows.Forms.Button btn_TBPowerOff;
        private System.Windows.Forms.Button btn_TBPowerOn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label CommitImage;
        private System.Windows.Forms.Label RunImage;
        private System.Windows.Forms.Label ImageBVer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ImageAVer;
        private System.Windows.Forms.Button btn_GetStatus;
        private System.Windows.Forms.Button btn_CopyImageB2A;
        private System.Windows.Forms.Button btn_CopyImageA2B;
        private System.Windows.Forms.Button btn_CommitImage;
        private System.Windows.Forms.Button btn_RunImage;
        private System.Windows.Forms.GroupBox gb_ReadImage;
        private System.Windows.Forms.Button btn_ExportImage;
        private System.Windows.Forms.Button btn_ReadEPL;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btn_ReadLPL;
        private System.Windows.Forms.GroupBox gb_Function;
        private System.Windows.Forms.Button btn_WriteEPL;
        private System.Windows.Forms.TextBox txt_HexFile;
        private System.Windows.Forms.Button btn_WriteLPL;
        private System.Windows.Forms.GroupBox gb_BinFile;
        private System.Windows.Forms.RichTextBox rTxt_BinFile;
        private System.Windows.Forms.Button btn_SaveDatBin;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btn_LoadDatBin;
        private System.Windows.Forms.ComboBox cmbBoxDisplayLength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbBoxDisplayLineLen;
        private System.Windows.Forms.Button btn_AbortDownload;
        private System.Windows.Forms.TextBox txtLPLSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtDeviceAddr;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnReadImage;
        private System.Windows.Forms.Button btnReadSPI;
        private System.Windows.Forms.Button btnWriteSPI;
    }
}

