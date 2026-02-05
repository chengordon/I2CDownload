using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;//DllImport
//using System.Collections;
using System.Windows.Forms;

namespace TestboardSoftware
{
    public class ClsBase //: ClsInterface
    {
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

        #endregion //DllImport
        //
        #region 定义
        //
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

        public int gintModuleType;
        public enum enumMSAType
        {
            SFF8472Compatible,
            SFF8636Compatible,
            CMISCompatible,
            INF8077Compatible
        };
        public enumMSAType gMSACompatible;        

        //修改位置1
        //增加新模块时需要修改的位置，用以下一句可以搜索
        //NeedModified_when_add_new_class
        public enum enumModuleType
        {
            AEC_800G_AW100 = 0,
        };
        public enumModuleType gModuleType;


        public enum enumCalPointType
        {
            None = 0,
            OnePoint = 1,
            MultiPoint = 2,
            Both = 3,
        }
        public struct SuportCalPointStruct// 支持的校准点数
        {
            public enumCalPointType TempCalPoint;
            public enumCalPointType VoltCalPoint;
            public enumCalPointType BiasCalPoint;
            public enumCalPointType TxPoCalPoint;
            public enumCalPointType RxPoCalPoint;
            public enumCalPointType Aux1CalPoint;
            public enumCalPointType Aux2CalPoint;
            public enumCalPointType Volt3V3CalPoint;
        }
        public SuportCalPointStruct gModCalPoint;       

        public struct WidgetInfor// 各控件使用情况(设置调试页面时使用)
        {
            public int hScrollBar_Maxinum_DAC0;//横滚动条最大值
            public int hScrollBar_Maxinum_DAC1;            
            public int hScrollBar_Maxinum_DAC2;
            public int hScrollBar_Maxinum_DAC3;
            public int hScrollBar_Maxinum_PWM0;
            public int hScrollBar_Maxinum_PWM1;
            public int hScrollBar_Maxinum_PWM2;
            public int hScrollBar_Maxinum_PWM3;
            public int hScrollBar_Maxinum_Slope;
            //
            public bool hScrollBar_InUse_DAC0;//横滚动条是否在使用
            public bool hScrollBar_InUse_DAC1;            
            public bool hScrollBar_InUse_DAC2;
            public bool hScrollBar_InUse_DAC3;
            public bool hScrollBar_InUse_PWM0;
            public bool hScrollBar_InUse_PWM1;
            public bool hScrollBar_InUse_PWM2;
            public bool hScrollBar_InUse_PWM3;
            public bool hScrollBar_InUse_Slope;
            //
            public bool LUT_InUse_DAC0;//LUT是否使用，由具体模块决定
            public bool LUT_InUse_DAC1;
            public bool LUT_InUse_DAC2;
            public bool LUT_InUse_DAC3;
            public bool LUT_InUse_PWM0;
            public bool LUT_InUse_PWM1;
            public bool LUT_InUse_PWM2;
            public bool LUT_InUse_PWM3;
            public bool LUT_InUse_Slope;
        }
        public WidgetInfor WidgetState;
        #endregion //定义
        //
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

        public bool WriteBytes(Byte I2C_Address, Byte DstAddr, int WriteDataNum, Byte[] WriteDataBuf)
        {
            bool bState;
            if (hfWriteAddrI2c(I2C_Address, DstAddr, WriteDataNum, WriteDataBuf) == WriteDataNum)
            {
                DelayMS(6);
                bState = true;//写入成功
            }
            else
            {
                DelayMS(6);
                bState = false;//写入失败
            }
            return bState;
        }
        public bool ReadBytes(Byte I2C_Address, Byte DstAddr, Byte ReadDataNum, Byte[] ReadDataBuf)
        {
            int intTemp = ReadDataNum;
            bool bState;
            if (hfReadAddrI2c(I2C_Address, DstAddr, ReadDataNum, ReadDataBuf) == intTemp)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = false ;//读取失败
            }
            return bState;
        }
        public bool CurrentWriteBytes(Byte I2C_Address, Byte WriteDataNum, Byte[] WriteDataBuf)
        {
            bool bState;
            if (hfWriteI2c(I2C_Address, WriteDataNum, WriteDataBuf) == WriteDataNum)
            {
                DelayMS(6);
                bState = true;//写入成功
            }
            else
            {
                DelayMS(6);
                bState = false;//写入失败
            }
            return bState;
        }
        public bool CurrentReadBytes(Byte I2C_Address, Byte ReadDataNum, Byte[] ReadDataBuf)
        {
            int intTemp = ReadDataNum;
            bool bState;
            if (hfReadI2c(I2C_Address, ReadDataNum, ReadDataBuf) == intTemp)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }

