using System;
using USBIOX;

namespace CH341Library
{
    public class CH341_Device
	{
        public uint deviceNum = 0;

        private uint m_bitRateMode = 3;//0=低速/20KHz,1=标准/100KHz(默认值),2=快速/400KHz,3=高速/750KHz
        private uint m_writeTimeout = 1000;
        private uint m_readTimeout = 1000;

        private bool DelayMS(int delayTime_ms)
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

        private void memset(byte[] buf, byte val, int size)
        {
            for (int i = 0; i < size; i++)
            {
                buf[i] = val;
            }
        }
        private bool OpenDevice()
		{
            bool result = false;
            uint mode = (m_bitRateMode & 0x03)|0x80;

            USBIOXdll.USBIO_OpenDevice(deviceNum);
            DelayMS(1);
            result = USBIOXdll.USBIO_SetStream(deviceNum, mode);
            DelayMS(1);
            return result;
		}
        private bool CloseDevice()
        {
            USBIOXdll.USBIO_CloseDevice(deviceNum);
            DelayMS(1);
            return true;
        }
        private bool SetTimeout()
        {
            return USBIOXdll.USBIO_SetTimeout(deviceNum, m_writeTimeout, m_readTimeout);
        }
        private bool ReadAddrI2c(byte slaveAddress, byte OffsetAddr, int numBytesToRead, byte[] readBytes)
        {
            uint numBytesRead = (uint)numBytesToRead;
            byte[] readbuffer = new byte[USBIOXdll.mCH341_PACKET_LENGTH * 2];
            byte[] writebuffer = new byte[2];
            writebuffer[0] = slaveAddress;

            Array.Clear(readBytes, 0, numBytesToRead);

            bool result = false;
            uint iIndex = 0;
            uint numRead = 0;
            while (numBytesRead > 0)
            {
                numRead = numBytesRead;
                if (numRead > USBIOXdll.mCH341_PACKET_LENGTH) numRead = USBIOXdll.mCH341_PACKET_LENGTH;
                writebuffer[1] = (byte)(OffsetAddr + iIndex);

                result = USBIOXdll.USBIO_StreamI2C(deviceNum, 2, writebuffer, numRead, readbuffer);

                if (result != true)
                {
                    break;
                }
                else
                {
                    Array.Copy(readbuffer, 0, readBytes, iIndex, numRead);
                    iIndex += numRead;
                    numBytesRead -= numRead;
                }
                DelayMS(1);
            }

            return result;
        }
        private bool WriteAddrI2c(byte slaveAddress, byte OffsetAddr, int numBytesToWrite, byte[] WriteBytes)
        {
            uint numBytesWrite = (uint)numBytesToWrite;
            byte[] writebuffer = new byte[USBIOXdll.mCH341_PACKET_LENGTH * 2];
            byte[] readbuffer = new byte[1];
            Array.Clear(readbuffer, 0, readbuffer.Length);
            writebuffer[0] = slaveAddress;

            bool result = false;
            uint iIndex = 0;
            uint numwrite = 0;
            while (numBytesWrite > 0)
            {
                numwrite = numBytesWrite;
                if (numwrite > USBIOXdll.mCH341_PACKET_LENGTH - 2) numwrite = USBIOXdll.mCH341_PACKET_LENGTH - 2;
                writebuffer[1] = (byte)(OffsetAddr + iIndex);
                Array.Copy(WriteBytes, iIndex, writebuffer, 2, numwrite);

                result = USBIOXdll.USBIO_StreamI2C(deviceNum, numwrite + 2, writebuffer, 0, readbuffer);

                if (result != true)
                {
                    break;
                }
                else
                {

                    iIndex += numwrite;
                    numBytesWrite -= numwrite;
                }
                DelayMS(1);
            }

            return result;
        }

