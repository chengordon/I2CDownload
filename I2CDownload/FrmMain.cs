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


namespace DSPFlashDownloader
{   
    public partial class FrmMain : Form
    {
        ClsDealHexFile mclsDealHexFile = new ClsDealHexFile();
        //public ClsBase mclsModule = new ClsCMISFlashDownloader();
        private string strFilePath_Setup_ini;

        string strBinFilePath = null;
        const int AddressSize = 8;//地址位长度定为8位
        const int SectorSize = 2048;//扇区长度定为4096Bytes
        int FlashSize;
        int DisplayLineLen;
        //
        byte[,] bytFlashBinData_2Dim = new byte[2048, 4096];//保存要烧录的数据，以4096Byte为1扇区保存，可以保存2048*4096/1024=8192K
        byte[] bytFlashBinData_1Dim = new byte[1];
        ClsIniFile mclsIniFile = new ClsIniFile();
        CDatabase cdaba = new CDatabase();
        string[] str_ReportDatabase = { "UsbAdapter", "Type", "DevName", "PassNum", "TotalNum", "Date", "CostTime", "State", "HexFile" };
        int USBI2CDriver = 2; //0: TestBoard; 1:WDTBox; 2:Aardvark Box
        public I2CToolLibrary.I2C_Device i2cDevice;// = new I2CToolLibrary.I2C_Device();
        public TestboardSoftware.ClsBase mclsModule;//=new ClsBase();
        public const byte CDBTableIndex = 0x9F; //CDB表索引
        const int CDBCMDSize = 6;//指令长度定为6Bytes
        static byte DeviceAddr = 0xA0; //50
        const byte CDBCmdAddr = 0x80;
        const byte CDBDataAddr = 0x82;
        const byte BlockSize = 128;
        const UInt16 EPLDataSize = 0x800;//数据块长度定为2048Bytes
        const byte DelayTime = 5;
        bool isAbortDownload = false;
        bool m_StartProgram = false;
        bool m_LowPower = false;
        byte[] mbytVendorPW = { 0x80, 0x00, 0x00, 0x03 };
        string SoftwareType = "DSP";//FW
        private bool bTBConnected = false;//true:测试板已经连接
        string[] usbDrivers = { "C8051F340", "CP2112", "TP240141" };
        public FrmMain()
        {
            InitializeComponent();
            bt_Connect.Enabled = false;
        }
        private bool InitTestBoard()
        {
            if (bTBConnected)
            {
                lblState.Text = "Testboard is connected";
                return true;
            }
            else
            {
                lblState.Text = "Testboard is disconnected";
                return false;
            }
        }
        private bool OpenI2CPort()
        {
            bool bOk = false;
            if (USBI2CDriver == 2)
            {
                bOk = AAI2cSlave.InitAAI2cSlave();
            }
            else if (USBI2CDriver == 1)
            {
                i2cDevice = new I2CToolLibrary.I2C_Device();
                bOk = i2cDevice.I2C_Open();
                //i2cDevice.I2C_SetRate(400);
            }
            else
            {
                mclsModule = new TestboardSoftware.ClsBase();
                bOk = mclsModule.InterfaceInit();
            }
            return bOk;
        }

        private bool CloseI2CPort()
        {
            bool bOk = false;
            if (USBI2CDriver == 2)
            {
                bOk = AAI2cSlave.Close();
            }
            else if (USBI2CDriver == 1)
            {
                if (i2cDevice != null)
                    bOk = i2cDevice.I2C_Close();
            }
            else
            {
                if (mclsModule != null)
                    mclsModule.InterfaceClose();
            }
            return bOk;
        }

        private bool WriteBytes(byte SlaveAddr, byte offsetAddr, int length, byte[] wtBytes)
        {
            bool bOk = false;
            if (USBI2CDriver == 2)
            {
                bOk = AAI2cSlave.WriteBytes(offsetAddr, length, wtBytes);
            }
            else if (USBI2CDriver == 1)
            {
                bOk = i2cDevice.WriteBytes(SlaveAddr, offsetAddr, length, wtBytes);
            }
            else
            {
                bOk = mclsModule.WriteBytes(SlaveAddr, offsetAddr, length, wtBytes);
            }
            return bOk;
            
        }

        private bool ReadBytes(byte SlaveAddr, byte offsetAddr, byte length, byte[] outBytes)
        {
            bool bOk = false;
            if (USBI2CDriver == 2)
            {
                bOk = AAI2cSlave.ReadBytes(offsetAddr, length, outBytes);
            }
            else if (USBI2CDriver == 1)
            {
                bOk = i2cDevice.ReadBytes(SlaveAddr, offsetAddr, length, outBytes);
            }
            else
            {
                bOk = mclsModule.ReadBytes(SlaveAddr, offsetAddr, length, outBytes);
            }
            return bOk;

        }

        bool SoftwareReset()
        {
            byte[] cmd = { 0x08 }; //SoftwareReset
            return WriteBytes(DeviceAddr, 0x1A, 1, cmd);
        }
        bool LowPwrRequestSW(bool onoff)
        {
            byte[] cmd = new byte[1];
            ReadBytes(DeviceAddr, 0x1A, 1, cmd);
            if (onoff)
                cmd[0] |= (byte)0x10;
            else
                cmd[0] &= 0xEF;
            return WriteBytes(DeviceAddr, 0x1A, 1, cmd);
        }

        private string GetOpenFilePath(string strFileFolder, string fileFilter)
        {
            string resultFile = "";
            OpenFileDialog openFile = new OpenFileDialog(); //实例化打开对话框对象
            openFile.Filter = fileFilter;//设置打开文件筛选器
            openFile.Multiselect = false;//设置打开对话框中不能多选
            openFile.RestoreDirectory = true;//控制对话框在关闭之前是否恢复当前目录
            openFile.FilterIndex = 1;//在对话框中选择的文件筛选器的索引，如果选第一项就设为1
            openFile.AddExtension = true;//自动在文件名中添加扩展名
            openFile.CheckFileExists = true;//指示用户指定不存在的文件名
            openFile.CheckPathExists = true;//指示用户指定不存在的路径
            openFile.Title = "Open File";
            openFile.InitialDirectory = strFileFolder;
            if (openFile.ShowDialog() == DialogResult.OK) //弹出选择框
            {
                resultFile = openFile.FileName;
            }
            else
            {
                MessageBox.Show("No file was selected!", "Message");
            }
            return resultFile;
        }
        private string GetOpenBinFilePath()
        {
            string resultFile = "";
            OpenFileDialog openFile = new OpenFileDialog(); //实例化打开对话框对象
            openFile.Filter = "binary File(*.bin)|*.bin";//设置打开文件筛选器
            openFile.Multiselect = false;//设置打开对话框中不能多选
            openFile.RestoreDirectory = true;//控制对话框在关闭之前是否恢复当前目录
            openFile.FilterIndex = 1;//在对话框中选择的文件筛选器的索引，如果选第一项就设为1
            openFile.AddExtension = true;//自动在文件名中添加扩展名
            openFile.CheckFileExists = true;//指示用户指定不存在的文件名
            openFile.CheckPathExists = true;//指示用户指定不存在的路径
            openFile.Title = "Load file";
            if (openFile.ShowDialog() == DialogResult.OK) //弹出选择框
            {
                resultFile = openFile.FileName;
            }
            else
            {
                MessageBox.Show("No file was selected!", "Message");
            }
            return resultFile;
        }
        private string GetSaveBinFilePath()
        {
            string resultFile = "";
            SaveFileDialog saveFile = new SaveFileDialog(); //实例化打开对话框对象                
            saveFile.Filter = "binary File(*.bin)|*.bin";//设置打开文件筛选器
            saveFile.RestoreDirectory = true;//控制对话框在关闭之前是否恢复当前目录
            saveFile.FilterIndex = 1;//在对话框中选择的文件筛选器的索引，如果选第一项就设为1
            saveFile.AddExtension = true;//自动在文件名中添加扩展名
            //saveFile.CheckFileExists = true;//指示用户指定不存在的文件名
            saveFile.CheckPathExists = true;//指示用户指定不存在的路径
            saveFile.Title = "Save Dat";

            if (saveFile.ShowDialog() == DialogResult.OK) //弹出选择框
            {
                resultFile = saveFile.FileName;
            }
            else
            {
                MessageBox.Show("No file was Saved!", "Message");
            }
            return resultFile;
        }
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