        public bool InterfaceInit()
        {
            hfInitI2c();
            if (hfSetI2CRateBps(200) == 0) return false;//0:设置失败；1：设置成功 200  400  500
            return true;
        }

        public int InterfaceClose()
        {
            hfUnInitI2c();
            return 0;
        }

        public bool GetSn(int size, Byte[] SnBuf)
        {
            bool bState;
            if (hfGetSn(SnBuf, size) == 1)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }
        public bool SetSn(int size, Byte[] SnBuf)
        {
            bool bState;
            if (hfSetSn(SnBuf, size) == 1)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }
        public bool ReadFlash(int Addr, float[] buf)
        {
            bool bState;
            if (hfReadFlash(Addr, buf) > 0)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }
        public bool ReadFlash(int Addr, UInt16[] buf)
        {
            bool bState;
            if (hfReadFlash(Addr, buf) > 0)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }
        public bool ReadFlash(int Addr, byte[] buf)
        {
            bool bState;
            if (hfReadFlash(Addr, buf) > 0)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }
        public bool WriteFlash(int Addr, int wlen, float[] buf)
        {
            bool bState;
            if (hfWriteFlash(Addr, wlen, buf) == 1)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }
        public bool WriteFlash(int Addr, int wlen, UInt16[] buf)
        {
            bool bState;
            if (hfWriteFlash(Addr, wlen, buf) == 1)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }
        public bool WriteFlash(int Addr, int wlen, byte[] buf)
        {
            bool bState;
            if (hfWriteFlash(Addr, wlen, buf) == 1)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }
        public bool CalVCSens(float Txvlt, float Rxvlt, float Txcrt, float Rxcrt)
        {
            bool bState;
            if (hfCalVCSens(Txvlt, Rxvlt, Txcrt, Rxcrt) == 1)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }

        public bool CalSlopeSens(float TxvltSlope, float RxvltSlope, float TxcrtSlope, float RxcrtSlope)
        {
            bool bState;
            if (hfCalSlopeSens(TxvltSlope, RxvltSlope, TxcrtSlope, RxcrtSlope) == 1)
            {
                DelayMS(6);
                bState = true;//读取成功
            }
            else
            {
                DelayMS(6);
                bState = true;//读取失败
            }
            return bState;
        }

        public int ReadTestBoard(ref TestBoardParamStruct TestBoardP)
        {
            hfReadBoardInt(ref TestBoardP);
            return 0;
        }

