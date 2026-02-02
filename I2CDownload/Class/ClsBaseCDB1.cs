using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;//DllImport
//using System.Collections;
using System.Windows.Forms;

namespace DSPFlashProductionDownload
{
    public class ClsBaseCDB
    {
        #region Variate Define
        public int gintIICChnl = 1;

        public byte gbytPasswordDevAddr = 0xA0;//模块密码输入的IIC器件地址
        public byte gbytPasswordTableSel = 0x00;//密码输入的IIC选表        
        public byte gbytPasswordWordAddr = 122;//升级的标志位的字节地址

        public byte gbytRevTableSel = 3;//Rev读取的IIC选表
        public byte gbytRevRegAddr = 251;//Rev寄存器地址
        public byte gbytRevLen = 6;//Rev寄存器地址

        public byte gbytCDBType = 0x00;//CDB类型(0:DSP;1:MCU)
        public byte gbytCDBHeaderLen = 0x40;//CDB头文件长度
        public byte gbytCDBCMDTrigMode = 0x00;//CDB指令格式(0:CMID;1:STOP)
        public byte gbytCDBDataModel = 0x00;//CDB数据格式(0:LPL;1:EPL)
        public byte gbytCDBCMDTableSel = 0x9F;//CDB指令表索引
        public byte gbytCDBDataTableBase = 0xA0;//CDB数据表开头
        public byte gbytCDBLPLDataSize = 116;//数据块长度定为116Bytes
        public ushort gwCDBEPLDataSize = 0x800;//数据块长度定为2048Bytes

        byte[] mbytUserPW = { 0x53, 0x46, 0x50, 0x58 };//用户密码
        byte[] mbytVendorPW = { 0xD6, 0x07, 0x42, 0xC6 };//厂商密码
        #endregion

        #region Const Define
        //
        const byte BLOCKSIZE = 128;//数据块长度

        #region CDB Module Commands
        const int CMD_CDB_QueryStatus = 0x0000;//Get Query Status
        const int CMD_CDB_EnterPassword = 0x0001;//Enter Password
        const int CMD_CDB_ChangePassword = 0x0002;//Change Password
        const int CMD_CDB_AbortProcessing = 0x0004;//Abort Processing
        #endregion

        #region CDB Feature and Capabilities Commands
        const int CMD_CDB_ModuleFeatures = 0x0040;//Get Module Features
        const int CMD_CDB_FirmwareFeatures = 0x0041;//Firmware Management Features
        const int CMD_CDB_PerformanceFeatures = 0x0042;//Performance Monitoring Features
        const int CMD_CDB_BERTFeatures = 0x0043;//BERT and Diagnostic Features
        const int CMD_CDB_SecurityFeatures = 0x0044;//Security Features
        const int CMD_CDB_ExternallyFeatures = 0x0045;//Externally Defined Features
        #endregion

        #region CDB Firmware Download Commands
        const int CMD_CDB_GetStatus = 0x0100;//Get Firmware Info
        const int CMD_CDB_Start = 0x0101;//Start Firmware Download
        const int CMD_CDB_Abort = 0x0102;//Abort Firmware Download
        const int CMD_CDB_LPLWrite = 0x0103;//Write Firmware Block LPL
        const int CMD_CDB_EPLWrite = 0x0104;//Write Firmware Block EPL
        const int CMD_CDB_LPLRead = 0x0105;//Read Firmware Block LPL
        const int CMD_CDB_EPLRead = 0x0106;//Read Firmware Block EPL
        const int CMD_CDB_Complete = 0x0107;//Complete Firmware Download
        const int CMD_CDB_Copy = 0x0108;//Copy Firmware Image
        const int CMD_CDB_Run = 0x0109;//Run Firmware Image
        const int CMD_CDB_Commit = 0x010A;//Commit Image
        #endregion

        #region CDB Performance Monitoring Commands
        const int CMD_CDB_ControlPM = 0x0200;//General Performance Monitoring Controls
        const int CMD_CDB_GetPMFeature = 0x0201;//Advertisement on optional PM is supported
        const int CMD_CDB_GetPMModuleLPL = 0x0210;//Get Module-level X16 PM using LPL
        const int CMD_CDB_GetPMModuleEPL = 0x0211;//Get Module-level X16 PM using EPL
        const int CMD_CDB_GetPMHostSideLPL = 0x0212;//Get Lane-specific host side X16 PM using LPL
        const int CMD_CDB_GetPMHostSideEPL = 0x0213;//Get Lane-specific host side X16 PM using EPL
        const int CMD_CDB_GetPMMediaSideLPL = 0x0214;//Get Lane-specific media side X16 PM using LPL
        const int CMD_CDB_GetPMMediaSideEPL = 0x0215;//Get Lane-specific media side X16 PM using EPL
        const int CMD_CDB_GetPMDataPathLPL = 0x0216;//Get Lane-specific Data Path X16 PM using LPL
        const int CMD_CDB_GetPMDataPathEPL = 0x0217;//Get Lane-specific Data Path X16 PM using EPL
        const int CMD_CDB_GetDataPathRMONStatistics = 0x0220;//Get Data Path RMON Statistics using LPL
        const int CMD_CDB_ControlDataPathSEWHistogram = 0x0230;//Control FEC Symbol Error Weight Histogram
        const int CMD_CDB_GetDataPathSEWHistogram = 0x0231;//Get FEC Symbol Error Weight Histogram
        const int CMD_CDB_ControlDataPathSEWmaxStats = 0x0232;//Control Max FEC Symbol Error Weight Statistics
        const int CMD_CDB_GetDataPathSEWmaxStats = 0x0233;//Get Max FEC Symbol Error Weight Statistics
        #endregion

