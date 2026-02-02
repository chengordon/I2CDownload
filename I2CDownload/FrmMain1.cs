using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using System.Runtime.InteropServices;//Marshal

namespace I2CDownload
{   
    public partial class FrmMain : Form
    {
        //AutoSizeForm asc = new AutoSizeForm();
        ClsBaseCDB mclsModule = new ClsBaseCDB();
        ClsIniFile mclsIniFile = new ClsIniFile();
        ClsFlashSetupConfig mclsFlashConfig = new ClsFlashSetupConfig();
        CDatabase cdaba = new CDatabase();

        IDOKMessageBox idokmessageBox;

        private struct DrawMessage
        {
            public string operateMsg;
            public Color color;
        }
        private DrawMessage drawMessage;

        string[] str_ReportDatabase = { "AdapterSN", "Type", "PassNum", "TotalNum", "Date", "State", "Time", "BinFile" };
        
        int nPassNum = 0;
        int nTotalNum = 0;

        string strCDBType = "DSP";
        string strCDBCMDTrigMode = "CMDID";
        string strCDBModel = "LPL";
        byte[] bytFlashBinData = new byte[1];

        private long mlngCDBPassword = 0x80001101;//CDB密码

        private string strBinRev = "";
        private string strModuleForm = "";
        private string strBinName = "";
        string strBinFilePath = null;

        private bool m_LowPower = false;

        private bool moduleAbs = false;
        private bool boardConneted = false;

        public FrmMain()
        {
            InitializeComponent();
            drawMessage = new DrawMessage();
        }

        /// <summary>
        /// 设置打印操作信息显示
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cor"></param>
        private void setShowMessage(string msg, Color cor)
        {
            this.drawMessage.operateMsg = msg;
            this.drawMessage.color = cor;
            msglabel_SizeChanged(new object(), new EventArgs());
        }

        //private void FrmMain_SizeChanged(object sender, EventArgs e)
        //{
        //    asc.controlAutoSize(this);
        //}

        #region TestBoard
        //连接测试板
        private bool InitTestBoard()
        {
            if (mclsModule.InterfaceInit())
            {
                lblState.Text = strings.TestboardConnected;
                return true;
            }
            else
            {
                setShowMessage(strings.TestboardDisconnected + ".", Color.Red);
                lblState.Text = strings.TestboardDisconnected;
                return false;
            }
        }
        //断开测试板
        private void DisconnectTestBoard()
        {
            mclsModule.InterfaceClose();
        }
        #endregion

        private void ReadBin(string filename, ref byte[] buffer)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader binreader = new BinaryReader(fs, Encoding.ASCII);
            int file_len = (int)fs.Length;//获取bin文件长度
            buffer = new byte[file_len];
            buffer = binreader.ReadBytes(file_len);
            binreader.Close();
            fs.Close();
        }