        public bool GpioSetMode(int Portmasking, int ReadLen, Byte[] ReadDataBuf)
        {
            if (hfGpioSetMode(Portmasking, ReadLen, ReadDataBuf) == 1)//读取成功
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Gpioread(int Portmasking, int ReadLen, Byte[] ReadDataBuf)
        {
            if (hfGpioRead(Portmasking, ReadLen, ReadDataBuf) == 1)//读取成功
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GpioWrite(int Portmasking, int WriteLen, Byte[] WriteDataBuf)
        {

            if (hfGpioWrite(Portmasking, WriteLen, WriteDataBuf) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public bool SetTBdRS0OnOff(bool bOnOff)
        {
            TestBoardParamStruct TestBoardP = new TestBoardParamStruct();
            int intTemp = hfReadBoardInt(ref TestBoardP);//0:读取失败；1：读取成功
            if (intTemp == 0) return false;
            if (bOnOff == false)//Close Board Power
            {
                intTemp = hfSetBoard(TestBoardP.BenValue, TestBoardP.TxVccEn, TestBoardP.RxVccEn, 0, TestBoardP.RxVltSet);//0:设置失败；1：设置成功
                DelayMS(200);
            }
            else //Open Board Power
            {
                intTemp = hfSetBoard(TestBoardP.BenValue, TestBoardP.TxVccEn, TestBoardP.RxVccEn, 1, TestBoardP.RxVltSet);
                DelayMS(200);
            }
            if (intTemp == 0) return false;//0:设置失败；1：设置成功
            return true;
        }
        public bool SetTBdRS1OnOff(bool bOnOff)
        {
            TestBoardParamStruct TestBoardP = new TestBoardParamStruct();
            int intTemp = hfReadBoardInt(ref TestBoardP);//0:读取失败；1：读取成功
            if (intTemp == 0) return false;
            if (bOnOff == false)//Close Board Power
            {
                intTemp = hfSetBoard(TestBoardP.BenValue, TestBoardP.TxVccEn, TestBoardP.RxVccEn, TestBoardP.TxVltSet, 0);//0:设置失败；1：设置成功
                DelayMS(200);
            }
            else //Open Board Power
            {
                intTemp = hfSetBoard(TestBoardP.BenValue, TestBoardP.TxVccEn, TestBoardP.RxVccEn, TestBoardP.TxVltSet, 1);
                DelayMS(200);
            }
            if (intTemp == 0) return false;//0:设置失败；1：设置成功
            return true;
        }
        public bool SetTBP4Ctrl(byte bState)
        {
            TestBoardParamStruct TestBoardP = new TestBoardParamStruct();
            int intTemp = hfReadBoardInt(ref TestBoardP);//0:读取失败；1：读取成功
            if (intTemp == 0) return false;

            intTemp = hfSetBoard(TestBoardP.BenValue, TestBoardP.TxVccEn, bState, TestBoardP.TxVltSet, TestBoardP.RxVltSet);//0:设置失败；1：设置成功
            DelayMS(200);

            if (intTemp == 0) return false;//0:设置失败；1：设置成功
            return true;
        }

        public bool GetTBP4Ctrl(ref byte bState)
        {
            TestBoardParamStruct TestBoardP = new TestBoardParamStruct();
            int intTemp = hfReadBoardInt(ref TestBoardP);//0:读取失败；1：读取成功
            if (intTemp == 0) return false;
            if ((TestBoardP.PortPIn[4] & 0xC0)==0x80)
            {
                bState = 0;
            }
            else if((TestBoardP.PortPIn[4] & 0xC0)==0x40)
            {
                bState = 1;
            }
            else if ((TestBoardP.PortPIn[4] & 0xC0) == 0x00)
            {
                bState =2;
            }            
            return true;
        }

        //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        #region 窗体相关设置
        //获取全部寄存器数据
        public virtual byte[,] GetAllRegisterData(ref string[] strNames, ref byte[] bytDevAddr,
                                             ref byte[] bytTable, ref byte[] bytStartAddr)
        {
            byte[,] registerData = new byte[1, 128];
            string[] name = new string[1];
            byte[] addr = new byte[1];
            byte[] table = new byte[1];
            byte[] wordAddr = new byte[1];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = "";
                addr[i] = 0;
                table[i] = 0;
                wordAddr[i] = 0;
            }
            strNames = name;
            bytDevAddr = addr;
            bytTable = table;
            bytStartAddr = wordAddr;
            return registerData;
        }
        //写入全部寄存器数据
        public virtual bool WriteAllRegisterData(byte[,] bytAllRegData, byte[] bytDevAddr,
                                             byte[] bytTable, byte[] bytStartAddr)
        {
            return false;
        }
        //获取全部EEprom数据
        public virtual byte[,] GetAllEEpromData(ref byte[] bytDevAddr, ref byte[] bytTable,
                                                ref byte[] bytStartAddr)
        {
            byte[,] registerData = new byte[1, 128];
            byte[] addr = new byte[1];
            byte[] table = new byte[1];
            byte[] wordAddr = new byte[1];
            for (int i = 0; i < addr.Length; i++)
            {
                addr[i] = 0;
                table[i] = 0;
                wordAddr[i] = 0;
            }
            bytDevAddr = addr;
            bytTable = table;
            bytStartAddr = wordAddr;
            return registerData;
        }
        //写入全部EEprom数据
        public virtual bool WriteAllEEpromData(byte[,] bytAllEEData, byte[] bytDevAddr,
                                             byte[] bytTable, byte[] bytStartAddr)
        {
            return false;
        }
        //返回全部的备份数据
        public virtual byte[,] GetAllBackupData(ref byte[] bytDevAddr, ref byte[] bytTable,
                                                ref byte[] bytStartAddr)
        {
            byte[,] registerData = new byte[1, 128];
            byte[] addr = new byte[1];
            byte[] table = new byte[1];
            byte[] wordAddr = new byte[1];
            for (int i = 0; i < addr.Length; i++)
            {
                addr[i] = 0;
                table[i] = 0;
                wordAddr[i] = 0;
            }
            bytDevAddr = addr;
            bytTable = table;
            bytStartAddr = wordAddr;
            return registerData;
        }
        //返回全部的表名
        public virtual void GetTableNames(ref string[] strNames, ref byte[] bytDevAddr,
                                             ref byte[] bytTable, ref byte[] bytStartAddr)
        {
            string[] name = new string[1];
            byte[] addr = new byte[1];
            byte[] table = new byte[1];
            byte[] wordAddr = new byte[1];
            for (int i = 0; i < name.Length; i++)
            {
                name[i] = "";
                addr[i] = 0;
                table[i] = 0;
                wordAddr[i] = 0;
            }
            strNames = name;
            bytDevAddr = addr;
            bytTable = table;
            bytStartAddr = wordAddr;
        }
        //返回全部的表的概述
        public virtual string[] GetTableOverview()
        {
            string[] strTemp = new string[1];
            return strTemp;
        }
        //返回接收点数描述
        public virtual string GetRxCalDotNumDescription()
        {
            string str = "";
            return str;
        }
        #endregion //窗体相关设置       
        public virtual bool EnterDefaultPW()//输入默认密码
        {
            return false;
        }
        public virtual bool EnterPW(int iPWType)//输入密码
        {
            return false;
        }        
        
        public virtual bool WriteTableSel(byte bytTable)//选表
        {
            return false;
        }        
        public virtual byte ReadTableSel()//读选表值
        {
            return 0;
        }

        public float ConvertADtoVal_Temp(byte bytMSB, byte bytLSB)
        {
            float fTemp;
            fTemp = bytMSB * 256 + bytLSB;
            if (fTemp > 32767)
            {
                fTemp = fTemp - 65536;
            }
            fTemp = fTemp / 256;
            return fTemp;
        }

        public float ConvertADtoVal_Temp(int intAD)
        {
            float fTemp;
            if (intAD > 65535 || intAD < 0) return 0;
            if (intAD > 32767)
            {
                intAD = intAD - 65536;
            }
            fTemp = intAD;
            fTemp = fTemp / 256;
            return fTemp;
        }

        public int ConvertValtoAD_Temp(float fVal)
        {
            int intTemp;
            intTemp = (int)(fVal * 256);
            if (intTemp < 0)
            {
                intTemp = intTemp + 65536;
            }
            return intTemp;
        }

        public float ConvertADtoVal_Volt(byte bytMSB, byte bytLSB)
        {
            float fTemp;
            fTemp = bytMSB * 256 + bytLSB;
            fTemp = fTemp / 10000;//LSB equal to 100 μVolt
            return fTemp;
        }

        public float ConvertADtoVal_Volt(int intAD)
        {
            float fTemp;
            if (intAD > 65535 || intAD < 0) return 0;
            fTemp = intAD;
            fTemp = fTemp / 10000;//LSB equal to 100 μVolt
            return fTemp;
        }
        public int ConvertValtoAD_Volt(float fVal)
        {
            int intTemp;
            intTemp = (int)(fVal * 10000);
            return intTemp;
        }

        public void SlopToBin(float fSlop, ref  byte bytMSB, ref  byte bytLSB)//斜率转二进制值(符合SFF8472)
        {
            //fSlop = fSlop * 256;
            bytMSB = (byte)(fSlop / 256);
            bytLSB = (byte)(fSlop % 256);
        }

        public void OffsetToBin(float fOffset, ref  byte bytMSB, ref  byte bytLSB)//偏移转二进制值(符合SFF8472)
        {
            fOffset = (int)fOffset;
            if (fOffset < 0) fOffset = fOffset + 0xFFFF;
            bytMSB = (byte)(fOffset / 256);
            bytLSB = (byte)(fOffset % 256);
        }

        public virtual int GetRawAD(int iIndex)//原始AD,iIndex=0:Temp,=1:Volt...=4:RxPo;=5:Aux1;=6:Aux2
        {
            return 0;
        }
        
        public virtual int GetRawAD_Volt3V3()//原始Volt3V3 AD
        {
            return 0;
        }
        public virtual int GetRealTimeVolt3V3_AD()//实时Volt3V3 AD
        {
            return 0;
        }
        //
        public byte[,] GenerateLutDataWithCurveCoef_Byte(float[] fCoef)//根据系数(Tindex-DA)生成Lut表
        {
            byte[,] bytTemp = new byte[1, 128];
            return bytTemp;
        }
        public int[,] GenerateLutDataWithCurveCoef_Int(float[] fCoef)//根据系数(Tindex-DA)生成Lut表
        {
            int[,] intTemp = new int[1, 128];
            return intTemp;
        }
        public byte[,] GenerateLutDataWithPolylineCoef_Byte(float[] fCoef)//根据系数(Tindex-DA)生成Lut表
        {
            byte[,] bytTemp = new byte[1, 128];
            return bytTemp;
        }
        public int[,] GenerateLutDataWithPolylineCoef_Int(float[] fCoef)//根据系数(Tindex-DA)生成Lut表
        {
            int[,] intTemp = new int[1, 128];
            return intTemp;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intLUTType">Lut类型：0:Bias;1:Mod;2:APD</param>
        /// <param name="intDataNum">返回数据个数</param>
        /// <returns></returns>
        public virtual int[] ReadLUTData(int intLUTType, ref int intDataNum)//从模块内部读取Lut
        {
            int[] intResult = new int[1];
            intDataNum = 1;
            return intResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intData">要写入的数据，与温度指针一一对应</param>
        /// <param name="intLUTType">Lut类型：0:Bias;1:Mod;2:APD</param>
        /// <param name="intDataNum">数据个数</param>
        /// <returns></returns>
        public virtual bool WriteLUTData(int[] intData, int intLUTType, int intDataNum)
        {
            return false;
        }
        
        public virtual bool WriteDAC0(int DACValue)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC0(int DACValue, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC0(int DACValue, int Side, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool ReadDAC0(out int DACValue, int iType)//读取DAC0调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        public virtual bool ReadDAC0(out int DACValue, int Side, int Port)
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WriteDAC1(int DACValue)//写入DAC1调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC1(int DACValue, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC1(int DACValue, int Side, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool ReadDAC1(out int DACValue, int iType)//读取DAC1调试DAC值
        {
            DACValue = 0;
            return false;
        }
        public virtual bool ReadDAC1(out int DACValue, int Side, int Port)
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WriteDAC2(int DACValue)//写入DAC2调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC2(int DACValue, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC2(int DACValue, int Side, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool ReadDAC2(out int DACValue, int iType)//读取DAC2调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        public virtual bool ReadDAC2(out int DACValue, int Side, int Port)
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WriteDAC3(int DACValue)//写入DAC3调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC3(int DACValue, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC3(int DACValue, int Side, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool ReadDAC3(out int DACValue, int iType)//读取DAC3调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        public virtual bool ReadDAC3(out int DACValue, int Side, int Port)
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WriteDAC4(int DACValue, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC4(int DACValue, int Side, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool ReadDAC4(out int DACValue, int Port)
        {
            DACValue = 0;
            return false;
        }
        public virtual bool ReadDAC4(out int DACValue, int Side, int Port)
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WriteDAC5(int DACValue, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC5(int DACValue, int Side, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool ReadDAC5(out int DACValue, int Port)
        {
            DACValue = 0;
            return false;
        }
        public virtual bool ReadDAC5(out int DACValue, int Side, int Port)
        {
            DACValue = 0;
            return false;
        }

        public virtual bool WriteDAC6(int DACValue, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool WriteDAC6(int DACValue, int Side, int Port)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool ReadDAC6(out int DACValue, int Port)
        {
            DACValue = 0;
            return false;
        }
        public virtual bool ReadDAC6(out int DACValue, int Side, int Port)
        {
            DACValue = 0;
            return false;
        }

        public virtual bool WritePWM0(int DACValue)//写入DAC0调试DAC值
        {
            return false;
        }
        public virtual bool ReadPWM0(out int DACValue, int iType)//读取DAC0调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WritePWM1(int DACValue)//写入DAC1调试DAC值
        {
            return false;
        }
        public virtual bool ReadPWM1(out int DACValue, int iType)//读取DAC1调试DAC值
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WritePWM2(int DACValue)//写入DAC2调试DAC值
        {
            return false;
        }
        public virtual bool ReadPWM2(out int DACValue, int iType)//读取DAC2调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WritePWM3(int DACValue)//写入DAC3调试DAC值
        {
            return false;
        }
        public virtual bool ReadPWM3(out int DACValue, int iType)//读取DAC3调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }

        public virtual bool WriteSlope(int DACValue)//写入Slope调试DAC值
        {
            return false;
        }
        public virtual bool ReadSlope(out int DACValue, int iType)//读取Slope调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WriteSlope1(int DACValue)//写入Slope调试DAC值
        {
            return false;
        }
        public virtual bool ReadSlope1(out int DACValue, int iType)//读取Slope调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WriteSlope2(int DACValue)//写入Slope调试DAC值
        {
            return false;
        }
        public virtual bool ReadSlope2(out int DACValue, int iType)//读取Slope调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        public virtual bool WriteSlope3(int DACValue)//写入Slope调试DAC值
        {
            return false;
        }
        public virtual bool ReadSlope3(out int DACValue, int iType)//读取Slope调试DAC值 iType = 1: read look-up table; iType = 0: read vendor set value
        {
            DACValue = 0;
            return false;
        }
        //
        public virtual bool ReadLutState_DAC0()//读DAC0 Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_DAC0(bool bSetState)//设置DAC0 Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_DAC1()//读DAC1 Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_DAC1(bool bSetState)//设置DAC1 Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }        
        public virtual bool ReadLutState_DAC2()//读DAC2 Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_DAC2(bool bSetState)//设置DAC2 Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_DAC3()//读DAC3 Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_DAC3(bool bSetState)//设置DAC3 Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_PWM0()//读DAC0 Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_PWM0(bool bSetState)//设置DAC0 Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_PWM1()//读DAC1 Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_PWM1(bool bSetState)//设置DAC1 Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_PWM2()//读DAC2 Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_PWM2(bool bSetState)//设置DAC2 Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_PWM3()//读DAC3 Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_PWM3(bool bSetState)//设置DAC3 Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_Slope()//读Slope Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_Slope(bool bSetState)//设置Slope Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_Slope1()//读Slope Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_Slope1(bool bSetState)//设置Slope Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_Slope2()//读Slope Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_Slope2(bool bSetState)//设置Slope Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }
        public virtual bool ReadLutState_Slope3()//读Slope Lut状态(true:LutEnable)
        {
            return false;
        }
        public virtual bool SetLutState_Slope3(bool bSetState)//设置Slope Lut状态(bSetState=true:LutEnable)
        {
            return false;
        }

        public virtual bool ReadCH1_Status()
        {
            return false;
        }
        public virtual bool ReadState_CH1()
        {
            return false;
        }
        public virtual bool SetState_CH1(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadCH2_Status()
        {
            return false;
        }
        public virtual bool ReadState_CH2()
        {
            return false;
        }
        public virtual bool SetState_CH2(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadCH3_Status()
        {
            return false;
        }
        public virtual bool ReadState_CH3()
        {
            return false;
        }
        public virtual bool SetState_CH3(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadCH4_Status()
        {
            return false;
        }
        public virtual bool ReadState_CH4()
        {
            return false;
        }
        public virtual bool SetState_CH4(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadCH5_Status()
        {
            return false;
        }
        public virtual bool ReadState_CH5()
        {
            return false;
        }
        public virtual bool SetState_CH5(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadCH6_Status()
        {
            return false;
        }
        public virtual bool ReadState_CH6()
        {
            return false;
        }
        public virtual bool SetState_CH6(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadCH7_Status()
        {
            return false;
        }
        public virtual bool ReadState_CH7()
        {
            return false;
        }
        public virtual bool SetState_CH7(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadCH8_Status()
        {
            return false;
        }
        public virtual bool ReadState_CH8()
        {
            return false;
        }
        public virtual bool SetState_CH8(bool bSetState)
        {
            return false;
        }

        public virtual bool ReadRX1_Status()
        {
            return false;
        }
        public virtual bool ReadState_RX1()
        {
            return false;
        }
        public virtual bool SetState_RX1(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadRX2_Status()
        {
            return false;
        }
        public virtual bool ReadState_RX2()
        {
            return false;
        }
        public virtual bool SetState_RX2(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadRX3_Status()
        {
            return false;
        }
        public virtual bool ReadState_RX3()
        {
            return false;
        }
        public virtual bool SetState_RX3(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadRX4_Status()
        {
            return false;
        }
        public virtual bool ReadState_RX4()
        {
            return false;
        }
        public virtual bool SetState_RX4(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadRX5_Status()
        {
            return false;
        }
        public virtual bool ReadState_RX5()
        {
            return false;
        }
        public virtual bool SetState_RX5(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadRX6_Status()
        {
            return false;
        }
        public virtual bool ReadState_RX6()
        {
            return false;
        }
        public virtual bool SetState_RX6(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadRX7_Status()
        {
            return false;
        }
        public virtual bool ReadState_RX7()
        {
            return false;
        }
        public virtual bool SetState_RX7(bool bSetState)
        {
            return false;
        }
        public virtual bool ReadRX8_Status()
        {
            return false;
        }
        public virtual bool ReadState_RX8()
        {
            return false;
        }
        public virtual bool SetState_RX8(bool bSetState)
        {
            return false;
        }

        public virtual bool CalTemp(float fCalVal)//Temp校准(单点)
        {
            return false;
        }

        public virtual bool CalVolt(float fCalVal)//Volt校准(单点)
        {
            return false;
        }
               
        /// <summary>
        /// 获取访问等级
        /// </summary>
        /// <returns>返回0：正常等级；1：用户等级；2：厂商等级</returns>
        public virtual int GetAccessLevel()//获取访问等级
        {
            return 0;
        }
        
        public virtual string ReadFWVersion()
        {
            return "";
        }

        public virtual bool Temp_getIndex(ref int Index)//读温度指针
        {
            return false;
        } 
        
        public virtual bool Voltage_getIndex(ref int Index)//读电压指针
        {
            return false;
        }
        //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        //温补

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">点的个数</param>
        /// <param name="step">点的温度间隔</param>
        /// <param name="itemNames">表的列名</param>
        /// <returns></returns>
        public virtual bool TempCurve_getInfo(ref float start, ref int count, ref float step, ref string[] itemNames, ref bool[] itemNamesVisible)
        {
            start = -40;
            count = 64;
            step = 2.5F;
            itemNames = new string[] { "bias", "mod" };
            itemNamesVisible = new bool[] { true, true, };

            return true;
        }
        public virtual bool SlopeCurve_getInfo(ref float start, ref int count, ref float step, ref string[] itemNames, ref bool[] itemNamesVisible)
        {
            start = -40;
            count = 64;
            step = 2.5F;
            itemNames = new string[] { "bias", "mod" };
            itemNamesVisible = new bool[] { true, true, };

            return true;
        }
        public virtual bool VoltCurve_getInfo(ref float start, ref int count, ref float step, ref string[] itemNames, ref bool[] itemNamesVisible)
        {
            start = 2.4F;
            count = 64;
            step = 2.5F;
            itemNames = new string[] { "bias", "mod" };
            itemNamesVisible = new bool[] { true, true, };

            return true;
        }

        public virtual bool VoltCompen_getInfo(ref float start, ref int count, ref float step, ref string[] itemNames, ref bool[] itemNamesVisible)
        {
            start = 2.4F;
            count = 64;
            step = 2.5F;
            itemNames = new string[] { "bias", "mod" };
            itemNamesVisible = new bool[] { true, true, };

            return true;
        }              

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="dgv"></param>
        /// <param name="count">补偿表点的个数</param>
        /// <returns></returns>
        /*
        public virtual bool Curve_importExcel(string file, DataGridView dgv, int count, int iType)
        {
            bool bState = false;
            using (XLSXWrapper xls = new XLSXWrapper(file))
            {
                switch (iType)
                {
                    case 0:
                        bState = xls.selectSheet("VoltCurve");
                        break;
                    case 1:
                        bState = xls.selectSheet("SlopeCurve");
                        break;
                    case 2:
                        bState = xls.selectSheet("TempCurve");
                        break;
                    case 3:
                        bState = xls.selectSheet("VoltCompen");
                        break;
                    default:
                        break;
                }

                if (bState)
                {
                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < dgv.ColumnCount; j++)
                        {
                            if (j == 0) continue;//为温度值

                            float f = 0.0F;
                            if (float.TryParse(xls.read(2 + i, 1 + j).ToString(), out f))
                            {
                                dgv.Rows[i].Cells[j].Value = f;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        
        public virtual bool Curve_exportExcel(string file, DataGridView dgv, int iType)
        {
            using (XLSXWrapper xls = new XLSXWrapper(file))
            {
                switch (iType)
                {
                    case 0:
                        if (xls.selectSheet("VoltCurve"))
                        {
                        }
                        else
                        {
                            xls.addSheet("VoltCurve");
                        }
                        break;
                    case 1:
                        if (xls.selectSheet("SlopeCurve"))
                        {
                        }
                        else
                        {
                            xls.addSheet("SlopeCurve");
                        }
                        break;
                    case 2:
                        if (xls.selectSheet("TempCurve"))
                        {
                        }
                        else
                        {
                            xls.addSheet("TempCurve");
                        }
                        break;
                    case 3:
                        if (xls.selectSheet("VoltCompen"))
                        {
                        }
                        else
                        {
                            xls.addSheet("VoltCompen");
                        }
                        break;
                    default:
                        break;
                }
                                

                int row = 1;
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    xls.write(row, 1 + i, dgv.Columns[i].HeaderText);
                }
                row++;

                for (int i = 0; i < dgv.RowCount; i++)
                {
                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        xls.write(row, j + 1, dgv.Rows[i].Cells[j].Value.ToString());
                    }
                    row++;
                }

                return xls.saveAs(file);
            }
        }        
        */
        public virtual bool TempCurve_read(int[,] data_out)
        {
            return false;
        }
        public virtual bool SlopeCurve_read(int[,] data_out)
        {
            return false;
        }
        public virtual bool VoltCurve_read(int[,] data_out)
        {
            return false;
        }
        public virtual bool VoltCompen_read(int[,] data_out)
        {
            return false;
        }

        public virtual bool TempCurve_write(int[,] data_in, int tempTabIndex/* -1=写入全部表 */)
        {

            return false;
        }
        public virtual bool SlopeCurve_write(int[,] data_in, int tempTabIndex/* -1=写入全部表 */)
        {

            return false;
        }
        public virtual bool VoltCurve_write(int[,] data_in, int tempTabIndex/* -1=写入全部表 */)
        {

            return false;
        }
        public virtual bool VoltCompen_write(int[,] data_in, int tempTabIndex/* -1=写入全部表 */)
        {

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data_in">平移前的数据</param>
        /// <param name="data_out">平移后的数据</param>
        /// <param name="tempStep">点的温度间隔</param>
        /// <returns></returns>
        public virtual bool TempCurve_parallelTransform(int[,] data_in, int[,] data_out, int tempTabIndex, int TranslateValue)
        {
            return false;
        }
        public virtual bool VoltCurve_parallelTransform(int[,] data_in, int[,] data_out, int tempTabIndex, int TranslateValue)
        {
            return false;
        }
        public virtual bool VoltCompen_parallelTransform(int[,] data_in, int[,] data_out, int tempTabIndex, int TranslateValue)
        {
            return false;
        }
        public virtual bool SlopeCurve_parallelTransform(int[,] data_in, int[,] data_out, int tempTabIndex, int TranslateValue)
        {
            return false;
        }

        /// <summary>
        /// flash保存标志
        /// </summary>
        /// <param name="data_in"></param>
        /// <param name="data_out">/param>
        /// <param name="tempStep"></param>
        /// <returns></returns>
        public virtual bool SaveToFlashStr()
        {
            return true;
        }
        public virtual bool SoftwareReset()
        {
            return true;
        }                       
    }
}