        #region CDB BERT Commands
        const int CMD_CDB_PRBSRelated = 0x0300;//PRBS related capabilities
        //const int CMD_CDB_PRBSRelated = 0x030x;//PRBS related capabilities
        //const int CMD_CDB_PRBSGenerator = 0x030x;//PRBS Generator Config, Enable and Disable
        //const int CMD_CDB_PRBSErrorInjector = 0x030x;//PRBS Error Injector
        //const int CMD_CDB_PRBSDetector = 0x030x;//PRBS Detector (Checker) Config, Enable and Disable
        //const int CMD_CDB_PRBSErrorCounts = 0x030x;//PRBS Error Counts
        //const int CMD_CDB_PRBSBER = 0x030x;//PRBS BER
        const int CMD_CDB_Loopbacks = 0x0380;//Loopbacks
        const int CMD_CDB_PAM4Histogram = 0x0390;//PAM4 Histogram
        const int CMD_CDB_EyeMonitors = 0x03A0;//Eye Monitors
        #endregion

        #region CDB Diagnostics and Debug Commands
        const int CMD_CDB_GetIDevIDCertificateLPL = 0x0400;//Get IDevID Certificate in LPL
        const int CMD_CDB_GetIDevIDCertificateEPL = 0x0401;//Get IDevID Certificate in EPL
        const int CMD_CDB_SetDigestToSigngivenLPL = 0x0402;//Set Digest To Sign given in LPL
        const int CMD_CDB_SetDigestToSigngivenEPL = 0x0403;//Set Digest To Sign given in EPL
        const int CMD_CDB_GetDigestSignatureLPL = 0x0404;//Get Digest Signature in LPL
        const int CMD_CDB_GetDigestSignatureEPL = 0x0405;//Get Digest Signature in EPL

        const int Security_ReturnStatus_OK = 0x0000;//Successful command completion

        const int Security_OK = 0x0000;//Successful command completion
        const int Security_CommunicationError = 0x0001;//General internal communication error
        const int Security_UnexpectedError = 0x0002;//General unspecific error
        const int Security_UserCommandDataError = 0x0010;//General command data error
        const int Security_UserNoDigestError = 0x0011;//Signature request without digest supplied before
        const int Security_UserNotReadyError = 0x0012;//Signature request during processing
        const int Security_SecInvalidPrinateKey = 0x0020;//Invalid private key
        const int Security_SecInvalidPublicKey = 0x0021;//Invalid public key in certificate
        const int Security_SecDeviceNotFound = 0x0022;//Security device not present or not operational
        const int Security_SecDeviceReadError = 0x0023;//Security device read error
        const int Security_SecDeviceWriteError = 0x0024;//Security device write error 
        #endregion
        #endregion

        #region DllImport

        [DllImport("UsbConvertorI2c")]
        public static extern void hfInitI2c();
        [DllImport("UsbConvertorI2c")]
        public static extern void hfUnInitI2c();

        [DllImport("UsbConvertorI2c")]
        public static extern int hfSetI2CRateBps(int rate);