        private void SetClsModType()
        {
            strBinName = "";
            mclsFlashConfig.RefreshData();
            byte[] bytPW = new byte[4];

            int iIndex = -1;
            
            for (int i = 0; i < mclsFlashConfig.gmstrICType.Length; i++)//由IC类型找出索引
            {
                if (mclsFlashConfig.strModuleTypeSel.Equals(mclsFlashConfig.gmstrICType[i]))
                {
                    iIndex = i;
                    break;
                }
            }

            if (iIndex >= 0)
            {
                mlngCDBPassword = mclsFlashConfig.gmlngCDBPassword[iIndex];//CDB密码
                strBinRev = mclsFlashConfig.gmstrBinRev[iIndex];//Bin版本
                strModuleForm = mclsFlashConfig.gmstrModuleForm[iIndex];//模块接口
                mclsModule.strAPP = mclsFlashConfig.gmstrAPP[iIndex];//APP
                strBinName = mclsFlashConfig.gmstrBinName[iIndex];//Bin名字                                

                mclsModule.gbytPasswordDevAddr = (byte)mclsFlashConfig.gmintPwIICDevAddr[iIndex];//模块密码输入的IIC器件地址
                mclsModule.gbytPasswordTableSel = (byte)mclsFlashConfig.gmintPasswordTableSel[iIndex];//密码输入的IIC选表
                mclsModule.gbytPasswordWordAddr = (byte)mclsFlashConfig.gmintPasswordWordAddr[iIndex];//密码输入的字节地址
                mclsModule.gbytRevTableSel = (byte)mclsFlashConfig.gmintRevTableSel[iIndex];//Rev读取的IIC选表
                mclsModule.gbytRevRegAddr = (byte)mclsFlashConfig.gmintRevRegAddr[iIndex];//Rev寄存器地址
                mclsModule.gbytRevLen = (byte)mclsFlashConfig.gmintRevLen[iIndex];//Rev长度

                mclsModule.gbytCDBType = (byte)mclsFlashConfig.gmintCDBType[iIndex];//CDB类别
                mclsModule.gbytCDBHeaderLen = (byte)mclsFlashConfig.gmintCDBHeaderLen[iIndex];//CDB头文件长度
                mclsModule.gbytCDBCMDTrigMode = (byte)mclsFlashConfig.gmintCDBCMDTrigMode[iIndex];//CDB指令格式
                mclsModule.gbytCDBDataModel = (byte)mclsFlashConfig.gmintCDBDataModel[iIndex];//CDB数据格式 

                mclsModule.gbytMediaInterface = (byte)mclsFlashConfig.gmintMediaInterface[iIndex];

                mclsModule.gbytConfigAFETrim = (byte)mclsFlashConfig.gmintConfigAFETrim[iIndex];
                mclsModule.gbytConfigSCDREN = (byte)mclsFlashConfig.gmintConfigSCDREN[iIndex];
                mclsModule.gbytConfigCDRParams = (byte)mclsFlashConfig.gmintConfigCDRParams[iIndex];
                mclsModule.gbytConfigSCDRMulSel = (byte)mclsFlashConfig.gmintConfigSCDRMulSel[iIndex];

                //
                bytPW[0] = (byte)(mlngCDBPassword >> 24);
                bytPW[1] = (byte)((mlngCDBPassword & 0xFFFFFF) >> 16);
                bytPW[2] = (byte)((mlngCDBPassword & 0xFFFF) >> 8);
                bytPW[3] = (byte)(mlngCDBPassword & 0xFF);
                mclsModule.SetCDBPW(bytPW);

                if (mclsModule.gbytCDBType == 2)
                {
                    strCDBType = "MCU&&DSP";
                    mclsModule.gbytBinType = 1;
                }
                else if (mclsModule.gbytCDBType == 1)
                {
                    strCDBType = "MCU";
                    mclsModule.gbytBinType = 1;
                }
                else
                {
                    strCDBType = "DSP";
                    mclsModule.gbytBinType = 0;
                }

                if (mclsModule.gbytCDBCMDTrigMode == 0)
                {
                    strCDBCMDTrigMode = "CMDID";
                }
                else
                {
                    strCDBCMDTrigMode = "STOP";
                }

                if (mclsModule.gbytCDBDataModel == 0)
                {
                    strCDBModel = "LPL";

                    if (mclsFlashConfig.gmintCDBDataLen[iIndex] > 116)
                    {
                        mclsFlashConfig.gmintCDBDataLen[iIndex] = 116;
                        mclsModule.gbytCDBLPLDataSize = 116;//LPL数据长度
                    }
                    else
                    {
                        mclsModule.gbytCDBLPLDataSize = (byte)mclsFlashConfig.gmintCDBDataLen[iIndex];//LPL数据长度
                    }
                }
                else
                {
                    strCDBModel = "EPL";

                    if (mclsFlashConfig.gmintCDBDataLen[iIndex] > 2048)
                    {
                        mclsFlashConfig.gmintCDBDataLen[iIndex] = 2048;
                        mclsModule.gwCDBEPLDataSize = 2048;//EPL数据长度
                    }
                    else
                    {
                        mclsModule.gwCDBEPLDataSize = (ushort)mclsFlashConfig.gmintCDBDataLen[iIndex];//LPL数据长度
                    }
                }

                //
                string strText = "";
                strText += strings.ModuleForm + ":" + strModuleForm + "\r\n";
                strText += strings.CDBType.Replace("(Hex)", "").Trim() + ":" + strCDBType + "       ";
                strText += strings.CDBHeaderLen.Replace("(Hex)", "").Trim() + ":0x" + mclsModule.gbytCDBHeaderLen.ToString("X2") + "\r\n";
                strText += strings.CDBCMDTrigMode.Replace("(Hex)", "").Trim() + ":" + strCDBCMDTrigMode + "       ";
                strText += strings.CDBDataModel.Replace("(Hex)", "").Trim() + ":" + strCDBModel + "\r\n";

                strText += strings.CDBDataLen.Replace("(Hex)", "").Trim() + ":0x" + Convert.ToString(mclsFlashConfig.gmintCDBDataLen[iIndex], 16).ToUpper() + "       ";

                strText += strings.MediaInterface.Replace("(Hex)", "").Trim() + ":0x" + mclsModule.gbytMediaInterface.ToString("X2") + "\r\n";

                #region AFETrim
                if (mclsModule.gbytConfigAFETrim == 0x01)
                {
                    strText += strings.AFETrim.Replace("(Hex)", "").Trim() + ":" + "ISSE" + "       ";
                }
                else if (mclsModule.gbytConfigAFETrim == 0x02)
                {
                    strText += strings.AFETrim.Replace("(Hex)", "").Trim() + ":" + "0dB" + "       ";
                }
                else if (mclsModule.gbytConfigAFETrim == 0x03)
                {
                    strText += strings.AFETrim.Replace("(Hex)", "").Trim() + ":" + "NEG_0p5dB" + "       ";
                }
                else if (mclsModule.gbytConfigAFETrim == 0x04)
                {
                    strText += strings.AFETrim.Replace("(Hex)", "").Trim() + ":" + "NEG_1dB" + "       ";
                }
                else if (mclsModule.gbytConfigAFETrim == 0x05)
                {
                    strText += strings.AFETrim.Replace("(Hex)", "").Trim() + ":" + "NEG_2dB" + "       ";
                }
                else if (mclsModule.gbytConfigAFETrim == 0x06)
                {
                    strText += strings.AFETrim.Replace("(Hex)", "").Trim() + ":" + "NEG_4dB" + "       ";
                }
                else if (mclsModule.gbytConfigAFETrim == 0x07)
                {
                    strText += strings.AFETrim.Replace("(Hex)", "").Trim() + ":" + "NEG_7dB" + "       ";
                }
                else if (mclsModule.gbytConfigAFETrim == 0x08)
                {
                    strText += strings.AFETrim.Replace("(Hex)", "").Trim() + ":" + "NEG_11dB" + "       ";
                }
                #endregion

                if (mclsModule.gbytConfigSCDREN == 0x00)
                {
                    strText += strings.SCDREN.Replace("(Hex)", "").Trim() + ":" + "OFF" + "\r\n";
                }
                else if (mclsModule.gbytConfigSCDREN == 0x01)
                {
                    strText += strings.SCDREN.Replace("(Hex)", "").Trim() + ":" + "ON" + "\r\n";
                }

                #region CDRParams
                if (mclsModule.gbytConfigCDRParams == 0x00)
                {
                    strText += strings.CDRParams.Replace("(Hex)", "").Trim() + ":" + "CDR_OPT#1" + "       ";
                }
                else if (mclsModule.gbytConfigCDRParams == 0x01)
                {
                    strText += strings.CDRParams.Replace("(Hex)", "").Trim() + ":" + "CDR_OPT#2" + "       ";
                }
                else if (mclsModule.gbytConfigCDRParams == 0x02)
                {
                    strText += strings.CDRParams.Replace("(Hex)", "").Trim() + ":" + "CDR_OPT#3" + "       ";
                }
                else if (mclsModule.gbytConfigCDRParams == 0x03)
                {
                    strText += strings.CDRParams.Replace("(Hex)", "").Trim() + ":" + "CDR_OPT#4" + "       ";
                }
                else if (mclsModule.gbytConfigCDRParams == 0x04)
                {
                    strText += strings.CDRParams.Replace("(Hex)", "").Trim() + ":" + "CDR_OPT#5" + "       ";
                }                
                #endregion

                strText += strings.SCDRMulSel.Replace("(Hex)", "").Trim() + ":0x" + mclsModule.gbytConfigSCDRMulSel.ToString("X2") + "\r\n";
                
                strText += strings.BinRev + ":" + strBinRev + "\r\n";

                if (mclsModule.strAPP.Length > 32)
                {
                    strText += strings.APPCode + ":" + mclsModule.strAPP.Substring(0, 32) + "\r\n";
                    strText += mclsModule.strAPP.Substring(32, mclsModule.strAPP.Length - 32) + "\r\n";
                }
                else
                {
                    strText += strings.APPCode + ":" + mclsModule.strAPP + "\r\n";
                }

                if (strBinName.Length > 32)
                {
                    strText += strings.BinName + ":" + strBinName.Substring(0, 32) + "\r\n";
                    strText += strBinName.Substring(32, strBinName.Length - 32) + "\r\n";
                }
                else
                {
                    strText += strings.BinName + ":" + strBinName + "\r\n";
                }                
                
                label4.Text = strText;

                //
                this.Text = System.Windows.Forms.Application.ProductName + " V" + System.Windows.Forms.Application.ProductVersion + " (" + mclsFlashConfig.strModuleTypeSel + ")";
            }

            //BinFilePath
            strBinFilePath = Application.StartupPath + "\\Files\\" + strBinName;

            pb_TestResult.Image = null;
            mProgressBar1.Value = 0;
            boardConneted = false;

            if (!File.Exists(strBinFilePath))
            {
                strBinFilePath = "";
                return;
            }

            ReadBin(strBinFilePath, ref bytFlashBinData);
        }