        private void WriteBin(string filename, byte[] buffer)
        {
            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);//创建写入文件 
            BinaryWriter binreader = new BinaryWriter(fs, Encoding.ASCII);
            foreach (byte j in buffer)
            {
                binreader.Write(j);
            }
            binreader.Close();
            fs.Close();
        }
        
        public bool EnterVendorPW()//输入厂商密码
        {
            byte[] bytPW = new byte[4];
            bytPW[0] = mbytVendorPW[0];
            bytPW[1] = mbytVendorPW[1];
            bytPW[2] = mbytVendorPW[2];
            bytPW[3] = mbytVendorPW[3];
            return WriteBytes(DeviceAddr, 0x7A, 4, bytPW);
        }
        public byte CdbChkCode(byte[] bytCmd, byte[] bytData, UInt16 IDataLen)
        {
            byte chk = 0;
            int i = 0;
            chk += bytCmd[0];
            chk += bytCmd[1];
            for (i = 0; i < 3; i++)
                chk += bytData[i];

            for (i = 0; i < IDataLen; i++)
                chk += bytData[136 - 130 + i];

            chk = (byte)((chk & 0xFF) ^ 0xFF);
            return chk;
        }
        public bool WriteCDBTableSel(byte bytTable)//选表
        {
            byte[] tablesel = new byte[1];
            byte[] bytTemp = new byte[1];
            tablesel[0] = bytTable;
            if (WriteBytes(DeviceAddr, 0x7F, 1, tablesel) == false) return false; //A0 7F ['0x9F']
            if (ReadBytes(DeviceAddr, 0x7F, 1, bytTemp) == false) return false;
            if (tablesel[0] != bytTemp[0]) return false;
            return true;
        }

        public byte ReadStatus()//读状态值
        {
            byte[] tablesel = new byte[1];  //A0L byte37 
            if (ReadBytes(DeviceAddr, 37, 1, tablesel) == false)
                return 0x40;
            return tablesel[0];
        }

        private bool CDBStatusError(byte bytStatus, string errorMsg)
        {
            if ((bytStatus & 0x40) != 0)
            {
                lblStatus.Text = errorMsg;
                switch (bytStatus & 0x07)
                {
                    case 2:
                        lblStatus.Text += ":Parameter range error or not supported!";
                        break;
                    case 5:
                        lblStatus.Text += ":CdbChkCode error!";
                        break;
                    case 6:
                        lblStatus.Text += ":Password error – insufficient privilege!";
                        break;
                    default:
                        lblStatus.Text += "!"; //Failed, no specific failure code
                        break;

                }
                return false;
            }
            else
            {
                lblStatus.Text += " Success!";
                return true;
            }
        }