        [DllImport("UsbConvertorI2c")]////带数据地址的I2C读写,返回实际的读写长度
        public static extern int hfWriteAddrI2c(Byte i2cAddr, Byte DataAddr, int Writelen, Byte[] WriteDataBuf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfReadAddrI2c(Byte i2cAddr, Byte DataAddr, int ReadLen, Byte[] WriteDataBuf);

        [DllImport("UsbConvertorI2c")]////直接I2C读写,返回实际的读写长度
        public static extern int hfWriteI2c(Byte i2cAddr, int Writelen, Byte[] WriteDataBuf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfReadI2c(Byte i2cAddr, int ReadLen, Byte[] ReadDataBuf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfGetSn(Byte[] SnBuf, int size);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfSetSn(Byte[] SnBuf, int size);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfReadFlash(int Addr, float[] buf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfReadFlash(int Addr, UInt16[] buf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfReadFlash(int Addr, byte[] buf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfWriteFlash(int Addr, int wlen, float[] buf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfWriteFlash(int Addr, int wlen, UInt16[] buf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfWriteFlash(int Addr, int wlen, byte[] buf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfGpioSetMode(int Portmasking, int WriteLen, Byte[] WriteDataBuf); 
        [DllImport("UsbConvertorI2c")]       
        public static extern int hfGpioWrite(int Portmasking, int WriteLen, Byte[] WriteDataBuf);
        [DllImport("UsbConvertorI2c")]        
        public static extern int hfGpioRead(int Portmasking, int ReadLen, Byte[] ReadDataBuf);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfCalVCSens(float Txvlt, float Rxvlt, float Txcrt, float Rxcrt);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfCalSlopeSens(float TxvltSlope, float RxvltSlope, float TxcrtSlope, float RxcrtSlope);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfSetBoard(int ben, int TxVccEn, int RxVccEn, int TxVltSet, int RxVltSet);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfReadBoard(ref TestBoardParamStruct pTestBoardParam);
        [DllImport("UsbConvertorI2c")]
        public static extern int hfReadBoardInt(ref TestBoardParamStruct TestBoardParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct TestBoardParamStruct
        {
            public byte BenValue;
            public byte TxVccEn;
            public byte RxVccEn;
            public byte TxVltSet;
            public byte RxVltSet;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public byte[] PortPIn;
            //public int Temp;
            //public int TxVlt;
            //public int RxVlt;
            //public int TxCrt;
            //public int RxCrt;
            public float Temp;
            public float TxVlt;
            public float RxVlt;
            public float TxCrt;
            public float RxCrt;  
        }
        #endregion //DllImport

        CP2112Library.CP2112_Device CP2112Device = new CP2112Library.CP2112_Device();
        AardvarkLibrary.Aardvark_Device Aardvark_Device = new AardvarkLibrary.Aardvark_Device();

        public bool DelayMS(int delayTime_ms)
        {
            DateTime now = DateTime.Now;
            int s;
            do
            {
                TimeSpan spand = DateTime.Now - now;
                s = (int)spand.TotalMilliseconds;
                //Application.DoEvents();
            }
            while (s < delayTime_ms);
            return true;
        }

        #region TestBoard Control
        public bool WriteBytes(Byte I2C_Address, Byte DstAddr, int WriteDataNum, Byte[] WriteDataBuf)
        {
            bool bState;
            if (gintIICChnl == 2)
            {
                if (Aardvark_Device.WriteBytes(I2C_Address, DstAddr, WriteDataNum, WriteDataBuf))
                {
                    bState = true;//写入成功
                }
                else
                {
                    bState = false;//写入失败
                }

            }
            else if (gintIICChnl == 1)
            {
                if (CP2112Device.WriteBytes(I2C_Address, DstAddr, WriteDataNum, WriteDataBuf))
                {
                    bState = true;//写入成功
                }
                else
                {
                    bState = false;//写入失败
                }

            }
            else
            {
                if (hfWriteAddrI2c(I2C_Address, DstAddr, WriteDataNum, WriteDataBuf) == WriteDataNum)
                {
                    bState = true;//写入成功
                }
                else
                {
                    bState = false;//写入失败
                }
            }
            
            return bState;
        }
        public bool ReadBytes(Byte I2C_Address, Byte DstAddr, Byte ReadDataNum, Byte[] ReadDataBuf)
        {
            int intTemp = ReadDataNum;
            bool bState;
            if (gintIICChnl == 2)
            {
                if (Aardvark_Device.ReadBytes(I2C_Address, DstAddr, ReadDataNum, ReadDataBuf))
                {
                    bState = true;//读取成功
                }
                else
                {
                    bState = false;//读取失败
                }
            }
            else if (gintIICChnl == 1)
            {
                if (CP2112Device.ReadBytes(I2C_Address, DstAddr, ReadDataNum, ReadDataBuf))
                {
                    bState = true;//读取成功
                }
                else
                {
                    bState = false;//读取失败
                }
            }
            else
            {
                if (hfReadAddrI2c(I2C_Address, DstAddr, ReadDataNum, ReadDataBuf) == intTemp)
                {
                    bState = true;//读取成功
                }
                else
                {
                    bState = false;//读取失败
                }
            }            
            return bState;
        }
        public bool CurWriteBytes(Byte I2C_Address, Byte WriteDataNum, Byte[] WriteDataBuf)
        {
            bool bState;
            if (gintIICChnl == 2)
            {
                if (Aardvark_Device.CurrentWriteBytes(I2C_Address, WriteDataNum, WriteDataBuf))
                {
                    bState = true;//读取成功
                }
                else
                {
                    bState = false;//读取失败
                }
            }
            else if (gintIICChnl == 1)
            {
                if (CP2112Device.CurrentWriteBytes(I2C_Address, WriteDataNum, WriteDataBuf))
                {
                    bState = true;//读取成功
                }
                else
                {
                    bState = false;//读取失败
                }
            }
            else
            {
                if (hfWriteI2c(I2C_Address, WriteDataNum, WriteDataBuf) == WriteDataNum)
                {
                    bState = true;//写入成功
                }
                else
                {
                    bState = false;//写入失败
                }
            }            
            return bState;
        }
        public bool CurReadBytes(Byte I2C_Address, Byte ReadDataNum, Byte[] ReadDataBuf)
        {
            int intTemp = ReadDataNum;
            bool bState;
            if (gintIICChnl == 2)
            {
                if (Aardvark_Device.CurrentReadBytes(I2C_Address, ReadDataNum, ReadDataBuf))
                {
                    bState = true;//读取成功
                }
                else
                {
                    bState = false;//读取失败
                }

            }
            else if (gintIICChnl == 1)
            {
                if (CP2112Device.CurrentReadBytes(I2C_Address, ReadDataNum, ReadDataBuf))
                {
                    bState = true;//读取成功
                }
                else
                {
                    bState = false;//读取失败
                }

            }
            else
            {
                if (hfReadI2c(I2C_Address, ReadDataNum, ReadDataBuf) == intTemp)
                {
                    bState = true;//读取成功
                }
                else
                {
                    bState = false;//读取失败
                }
            }
            
            return bState;
        }

        public bool InterfaceInit()
        {
            if (gintIICChnl == 2)
            {
                Aardvark_Device.deviceNum = 0;
                Aardvark_Device.I2C_Open();
                if (Aardvark_Device.I2C_SetRate(800) == false)
                {
                    Aardvark_Device.I2C_Close();
                    return false;//0:设置失败；1：设置成功
                }                    
            }
            else if (gintIICChnl == 1)
            {
                CP2112Device.deviceNum = 0;
                CP2112Device.I2C_Open();
                if (CP2112Device.I2C_SetRate(400) == false)
                {
                    CP2112Device.I2C_Close();
                    return false;//0:设置失败；1：设置成功
                }                    
            }
            else
            {
                hfInitI2c();
                if (hfSetI2CRateBps(200) == 0) return false;//0:设置失败；1：设置成功
            }
            return true;
        }

        public int InterfaceClose()
        {
            if (gintIICChnl == 2)
            {
                Aardvark_Device.I2C_Close();
            }
            else if (gintIICChnl == 1)
            {
                CP2112Device.I2C_Close();
            }
            else
            {
                hfUnInitI2c();
            }            
            return 0;
        }        

        public int ReadTestBoard(ref TestBoardParamStruct TestBoardP)
        {
            hfReadBoardInt(ref TestBoardP);
            return 0;
        }

        //
        //测试板上电关电
        public bool SetTBdPowerOnOff(bool bOnOff)
        {
            TestBoardParamStruct TestBoardP = new TestBoardParamStruct();
            int intTemp = hfReadBoardInt(ref TestBoardP);//0:读取失败；1：读取成功
            if (intTemp == 0) return false;
            if (bOnOff == false)//Close Board Power
            {
                intTemp = hfSetBoard(TestBoardP.BenValue, 0, TestBoardP.RxVccEn, TestBoardP.TxVltSet, TestBoardP.RxVltSet);//0:设置失败；1：设置成功

                DelayMS(200);                
            }
            else //Open Board Power
            {
                intTemp = hfSetBoard(TestBoardP.BenValue, 1, TestBoardP.RxVccEn, TestBoardP.TxVltSet, TestBoardP.RxVltSet);

                DelayMS(200);                
            }
            if (intTemp == 0) return false;//0:设置失败；1：设置成功
            return true;
        }        
        //测试板BEN控制
        public bool SetTBdBenOnOff(bool bOnOff)
        {
            TestBoardParamStruct TestBoardP = new TestBoardParamStruct();
            int intTemp = hfReadBoardInt(ref TestBoardP);//0:读取失败；1：读取成功
            if (intTemp == 0) return false;
            if (bOnOff == false)//Close Board Power
            {
                intTemp = hfSetBoard(0, TestBoardP.TxVccEn, TestBoardP.RxVccEn, TestBoardP.TxVltSet, TestBoardP.RxVltSet);//0:设置失败；1：设置成功
                DelayMS(200);                
            }
            else //Open Board Power
            {
                intTemp = hfSetBoard(1, TestBoardP.TxVccEn, TestBoardP.RxVccEn, TestBoardP.TxVltSet, TestBoardP.RxVltSet);
                DelayMS(200);               
            }
            if (intTemp == 0) return false;//0:设置失败；1：设置成功
            return true;
        }
        #endregion

        #region Module Control
        public bool WriteTableSel(byte bytTable)//选表
        {
            byte[] tablesel = new byte[1];
            byte[] bytTemp = new byte[1];
            tablesel[0] = bytTable;
            if (WriteBytes(gbytPasswordDevAddr, 0x7F, 1, tablesel) == false) return false;
            if (ReadBytes(gbytPasswordDevAddr, 0x7F, 1, bytTemp) == false) return false;
            if (tablesel[0] != bytTemp[0]) return false;
            return true;
        }        
        public byte ReadTableSel()//读选表值
        {
            byte[] tablesel = new byte[1];
            if (ReadBytes(gbytPasswordDevAddr, 0x7F, 1, tablesel) == false) return 0;
            return tablesel[0];
        }

        public bool EnterPW(byte[] bytPW)//输入密码
        {
            return WriteBytes(gbytPasswordDevAddr, gbytPasswordWordAddr, 4, bytPW);
        }
        public bool SetUserPW(byte[] bytPW)
        {
            if (bytPW.Length != 4) return false;
            for (int i = 0; i < 4; i++)
            {
                mbytUserPW[i] = bytPW[i];
            }
            return true;
        }
        public bool SetVendorPW(byte[] bytPW)
        {
            if (bytPW.Length != 4) return false;
            for (int i = 0; i < 4; i++)
            {
                mbytVendorPW[i] = bytPW[i];
            }
            return true;
        }
        public bool EnterUserPW()//输入用户密码
        {
            byte[] bytPW = new byte[4];
            if (!WriteTableSel((byte)gbytPasswordTableSel)) return false;
            bytPW[0] = mbytUserPW[0];
            bytPW[1] = mbytUserPW[1];
            bytPW[2] = mbytUserPW[2];
            bytPW[3] = mbytUserPW[3];
            return WriteBytes(gbytPasswordDevAddr, gbytPasswordWordAddr, 4, bytPW);
        }
        public bool EnterVendorPW()//输入厂商密码
        {
            byte[] bytPW = new byte[4];
            if (!WriteTableSel((byte)gbytPasswordTableSel)) return false;
            bytPW[0] = mbytVendorPW[0];
            bytPW[1] = mbytVendorPW[1];
            bytPW[2] = mbytVendorPW[2];
            bytPW[3] = mbytVendorPW[3];
            return WriteBytes(gbytPasswordDevAddr, gbytPasswordWordAddr, 4, bytPW);
        }

        public bool CheckI2CRespond()//正常状态下检查I2C是否响应读操作来判断模块是否在位
        {
            byte[] bytRBuf = new byte[1];
            return CurReadBytes((byte)gbytPasswordDevAddr, 1, bytRBuf);
        }

        public string GetModuleRev()
        {
            string strTemp = "";
            byte[] bytTemp = new byte[gbytRevLen];
            if (EnterVendorPW() == false) return strTemp;
            if (!WriteTableSel((byte)gbytRevTableSel)) return strTemp;
            DelayMS(10);
            if (ReadBytes(gbytPasswordDevAddr, gbytRevRegAddr, gbytRevLen, bytTemp) == false) return strTemp;
            for (int i = 0; i < gbytRevLen;i++ )
            {                
                if(i == (gbytRevLen - 1))
                {
                    strTemp += bytTemp[i].ToString();                    
                }
                else
                {
                    strTemp += bytTemp[i].ToString() + ".";
                }
            }
            return strTemp;
        }

        public bool SoftwareReset()
        {
            byte[] bytWBuf = new byte[1];
            bytWBuf[0] = 0x08;
            if (WriteBytes(gbytPasswordDevAddr, 26, 1, bytWBuf) == false) return false;
            return true;
        }
        public bool LowPwrRequestSW(bool bSetState)
        {
            byte[] bytWBuf = new byte[1];
            byte[] bytRBuf = new byte[1];

            if (!ReadBytes(gbytPasswordDevAddr, 26, 1, bytRBuf)) return false;

            if (bSetState == true)
            {
                bytWBuf[0] = (byte)(bytRBuf[0] | 0x10);
            }
            else
            {
                bytWBuf[0] = (byte)(bytRBuf[0] & ~0x10);
            }
            if (WriteBytes(gbytPasswordDevAddr, 26, 1, bytWBuf) == false) return false;

            return true;
        }
        public bool LowPwrAllowRequestHW(bool bSetState)
        {
            byte[] bytWBuf = new byte[1];
            byte[] bytRBuf = new byte[1];

            if (!ReadBytes(gbytPasswordDevAddr, 26, 1, bytRBuf)) return false;

            if (bSetState == true)
            {
                bytWBuf[0] = (byte)(bytRBuf[0] | 0x40);
            }
            else
            {
                bytWBuf[0] = (byte)(bytRBuf[0] & ~0x40);
            }
            if (WriteBytes(gbytPasswordDevAddr, 26, 1, bytWBuf) == false) return false;

            return true;
        }
        #endregion

        #region CDB Base
        private bool GetCDBStatus(ref string errorStr, string errorMsg = "")
        {
            ushort wStatus = 0;
            byte[] tablesel = new byte[2];

            errorStr = errorMsg;
            for (int i = 0; i < 1000; i++)
            {
                if (ReadBytes(gbytPasswordDevAddr, 37, 2, tablesel) == false) return false;
                wStatus = (ushort)(tablesel[0] << 8 + tablesel[1]);
                if ((wStatus & 0x8080) == 0)
                {
                    if ((wStatus & 0x4040) == 0)
                    {
                        if ((wStatus & 0x0101) != 0)
                            return true;
                    }
                    else
                    {
                        switch (wStatus & 0x0707)
                        {
                            case 0x0002:
                            case 0x0200:
                            case 0x0202:
                                errorStr += ":Parameter range error or not supported!";
                                break;
                            case 0x0005:
                            case 0x0500:
                            case 0x0505:
                                errorStr += ":CdbChkCode error!";
                                break;
                            case 0x0006:
                            case 0x0600:
                            case 0x0606:
                                errorStr += ":Password error – insufficient privilege!";
                                break;
                            default:
                                errorStr += "!"; //Failed, no specific failure code
                                break;
                        }
                        return false;
                    }
                }
                DelayMS(50);
            }
            return false;
        }

        private bool SendCMD(byte[] bytData)
        {
            byte[] bytCMDBuf = new byte[BLOCKSIZE];
            byte[] bytDataBuf = new byte[BLOCKSIZE];
            byte bytCheckSum = 0;
            byte UpdateDevAddr = (byte)gbytPasswordDevAddr;
            byte bytDataLen = (byte)(8 + bytData[132 - 128]);

            for (int i = 0; i < bytDataLen; i++)
            {
                if (i < (130 - 128))
                {
                    bytCMDBuf[i] = bytData[i];
                    bytCheckSum += bytData[i];
                }
                else if (i < (133 - 128) || i > (135 - 128))
                {
                    bytDataBuf[i - (130 - 128)] = bytData[i];
                    bytCheckSum += bytData[i];
                }                
            }

            bytData[133 - 128] = (byte)(bytCheckSum ^ 0xFF);
            bytDataBuf[(133 - 128) - (130 - 128)] = bytData[133 - 128];

            //选取数据表
            if (WriteTableSel(gbytCDBCMDTableSel) == false) return false;

            if (gbytCDBCMDTrigMode == 0)
            {
                if (WriteBytes(UpdateDevAddr, 0x82, bytDataLen - 2, bytDataBuf) == false || WriteBytes(UpdateDevAddr, 0x80, 2, bytCMDBuf) == false)
                {
                    return false;
                }

                return true;
            }
            else
            {
                return WriteBytes(UpdateDevAddr, 0x80, bytDataLen, bytData);
            }            
        }

        #endregion

        #region  Private CDB Firmware
        private bool SendCMD_GetStatus()
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_GetStatus / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_GetStatus % 256);
            bytDataBuf[132 - 128] = 0x00;

            return SendCMD(bytDataBuf);
        }

        private bool ReadStatusData(byte[] bytData, byte DataLen = 120)
        {
            byte bytCheckSum = 0;
            byte UpdateDevAddr = (byte)gbytPasswordDevAddr;
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            //选取数据表
            if (WriteTableSel(gbytCDBCMDTableSel) == false) return false;
            if (ReadBytes(UpdateDevAddr, 0x80, 128, bytDataBuf) == false) return false;

            byte bytDataLen = bytDataBuf[134 - 128];

            for (int i = 0; i < bytDataLen; i++)
            {
                bytCheckSum += bytDataBuf[8 + i];
            }

            bytCheckSum = (byte)(bytCheckSum ^ 0xFF);

            if (bytCheckSum == bytDataBuf[135 - 128])
            {
                Array.Copy(bytDataBuf, (136 - 128), bytData, 0, DataLen);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SendCMD_Start(byte[] bytData, int FileLen, byte DataLen)
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_Start / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_Start % 256);
            bytDataBuf[132 - 128] = (byte)(8 + DataLen);
            bytDataBuf[136 - 128] = (byte)((FileLen >> 24) & 0xFF);
            bytDataBuf[137 - 128] = (byte)((FileLen >> 16) & 0xFF);
            bytDataBuf[138 - 128] = (byte)((FileLen >> 8) & 0xFF);
            bytDataBuf[139 - 128] = (byte)(FileLen & 0xFF);
            Array.Copy(bytData, 0, bytDataBuf, (144 - 128), DataLen);
            
            return SendCMD(bytDataBuf);
        }

        private bool SendCMD_Abort()
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_Abort / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_Abort % 256);
            bytDataBuf[132 - 128] = 0x00;

            return SendCMD(bytDataBuf);
        }

        private bool SendCMD_LPLWrite(byte[] bytData, uint BlockAddress, byte DataLen)
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_LPLWrite / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_LPLWrite % 256);
            bytDataBuf[132 - 128] = (byte)(4 + DataLen);
            bytDataBuf[136 - 128] = (byte)((BlockAddress >> 24) & 0xFF);
            bytDataBuf[137 - 128] = (byte)((BlockAddress >> 16) & 0xFF);
            bytDataBuf[138 - 128] = (byte)((BlockAddress >> 8) & 0xFF);
            bytDataBuf[139 - 128] = (byte)(BlockAddress & 0xFF);
            Array.Copy(bytData, 0, bytDataBuf, (140 - 128), DataLen);
            
            return SendCMD(bytDataBuf);
        }

        private bool SendCMD_EPLWrite(ushort EPLLength, uint BlockAddress)
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_EPLWrite / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_EPLWrite % 256);
            bytDataBuf[130 - 128] = (byte)(EPLLength / 256);
            bytDataBuf[131 - 128] = (byte)(EPLLength % 256);
            bytDataBuf[132 - 128] = 0x04;
            bytDataBuf[136 - 128] = (byte)((BlockAddress >> 24) & 0xFF);
            bytDataBuf[137 - 128] = (byte)((BlockAddress >> 16) & 0xFF);
            bytDataBuf[138 - 128] = (byte)((BlockAddress >> 8) & 0xFF);
            bytDataBuf[139 - 128] = (byte)(BlockAddress & 0xFF);
            
            return SendCMD(bytDataBuf);
        }

        private bool SendCMD_LPLRead(uint BlockAddress, ushort Length)
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_LPLRead / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_LPLRead % 256);
            bytDataBuf[132 - 128] = 0x06;
            bytDataBuf[136 - 128] = (byte)((BlockAddress >> 24) & 0xFF);
            bytDataBuf[137 - 128] = (byte)((BlockAddress >> 16) & 0xFF);
            bytDataBuf[138 - 128] = (byte)((BlockAddress >> 8) & 0xFF);
            bytDataBuf[139 - 128] = (byte)(BlockAddress & 0xFF);
            bytDataBuf[140 - 128] = (byte)((Length >> 8) & 0xFF);
            bytDataBuf[141 - 128] = (byte)(Length & 0xFF);
            
            return SendCMD(bytDataBuf);
        }