        private bool DelayMS(int delayTime_ms)
        {
            DateTime now = DateTime.Now;
            int s;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                s = (int)spand.TotalMilliseconds;
                System.Windows.Forms.Application.DoEvents();
            }
            while (s < delayTime_ms);
            return true;
        }
        private void AccessInsert(string sAdapterSN, string sType, string sPassNum, string sTotalNum, string sDate, string sState, string sTime, string sBinFile)
        {
            string cmdstr = "";
            string cmdstr1 = ") values ('";
            cmdstr = "insert into " + cdaba.DatabaseParam.TabName + " ([";

            cmdstr += ("AdapterSN" + "],[");
            cmdstr1 += (sAdapterSN + "','");

            cmdstr += ("Type" + "],[");
            cmdstr1 += (sType + "','");

            cmdstr += ("PassNum" + "],[");
            cmdstr1 += (sPassNum + "','");

            cmdstr += ("TotalNum" + "],[");
            cmdstr1 += (sTotalNum + "','");

            cmdstr += ("Date" + "],[");
            cmdstr1 += (sDate + "','");

            cmdstr += ("State" + "],[");
            cmdstr1 += (sState + "','");

            cmdstr += ("Time" + "],[");
            cmdstr1 += (sTime + "','");

            cmdstr += ("BinFile" + "]");
            cmdstr1 += (sBinFile + "')");

            cmdstr += cmdstr1;

            cdaba.DatabaseParam.SqlStr = cmdstr;
            cdaba.GetQuery();
        }
        public string GetTime()
        {
            DateTime time = System.DateTime.Now;
            int hour = time.Hour;
            int h1 = 0;
            int h2 = hour;
            if (hour >= 10)
            {
                h1 = hour / 10;
                h2 = hour % 10;
            }

            int minute = time.Minute;
            int m1 = 0;
            int m2 = minute;
            if (minute >= 10)
            {
                m1 = minute / 10;
                m2 = minute % 10;
            }

            int second = time.Second;
            int s1 = 0;
            int s2 = second;
            if (second >= 10)
            {
                s1 = second / 10;
                s2 = second % 10;
            }

            string str = string.Format("{0}{1}:{2}{3}:{4}{5}", h1, h2, m1, m2, s1, s2);
            return str;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = strings.Time + " " + GetTime();
        }

        private void msglabel_SizeChanged(object sender, EventArgs e)
        {
            if (this.msglabel.Height > 0)
            {
                float fontSize = this.msglabel.Height / 4F * 3F / 5F;
                this.msglabel.Font = new System.Drawing.Font("宋体", fontSize, System.Drawing.FontStyle.Bold);
                this.msglabel.ForeColor = this.drawMessage.color;
                this.msglabel.Text = this.drawMessage.operateMsg;
            }
        }

        private void timerMon_Tick(object sender, EventArgs e)
        {
            if (InitTestBoard() == false) return;

            if (strBinFilePath.Equals(""))
            {
                setShowMessage(strings.BinFileNotExist, Color.Red);
                return;
            }

            moduleAbs = mclsModule.CheckI2CRespond();
            if (moduleAbs == true && boardConneted == true)
            {
                /* 执行一次性动作 */
                btnStartProgram_Click(new object(), new EventArgs());
                boardConneted = false;
            }

            if (moduleAbs == false)
            {
                boardConneted = true;
                setShowMessage(strings.InsertModule, Color.Green);
            }
            else
            {
                setShowMessage(strings.PulloutModule, Color.Green);
            }
        }

        private bool EnterCDB()
        {
            byte[] bytData = new byte[mclsModule.gbytCDBHeaderLen];
            string DownloadType = "EnterCDB " + strCDBType;
            string errorStr = "", errorMsg = DownloadType + " Fail";
            uint iDataLen = (uint)bytFlashBinData.Length;

            if (mclsModule.EnterCDBPW() == false || mclsModule.SendGetCDBStatus(ref errorStr, errorMsg) == false)
            {
                lblStatus.Text = errorStr;
                return false;
            }

            Array.Copy(bytFlashBinData, 0, bytData, 0, mclsModule.gbytCDBHeaderLen);  //文件头40个字节

            if (mclsModule.SendCDBStart(bytData, iDataLen, mclsModule.gbytCDBHeaderLen,ref errorStr, errorMsg) == false)
            {
                lblStatus.Text = errorStr;
                return false;
            }
            return true;
        }

        private bool WriteDataCDBLPL()
        {
            string DownloadType = "WriteDataLPL " + strCDBType;
            string errorStr = "", errorMsg = DownloadType + " Fail";
            uint iDataLen = (uint)(bytFlashBinData.Length - mclsModule.gbytCDBHeaderLen);
            uint TotalLen = iDataLen, iCurWriteNum = 0, iAddr = 0;
            byte[] bytData = new byte[mclsModule.gbytCDBLPLDataSize];
            float fProgress = 0;
            mProgressBar1.Value = 0;

            while (iDataLen > 0)
            {
                iCurWriteNum = iDataLen;
                if (iCurWriteNum > mclsModule.gbytCDBLPLDataSize) iCurWriteNum = mclsModule.gbytCDBLPLDataSize;//分段写，每次最多(CDBDataSize)个数据
                iDataLen -= iCurWriteNum;

                Array.Copy(bytFlashBinData, iAddr + mclsModule.gbytCDBHeaderLen, bytData, 0, iCurWriteNum);

                if (mclsModule.WriteDataLPL(iAddr, bytData, iCurWriteNum, ref errorStr, errorMsg) == false)
                {
                    break;
                }

                iAddr = iAddr + iCurWriteNum;

                fProgress = (TotalLen - iDataLen) / (float)TotalLen;
                if (fProgress > 1) fProgress = 1;
                mProgressBar1.Value = (int)(mProgressBar1.Maximum * fProgress);
                lblStatus.Text = "Download: " + String.Format("{0:N2}", fProgress * 100) + "%";
            }

            if (iDataLen != 0)
            {
                lblStatus.Text = "Download Fail Remain " + iDataLen + " Byte.";
                return false;
            }
            return true;
        }

        private bool WriteDataCDBEPL()
        {
            string DownloadType = "WriteDataEPL " + strCDBType;
            string errorStr = "", errorMsg = DownloadType + " Fail";
            uint iDataLen = (uint)(bytFlashBinData.Length - mclsModule.gbytCDBHeaderLen);
            uint TotalLen = iDataLen, iCurWriteNum = 0, iAddr = 0;
            byte[] bytData = new byte[mclsModule.gwCDBEPLDataSize];
            float fProgress = 0;
            mProgressBar1.Value = 0;

            while (iDataLen > 0)
            {
                iCurWriteNum = iDataLen;
                if (iCurWriteNum > mclsModule.gwCDBEPLDataSize) iCurWriteNum = mclsModule.gwCDBEPLDataSize;//分段写，每次最多(CDBDataSize)个数据
                iDataLen -= iCurWriteNum;

                Array.Copy(bytFlashBinData, iAddr + mclsModule.gbytCDBHeaderLen, bytData, 0, iCurWriteNum);

                if (mclsModule.WriteDataEPL(iAddr, bytData, iCurWriteNum, ref errorStr, errorMsg) == false)
                {
                    break;
                }

                iAddr = iAddr + iCurWriteNum;

                fProgress = (TotalLen - iDataLen) / (float)TotalLen;
                if (fProgress > 1) fProgress = 1;
                mProgressBar1.Value = (int)(mProgressBar1.Maximum * fProgress);
                lblStatus.Text = "Download: " + String.Format("{0:N2}", fProgress * 100) + "%";
            }

            if (iDataLen != 0)
            {
                lblStatus.Text = "Download Fail Remain " + iDataLen + " Byte.";
                return false;
            }
            return true;
        }

        private bool WriteDataCDB()
        {
            if (mclsModule.gbytCDBDataModel == 0)
            {
                return WriteDataCDBLPL();
            }
            else
            {
                return WriteDataCDBEPL();
            }

        }

        private bool CDBComplete()
        {
            string DownloadType = "WriteDataEPL " + strCDBType;
            string errorStr = "", errorMsg = DownloadType + " Fail";

            if (mclsModule.SendCDBComplete(ref errorStr, errorMsg) == false)
            {
                lblStatus.Text = errorStr;
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CDBRunImage()
        {
            string DownloadType = "RunImage Command " + strCDBType;
            string errorStr = "", errorMsg = DownloadType + " Fail";

            if (mclsModule.SendGetCDBStatus(ref errorStr, errorMsg) == false || 
                mclsModule.SendCDBRunImage(ref errorStr, errorMsg) == false)
            {
                lblStatus.Text = errorStr;
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CDBCommitImage()
        {
            string DownloadType = "CommitImage Command " + strCDBType;
            string errorStr = "", errorMsg = DownloadType + " Fail";

            if (mclsModule.EnterCDBPW() == false || mclsModule.SendGetCDBStatus(ref errorStr, errorMsg) == false  || mclsModule.SendCDBCommitImage(ref errorStr, errorMsg) == false)
            {
                lblStatus.Text = errorStr;
                return false;
            }
            else
            {
                return true;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            lblState.Text = "";
            setShowMessage(" ", Color.Green);
            //lblStatus.Text = "";
            lblState.Width = this.Width * 2 / 3;
            lblAuthor.Width = this.Width / 3;
            lblTime.Text = strings.CurrentTime + " " + GetTime();

            string strTemp = "";
            #region Load IniSetup
            mclsIniFile.FileName = Application.StartupPath + "\\Files\\Config.ini";
            mclsFlashConfig.FilePath = Application.StartupPath + "\\Files\\SetupData.dat";

            //Item
            mclsFlashConfig.strModuleTypeSel = mclsIniFile.ReadString("FrmCDBSetup", "Item", "DSP");

            //I2CDriver
            strTemp = mclsIniFile.ReadString("FrmCDBSetup", "I2CDriver", "1");
            mclsModule.gintIICChnl = Convert.ToInt32(strTemp);
            cmb_DeviceList.SelectedIndex = mclsModule.gintIICChnl;

            //LowPower
            strTemp = mclsIniFile.ReadString("FrmCDBSetup", "chkLowPower", "true");
            if (strTemp.ToLower().Equals("true"))
            {
                chk_LowPower.Checked = true;
            }
            else
            {
                chk_LowPower.Checked = false;
            }

            //txtPass
            strTemp = mclsIniFile.ReadString("FrmCDBSetup", "txtPass", "0");
            nPassNum = Convert.ToInt32(strTemp);
            txt_Pass.Text = strTemp;

            //txtTotal
            strTemp = mclsIniFile.ReadString("FrmCDBSetup", "txtTotal", "0");
            nTotalNum = Convert.ToInt32(strTemp);
            txt_Total.Text = strTemp;
            #endregion 

            string str_TableName = "Records";

            cdaba.DBParamInit();
            cdaba.DatabaseParam.DBName = Application.StartupPath + "\\Files\\CDBDatabase.mdb";
            //string[] tableArray = cdaba.getTableName();
            //if (!tableArray.Contains(str_TableName))
            //{
            //    cdaba.TableCreate(str_TableName, str_ReportDatabase);
            //}
            cdaba.DatabaseParam.TabName = str_TableName;
            cdaba.DatabaseParam.ColumnName = str_ReportDatabase;

            SetClsModType();

            //mclsModule.SetTBdPowerOnOff(true);//上电
            //mclsModule.SetTBdBenOnOff(false);

            timer1.Enabled = true;
            timerMon.Enabled = true;
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            string strTemp = "";
            mclsModule.InterfaceClose();

            #region Save IniSetup

            //Item
            mclsIniFile.WriteString("FrmCDBSetup", "Item", mclsFlashConfig.strModuleTypeSel);

            //I2CDriver            
            strTemp = cmb_DeviceList.SelectedIndex.ToString();
            mclsIniFile.WriteString("FrmCDBSetup", "I2CDriver", strTemp);

            //chk_LowPower            
            strTemp = m_LowPower.ToString().ToLower();
            mclsIniFile.WriteString("FrmCDBSetup", "chkLowPower", strTemp);

            //txtPass
            strTemp = nPassNum.ToString();
            mclsIniFile.WriteString("FrmCDBSetup", "txtPass", strTemp);

            //txtTotal
            strTemp = nTotalNum.ToString();
            mclsIniFile.WriteString("FrmCDBSetup", "txtTotal", strTemp);
            #endregion

            this.Dispose();
        }

        private void btnStartProgram_Click(object sender, EventArgs e)
        {
            string strFormFormModule = " ";
            string strRevFormModule = " ";
            string sState = " ";
            bool stateflag = false;

            DateTime dtStart = DateTime.Now;
            TimeSpan Tspan;

            /* 关闭moduleAbs检测 */
            timerMon.Enabled = false;
            //btnStartProgram.Enabled = false;
            menuStrip1.Enabled = false;

            setShowMessage("", Color.Green);            
            pb_TestResult.Image = null;
            //lblStatus.Text = " ";
            mProgressBar1.Value = 0;
            gb_TestResult.Enabled = false;

            if (mclsModule.LowPwrAllowRequestHW(false) && mclsModule.LowPwrRequestSW(m_LowPower))
            {
                DelayMS(5000);
                DelayMS(2000);

                #region ModuleForm
                if (strModuleForm.Equals("") == false)
                {
                    strFormFormModule = mclsModule.GetModuleForm();//从模块获取接口类型
                    if (strModuleForm != strFormFormModule)
                    {
                        string strTemp = "Module Form:" + strFormFormModule + "\r\n";
                        strTemp += "Define Form:" + strModuleForm + "\r\n";
                        setShowMessage(strTemp, Color.Red);

                        mProgressBar1.Value = mProgressBar1.Maximum;
                        mclsModule.LowPwrRequestSW(true);

                        idokmessageBox = new IDOKMessageBox("The Module Form not compatible.");
                        idokmessageBox.ShowDialog();
                    }
                }
                #endregion

                #region ModuleTXH0_7Config
                if ((mclsModule.strAPP.Equals("") == false) && (mProgressBar1.Value != mProgressBar1.Maximum))
                {
                    if (mclsModule.WriteModuleAPP() == false || mclsModule.WriteTXH0_7Config() == false)
                    {
                        mProgressBar1.Value = mProgressBar1.Maximum;
                    }
                }
                #endregion

                #region ModuleRev
                if ((strBinRev.Equals("") == false) && (mProgressBar1.Value != mProgressBar1.Maximum))
                {
                    strRevFormModule = mclsModule.GetModuleRev();//从模块获取版本
                    if (strBinRev == strRevFormModule)
                    {
                        string strTemp = "Module Rev:" + strRevFormModule + "\r\n";
                        strTemp += "Bin Rev:" + strBinRev + "\r\n";
                        setShowMessage(strTemp, Color.Red);

                        if (mclsModule.LowPwrRequestSW(true))
                        {
                            idokmessageBox = new IDOKMessageBox("The Module version is the latest.");
                            idokmessageBox.ShowDialog();

                            stateflag = true;
                            mProgressBar1.Value = mProgressBar1.Maximum;

                            //if (!idokmessageBox.GettValue())
                            //{
                            //    stateflag = true;
                            //    mProgressBar1.Value = mProgressBar1.Maximum;
                            //}
                            //else
                            //{
                            //    if (mclsModule.LowPwrAllowRequestHW(false) && mclsModule.LowPwrRequestSW(m_LowPower))
                            //    {
                            //        DelayMS(5000);
                            //    }
                            //}
                        }                        
                    }
                }
                #endregion

                #region CDB
                if (stateflag == false && mProgressBar1.Value != mProgressBar1.Maximum && EnterCDB() && WriteDataCDB() && CDBComplete())
                {
                    stateflag = true;

                    if (mclsModule.gbytCDBType != 0)
                    {
                        DelayMS(1000);

                        if (CDBRunImage())//RunImage
                        {
                            DelayMS(5000);

                            if (CDBCommitImage() == false)//CommitImage
                            {
                                stateflag = false;
                            }
                        }
                        else
                        {
                            stateflag = false;
                        }
                    }

                    #region ModuleRev
                    if (stateflag == true && strBinRev.Equals("") == false)
                    {
                        if (mclsModule.SoftwareReset())
                        {
                            DelayMS(5000);
                            //
                            if (mclsModule.LowPwrAllowRequestHW(false) && mclsModule.LowPwrRequestSW(m_LowPower))
                            {
                                DelayMS(5000);
                                DelayMS(2000);
                                strRevFormModule = mclsModule.GetModuleRev();//从模块获取版本

                                if (strBinRev != strRevFormModule)
                                {
                                    stateflag = false;
                                }
                            }
                            else
                            {
                                stateflag = false;
                            }
                        }
                        else
                        {
                            stateflag = false;
                        }
                    }
                    #endregion
                }
                #endregion
            }

            if (mclsModule.LowPwrRequestSW(true) == false)
            {
                stateflag = false;
            }

            if (stateflag)
            {
                sState = "PASS";
                nPassNum++;
                txt_Pass.Text = nPassNum.ToString();
                pb_TestResult.Image = Properties.Resources.Pass;
            }
            else
            {
                sState = "FAIL";
                pb_TestResult.Image = Properties.Resources.Fail;
            }

            nTotalNum++;
            txt_Total.Text = nTotalNum.ToString();

            Tspan = DateTime.Now - dtStart;

            string sTime = (Tspan.Minutes * 60 + Tspan.Seconds).ToString();
            string sAdapterSN = cmb_DeviceList.SelectedItem.ToString();
            string sType = strCDBType;
            string sPassNum = nPassNum.ToString();
            string sTotalNum = nTotalNum.ToString();
            string sDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); //获取当前时间
            string sBinFile = Path.GetFileName(strBinFilePath);

            AccessInsert(sAdapterSN, sType, sPassNum, sTotalNum, sDate, sState, sTime, sBinFile);

            lblStatus.Text = sTime + "s";

            gb_TestResult.Enabled = true;
            //btnStartProgram.Enabled = true;
            menuStrip1.Enabled = true;
            /* 打开moduleAbs检测 */
            timerMon.Enabled = true;
        }

        private void txt_Pass_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            nPassNum = 0;
            txt_Pass.Text = nPassNum.ToString();
        }
        private void txt_Total_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            nTotalNum = 0;
            txt_Total.Text = nTotalNum.ToString();
        }
        private void bt_TestReport_Click(object sender, EventArgs e)
        {
            FrmReport fReport = new FrmReport();
            fReport.cdaba = this.cdaba;  
            this.Hide();
            fReport.ShowDialog();
            fReport.Dispose();
            this.Show();
        }

        private void menuItemSetup_Click(object sender, EventArgs e)
        {
            string strTemp;
            mclsFlashConfig.gbAccessPass = false;
            FrmPassword fm = new FrmPassword();
            fm.mclsFlashSetup = this.mclsFlashConfig;

            this.Hide();
            strTemp = mclsFlashConfig.strModuleTypeSel;
            fm.ShowDialog();

            if (mclsFlashConfig.gbAccessPass == true)
            {
                FrmFlashSetup fmSetup = new FrmFlashSetup();
                fmSetup.mclsFlashSetup = this.mclsFlashConfig;
                fmSetup.ShowDialog();

                //
                mclsFlashConfig.RefreshData();
                string[] strArray = mclsFlashConfig.gmstrICType;// GetICTypes();
                if (strArray.Length > 0)
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (strArray[i].Equals(strTemp))
                        {
                            mclsFlashConfig.strModuleTypeSel = strTemp;                            
                            break;                            
                        }
                        mclsFlashConfig.strModuleTypeSel = null;
                    }
                }
                SetClsModType();
            }
            this.Show();
        }

        private void menuModifySetupPassword_Click(object sender, EventArgs e)
        {
            mclsFlashConfig.gbAccessPass = false;
            FrmPasswordModify fm = new FrmPasswordModify();
            fm.clFlashSetup = this.mclsFlashConfig;
            fm.ShowDialog();
        }

        private void MenuOptions_Click(object sender, EventArgs e)
        {
            new FrmOptions().ShowDialog();
        }

        private void menuItemSwitch_Click(object sender, EventArgs e)
        {
            string strTemp;
            mclsFlashConfig.gbAccessPass = false;
            FrmPassword fm = new FrmPassword();
            fm.mclsFlashSetup = this.mclsFlashConfig;

            this.Hide();
            strTemp = mclsFlashConfig.strModuleTypeSel;
            fm.ShowDialog();

            if (mclsFlashConfig.gbAccessPass == true)
            {
                FrmModuleSelect fmModuleSelect = new FrmModuleSelect();
                fmModuleSelect.mclsFlashConfig = this.mclsFlashConfig;
                fmModuleSelect.ShowDialog();                
                if (!strTemp.Equals(mclsFlashConfig.strModuleTypeSel))
                {
                    //txtPass
                    nPassNum = 0;
                    txt_Pass.Text = nPassNum.ToString();

                    //txtTotal
                    nTotalNum = 0;
                    txt_Total.Text = nTotalNum.ToString();
                }
                SetClsModType();
            }
            this.Show();
        }

        private void chk_LowPower_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_LowPower.Checked)
            {
                m_LowPower = true;
            }
            else
            {
                m_LowPower = false;
            }
        }
    }
}