        public bool CDBUpgradeGetStatus()
        {
            byte[] bytCmdBuf = { 0x01, 0x00 };
            byte[] bytDataBuf = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]);
            //选取数据表
            if (WriteCDBTableSel(CDBTableIndex) == false) return false;   //A0 7F ['0x9F']

            if (WriteBytes(DeviceAddr, CDBDataAddr, 6, bytDataBuf) == false) return false;
            if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false) return false;
            return true;
        }
        public bool CDBUpgradeStart(byte[] bytData, UInt32 IFileLen, byte IDataLen)
        {
            byte[] bytCmdBuf = { 0x01, 0x01 };
            byte[] bytDataBuf = new byte[BlockSize];//{ 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            //包头6个字节
            bytDataBuf[0] = 0x0;
            bytDataBuf[1] = 0x0;
            bytDataBuf[2] = 0x48; //固定长度
            bytDataBuf[3] = 0x0; //CdbChkCode(bytCmdBuf, bytDataBuf, 0x44);
            bytDataBuf[4] = 0x0;
            bytDataBuf[5] = 0x0;

            bytDataBuf[6] = 0x00;// (byte)((IFileLen >> 24) & 0xFF);
            bytDataBuf[7] = 0x08;// (byte)((IFileLen >> 16) & 0xFF);
            bytDataBuf[8] = 0x00;// (byte)((IFileLen >> 8) & 0xFF);
            bytDataBuf[9] = 0x40;// (byte)(IFileLen & 0xFF);
            bytDataBuf[6] = (byte)((IFileLen >> 24) & 0xFF);
            bytDataBuf[7] = (byte)((IFileLen >> 16) & 0xFF);
            bytDataBuf[8] = (byte)((IFileLen >> 8) & 0xFF);
            bytDataBuf[9] = (byte)(IFileLen & 0xFF);

            bytDataBuf[10] = 0x0;
            bytDataBuf[11] = 0x0;
            bytDataBuf[12] = 0x0;
            bytDataBuf[13] = 0x0;
            Array.Copy(bytData, 0, bytDataBuf, 14, IDataLen);  // 文件前64位是校验标记
            bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]);
            //选取数据表
            if (WriteCDBTableSel(CDBTableIndex) == false) return false;   //A0 7F ['0x9F']
            byte len = (byte)(CDBCMDSize + bytDataBuf[2]);
            if (WriteBytes(DeviceAddr, CDBDataAddr, len, bytDataBuf) == false) return false;
            if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false) return false;
            return true;
        }
        public bool CDBUpgradeComplete()
        {
            byte[] bytCmdBuf = { 0x01, 0x07 };
            byte[] bytDataBuf = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]);
            //选取数据表
            if (WriteCDBTableSel(CDBTableIndex) == false) return false;   //A0 7F ['0x9F']

            if (WriteBytes(DeviceAddr, CDBDataAddr, CDBCMDSize, bytDataBuf) == false) return false;
            if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false) return false;
            return true;
        }

        public bool CDBUpgradeRunImage()
        {
            byte[] bytCmdBuf = { 0x01, 0x09 };
            byte[] bytDataBuf = { 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x64 };
            bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]);
            //选取数据表
            if (WriteCDBTableSel(CDBTableIndex) == false) return false;   //A0 7F ['0x9F']

            if (WriteBytes(DeviceAddr, CDBDataAddr, (byte)(CDBCMDSize + bytDataBuf[2]), bytDataBuf) == false) return false;
            if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false) return false;
            return true;
        }
        public bool CDBUpgradeCommitImage()
        {
            byte[] bytCmdBuf = { 0x01, 0x0a };
            byte[] bytDataBuf = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]);
            //选取数据表
            if (WriteCDBTableSel(CDBTableIndex) == false) return false;   //A0 7F ['0x9F']

            if (WriteBytes(DeviceAddr, CDBDataAddr, CDBCMDSize, bytDataBuf) == false) return false;
            if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false) return false;

            return true;
        }
        public bool CDBUpgradeDownloadLPL(UInt32 FlashAddr, byte[] bytData, UInt32 IDataLen)
        {
            UInt32 i = 0, iIndex = 0;
            UInt32 iFAddr = FlashAddr;
            byte bytStatus = 0;
            byte[] bytCmdBuf = { 0x01, 0x03 }; //0x80
            byte[] bytDataBuf = new byte[BlockSize]; // 0x82 每次只发送cmd2+data6+ offset4 + 0x40字节  一共76个字节，每次发送64个净荷 LPL长度0x44
            byte LPLDataSize = byte.Parse(txtLPLSize.Text);
            //包头6个字节
            bytDataBuf[0] = 0x0;
            bytDataBuf[1] = 0x0;
            bytDataBuf[2] = (byte)(LPLDataSize+0x04); //固定长度
            bytDataBuf[3] = 0x0; //CdbChkCode(bytCmdBuf, bytDataBuf, 0x44);
            bytDataBuf[4] = 0x0;
            bytDataBuf[5] = 0x0;
            UInt32 iDataLenTemp = IDataLen;
            UInt32 iCurWriteNum;
            do
            {
                iCurWriteNum = iDataLenTemp;
                if (iCurWriteNum > LPLDataSize) iCurWriteNum = LPLDataSize;//分段写，每次最多(BlockSize)个数据 0x40
                iDataLenTemp -= iCurWriteNum;

                bytDataBuf[6] = (byte)((iFAddr >> 24) & 0xFF);
                bytDataBuf[7] = (byte)((iFAddr >> 16) & 0xFF);
                bytDataBuf[8] = (byte)((iFAddr >> 8) & 0xFF);
                bytDataBuf[9] = (byte)(iFAddr & 0xFF);
                for (i = 0; i < LPLDataSize; i++)
                {
                    bytDataBuf[10 + i] = 0xFF;
                }
                Array.Copy(bytData, iIndex, bytDataBuf, 10, iCurWriteNum); //构造发送的数据包 包头6+地址4+净荷0x40

                //选取数据表
                if (WriteCDBTableSel(CDBTableIndex) == false)
                    return false;   //A0 7F ['0x9f']

                bytDataBuf[2] = (byte)(iCurWriteNum + 4);
                bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]); //生成校验码

                //下发数据
                if (WriteBytes(DeviceAddr, CDBDataAddr, CDBCMDSize + bytDataBuf[2], bytDataBuf) == false)
                    return false; //6+LPL len 0x44
                Thread.Sleep(5);
                //下发命令
                if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false)
                    return false;
                Thread.Sleep(10);
                //读取状态
                for (i = 0; i < 1000; i++)
                {
                    //读当前37字节的高位是否为1
                    bytStatus = ReadStatus();
                    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                    {
                        if (bytStatus == 1)
                            break;
                    }
                    Thread.Sleep(50); //需要等待,完成才能继续下发
                }

                iIndex = (UInt32)(iIndex + iCurWriteNum);
                iFAddr = (UInt32)(iFAddr + iCurWriteNum);
            } while (iDataLenTemp > 0);
            return true;
        }
        public bool CDBUpgradeDownloadEPL(UInt32 FlashAddr, byte[] bytData, UInt32 IDataLen)
        {
            UInt32 i = 0, iIndex = 0;
            UInt32 iFAddr = FlashAddr;

            byte bytStatus = 0;
            byte[] bytCmdBuf = { 0x01, 0x04 }; //CDB_CMD_FW_UPGRADE_DOWN_LOAD_EPL 0x0104
            byte[] bytDataBuf = new byte[BlockSize]; // 0x82 每次只发送cmd2+data6+ offset4 + 0x40字节  一共76个字节，每次发送64个净荷 LPL长度0x44

            UInt32 iDataLenTemp = IDataLen;
            UInt32 iCurWriteNum;

            do
            {
                iCurWriteNum = iDataLenTemp;
                if (iCurWriteNum > EPLDataSize) iCurWriteNum = EPLDataSize;//分段写，每次最多(BlockSize)个数据 0x40
                iDataLenTemp -= iCurWriteNum;

                for (int page_offset = 0; page_offset < 16; page_offset++) //16页*128 = 2048
                {
                    if (WriteCDBTableSel((byte)(0xA0 + page_offset)) == false)
                        return false;   //A0 7F ['0xA0']--AF
                    //下发数据
                    DelayMS(DelayTime);
                    Array.Copy(bytData, page_offset * 128, bytDataBuf, 0, 128);
                    if (WriteBytes(DeviceAddr, CDBCmdAddr, 0x80, bytDataBuf) == false)  // 0x80 = 128
                        return false; //6+LPL len 0x44
                    DelayMS(DelayTime);// Thread.Sleep(5);
                }

                //包头6个字节
                bytDataBuf[0] = 0x8; //EPL len 0x08 表示2048字节 128*16
                bytDataBuf[1] = 0x0; //EPL len 0x00
                bytDataBuf[2] = 0x4; //LPL固定长度,放地址偏移量
                bytDataBuf[3] = 0x0; //CdbChkCode(bytCmdBuf, bytDataBuf, 0x44);
                bytDataBuf[4] = 0x0;
                bytDataBuf[5] = 0x0;
                bytDataBuf[6] = (byte)((iFAddr >> 24) & 0xFF);
                bytDataBuf[7] = (byte)((iFAddr >> 16) & 0xFF);
                bytDataBuf[8] = (byte)((iFAddr >> 8) & 0xFF);
                bytDataBuf[9] = (byte)(iFAddr & 0xFF);

                //选取数据表
                if (WriteCDBTableSel(CDBTableIndex) == false)
                    return false;   //A0 7F ['0x9f']

                bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]); //生成校验码

                //下发数据
                if (WriteBytes(DeviceAddr, CDBDataAddr, (byte)(CDBCMDSize + bytDataBuf[2]), bytDataBuf) == false)
                    return false; //6+LPL len 0x44
                DelayMS(DelayTime); //Thread.Sleep(5);
                //下发命令
                if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false)
                    return false;
                DelayMS(DelayTime); //Thread.Sleep(10);
                //读取状态
                for (i = 0; i < 2000; i++)
                {
                    //读当前37字节的高位是否为1
                    bytStatus = ReadStatus();
                    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                    {
                        if (bytStatus == 1)
                            break;
                    }
                    DelayMS(DelayTime*10);// Thread.Sleep(50); //需要等待,完成才能继续下发
                }

                iIndex = (UInt32)(iIndex + iCurWriteNum);
                iFAddr = (UInt32)(iFAddr + iCurWriteNum);
            } while (iDataLenTemp > 0);
            return true;
        }
        public bool CDBReadFlashLPL(UInt32 FlashAddr, ref byte[] bytData, UInt32 DataLen) //112
        {
            UInt32 i = 0, iIndex = 0;
            UInt32 iFAddr = FlashAddr;
            byte[] bytRBuf = new byte[BlockSize];
            byte[] bytCtrlBuf = new byte[7];
            byte[] bytStatusBuf = new byte[1];
            byte bytStatus = 0;// bytCheckSum;
            UInt32 iDataLenTemp = DataLen;
            UInt32 iCurReadNum;
            byte LPLDataSize = 0x40;

            byte[] bytCmdBuf = { 0x01, 0x05 };
            byte[] bytDataBuf = { 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            do
            {
                //bytCheckSum = 0;
                iCurReadNum = iDataLenTemp;
                if (iCurReadNum > LPLDataSize) iCurReadNum = LPLDataSize;//分段写，每次最多(BlockSize)个数据
                iDataLenTemp -= iCurReadNum;

                //包头6个字节
                bytDataBuf[0] = 0x0; //EPL len 0x00
                bytDataBuf[1] = 0x0; //EPL len 0x00
                bytDataBuf[2] = 0x6; //LPL固定长度,放地址偏移量
                bytDataBuf[3] = 0x0; //CdbChkCode(bytCmdBuf, bytDataBuf, 0x44);
                bytDataBuf[4] = 0x0;
                bytDataBuf[5] = 0x0;
                bytDataBuf[6] = (byte)((iFAddr >> 24) & 0xFF);
                bytDataBuf[7] = (byte)((iFAddr >> 16) & 0xFF);
                bytDataBuf[8] = (byte)((iFAddr >> 8) & 0xFF);
                bytDataBuf[9] = (byte)(iFAddr & 0xFF);
                bytDataBuf[10] = 0x00; //LPL len 0x800
                bytDataBuf[11] = LPLDataSize; //LPL len 0x70


                bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]);
                //选取数据表
                if (WriteCDBTableSel(CDBTableIndex) == false) return false;   //A0 7F ['0x9F']
                //下发数据
                if (WriteBytes(DeviceAddr, CDBDataAddr, CDBCMDSize + bytDataBuf[2], bytDataBuf) == false) return false;
                Thread.Sleep(5);
                //下发命令
                if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false) return false;
                Thread.Sleep(10);
                //读取状态
                for (i = 0; i < 1000; i++)
                {
                    //读当前37字节的高位是否为1
                    bytStatus = ReadStatus();
                    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                    {
                        if (bytStatus == 1)
                            break;
                    }
                    Thread.Sleep(50); //需要等待,完成才能继续下发
                }


                if (ReadBytes(DeviceAddr, 0x80, (byte)(iCurReadNum + 12), bytRBuf) == false) return false;  //12个字节是包头开销，后面的是净荷
                Array.Copy(bytRBuf, 12, bytData, iIndex, iCurReadNum);
                Thread.Sleep(5);

                /*
                for (i = 0; i < iCurReadNum; i++)
                {
                    bytCheckSum += bytRBuf[i];
                }*/

                iIndex = (UInt32)(iIndex + iCurReadNum);
                iFAddr = (UInt32)(iFAddr + iCurReadNum);
            } while (iDataLenTemp > 0);
            return true;
        }//End of Function
        public bool CDBReadFlashEPL(UInt32 FlashAddr, ref byte[] bytData, UInt32 DataLen) //ReadCDBFlash EPL 16*128 = 2048 Byte
        {
            UInt32 i = 0, iIndex = 0;
            UInt32 iFAddr = FlashAddr;
            byte[] bytRBuf = new byte[BlockSize];
            //byte[] bytCtrlBuf = new byte[7];
            byte[] bytStatusBuf = new byte[1];
            byte bytStatus = 0;// bytCheckSum;
            UInt32 iDataLenTemp = DataLen;
            UInt32 iCurReadNum;

            byte[] bytCmdBuf = { 0x01, 0x06 };
            byte[] bytDataBuf = { 0x08, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            do
            {
                //bytCheckSum = 0;
                iCurReadNum = iDataLenTemp;
                if (iCurReadNum > EPLDataSize) iCurReadNum = EPLDataSize;//分段写，每次最多(BlockSize)个数据
                iDataLenTemp -= iCurReadNum;

                //包头6个字节
                bytDataBuf[0] = 0x8; //EPL len 0x800
                bytDataBuf[1] = 0x0; //EPL len 0x800
                bytDataBuf[2] = 0x6; //LPL固定长度,放地址偏移量
                bytDataBuf[3] = 0x0; //CdbChkCode(bytCmdBuf, bytDataBuf, 0x44);
                bytDataBuf[4] = 0x0;
                bytDataBuf[5] = 0x0;
                bytDataBuf[6] = (byte)((iFAddr >> 24) & 0xFF);
                bytDataBuf[7] = (byte)((iFAddr >> 16) & 0xFF);
                bytDataBuf[8] = (byte)((iFAddr >> 8) & 0xFF);
                bytDataBuf[9] = (byte)(iFAddr & 0xFF);
                bytDataBuf[10] = 0x8; //EPL len 0x800
                bytDataBuf[11] = 0x0; //EPL len 0x800


                bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]);
                //选取数据表
                if (WriteCDBTableSel(CDBTableIndex) == false) return false;   //A0 7F ['0x9F']
                //下发数据
                if (WriteBytes(DeviceAddr, CDBDataAddr, CDBCMDSize + bytDataBuf[2], bytDataBuf) == false) return false;
                Thread.Sleep(5);
                //下发命令
                if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false) return false;
                Thread.Sleep(10);
                //读取状态
                for (i = 0; i < 1000; i++)
                {
                    //读当前37字节的高位是否为1
                    bytStatus = ReadStatus();
                    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                    {
                        if (bytStatus == 1)
                            break;
                    }
                    Thread.Sleep(50); //需要等待,完成才能继续下发
                }
                ReadBytes(DeviceAddr, 0x80, (byte)BlockSize, bytRBuf);

                for (int page_offset = 0; page_offset < 16; page_offset++) //16页*128 = 2048
                {
                    if (WriteCDBTableSel((byte)(0xA0 + page_offset)) == false)
                        return false;   //A0 7F ['0xA0']--AF
                    //读数据
                    if (ReadBytes(DeviceAddr, 0x80, (byte)BlockSize, bytRBuf) == false) return false;
                    Array.Copy(bytRBuf, 0, bytData, (iIndex + page_offset * BlockSize), BlockSize);
                    Thread.Sleep(5);
                }

                /*
                for (i = 0; i < iCurReadNum; i++)
                {
                    bytCheckSum += bytRBuf[i];
                }*/

                iIndex = (UInt32)(iIndex + iCurReadNum);
                iFAddr = (UInt32)(iFAddr + iCurReadNum);
            } while (iDataLenTemp > 0);
            return true;
        }//End of Function
        private bool ReadFlashLPL()
        {
            lblStatus.Text = "Read Image LPL";
            EnterVendorPW();
            UInt32 DataLen = (UInt32)FlashSize * 1024;
            bytFlashBinData_1Dim = new byte[DataLen];
            UInt32 iCurReadNum;
            UInt32 iAddr = 0;
            byte[] bytRBuf = new byte[SectorSize];
            DateTime start = DateTime.Now;
            UInt32 TotalLen = DataLen;
            float percent = 0;
            while (DataLen > 0 && !isAbortDownload)
            {
                iCurReadNum = DataLen;
                if (iCurReadNum > SectorSize) iCurReadNum = SectorSize;//按扇区检查，每次最多(SectorSize)个数据
                DataLen -= iCurReadNum;
                lblStatus.Text = "Reading Image LPL " + String.Format("{0:N2}", percent) + "%";
                if (CDBReadFlashLPL(iAddr, ref bytRBuf, iCurReadNum) == false)
                {
                    DateTime stop1 = DateTime.Now;
                    lblStatus.Text = "Reading Image LPL Fail! cost " + (stop1 - start).TotalMilliseconds.ToString() + "ms";
                    return false;
                }
                percent = (TotalLen - DataLen) / (float)TotalLen * 100;
                lblStatus.Text = "Reading Image LPL " + String.Format("{0:N2}", percent) + "%";
                Array.Copy(bytRBuf, 0, bytFlashBinData_1Dim, iAddr, iCurReadNum);
                iAddr = (UInt32)(iAddr + iCurReadNum);
                DelayMS(10);
            }
            if (isAbortDownload)
            {
                lblStatus.Text = "Read Image LPL Abort!";
                return false;
            }
            else
            {
                DateTime stop = DateTime.Now;
                lblStatus.Text = "Read Image LPL Successful! cost " + (stop - start).TotalMilliseconds.ToString() + "ms";
                return true;
            }
        }
        private bool ReadFlashEPL()
        {
            lblStatus.Text = "Read Image EPL";
            EnterVendorPW();
            UInt32 DataLen = (UInt32)FlashSize * 1024;
            bytFlashBinData_1Dim = new byte[DataLen];
            UInt32 iCurReadNum;
            UInt32 iAddr = 0;
            byte[] bytRBuf = new byte[SectorSize];
            DateTime start = DateTime.Now;
            UInt32 TotalLen = DataLen;
            float percent = 0;
            while (DataLen > 0 && !isAbortDownload)
            {
                iCurReadNum = DataLen;
                if (iCurReadNum > SectorSize) iCurReadNum = SectorSize;//按扇区检查，每次最多(SectorSize)个数据
                DataLen -= iCurReadNum;
                lblStatus.Text = "Reading Image EPL " + String.Format("{0:N2}", percent) + "%";
                if (CDBReadFlashEPL(iAddr, ref bytRBuf, iCurReadNum) == false)
                {
                    DateTime stop1 = DateTime.Now;
                    lblStatus.Text = "Reading Image EPL Fail! cost " + (stop1 - start).TotalMilliseconds.ToString() + "ms";
                    return false;
                }
                percent = (TotalLen - DataLen) / (float)TotalLen * 100;
                lblStatus.Text = "Reading Image EPL " + String.Format("{0:N2}", percent) + "%";
                Array.Copy(bytRBuf, 0, bytFlashBinData_1Dim, iAddr, iCurReadNum);
                iAddr = (UInt32)(iAddr + iCurReadNum);
                DelayMS(10);
            }
            if (isAbortDownload)
            {
                lblStatus.Text = "Read Image EPL Abort!";
                return false;
            }
            else
            {
                DateTime stop = DateTime.Now;
                lblStatus.Text = "Read Image EPL Successful! cost " + (stop - start).TotalMilliseconds.ToString() + "ms";
                return true;
            }
        }
        private bool WriteFlashLPL()
        {
            lblStatus.Text = "Strat Program LPL!";
            EnterVendorPW(); //0x7A, [0x00, 0x00, 0x10, 0x11] 密码校验
            UInt32 DataLen = (UInt32)bytFlashBinData_1Dim.Length;
            UInt32 iCurWriteNum;
            UInt32 iAddr = 0;
            int i = 0;
            byte[] bytData = new byte[SectorSize];
            byte bytStatus = 0;
            DateTime start = DateTime.Now;
            UInt32 TotalLen = DataLen;
            float percent = 0;
            if (DataLen <= 1)
            {
                lblStatus.Text = "Program LPL, Please load Bin File!";
                return false;
            }
            CDBUpgradeGetStatus();
            for (i = 0; i < 1000; i++) //读当前37字节的高位是否为1
            {
                bytStatus = ReadStatus();
                if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                {
                    break;
                }
                DelayMS(1000);
            }
            if (!CDBStatusError(bytStatus, "Program LPL Fail")) return false;

            Array.Copy(bytFlashBinData_1Dim, 0, bytData, 0, 0x40);
            CDBUpgradeStart(bytData, DataLen, 0x40);
            for (i = 0; i < 1000; i++) //读当前37字节的高位是否为1
            {
                bytStatus = ReadStatus();
                if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                {
                    break;
                }
                DelayMS(1000);
            }
            if (!CDBStatusError(bytStatus, "Program LPL Fail")) return false;

            DataLen -= 0x40; //去掉头部0x40个字节
            while (DataLen > 0 && !isAbortDownload)
            {
                iCurWriteNum = DataLen;
                if (iCurWriteNum > SectorSize) iCurWriteNum = SectorSize;//分段写，每次最多(SectorSize)个数据
                DataLen -= iCurWriteNum;
                lblStatus.Text = "Program LPL " + String.Format("{0:N2}", percent) + "%";
                //Copy (Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
                Array.Copy(bytFlashBinData_1Dim, iAddr + 0x40, bytData, 0, iCurWriteNum);
                if (CDBUpgradeDownloadLPL(iAddr, bytData, iCurWriteNum) == false)
                {
                    DateTime stop1 = DateTime.Now;
                    lblStatus.Text = "Program LPL Fail Remain " + DataLen + " cost " + (stop1 - start).TotalMilliseconds.ToString() + "ms";
                    return false;
                }
                percent = (TotalLen - DataLen) / (float)TotalLen * 100;
                lblStatus.Text = "Program LPL " + String.Format("{0:N2}", percent) + "%";
                iAddr += iCurWriteNum;
                DelayMS(10);
            }
            if (isAbortDownload)
            {
                lblStatus.Text = "Program LPL Abort! Remain " + DataLen + " Byte.";
                return false;
            }
            if (DataLen != 0)
            {
                lblStatus.Text = "Program LPL Failed, Don't load bin file! DataLen = " + DataLen + " Byte.";
                return false;
            }
            else
            {
                CDBUpgradeComplete();
                for (i = 0; i < 1000; i++) //读当前37字节的高位是否为1
                {
                    bytStatus = ReadStatus();
                    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                    {
                        break;
                    }
                    DelayMS(1000);
                }
                if (!CDBStatusError(bytStatus, "Program LPL Complete Command Fail")) return false;
                //CDBUpgradeRunImage();
                //for (i = 0; i < 1000; i++) //读当前37字节的高位是否为1
                //{
                //    bytStatus = ReadStatus();
                //    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                //    {
                //        break;
                //    }
                //    DelayMS(1000);
                //}
                //if (!CDBStatusError(bytStatus, "Program LPL Run Command Fail")) return false;
            }

            DateTime stop = DateTime.Now;
            lblStatus.Text = "Program LPL Successful " + (stop - start).TotalMilliseconds.ToString() + "ms";
            return true;
        }

        private bool WriteFlashEPL()
        {
            lblStatus.Text = "Strat Download DSP!";
            string DownloadType = "Download " + SoftwareType;
            EnterVendorPW(); //输入密码校验
            UInt32 DataLen = (UInt32)bytFlashBinData_1Dim.Length;
            UInt32 iCurWriteNum;
            UInt32 iAddr = 0;
            int i = 0;
            byte[] bytData = new byte[EPLDataSize];
            byte[] bytFFData = new byte[EPLDataSize];
            byte bytStatus = 0;
            DateTime start = DateTime.Now;
            UInt32 TotalLen = DataLen;
            float percent = 0;
            if (DataLen <= 1)
            {
                lblStatus.Text = DownloadType + ", Please load Bin File! ";
                return false;
            }
            for (i = 0; i < EPLDataSize; i++)
            {
                bytFFData[i] = 0xFF;
            }
            CDBUpgradeGetStatus();
            for (i = 0; i < 1000; i++)  //读当前37字节的高位是否为1
            {
                bytStatus = ReadStatus();
                if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                {
                    break;
                }
                DelayMS(DelayTime);
            }
            if (!CDBStatusError(bytStatus, DownloadType + " Fail")) return false;

            Array.Copy(bytFlashBinData_1Dim, 0, bytData, 0, 0x40);  //文件头40个字节
            CDBUpgradeStart(bytData, DataLen, 0x40);
            for (i = 0; i < 1000; i++)  //读当前37字节的高位是否为1
            {
                bytStatus = ReadStatus();
                if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                {
                    break;
                }
                DelayMS(DelayTime);
            }
            if (!CDBStatusError(bytStatus, DownloadType + " Fail")) return false;

            DataLen -= 0x40;
            while (DataLen > 0 && !isAbortDownload)
            {
                iCurWriteNum = DataLen;
                if (iCurWriteNum > EPLDataSize) iCurWriteNum = EPLDataSize;//分段写，每次最多(SectorSize)个数据
                DataLen -= iCurWriteNum;
                lblStatus.Text = "Download " + String.Format("{0:N2}", percent) + "%";

                //Copy (Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
                Array.Copy(bytFlashBinData_1Dim, iAddr + 0x40, bytData, 0, iCurWriteNum);
                //if (bytData.SequenceEqual(bytFFData))
                //    continue;
                if (CDBUpgradeDownloadEPL(iAddr, bytData, iCurWriteNum) == false)
                {
                    DateTime stop1 = DateTime.Now;
                    lblStatus.Text = "Download Fail Remain " + DataLen + " cost " + (stop1 - start).TotalMilliseconds.ToString() + "ms";
                    return false;
                }
                percent = (TotalLen - DataLen) / (float)TotalLen * 100;
                lblStatus.Text = "Download " + String.Format("{0:N2}", percent) + "%";
                iAddr += iCurWriteNum;
                DelayMS(DelayTime);
            }
            if (isAbortDownload)
            {
                lblStatus.Text = "Download Abort! Remain " + DataLen + " Byte.";
                return false;
            }
            if (DataLen != 0)
            {
                lblStatus.Text = "Download Failed, Don't load bin file! DataLen = " + DataLen + " Byte.";
                return false;
            }
            else
            {
                CDBUpgradeComplete();
                for (i = 0; i < 1000; i++)  //读当前37字节的高位是否为1
                {
                    bytStatus = ReadStatus();
                    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                    {
                        break;
                    }
                    DelayMS(DelayTime);
                }
                if (!CDBStatusError(bytStatus, DownloadType + " Complete Command Fail")) return false;

                //CDBUpgradeRunImage();
                //for (i = 0; i < 1000; i++) //读当前37字节的高位是否为1
                //{
                //    bytStatus = ReadStatus();
                //    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                //    {
                //        break;
                //    }
                //    DelayMS(DelayTime);
                //}
                //if (!CDBStatusError(bytStatus, DownloadType + " RunImage Command Fail")) return false;
                //DelayMS(6000);
                //EnterVendorPW();
                //CDBUpgradeCommitImage();
                //for (i = 0; i < 1000; i++) //读当前37字节的高位是否为1
                //{
                //    bytStatus = ReadStatus();
                //    if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                //    {
                //        break;
                //    }
                //    DelayMS(DelayTime);
                //}
                //if (!CDBStatusError(bytStatus, DownloadType + " Commit Command Fail")) return false;

            }

            //DateTime stop = DateTime.Now;
            //lblStatus.Text = "Download Successful " + (stop - start).TotalMilliseconds.ToString() + "ms";
            return true;
        }

        public bool CDBUpgradeAbort()
        {
            byte[] bytCmdBuf = { 0x01, 0x02 };
            byte[] bytDataBuf = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            bytDataBuf[3] = CdbChkCode(bytCmdBuf, bytDataBuf, bytDataBuf[2]);
            //选取数据表
            if (WriteCDBTableSel(CDBTableIndex) == false) return false;   //A0 7F ['0x9F']

            if (WriteBytes(DeviceAddr, CDBDataAddr, CDBCMDSize, bytDataBuf) == false) return false;
            if (WriteBytes(DeviceAddr, CDBCmdAddr, 2, bytCmdBuf) == false) return false;
            return true;
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
        private void AccessInsert(string sUsbAdapter, string sType, string sDevName, string sPassNum, string sTotalNum, string sDate, string sCostTime, string sState, string sHexFile)
        {
            string cmdstr = "";
            string cmdstr1 = ") values ('";
            cmdstr = "insert into " + cdaba.DatabaseParam.TabName + " ([";

            cmdstr += ("UsbAdapter" + "],[");
            cmdstr1 += (sUsbAdapter + "','");

            cmdstr += ("Type" + "],[");
            cmdstr1 += (sType + "','");

            cmdstr += ("DevName" + "],[");
            cmdstr1 += (sDevName + "','");

            cmdstr += ("PassNum" + "],[");
            cmdstr1 += (sPassNum + "','");

            cmdstr += ("TotalNum" + "],[");
            cmdstr1 += (sTotalNum + "','");

            cmdstr += ("Date" + "],[");
            cmdstr1 += (sDate + "','");

            cmdstr += ("CostTime" + "],[");
            cmdstr1 += (sCostTime + "','");

            cmdstr += ("State" + "],[");
            cmdstr1 += (sState + "','");

            cmdstr += ("HexFile" + "]");
            cmdstr1 += (sHexFile + "')");

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
            tsTime.Text = "Time: " + GetTime();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            string strTemp = "";
            #region Load IniSetup
            strFilePath_Setup_ini = Application.StartupPath + "\\Files\\Config.ini";
            mclsIniFile.FileName = strFilePath_Setup_ini;

            //USBI2CDriver
            strTemp = mclsIniFile.ReadString("FrmMCUDownloadSetup", "USBI2CDriver", "0");
            if (strTemp.ToLower().Equals("2"))  //USBI2CDriver
            {
                USBI2CDriver = 2;
            }
            else if (strTemp.ToLower().Equals("1")) //WDT Box
            {
                USBI2CDriver = 1;
            }
            else //Test Board
            {
                USBI2CDriver = 0;
            }
            comboUSBDriver.Items.AddRange(usbDrivers);
            comboUSBDriver.SelectedIndex = USBI2CDriver;

            //HexFilePath
            strBinFilePath = mclsIniFile.ReadString("FrmMCUDownloadSetup", "strBinFilePath", "");
            txt_HexFileName.Text = strBinFilePath;


            strTemp = mclsIniFile.ReadString("FlashDownloaderSetup", "DisplayFlashSize", "512");
            string str;
            cmbBoxDisplayLength.SelectedIndex = 0;
            for (int i = 0; i < cmbBoxDisplayLength.Items.Count; i++)
            {
                str = cmbBoxDisplayLength.Items[i].ToString().ToUpper().Replace("K", "");
                if (str.Equals(strTemp))
                {
                    cmbBoxDisplayLength.SelectedIndex = i;
                    break;
                }
            }
            //
            strTemp = mclsIniFile.ReadString("FlashDownloaderSetup", "cmbBoxDisplayLineLen", "32bytes");
            cmbBoxDisplayLineLen.SelectedIndex = 0;
            for (int i = 0; i < cmbBoxDisplayLineLen.Items.Count; i++)
            {
                str = cmbBoxDisplayLineLen.Items[i].ToString().ToLower();//.Replace("bytes", "");
                if (str.Equals(strTemp))
                {
                    cmbBoxDisplayLineLen.SelectedIndex = i;
                    break;
                }
            }
            #endregion           

            string str_TableName = "Records";

            cdaba.DBParamInit();
            cdaba.DatabaseParam.DBName = Application.StartupPath + "\\Files\\MCUDownloadDatabase.mdb";
            //string[] tableArray = cdaba.getTableName();
            //if (!tableArray.Contains(str_TableName))
            //{
            //    cdaba.TableCreate(str_TableName, str_ReportDatabase);
            //}
            cdaba.DatabaseParam.TabName = str_TableName;
            cdaba.DatabaseParam.ColumnName = str_ReportDatabase;          
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            string strTemp = "";
            //if (mclsSilabUsb.USBConnected())
            //{
            //    mclsSilabUsb.USBDisconnect();
            //}

            #region Save IniSetup

            //DisableDialog            
            //strTemp = m_DisableDialog.ToString().ToLower();
            //mclsIniFile.WriteString("FrmMCUDownloadSetup", "chkDisableDialog", strTemp);

            ////USBPower            
            //strTemp = m_USBPower.ToString().ToLower();
            //mclsIniFile.WriteString("FrmMCUDownloadSetup", "chkUSBPower", strTemp);

            //USBI2CDriver            
            strTemp = USBI2CDriver.ToString();
            mclsIniFile.WriteString("FrmMCUDownloadSetup", "USBI2CDriver", strTemp);

            //HexFilePath
            strTemp = strBinFilePath;
            mclsIniFile.WriteString("FrmMCUDownloadSetup", "strBinFilePath", strTemp);
            #endregion

            this.Dispose();
        }        
        void RefreshDeviceList()
        {
            uint NumDev = 1;
            if (bTBConnected == false)
            {
                if (OpenI2CPort() == false)
                {
                    lblState.Text = "Connect  Fail";
                    //bt_Connect.Enabled = true;
                    NumDev = 0;
                }
                else
                {
                    bTBConnected = true;
                    NumDev = 1;
                }
            }
            if (NumDev > 0)
            {
                bt_Connect.Enabled = true;
            }
        }
        
        private void bt_Browse_Click(object sender, EventArgs e)
        {
            string strFileFolder;
            if (File.Exists(strBinFilePath) == false)
            {
                strBinFilePath = "";
            }

            try
            {
                strFileFolder = System.IO.Path.GetDirectoryName(strBinFilePath);
            }
            catch
            {
                strFileFolder = "";
            }
            string strPath = GetOpenFilePath(strFileFolder, "Bin (*.bin)|*.bin");
            if (string.IsNullOrEmpty(strPath)) return;
            strBinFilePath = strPath;
            mclsIniFile.WriteString("FrmMCUDownloadSetup", "strBinFilePath", strBinFilePath);
            txt_HexFileName.Text = strBinFilePath;
            ReadBin(strBinFilePath, ref bytFlashBinData_1Dim);
            rTxt_BinFile.Text = mclsDealHexFile.ConvetArrayToBinFile(bytFlashBinData_1Dim, FlashSize, DisplayLineLen, AddressSize).Trim();
        }

        private void bt_Connect_Click(object sender, EventArgs e)
        {
            bt_Connect.Enabled = false;
            lblStatus.Text = " ";
            if (bt_Connect.Text == "Connect")
            {
                if (bTBConnected == false)
                {
                    if (OpenI2CPort() == false)
                    {
                        lblState.Text = "Connect  Fail";
                        bt_Connect.Enabled = true;
                        return;
                    }
                    else
                        bTBConnected = true;
                }

                bt_Connect.Text = "Disconnect";
                lblState.Text = "Connect  Successful";

            }
            else
            {
                if (bTBConnected == true)
                {
                    CloseI2CPort();
                    bTBConnected = false;
                    bt_Connect.Text = "Connect";
                    lblState.Text = "Disconnect Successful";
                }
            }
            bt_Connect.Enabled = true;
            ReadBin(strBinFilePath, ref bytFlashBinData_1Dim);
            rTxt_BinFile.Text = mclsDealHexFile.ConvetArrayToBinFile(bytFlashBinData_1Dim, FlashSize, DisplayLineLen, AddressSize).Trim();
        }
        private void bt_Download_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            lblStatus.Text = " ";
            bt_Download.Enabled = false;

            if (strBinFilePath.Equals(""))
            {
                MessageBox.Show("Please specify a intel-hex file to Download", "Warning");
                bt_Download.Enabled = true;
                gb_Filename.Enabled = true;
                bt_Browse.Focus();
                return;
            }

            if (bytFlashBinData_1Dim.Length <= 1)
                ReadBin(strBinFilePath, ref bytFlashBinData_1Dim);
            if (!bTBConnected)
            {
                if (!OpenI2CPort())
                {
                    lblState.Text = "Failed Downloading";
                    bt_Download.Enabled = true;
                    return;
                }
                lblState.Text = "Connect Successful";
                bt_Connect.Text = "Disconnect";
                bTBConnected = true;
            }
            lblState.Text = "Downloading";
            if (WriteFlashEPL() == false)//写Flash
            {
                lblState.Text = "Failed Downloading";
                bt_Download.Enabled = true;
                return;
            }
            else
            {
                if (m_StartProgram)
                {
                    //EnterVendorPW();
                    SoftwareReset();
                    DelayMS(2000);
                }
                if (SoftwareType.Equals("DSP") == true)
                {
                    byte[] password = { 0x80, 0x00, 0x11, 0x01 };
                    WriteBytes(DeviceAddr, 0x7A, 4, password);
                    WriteCDBTableSel(0xE6);
                    byte[] fwversion = new byte[4];
                    ReadBytes(DeviceAddr, 0x88, 4, fwversion);
                    
                }
                if (m_LowPower)
                {
                    LowPwrRequestSW(true);
                }
                lblState.Text = "Succeeded Downloading";
            }

            bt_Download.Enabled = true;
            bt_Download.Focus();
            DateTime stop = DateTime.Now;
            string sCostTime = (stop - start).TotalSeconds.ToString();
            lblStatus.Text = "Download Successful " + sCostTime + "s";
            string sAdapter = usbDrivers[USBI2CDriver];//"USB " + m_DeviceNum.ToString();


            //string sAdapterSN = "USB 0";
            //string sType = "Download" + SoftwareType;
            //string sDate = start.ToString(); //获取当前时间           

            //string sHexFile = Path.GetFileName(strBinFilePath);
            //AccessInsert(sAdapter, sType, sDevName, sPassNum, sTotalNum, sDate, sCostTime, sState, sHexFile);

            bt_Download.Enabled = true;
            bt_Download.Focus();
        }
     
        
        private void comboUSBDriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            CloseI2CPort();
            bt_Connect.Enabled = false;
            USBI2CDriver = comboUSBDriver.SelectedIndex;
            bTBConnected = false;// OpenI2CPort();
            bt_Connect.Text = "Connect";
            lblState.Text = "Switch USB Driver";
            //cmb_DeviceList.Enabled = true;
            //cmb_DeviceList.Items.Clear();
            RefreshDeviceList();       
        }

        private void cmbBoxDisplayLength_SelectedIndexChanged(object sender, EventArgs e)
        {
            //4K,8K,16K,32K,64K,128K,256K,512K,1024K,2048K,4096K,8192K
            string strTemp = cmbBoxDisplayLength.Text.Trim();
            strTemp = strTemp.ToUpper().Replace("K", "");
            FlashSize = Convert.ToInt16(strTemp);

            if (string.IsNullOrEmpty(rTxt_BinFile.Text.Trim())) return;

            rTxt_BinFile.Text = mclsDealHexFile.ConvetArrayToBinFile(bytFlashBinData_1Dim, FlashSize, DisplayLineLen, AddressSize);
        }

        private void cmbBoxDisplayLineLen_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strTemp = cmbBoxDisplayLineLen.Text.Trim();
            strTemp = strTemp.ToString().ToLower().Replace("bytes", "");
            DisplayLineLen = Convert.ToInt16(strTemp);

            if (string.IsNullOrEmpty(rTxt_BinFile.Text.Trim())) return;
            rTxt_BinFile.Text = mclsDealHexFile.ConvetArrayToBinFile(bytFlashBinData_1Dim, FlashSize, DisplayLineLen, AddressSize);
        }

        private void btn_LoadDatBin_Click(object sender, EventArgs e)
        {
            string strPath = GetOpenBinFilePath();

            if (File.Exists(strPath) == false) return;
            
            mclsIniFile.WriteString("FlashDownloaderSetup", "strHexFilePath", strPath);
            txt_HexFileName.Text = strPath;
            ReadBin(strPath, ref bytFlashBinData_1Dim);
            //
            rTxt_BinFile.Text = mclsDealHexFile.ConvetArrayToBinFile(bytFlashBinData_1Dim, FlashSize, DisplayLineLen, AddressSize).Trim();
        }

        private void btn_SaveDatBin_Click(object sender, EventArgs e)
        {
            string strPath = GetSaveBinFilePath();
            if (string.IsNullOrEmpty(strPath)) return;

            if (string.IsNullOrEmpty(rTxt_BinFile.Text.Trim())) return;

            WriteBin(strPath, bytFlashBinData_1Dim);
        }

        private void btn_TBPowerOff_Click(object sender, EventArgs e)
        {
            if (InitTestBoard() == false) return;
            if (mclsModule.SetTBdPowerOnOff(false) == false) return;
            lblStatus.Text = "Testboard PowerOFF"; 
        }

        private void btn_TBPowerOn_Click(object sender, EventArgs e)
        {
            if (InitTestBoard() == false) return;
            if (mclsModule.SetTBdPowerOnOff(true) == false) return;
            lblStatus.Text = "Testboard PowerON";
        }

        private void btn_GetStatus_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Get Firmware Info(0x0100h)";
            byte bytStatus = 0;
            EnterVendorPW(); //输入密码校验

            CDBUpgradeGetStatus();
            for (int i = 0; i < 1000; i++) //读当前37字节的高位是否为1
            {
                bytStatus = ReadStatus();
                if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                {
                    break;
                }
                DelayMS(1000);
            }
            if (!CDBStatusError(bytStatus, "Get Firmware Info Fail")) return;
            byte[] tablesel = new byte[120];  //A0L byte37 
            if (ReadBytes(0xA0, 0x88, 120, tablesel) == true)
            {
                ImageAVer.Text = tablesel[2].ToString() + "." + tablesel[3].ToString();
                ImageBVer.Text = tablesel[38].ToString() + "." + tablesel[39].ToString();
                if ((tablesel[0] & 0x01) != 0)
                    RunImage.Text = "A";
                if ((tablesel[0] & 0x10) != 0)
                    RunImage.Text = "B";
                if ((tablesel[0] & 0x02) != 0)
                    CommitImage.Text = "A";
                if ((tablesel[0] & 0x20) != 0)
                    CommitImage.Text = "B";
            }
        }

        private void btn_WriteEPL_Click(object sender, EventArgs e)
        {
            //"CDB_CMD_FW_UPGRADE_DOWN_LOAD_EPL"  : 0x0104,
            if (bytFlashBinData_1Dim.Length <= 1)
                ReadBin(strBinFilePath, ref bytFlashBinData_1Dim);
            if (!bTBConnected)
            {
                if (!OpenI2CPort())
                {
                    lblState.Text = "Failed Downloading";
                    bt_Download.Enabled = true;
                    return;
                }
                lblState.Text = "Connect Successful";
                bt_Connect.Text = "Disconnect";
                bTBConnected = true;
            }
            isAbortDownload = false;
            if (InitTestBoard() == false) return;

            gb_ReadImage.Enabled = gb_Function.Enabled = false;

            if (WriteFlashEPL() == false)//写Flash
            {
                gb_ReadImage.Enabled = gb_Function.Enabled = true;
                return;
            }
            gb_ReadImage.Enabled = gb_Function.Enabled = true;
        }

        private void btn_WriteLPL_Click(object sender, EventArgs e)
        {
            //"CDB_CMD_FW_UPGRADE_DOWN_LOAD"      : 0x0103,
            isAbortDownload = false;
            if (InitTestBoard() == false) return;

            gb_ReadImage.Enabled = gb_Function.Enabled = false;

            if (WriteFlashLPL() == false)//写Flash
            {
                gb_ReadImage.Enabled = gb_Function.Enabled = true;
                return;
            }
            gb_ReadImage.Enabled = gb_Function.Enabled = true;
        }

        private void btn_AbortDownload_Click(object sender, EventArgs e)
        {
            //"CDB_CMD_FW_UPGRADE_ABORT"          : 0x0102,
            lblStatus.Text = "Abort Firmware Download(0x0102h)";
            isAbortDownload = true;
            byte bytStatus = 0;
            EnterVendorPW();
            CDBUpgradeAbort();
            for (int i = 0; i < 1000; i++)  //读当前37字节的高位是否为1
            {
                bytStatus = ReadStatus();
                if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                {
                    break;
                }
                DelayMS(DelayTime);
            }
            CDBStatusError(bytStatus, "Abort Firmware Download Fail");
        }

        private void btn_CopyImageA2B_Click(object sender, EventArgs e)
        {
            //"CDB_CMD_FW_UPGRADE_JUMP_IMAGE"     : 0x0109,
            isAbortDownload = false;
            lblStatus.Text = "Don't support CopyImageA2B(0x0109)!";
        }

        private void btn_CopyImageB2A_Click(object sender, EventArgs e)
        {
            //"CDB_CMD_FW_UPGRADE_JUMP_IMAGE"     : 0x0109,
            isAbortDownload = false;
            lblStatus.Text = "Don't support CopyImageB2A(0x0109)!";
        }

        private void btn_RunImage_Click(object sender, EventArgs e)
        {
            byte bytStatus = 0;
            int i = 0;
            lblStatus.Text = "Strat Run Image!";
            //"CDB_CMD_FW_UPGRADE_RUN_IMAGE"     : 0x0109,
            isAbortDownload = false;
            if (InitTestBoard() == false) return;
            EnterVendorPW();
            gb_ReadImage.Enabled = gb_Function.Enabled = false;

            CDBUpgradeRunImage();
            for (i = 0; i < 1000; i++) //读当前37字节的高位是否为1
            {
                bytStatus = ReadStatus();
                if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                {
                    break;
                }
                DelayMS(1000);
            }
            CDBStatusError(bytStatus, "CDBUpgrade Run Image Command Fail");
            gb_ReadImage.Enabled = gb_Function.Enabled = true;
        }

        private void btn_CommitImage_Click(object sender, EventArgs e)
        {
            //"CDB_CMD_FW_UPGRADE_COMMIT"         : 0x010A,
            lblStatus.Text = "Strat Commit Image!";
            byte bytStatus = 0;
            isAbortDownload = false;
            if (InitTestBoard() == false) return;
            EnterVendorPW();
            CDBUpgradeCommitImage();
            for (int i = 0; i < 1000; i++) //读当前37字节的高位是否为1
            {
                bytStatus = ReadStatus();
                if ((bytStatus & 0x80) == 0) //0x1000 0xxx 表示mcu在忙Busy processing command,
                {
                    break;
                }
                DelayMS(1000);
            }
            CDBStatusError(bytStatus, "CDBUpgrade Commit Command Fail");
        }

        private void btn_ReadLPL_Click(object sender, EventArgs e)
        {
            isAbortDownload = false;
            if (InitTestBoard() == false) return;

            gb_ReadImage.Enabled = gb_Function.Enabled = false;

            if (ReadFlashLPL() == false)//读Flash
            {
                gb_ReadImage.Enabled = gb_Function.Enabled = true;
                return;
            }

            rTxt_BinFile.Text = mclsDealHexFile.ConvetArrayToBinFile(bytFlashBinData_1Dim, FlashSize, DisplayLineLen, AddressSize).Trim();
            txt_HexFile.Text = mclsDealHexFile.ConvetBinFileToHexFile(bytFlashBinData_1Dim).Trim();
            gb_ReadImage.Enabled = gb_Function.Enabled = true;
        }
        private void btn_ReadEPL_Click(object sender, EventArgs e)
        {
            isAbortDownload = false;
            if (InitTestBoard() == false) return;

            gb_ReadImage.Enabled = gb_Function.Enabled = false;

            if (ReadFlashEPL() == false)//读Flash
            {
                gb_ReadImage.Enabled = gb_Function.Enabled = true;
                return;
            }

            rTxt_BinFile.Text = mclsDealHexFile.ConvetArrayToBinFile(bytFlashBinData_1Dim, FlashSize, DisplayLineLen, AddressSize).Trim();
            txt_HexFile.Text = mclsDealHexFile.ConvetBinFileToHexFile(bytFlashBinData_1Dim).Trim();
            gb_ReadImage.Enabled = gb_Function.Enabled = true;
        }
        private void btn_ExportImage_Click(object sender, EventArgs e)
        {
            isAbortDownload = false;
            string strPath = GetSaveBinFilePath();
            if (string.IsNullOrEmpty(strPath)) return;

            if (string.IsNullOrEmpty(rTxt_BinFile.Text.Trim())) return;

            WriteBin(strPath, bytFlashBinData_1Dim);
        }

        
    }
}