        private bool ReadLPLData(byte[] bytData, byte DataLen = 116)
        {
            byte bytCheckSum = 0;
            byte UpdateDevAddr = (byte)gbytPasswordDevAddr;
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            //选取数据表
            if (WriteTableSel(gbytCDBCMDTableSel) == false) return false;
            if (ReadBytes(UpdateDevAddr, 0x80, 128, bytDataBuf) == false) return false;

            byte bytDataLen = bytDataBuf[134 - 128];

            for (int i = 0; i < bytDataLen; i++)
            {
                bytCheckSum += bytDataBuf[8 + i];
            }

            bytCheckSum = (byte)(bytCheckSum ^ 0xFF);

            if (bytCheckSum == bytDataBuf[135 - 128])
            {
                Array.Copy(bytDataBuf, (140 - 128), bytData, 0, DataLen);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SendCMD_EPLRead(uint BlockAddress, ushort Length)
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_EPLRead / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_EPLRead % 256);
            bytDataBuf[132 - 128] = 0x06;
            bytDataBuf[136 - 128] = (byte)((BlockAddress >> 24) & 0xFF);
            bytDataBuf[137 - 128] = (byte)((BlockAddress >> 16) & 0xFF);
            bytDataBuf[138 - 128] = (byte)((BlockAddress >> 8) & 0xFF);
            bytDataBuf[139 - 128] = (byte)(BlockAddress & 0xFF);
            bytDataBuf[140 - 128] = (byte)((Length >> 8) & 0xFF);
            bytDataBuf[141 - 128] = (byte)(Length & 0xFF);
            
            return SendCMD(bytDataBuf);
        }