        private bool ReadI2c(byte slaveAddress, int numBytesToRead, byte[] readBytes)
        {
            uint numBytesRead = (uint)numBytesToRead;
            byte[] readbuffer = new byte[USBIOXdll.mCH341_PACKET_LENGTH * 2];
            byte[] writebuffer = new byte[1];
            writebuffer[0] = slaveAddress;

            Array.Clear(readBytes, 0, numBytesToRead);

            bool result = false;
            uint iIndex = 0;
            uint numRead = 0;

            while (numBytesRead > 0)
            {
                numRead = numBytesRead;
                if (numRead > USBIOXdll.mCH341_PACKET_LENGTH) numRead = USBIOXdll.mCH341_PACKET_LENGTH;

                result = USBIOXdll.USBIO_StreamI2C(deviceNum, 1, writebuffer, numRead, readbuffer);

                if (result != true)
                {
                    break;
                }
                else
                {
                    Array.Copy(readbuffer, 0, readBytes, iIndex, numRead);
                    iIndex += numRead;
                    numBytesRead -= numRead;
                }
            }
            return result;
        }
        private bool WriteI2c(byte slaveAddress, int numBytesToWrite, byte[] WriteBytes)
        {
            uint numBytesWrite = (uint)numBytesToWrite;
            byte[] writebuffer = new byte[USBIOXdll.mCH341_PACKET_LENGTH * 2];
            byte[] readbuffer = new byte[1];
            writebuffer[0] = slaveAddress;

            bool result = false;
            uint iIndex = 0;
            uint numwrite = 0;

            while (numBytesWrite > 0)
            {
                numwrite = numBytesWrite;
                if (numwrite > USBIOXdll.mCH341_PACKET_LENGTH - 1) numwrite = USBIOXdll.mCH341_PACKET_LENGTH - 1;
                Array.Copy(WriteBytes, iIndex, writebuffer, 1, numwrite);

                result = USBIOXdll.USBIO_StreamI2C(deviceNum, numwrite + 1, writebuffer, 0, readbuffer);

                if (result != true)
                {
                    break;
                }
                else
                {

                    iIndex += numwrite;
                    numBytesWrite -= numwrite;
                }
            }
            return result;
        }
        
        public bool I2C_Open()
        {
            if (OpenDevice() == true)
            {
                return true;
            }
            return false;
        }
        public bool I2C_Close()
        {
            return CloseDevice();
        }
        public bool I2C_SetRate(int rate)
        {
            if (rate > 0 && rate<100)
            {
                m_bitRateMode = 0;               
            }
            else if (rate >= 100 && rate < 400)
            {
                m_bitRateMode = 1;
            }
            else if (rate >= 400 && rate < 750)
            {
                m_bitRateMode = 2;
            }
            else
            {
                m_bitRateMode = 3;
            }

            uint mode = (m_bitRateMode & 0x03) | 0x80;

            return USBIOXdll.USBIO_SetStream(deviceNum, mode);           
        }      
        public bool I2C_SetWriteTimeout(int writeTimeout)
        {
            m_writeTimeout = (ushort)writeTimeout;
            return USBIOXdll.USBIO_SetTimeout(deviceNum, m_writeTimeout, m_readTimeout);
        }    
        public bool I2C_SetReadTimeout(int readTimeout)
        {
            m_readTimeout = (ushort)readTimeout;
            return USBIOXdll.USBIO_SetTimeout(deviceNum, m_writeTimeout, m_readTimeout);
        }
        public bool ReadBytes(byte SlaveAddr, byte offsetAddr, int nBytes, byte[] rdBytes)
        {
            if (ReadAddrI2c(SlaveAddr, offsetAddr, nBytes, rdBytes) == true)
            {
                return true;
            }
            return false;
        }
        public bool WriteBytes(byte SlaveAddr, byte offsetAddr, int nBytes, byte[] wtBytes)
        {
            if (WriteAddrI2c(SlaveAddr, offsetAddr, nBytes, wtBytes) == true)
            {
                return true;
            }
            return false;
        }
        public bool CurrentReadBytes(byte SlaveAddr, int nBytes, byte[] rdBytes)
        {
            if (ReadI2c(SlaveAddr, nBytes, rdBytes) == true)
            {
                return true;
            }
            return false;
        }
        public bool CurrentWriteBytes(byte SlaveAddr, int nBytes, byte[] wtBytes)
        {
            if (WriteI2c(SlaveAddr, nBytes, wtBytes) == true)
            {
                return true;
            }
            return false;
        }
    }
}
