using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace I2CDownload
{
    public class ClsFlashSetupConfig
    {
        public string[] gmstrICType = new string[0];
        public int[] gmintPwIICDevAddr = new int[0];
        public int[] gmintPasswordTableSel = new int[0];
        public int[] gmintPasswordWordAddr = new int[0];        
        public long[] gmlngCDBPassword = new long[0];
        public int[] gmintRevTableSel = new int[0];
        public int[] gmintRevRegAddr = new int[0];
        public int[] gmintRevLen = new int[0];
        public int[] gmintCDBType = new int[0];
        public int[] gmintCDBHeaderLen = new int[0];
        public int[] gmintCDBCMDTrigMode = new int[0];
        public int[] gmintCDBDataModel = new int[0];
        public int[] gmintCDBDataLen = new int[0];
        public int[] gmintMediaInterface = new int[0];
        public int[] gmintConfigAFETrim = new int[0];
        public int[] gmintConfigSCDREN = new int[0];
        public int[] gmintConfigCDRParams = new int[0];
        public int[] gmintConfigSCDRMulSel = new int[0];
        public string[] gmstrBinRev = new string[0];
        public string[] gmstrModuleForm = new string[0];
        public string[] gmstrAPP = new string[0];
        public string[] gmstrBinName = new string[0];

        public bool gbAccessPass = false;
        public string strModuleTypeSel = null;

        private const string SETUP_DEFAULT_PASSWORD = "123";        
        private const int BIN_LOCAL_SETUP_PAGE_PW_ADDR = 0;//(16 Byte)
        private const int BIN_LOCAL_SETUP_PAGE_PW_LEN = 16;
        private const int BIN_FILE_HEAD_INFOR_LEN = 64; //文件信息头的长度
        private const int BIN_PAGE_LEN = 256; //每页数据个数

        private const int BIN_IC_TYPE_NAME_LEN = 32;
        private const int BIN_LOCAL_IC_TYPE = 0;//(32 Byte)
        private const int BIN_LOCAL_PW_IIC_DEV_ADDR = 32; //密码的器件地址(1 Byte)
        private const int BIN_LOCAL_PW_TABSEL = 33; //升级选表(1 Byte)
        private const int BIN_LOCAL_PW_ADDR = 34; //输入密码地址(1 Byte)
        private const int BIN_LOCAL_CDB_PW = 35; //升级密码(4 Byte)
        private const int BIN_LOCAL_REV_TABSEL = 39; //FW选表(1 Byte)
        private const int BIN_LOCAL_REVREG_ADDR = 40; //模块版本地址(1 Byte)
        private const int BIN_LOCAL_REV_LEN = 41; //模块版本长度(1 Byte)
        private const int BIN_LOCAL_CDB_TYPE = 42; //CDB类型长度(1 Byte)
        private const int BIN_LOCAL_CDB_HEADERLEN = 43; //CDB头长度(1 Byte)
        private const int BIN_LOCAL_CDB_CMD_TRIGMODE = 44; //CDB指令触发方式(1 Byte)
        private const int BIN_LOCAL_CDB_DATA_MODEL = 45; //CDB数据模式(1 Byte)      
        private const int BIN_LOCAL_CDB_DATALEN = 46; //CDB数据长度(2 Byte)
        private const int BIN_LOCAL_MEDIA_INTERFACE = 48; //媒介接口(1 Byte)
        private const int BIN_LOCAL_CONFIG_AFETRIM = 49; //AFETrim参数(1 Byte)
        private const int BIN_LOCAL_CONFIG_SCDREN = 50; //SCDREN参数(1 Byte)
        private const int BIN_LOCAL_CONFIG_CDRPARAMS = 51; //CDRParams参数(1 Byte)
        private const int BIN_LOCAL_CONFIG_SCDRMULSEL = 52; //SCDRMulSel参数(1 Byte) 
        private const int BIN_LOCAL_BINREV = 53; //模块版本(16 Byte)
        private const int BIN_LOCAL_MODULEFORM = 69; //模块接口(16 Byte)
        private const int BIN_LOCAL_APP = 85; //APP(64 Byte)
        private const int BIN_LOCAL_BIN_NAME = 149;//(64 Byte)

        private string mstrFilePath;
        private string mstrFlashPassword = "";        

        public ClsFlashSetupConfig()//构造函数
        {
        }

        #region Conversion
        public string Dec2Hex(int intDec, int intResultLen)
        {
            string strTemp = Convert.ToString(intDec, 16);
            int intLenTemp = intResultLen - strTemp.Length;
            strTemp = strTemp.ToUpper();
            if (strTemp.Length < intResultLen)
            {
                for (int i = 0; i < intLenTemp; i++)
                {
                    strTemp = "0" + strTemp;
                }
            }
            return strTemp;
        }
        public string Dec2Hex(long intDec, int intResultLen)
        {
            string strTemp = Convert.ToString(intDec, 16);
            int intLenTemp = intResultLen - strTemp.Length;
            strTemp = strTemp.ToUpper();
            if (strTemp.Length < intResultLen)
            {
                for (int i = 0; i < intLenTemp; i++)
                {
                    strTemp = "0" + strTemp;
                }
            }
            return strTemp;
        }
        public int Hex2Dec(string strHex)
        {
            int intTemp;
            try
            {
                intTemp = Convert.ToInt32(strHex, 16);
            }
            catch
            {
                intTemp = 0;
            }
            return intTemp;
        }
        public string Dec2Bin(int intDec)
        {
            if (intDec > 255) return "00000000";
            string strTemp;
            try
            {
                strTemp = Convert.ToString(intDec, 2);
                if (strTemp.Length < 8)
                {
                    int intLen = 8 - strTemp.Length;
                    for (int i = 0; i < intLen; i++)
                    {
                        strTemp = "0" + strTemp;
                    }
                }
            }
            catch
            {
                strTemp = "";
            }
            return strTemp;
        }
        #endregion

        #region File
        public string ReadTxtFile(string strFilePath)
        {
            string strTemp = "";
            if (File.Exists(strFilePath))
            {
                StreamReader sr = new StreamReader(strFilePath);
                strTemp = sr.ReadToEnd();
                sr.Close();
            }
            return strTemp;
        }
        public bool WriteTxtFile(string strFilePath, string strContent)
        {
            try
            {
                FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);//创建写入文件 
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(strContent);
                sw.Close();
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }            
        }
        public bool ReadBinFile(string strFilePath, ref byte[] bytRead)
        {
            if (File.Exists(strFilePath))
            {
                FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs, Encoding.ASCII);
                int len = (int)fs.Length;//获取bin文件长度
                bytRead = new byte[len];
                bytRead = br.ReadBytes(len);
                br.Close();
                fs.Close();
                return true;
            }
            return false;
        }
        public bool WriteBinFile(string strFilePath, byte[] bytBinData)
        {
            try
            {
                FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);//创建写入文件 
                BinaryWriter br = new BinaryWriter(fs, Encoding.ASCII);
                foreach (byte j in bytBinData)
                {
                    br.Write(j);
                }
                br.Close();
                fs.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        public string FilePath
        {
            get { return mstrFilePath; }
            set { mstrFilePath = value;}
        }

        public void RefreshData()
        {
            byte[] bytRead = new byte[0];
            ReadBinFile(mstrFilePath, ref bytRead);
            int iRowsNum = 0;
            if (bytRead.Length > BIN_FILE_HEAD_INFOR_LEN)
            {
                iRowsNum = (int)((bytRead.Length - BIN_FILE_HEAD_INFOR_LEN) / BIN_PAGE_LEN);
                gmstrICType = new string[iRowsNum];
                gmintPwIICDevAddr = new int[iRowsNum];
                gmintPasswordTableSel = new int[iRowsNum];
                gmintPasswordWordAddr = new int[iRowsNum];
                gmlngCDBPassword = new long[iRowsNum];
                gmintRevTableSel = new int[iRowsNum];
                gmintRevRegAddr = new int[iRowsNum];
                gmintRevLen = new int[iRowsNum];
                gmintCDBType = new int[iRowsNum];
                gmintCDBHeaderLen = new int[iRowsNum];
                gmintCDBCMDTrigMode = new int[iRowsNum];
                gmintCDBDataModel = new int[iRowsNum];
                gmintCDBDataLen = new int[iRowsNum];
                gmintMediaInterface = new int[iRowsNum];
                gmintConfigAFETrim = new int[iRowsNum];
                gmintConfigSCDREN = new int[iRowsNum];
                gmintConfigCDRParams = new int[iRowsNum];
                gmintConfigSCDRMulSel = new int[iRowsNum];
                gmstrBinRev = new string[iRowsNum];
                gmstrModuleForm = new string[iRowsNum];
                gmstrAPP = new string[iRowsNum];
                gmstrBinName = new string[iRowsNum];
            }
            else
            {
                mstrFlashPassword = "";
                gmstrICType = new string[0];
                gmintPwIICDevAddr = new int[0];
                gmintPasswordTableSel = new int[0];
                gmintPasswordWordAddr = new int[0];
                gmlngCDBPassword = new long[0];
                gmintRevTableSel = new int[0];
                gmintRevRegAddr = new int[0];
                gmintRevLen = new int[0];
                gmintCDBType = new int[0];
                gmintCDBHeaderLen = new int[0];
                gmintCDBCMDTrigMode = new int[0];
                gmintCDBDataModel = new int[0];
                gmintCDBDataLen = new int[0];
                gmintMediaInterface = new int[0];
                gmintConfigAFETrim = new int[0];
                gmintConfigSCDREN = new int[0];
                gmintConfigCDRParams = new int[0];
                gmintConfigSCDRMulSel = new int[0];
                gmstrBinRev = new string[0];
                gmstrModuleForm = new string[0];
                gmstrAPP = new string[0];
                gmstrBinName = new string[0];
                return;
            }
            //
            int iTemp = BIN_LOCAL_SETUP_PAGE_PW_ADDR;
            string strTemp = System.Text.Encoding.Default.GetString(bytRead, iTemp, BIN_LOCAL_SETUP_PAGE_PW_LEN);
            mstrFlashPassword = strTemp.Replace('\0', ' ').Trim();

            //
            for (int i = 0; i < iRowsNum; i++)
            {
                //IC类型
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_IC_TYPE;
                strTemp = System.Text.Encoding.Default.GetString(bytRead, iTemp, BIN_IC_TYPE_NAME_LEN);
                gmstrICType[i] = strTemp.Trim();

                //Password IIC Dev Addr
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_PW_IIC_DEV_ADDR;
                gmintPwIICDevAddr[i] = bytRead[iTemp];
                //PasswordTableSel
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_PW_TABSEL;
                gmintPasswordTableSel[i] = bytRead[iTemp];
                //PasswordWordAddr
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_PW_ADDR;
                gmintPasswordWordAddr[i] = bytRead[iTemp];
                //CDB密码
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_PW;
                gmlngCDBPassword[i] = bytRead[iTemp];
                gmlngCDBPassword[i] <<= 8;
                gmlngCDBPassword[i] += bytRead[iTemp + 1];
                gmlngCDBPassword[i] <<= 8;
                gmlngCDBPassword[i] += bytRead[iTemp + 2];
                gmlngCDBPassword[i] <<= 8;
                gmlngCDBPassword[i] += bytRead[iTemp + 3];
                //FWTableSel
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_REV_TABSEL;
                gmintRevTableSel[i] = bytRead[iTemp];               
                //RevRegAddr
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_REVREG_ADDR;
                gmintRevRegAddr[i] = bytRead[iTemp];
                //RevLen
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_REV_LEN;
                gmintRevLen[i] = bytRead[iTemp];
                //CMDType
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_TYPE;
                gmintCDBType[i] = bytRead[iTemp];
                //HeaderLen
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_HEADERLEN;
                gmintCDBHeaderLen[i] = bytRead[iTemp];
                //CDBCMDTrigMode
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_CMD_TRIGMODE;
                gmintCDBCMDTrigMode[i] = bytRead[iTemp];
                //CDBDataModel
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_DATA_MODEL;
                gmintCDBDataModel[i] = bytRead[iTemp];                
                //CDBDataLen
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_DATALEN;
                gmintCDBDataLen[i] = bytRead[iTemp];
                gmintCDBDataLen[i] <<= 8;
                gmintCDBDataLen[i] += bytRead[iTemp + 1];
                //MediaInterface
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_MEDIA_INTERFACE;
                gmintMediaInterface[i] = bytRead[iTemp];
                //AFETrim
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CONFIG_AFETRIM;        
                gmintConfigAFETrim[i] = bytRead[iTemp];
                //SCDREN
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CONFIG_SCDREN;
                gmintConfigSCDREN[i] = bytRead[iTemp];
                //CDRParams
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CONFIG_CDRPARAMS;
                gmintConfigCDRParams[i] = bytRead[iTemp];
                //SCDRMulSel
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CONFIG_SCDRMULSEL;
                gmintConfigSCDRMulSel[i] = bytRead[iTemp];
                //BinRev
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_BINREV;
                strTemp = System.Text.Encoding.Default.GetString(bytRead, iTemp, BIN_IC_TYPE_NAME_LEN/2);
                gmstrBinRev[i] = strTemp.Trim();
                //ModuleForm
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_MODULEFORM;
                strTemp = System.Text.Encoding.Default.GetString(bytRead, iTemp, BIN_IC_TYPE_NAME_LEN/2);
                gmstrModuleForm[i] = strTemp.Trim();                
                //APP
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_APP;
                strTemp = System.Text.Encoding.Default.GetString(bytRead, iTemp, BIN_IC_TYPE_NAME_LEN*2);
                gmstrAPP[i] = strTemp.Trim();                
                //BinName
                iTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_BIN_NAME;
                strTemp = System.Text.Encoding.Default.GetString(bytRead, iTemp, BIN_IC_TYPE_NAME_LEN*2);
                gmstrBinName[i] = strTemp.Trim();   
            }
        }

        public bool CheckPassword(string strPW)
        {
            string strTemp = "";
            RefreshData();
            if (mstrFlashPassword.Equals(""))
            {
                strTemp = SETUP_DEFAULT_PASSWORD;
            }
            else
            {
                strTemp = mstrFlashPassword;
            }

            if (strPW.Equals(strTemp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ModifyPassword(string strPW)
        {
            int iPasswordLen = BIN_LOCAL_SETUP_PAGE_PW_LEN;
            if (strPW.Length > iPasswordLen)
            {
                strPW = strPW.Substring(0, iPasswordLen);
            }
            if (strPW.Length < iPasswordLen)
            {
                strPW += ("").PadRight(iPasswordLen - strPW.Length, ' ');
            }
            byte[] bytPW = new byte[iPasswordLen];
            bytPW = System.Text.Encoding.Default.GetBytes(strPW);
            byte[] bytRead = new byte[0];
            ReadBinFile(mstrFilePath, ref bytRead);
            int iTemp = BIN_LOCAL_SETUP_PAGE_PW_ADDR;
            for (int i = 0; i < iPasswordLen; i++)
            {
                bytRead[iTemp] = bytPW[i];
                iTemp++;
            }
            return WriteBinFile(mstrFilePath, bytRead);
        }

        public void ReadDataFromFile(ref DataGridView dtGrid)
        {
            RefreshData();
            int iRowsNum = gmstrICType.Length;
            if (iRowsNum <= 0) return;
            for (int i = 0; i < iRowsNum; i++)
            {
                //IC类型
                int iCol = 0;
                dtGrid[iCol, i].Value = gmstrICType[i].Trim();
                //Password IIC Dev Addr
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintPwIICDevAddr[i], 2);
                //PasswordTableSel
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintPasswordTableSel[i], 2);
                //PasswordWordAddr
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintPasswordWordAddr[i], 2);
                //CDB密码
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmlngCDBPassword[i], 8);
                //REVTableSel
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintRevTableSel[i], 2);
                //RevRegAddr
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintRevRegAddr[i], 2);
                //RevLen
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintRevLen[i], 2);
                //CMDType
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintCDBType[i], 2);
                //CMDHeaderLen
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintCDBHeaderLen[i], 2);
                //CDBCMDTrigMode
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintCDBCMDTrigMode[i], 2);
                //CDBDataModel
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintCDBDataModel[i], 2);
                //CDBDataLen
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintCDBDataLen[i], 2);
                //MediaInterface
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintMediaInterface[i], 2);
                //AFETrim
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintConfigAFETrim[i], 2);
                //SCDREN
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintConfigSCDREN[i], 2);
                //CDRParams
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintConfigCDRParams[i], 2);
                //SCDRMulSel
                iCol++;
                dtGrid[iCol, i].Value = Dec2Hex(gmintConfigSCDRMulSel[i], 2);
                //BinRev
                iCol++;
                dtGrid[iCol, i].Value = gmstrBinRev[i].Trim();
                //ModuleForm
                iCol++;
                dtGrid[iCol, i].Value = gmstrModuleForm[i].Trim();
                //APP
                iCol++;
                dtGrid[iCol, i].Value = gmstrAPP[i].Trim();
                //BinName
                iCol++;
                dtGrid[iCol, i].Value = gmstrBinName[i].Trim();                
            }
        }

        public bool SaveDataToFile(DataGridView dtGrid)
        {
            int iRowsNum = dtGrid.RowCount;

            byte[] bytWrite = new byte[BIN_PAGE_LEN * (iRowsNum + 1)];//page0保存额外信息
            byte[] bytRead = new byte[0];
            ReadBinFile(mstrFilePath, ref bytRead);//读出原文件信息
            if (bytRead.Length >= BIN_FILE_HEAD_INFOR_LEN)
            {
                for (int i = 0; i < BIN_FILE_HEAD_INFOR_LEN; i++)
                {
                    bytWrite[i] = bytRead[i];
                }
            }

            for (int i = 0; i < iRowsNum; i++)
            {
                string strTemp;
                int iTemp;
                //Type
                int iCol = 0;
                if (dtGrid[iCol, i].Value == null)
                {
                    strTemp = "";
                }
                else
                {
                    strTemp = dtGrid[iCol, i].Value.ToString();
                }

                if (strTemp.Length < BIN_IC_TYPE_NAME_LEN)
                {
                    strTemp += ("").PadRight(BIN_IC_TYPE_NAME_LEN - strTemp.Length, ' ');
                }
                if (strTemp.Length > BIN_IC_TYPE_NAME_LEN)
                {
                    strTemp = strTemp.Substring(0, BIN_IC_TYPE_NAME_LEN);
                }
                byte[] bytTemp = System.Text.Encoding.Default.GetBytes(strTemp);
                int intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_IC_TYPE;
                for (int j = 0; j < BIN_IC_TYPE_NAME_LEN; j++)
                {
                    bytWrite[intTemp] = bytTemp[j];
                    intTemp++;
                }

                //Password IIC Dev Addr
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 160;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_PW_IIC_DEV_ADDR;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //PasswordTableSel
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_PW_TABSEL;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //PasswordWordAddr
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 251;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_PW_ADDR;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);                
                //CDBPassword
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 0;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_PW;
                bytWrite[intTemp] = (byte)(iTemp >> 24);
                bytWrite[intTemp + 1] = (byte)((iTemp & 0xFFFFFF) >> 16);
                bytWrite[intTemp + 2] = (byte)((iTemp & 0xFFFF) >> 8);
                bytWrite[intTemp + 3] = (byte)(iTemp & 0xFF);
                //REVTableSel
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_REV_TABSEL;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);

                //RevRegAddr
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_REVREG_ADDR;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //RevLen
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_REV_LEN;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //CDBType
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_TYPE;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //CDBHeaderLen
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_HEADERLEN;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //CDBCMDTrigMode
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_CMD_TRIGMODE;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //CDBDataModel
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_DATA_MODEL;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //CDBDataLen
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CDB_DATALEN;
                bytWrite[intTemp] = (byte)((iTemp & 0xFFFF) >> 8);
                bytWrite[intTemp + 1] = (byte)(iTemp & 0xFF);
                //MediaInterface
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_MEDIA_INTERFACE;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //AFETrim
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CONFIG_AFETRIM;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //SCDREN
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CONFIG_SCDREN;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //CDRParams
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CONFIG_CDRPARAMS;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //SCDRMulSel
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    iTemp = 2;
                }
                else
                {
                    iTemp = Hex2Dec(dtGrid[iCol, i].Value.ToString());
                }
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_CONFIG_SCDRMULSEL;
                bytWrite[intTemp] = (byte)(iTemp & 0xFF);
                //BinRev
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    strTemp = "";
                }
                else
                {
                    strTemp = dtGrid[iCol, i].Value.ToString();
                }
                if (strTemp.Length < BIN_IC_TYPE_NAME_LEN/2)
                {
                    strTemp += ("").PadRight(BIN_IC_TYPE_NAME_LEN/2 - strTemp.Length, ' ');
                }
                if (strTemp.Length > BIN_IC_TYPE_NAME_LEN/2)
                {
                    strTemp = strTemp.Substring(0, BIN_IC_TYPE_NAME_LEN/2);
                }
                bytTemp = System.Text.Encoding.Default.GetBytes(strTemp);
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_BINREV;
                for (int j = 0; j < BIN_IC_TYPE_NAME_LEN/2; j++)
                {
                    bytWrite[intTemp] = bytTemp[j];
                    intTemp++;
                }
                //ModuleForm
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    strTemp = "";
                }
                else
                {
                    strTemp = dtGrid[iCol, i].Value.ToString();
                }
                if (strTemp.Length < BIN_IC_TYPE_NAME_LEN / 2)
                {
                    strTemp += ("").PadRight(BIN_IC_TYPE_NAME_LEN / 2 - strTemp.Length, ' ');
                }
                if (strTemp.Length > BIN_IC_TYPE_NAME_LEN / 2)
                {
                    strTemp = strTemp.Substring(0, BIN_IC_TYPE_NAME_LEN / 2);
                }
                bytTemp = System.Text.Encoding.Default.GetBytes(strTemp);
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_MODULEFORM;
                for (int j = 0; j < BIN_IC_TYPE_NAME_LEN / 2; j++)
                {
                    bytWrite[intTemp] = bytTemp[j];
                    intTemp++;
                }
                //APP
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    strTemp = "";
                }
                else
                {
                    strTemp = dtGrid[iCol, i].Value.ToString();
                }
                if (strTemp.Length < BIN_IC_TYPE_NAME_LEN*2)
                {
                    strTemp += ("").PadRight(BIN_IC_TYPE_NAME_LEN*2- strTemp.Length, ' ');
                }
                if (strTemp.Length > BIN_IC_TYPE_NAME_LEN*2)
                {
                    strTemp = strTemp.Substring(0, BIN_IC_TYPE_NAME_LEN*2);
                }
                bytTemp = System.Text.Encoding.Default.GetBytes(strTemp);
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_APP;
                for (int j = 0; j < BIN_IC_TYPE_NAME_LEN*2; j++)
                {
                    bytWrite[intTemp] = bytTemp[j];
                    intTemp++;
                }
                //BinName
                iCol++;
                if (dtGrid[iCol, i].Value == null)
                {
                    strTemp = "";
                }
                else
                {
                    strTemp = dtGrid[iCol, i].Value.ToString();
                }

                if (strTemp.Length < BIN_IC_TYPE_NAME_LEN*2)
                {
                    strTemp += ("").PadRight(BIN_IC_TYPE_NAME_LEN*2 - strTemp.Length, ' ');
                }
                if (strTemp.Length > BIN_IC_TYPE_NAME_LEN*2)
                {
                    strTemp = strTemp.Substring(0, BIN_IC_TYPE_NAME_LEN*2);
                }
                bytTemp = System.Text.Encoding.Default.GetBytes(strTemp);
                intTemp = BIN_FILE_HEAD_INFOR_LEN + i * BIN_PAGE_LEN + BIN_LOCAL_BIN_NAME;
                for (int j = 0; j < BIN_IC_TYPE_NAME_LEN*2; j++)
                {
                    bytWrite[intTemp] = bytTemp[j];
                    intTemp++;
                }   
            }
            try
            {
                WriteBinFile(mstrFilePath, bytWrite);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