        private bool ReadEPLData(byte[] bytData, byte DataLen, byte TableSel)
        {
            byte bytCheckSum = 0;
            byte UpdateDevAddr = (byte)gbytPasswordDevAddr;
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            //选取数据表
            if (WriteTableSel(gbytCDBCMDTableSel) == false) return false;
            if (ReadBytes(UpdateDevAddr, 0x80, 8, bytDataBuf) == false) return false;

            bytCheckSum = (byte)(bytDataBuf[134 - 128] - 240);

            bytCheckSum = (byte)(bytCheckSum ^ 0xFF);

            if (bytCheckSum == bytDataBuf[135 - 128])
            {
                //选取数据表
                if (WriteTableSel(TableSel) == false) return false;
                if (ReadBytes(UpdateDevAddr, 0x80, (byte)DataLen, bytDataBuf) == false) return false;
                Array.Copy(bytDataBuf, 0, bytData, 0, DataLen);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SendCMD_Complete()
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_Complete / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_Complete % 256);
            bytDataBuf[132 - 128] = 0x00;

            return SendCMD(bytDataBuf);
        }

        private bool SendCMD_Copy(byte Direction = 0xBA)
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_Copy / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_Copy % 256);
            bytDataBuf[132 - 128] = 0x01;
            bytDataBuf[136 - 128] = Direction;
            
            return SendCMD(bytDataBuf);
        }

        private bool ReadCopyData(byte[] bytData, byte DataLen = 6)
        {
            byte bytCheckSum = 0;
            byte UpdateDevAddr = (byte)gbytPasswordDevAddr;
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            //选取数据表
            if (WriteTableSel(gbytCDBCMDTableSel) == false) return false;
            if (ReadBytes(UpdateDevAddr, 0x80, 14, bytDataBuf) == false) return false;

            byte bytDataLen = bytDataBuf[134 - 128];

            for (int i = 0; i < bytDataLen; i++)
            {
                bytCheckSum += bytDataBuf[8 + i];
            }

            bytCheckSum = (byte)(bytCheckSum ^ 0xFF);

            if (bytCheckSum == bytDataBuf[135 - 128])
            {
                Array.Copy(bytDataBuf, (136 - 128), bytData, 0, DataLen);
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool SendCMD_RunImage(byte ImageToRun = 0, ushort DelayToReset = 100)
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_Run / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_Run % 256);
            bytDataBuf[132 - 128] = 0x04;
            bytDataBuf[137 - 128] = ImageToRun;
            bytDataBuf[138 - 128] = (byte)((DelayToReset >> 8) & 0xFF);
            bytDataBuf[139 - 128] = (byte)(DelayToReset & 0xFF);
            
            return SendCMD(bytDataBuf);
        }

        private bool SendCMD_CommitImage()
        {
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];

            bytDataBuf[128 - 128] = (byte)(CMD_CDB_Commit / 256);
            bytDataBuf[129 - 128] = (byte)(CMD_CDB_Commit % 256);
            bytDataBuf[132 - 128] = 0x00;

            return SendCMD(bytDataBuf);
        }

        #endregion

        #region public CDB Firmware
        public bool SendGetCDBStatus(ref string errorStr, string errorMsg = "")
        {
            if (SendCMD_GetStatus() == false || GetCDBStatus(ref errorStr, errorMsg) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool SendCDBStart(byte[] bytData, int FileLen, byte DataLen, ref string errorStr, string errorMsg = "")
        {
            if (SendCMD_Start(bytData, FileLen, DataLen) == false || GetCDBStatus(ref errorStr, errorMsg) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool SendGetCDBAbort()
        {
            return SendCMD_Abort();
        }

        public bool WriteDataLPL(int FlashAddr, byte[] bytData, int DataLen, ref string errorStr, string errorMsg = "")
        {
            int iAddr = FlashAddr;
            int iIndex = 0;
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];
            int iCurWriteNum = DataLen;

            do
            {
                iCurWriteNum = DataLen;
                if (iCurWriteNum > gbytCDBLPLDataSize) iCurWriteNum = gbytCDBLPLDataSize;//分段写，每次最多116个数据
                DataLen -= iCurWriteNum;

                for (int j = 0; j < bytDataBuf.Length; j++)
                {
                    bytDataBuf[j] = 0xFF;
                }

                Array.Copy(bytData, iIndex, bytDataBuf, 0, iCurWriteNum);

                if (SendCMD_LPLWrite(bytDataBuf, (uint)iAddr, (byte)iCurWriteNum) == false) return false;

                if (GetCDBStatus(ref errorStr, errorMsg) == false) return false;

                iIndex = iIndex + iCurWriteNum;
                iAddr = iAddr + iCurWriteNum;
            } while (DataLen > 0);

            return true;
        }

        public bool WriteDataEPL(int FlashAddr, byte[] bytData, int DataLen, ref string errorStr, string errorMsg = "")
        {
            int iAddr = FlashAddr;
            int iIndex = 0;
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];
            int iCurWriteNum = DataLen;
            int iWriteNum = BLOCKSIZE;
            do
            {
                iCurWriteNum = DataLen;
                if (iCurWriteNum > gwCDBEPLDataSize) iCurWriteNum = gwCDBEPLDataSize;//分段写，每次最多(CDBDataSize)个数据
                DataLen -= iCurWriteNum;

                for (int i = 0; i < ((iCurWriteNum - 1) / BLOCKSIZE + 1); i++)
                {
                    if ((iCurWriteNum - i * BLOCKSIZE) > BLOCKSIZE)
                    {
                        iWriteNum = BLOCKSIZE;
                    }
                    else
                    {
                        iWriteNum = iCurWriteNum - i * BLOCKSIZE;
                    }

                    if (WriteTableSel((byte)(gbytCDBDataTableBase + i)) == false) return false;

                    for (int j = 0; j < bytDataBuf.Length; j++)
                    {
                        bytDataBuf[j] = 0xFF;
                    }

                    Array.Copy(bytData, iIndex + i * BLOCKSIZE, bytDataBuf, 0, iWriteNum);
                    if (WriteBytes(gbytPasswordDevAddr, 0x80, iWriteNum, bytDataBuf) == false) return false;
                }

                if (SendCMD_EPLWrite((ushort)iCurWriteNum, (uint)iAddr) == false) return false;

                if (GetCDBStatus(ref errorStr, errorMsg) == false) return false;

                iIndex = iIndex + iCurWriteNum;
                iAddr = iAddr + iCurWriteNum;
            } while (DataLen > 0);

            return true;
        }

        public bool ReadDataLPL(int FlashAddr, byte[] bytData, int DataLen, ref string errorStr, string errorMsg = "")
        {
            int iAddr = FlashAddr;
            int iIndex = 0;
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];
            int iCurReadNum = DataLen;

            do
            {
                iCurReadNum = DataLen;
                if (iCurReadNum > gbytCDBLPLDataSize) iCurReadNum = gbytCDBLPLDataSize;//分段写，每次最多116个数据
                DataLen -= iCurReadNum;

                for (int j = 0; j < bytDataBuf.Length; j++)
                {
                    bytDataBuf[j] = 0xFF;
                }                

                if (SendCMD_LPLRead((uint)iAddr, (byte)iCurReadNum) == false) return false;

                if (GetCDBStatus(ref errorStr, errorMsg) == false) return false;

                if (ReadLPLData(bytDataBuf, (byte)iCurReadNum) == false) return false;

                Array.Copy(bytDataBuf, 0, bytData, iIndex, iCurReadNum);

                iIndex = iIndex + iCurReadNum;
                iAddr = iAddr + iCurReadNum;
            } while (DataLen > 0);

            return true;
        }

        public bool ReadDataEPL(int FlashAddr, byte[] bytData, int DataLen, ref string errorStr, string errorMsg = "")
        {
            int iAddr = FlashAddr;
            int iIndex = 0;
            byte[] bytDataBuf = new byte[BLOCKSIZE * 2];
            int iCurReadNum = DataLen;
            int iReadNum = BLOCKSIZE;

            do
            {
                iCurReadNum = DataLen;
                if (iCurReadNum > gwCDBEPLDataSize) iCurReadNum = gwCDBEPLDataSize;//分段写，每次最多(CDBDataSize)个数据
                DataLen -= iCurReadNum;

                if (SendCMD_EPLRead((uint)iAddr, (byte)iCurReadNum) == false) return false;

                if (GetCDBStatus(ref errorStr, errorMsg) == false) return false;

                for (int i = 0; i < ((iCurReadNum - 1) / BLOCKSIZE + 1); i++)
                {
                    if ((iCurReadNum - i * BLOCKSIZE) > BLOCKSIZE)
                    {
                        iReadNum = BLOCKSIZE;
                    }
                    else
                    {
                        iReadNum = iCurReadNum - i * BLOCKSIZE;
                    }

                    for (int j = 0; j < bytDataBuf.Length; j++)
                    {
                        bytDataBuf[j] = 0xFF;
                    }

                    if (ReadEPLData(bytDataBuf, (byte)iReadNum, (byte)(gbytCDBDataTableBase + i)) == false) return false;

                    Array.Copy(bytDataBuf, 0, bytData, iIndex + i * BLOCKSIZE, iReadNum);
                }

                iIndex = iIndex + iCurReadNum;
                iAddr = iAddr + iCurReadNum;
            } while (DataLen > 0);

            return true;
        }

        public bool SendCDBComplete(ref string errorStr, string errorMsg = "")
        {
            if (SendCMD_Complete() == false || GetCDBStatus(ref errorStr, errorMsg) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool SendCDBCopy(byte Direction=0xBA)
        {
            return SendCMD_Copy(Direction);
        }

        public bool SendCDBRunImage(ref string errorStr, string errorMsg = "", byte ImageToRun = 0, ushort DelayToReset = 100)
        {
            if (SendCMD_RunImage(ImageToRun, DelayToReset) == false || GetCDBStatus(ref errorStr, errorMsg) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool SendCDBCommitImage(ref string errorStr, string errorMsg = "")
        {
            if (SendCMD_CommitImage() == false || GetCDBStatus(ref errorStr, errorMsg) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

    }
}
