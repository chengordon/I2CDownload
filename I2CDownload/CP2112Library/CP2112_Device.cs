using System;
using SLAB_HID_TO_SMBUS;

namespace CP2112Library
{
    public class CP2112_Device
	{
        private const ushort HidSmbus_VID = 0X10C4;
        private const ushort HidSmbus_PID = 0XEA90;

        private static IntPtr m_hidSmbus;

        private uint m_bitRate = 400000;
        private byte m_ackAddress = 48;
        private int m_autoRespond = Convert.ToInt32(true);
        private ushort m_writeTimeout = 1000;
        private ushort m_readTimeout = 1000;
        private ushort m_transferRetries = 0;
        private int m_sclLowTimeout = Convert.ToInt32(false);
        private uint m_responseTimeout = 1000;

        public uint deviceNum;
        
        public bool GetNumDevices(ref uint numDevices)
		{
            return CP2112_DLL.HidSmbus_GetNumDevices(ref numDevices, HidSmbus_VID, HidSmbus_PID) == CP2112_DLL.HID_SMBUS_SUCCESS;
		}
        private int OpenDevice(uint deviceIndex)
		{
			uint num = 0;
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            if (IsOpen()==false)
			{
                result = CP2112_DLL.HidSmbus_GetNumDevices(ref num, HidSmbus_VID, HidSmbus_PID);

                if (result == CP2112_DLL.HID_SMBUS_SUCCESS)
				{
                    if (num == 0)
					{
                        result = CP2112_DLL.HID_SMBUS_DEVICE_NOT_FOUND;
					}
					else
					{
                        result = CP2112_DLL.HidSmbus_Open(ref m_hidSmbus, deviceIndex, HidSmbus_VID, HidSmbus_PID);

                        if (result == CP2112_DLL.HID_SMBUS_SUCCESS)
						{
                            result = CP2112_DLL.HidSmbus_SetSmbusConfig(m_hidSmbus, m_bitRate, m_ackAddress, m_autoRespond, m_writeTimeout, m_readTimeout, m_sclLowTimeout, m_transferRetries);
						}
					}
				}
			}
			return result;
		}
        private bool IsOpen()
		{
			int num = 0;
            if (m_hidSmbus != IntPtr.Zero)
			{
                if (CP2112_DLL.HidSmbus_IsOpened(m_hidSmbus, ref num) == CP2112_DLL.HID_SMBUS_SUCCESS)
				{
                    if (num == 1)
					{
						return true;
					}
				}
			}
			return false;
		}
        private bool CloseDevice()
        {
            return CP2112_DLL.HidSmbus_Close(m_hidSmbus) == CP2112_DLL.HID_SMBUS_SUCCESS;
        }
        private int SetSmbusConfig()
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                result = CP2112_DLL.HidSmbus_SetSmbusConfig(m_hidSmbus, m_bitRate, m_ackAddress, m_autoRespond, m_writeTimeout, m_readTimeout, m_sclLowTimeout, m_transferRetries);
            }
            return result;
        }
        private int GetSmbusConfig()
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                result = CP2112_DLL.HidSmbus_GetSmbusConfig(m_hidSmbus, ref m_bitRate, ref m_ackAddress, ref m_autoRespond, ref m_writeTimeout, ref m_readTimeout, ref m_sclLowTimeout, ref m_transferRetries);
            }
            return result;
        }
        private int SetTimeout()
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                result = CP2112_DLL.HidSmbus_SetTimeouts(m_hidSmbus, m_responseTimeout);
            }
            return result;
        }
        private int GetTimeout()
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                result = CP2112_DLL.HidSmbus_GetTimeouts(m_hidSmbus, ref m_responseTimeout);
            }
            return result;
        }

        private int ReadAddrI2c(byte slaveAddress, int OffsetAddr, int numBytesToRead, byte[] readBytes)
		{
            byte[] buffer = new byte[CP2112_DLL.HID_SMBUS_MAX_READ_RESPONSE_SIZE];
            byte[] targetAddress = new byte[CP2112_DLL.HID_SMBUS_MAX_TARGET_ADDRESS_SIZE];
            byte targetAddressSize =0;
            byte status = 0;
            byte numBytesRead = 0;
			Array.Clear(readBytes, 0, numBytesToRead);
            int i = OffsetAddr;
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen()==false)
			{
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
			}
			else
			{
                do
                {
                    targetAddress[targetAddressSize++] = (byte)(i%256);
                    i = i / 256;
                    if(targetAddressSize==CP2112_DLL.HID_SMBUS_MAX_TARGET_ADDRESS_SIZE)
                    {
                        break;
                    }
                } while (i > 0);

                targetAddress[0] = (byte)OffsetAddr;
                // Issue an address read request
                result = CP2112_DLL.HidSmbus_AddressReadRequest(m_hidSmbus, slaveAddress, (ushort)numBytesToRead, targetAddressSize, targetAddress);

                if (result == CP2112_DLL.HID_SMBUS_SUCCESS)
				{
                    ushort numRead = 0;
					do
					{
                        // Wait for a read response
                        result = CP2112_DLL.HidSmbus_GetReadResponse(m_hidSmbus, ref status, buffer, CP2112_DLL.HID_SMBUS_MAX_READ_RESPONSE_SIZE, ref numBytesRead);

                        if (result != CP2112_DLL.HID_SMBUS_SUCCESS)                        
						{
							break;
						}
                        else
                        {
                            if (status == CP2112_DLL.HID_SMBUS_S0_ERROR)
                            {
                                result = CP2112_DLL.HID_SMBUS_READ_ERROR;
                                break;
                            }
                            else
                            {
                                Array.Copy(buffer, 0, readBytes, numRead, numBytesRead);
                                numRead += numBytesRead;
                            }
                        }
                    } while (status == CP2112_DLL.HID_SMBUS_S0_BUSY);

                    CP2112_DLL.HidSmbus_CancelTransfer(m_hidSmbus);					
				}
			}
			return result;
		}
        private int WriteAddrI2c(byte slaveAddress, byte OffsetAddr, int numBytesToWrite, byte[] WriteBytes)
		{
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
			else
			{
                byte[] buffer = new byte[CP2112_DLL.HID_SMBUS_MAX_WRITE_REQUEST_SIZE];
                byte writeAddr = OffsetAddr;
                int numWrite = 0;
                byte status = 0;
                byte detailedStatus = 0;
                ushort numRetries = 0;
                ushort bytesRead = 0;
				while (numBytesToWrite > 0)
				{
                    int writeLen = numBytesToWrite;

                    if (numBytesToWrite >=CP2112_DLL.HID_SMBUS_MAX_WRITE_REQUEST_SIZE)
                    {
                        writeLen = CP2112_DLL.HID_SMBUS_MAX_WRITE_REQUEST_SIZE-1;
                    }

                    buffer[0] = writeAddr;
                    Array.Copy(WriteBytes, numWrite, buffer, 1, writeLen);
                    result = CP2112_DLL.HidSmbus_WriteRequest(m_hidSmbus, slaveAddress, buffer, (byte)(writeLen + 1));
                    if (result != CP2112_DLL.HID_SMBUS_SUCCESS)
					{
                        return result;
					}
					do
					{
                        // Issue transfer status request
                        result = CP2112_DLL.HidSmbus_TransferStatusRequest(m_hidSmbus);
                        if (result != CP2112_DLL.HID_SMBUS_SUCCESS)
						{
							break;
						}
                        // Transfer status request was successful
                        // Wait for transfer status response
                        result = CP2112_DLL.HidSmbus_GetTransferStatusResponse(m_hidSmbus, ref status, ref detailedStatus, ref numRetries, ref bytesRead);
                        if (result != CP2112_DLL.HID_SMBUS_SUCCESS)
						{
                            break;
						}

                        if (status == CP2112_DLL.HID_SMBUS_S0_ERROR)
						{
                            result = CP2112_DLL.HID_SMBUS_WRITE_ERROR;
                            break;
						}
                        // Transfer status response received successfully
					}while (status == CP2112_DLL.HID_SMBUS_S0_BUSY);
                    numBytesToWrite -= writeLen;
                    numWrite += writeLen;
                    writeAddr += (byte)writeLen;					
				}
                CP2112_DLL.HidSmbus_CancelTransfer(m_hidSmbus);				
			}

			return result;
		}
        private int ReadI2c(byte slaveAddress, int numBytesToRead, byte[] readBytes)
        {
            byte[] buffer = new byte[CP2112_DLL.HID_SMBUS_MAX_READ_RESPONSE_SIZE];
            byte status = 0;
            byte numBytesRead = 0;
            Array.Clear(readBytes, 0, numBytesToRead);
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                result = CP2112_DLL.HidSmbus_ReadRequest(m_hidSmbus, slaveAddress, (ushort)numBytesToRead);

                 if (result == CP2112_DLL.HID_SMBUS_SUCCESS)
                 {
                     ushort numRead = 0;
                     do
                     {
                         // Wait for a read response
                         result = CP2112_DLL.HidSmbus_GetReadResponse(m_hidSmbus, ref status, buffer, CP2112_DLL.HID_SMBUS_MAX_READ_RESPONSE_SIZE, ref numBytesRead);

                         if (result != CP2112_DLL.HID_SMBUS_SUCCESS)
                         {
                             break;
                         }
                         else
                         {
                             if (status == CP2112_DLL.HID_SMBUS_S0_ERROR)
                             {
                                 result = CP2112_DLL.HID_SMBUS_READ_ERROR;
                                 break;
                             }
                             else
                             {
                                 Array.Copy(buffer, 0, readBytes, numRead, numBytesRead);
                                 numRead += numBytesRead;
                             }
                         }
                     } while (status == CP2112_DLL.HID_SMBUS_S0_BUSY);

                     CP2112_DLL.HidSmbus_CancelTransfer(m_hidSmbus);
                 }               
            }
            return result;
        }
        private int WriteI2c(byte slaveAddress, int numBytesToWrite, byte[] WriteBytes)
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                byte[] buffer = new byte[CP2112_DLL.HID_SMBUS_MAX_WRITE_REQUEST_SIZE];
                int numWrite = 0;
                byte status = 0;
                byte detailedStatus = 0;
                ushort numRetries = 0;
                ushort bytesRead = 0;
                while (numBytesToWrite > 0)
                {
                    int writeLen = numBytesToWrite;

                    if (numBytesToWrite >= CP2112_DLL.HID_SMBUS_MAX_WRITE_REQUEST_SIZE)
                    {
                        writeLen = CP2112_DLL.HID_SMBUS_MAX_WRITE_REQUEST_SIZE;
                    }

                    Array.Copy(WriteBytes, numWrite, buffer, 0, writeLen);
                    result = CP2112_DLL.HidSmbus_WriteRequest(m_hidSmbus, slaveAddress, buffer, (byte)(writeLen));
                    if (result != CP2112_DLL.HID_SMBUS_SUCCESS)
                    {
                        return result;
                    }
                    do
                    {
                        // Issue transfer status request
                        result = CP2112_DLL.HidSmbus_TransferStatusRequest(m_hidSmbus);
                        if (result != CP2112_DLL.HID_SMBUS_SUCCESS)
                        {
                            break;
                        }
                        // Transfer status request was successful
                        // Wait for transfer status response
                        result = CP2112_DLL.HidSmbus_GetTransferStatusResponse(m_hidSmbus, ref status, ref detailedStatus, ref numRetries, ref bytesRead);
                        if (result != CP2112_DLL.HID_SMBUS_SUCCESS)
                        {
                            break;
                        }

                        if (status == CP2112_DLL.HID_SMBUS_S0_ERROR)
                        {
                            result = CP2112_DLL.HID_SMBUS_WRITE_ERROR;
                            break;
                        }
                        // Transfer status response received successfully
                    } while (status == CP2112_DLL.HID_SMBUS_S0_BUSY);
                    numBytesToWrite -= writeLen;
                    numWrite += writeLen;
                    continue;
                }
                CP2112_DLL.HidSmbus_CancelTransfer(m_hidSmbus);
            }

            return result;
        }
        private int GetGpioConfig(ref byte direction,ref byte mode,ref byte function,ref byte clkDiv)
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen()==false)
			{
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
			}
			else
            {
                result = CP2112_DLL.HidSmbus_GetGpioConfig(m_hidSmbus, ref direction, ref mode, ref function, ref clkDiv);
            }
            return result;
        }
        private int SetGpioConfig(byte direction, byte mode, byte function, byte clkDiv)
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                result = CP2112_DLL.HidSmbus_SetGpioConfig(m_hidSmbus, direction, mode, function, clkDiv);
            }
            return result;
        }
        private int ReadLatch(ref byte latchValue)
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                result = CP2112_DLL.HidSmbus_ReadLatch(m_hidSmbus, ref latchValue);
            }
            return result;
        }
        private int WriteLatch(byte latchValue)
        {
            int result = CP2112_DLL.HID_SMBUS_SUCCESS;
            byte mask = 0xFF;
            //byte mask = latchValue;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                result = CP2112_DLL.HID_SMBUS_INVALID_HANDLE;
            }
            else
            {
                result = CP2112_DLL.HidSmbus_WriteLatch(m_hidSmbus, latchValue, mask);
            }
            return result;
        }
        //public bool DelayMS(int delayTime_ms)
        //{
        //    DateTime now = DateTime.Now;
        //    int s;
        //    do
        //    {
        //        TimeSpan spand = DateTime.Now - now;
        //        s = (int)spand.TotalMilliseconds;
        //        //Application.DoEvents();
        //    }
        //    while (s < delayTime_ms);
        //    return true;
        //}
        public bool I2C_Open()
        {
            if (OpenDevice(deviceNum) == CP2112_DLL.HID_SMBUS_SUCCESS)
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
            if (rate > 0 && rate<400)
            {
                m_bitRate = (uint)(rate*1000);
            }
            else
            {
                m_bitRate = 400000;
            }
            
            return SetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS;
        }
        public bool I2C_GetRate(ref int rate)
        {
            if (GetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                rate = (int)(m_bitRate/1000);
                return true;
            }
            return false;
        }
        public bool I2C_SetAddress(byte address)
        {
            m_ackAddress = address;
            return SetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS;
        }
        public bool I2C_GetAddress(ref byte address)
        {
            if (GetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                address = m_ackAddress;
                return true;
            }
            return false;
        }
        public bool I2C_SetAutoRespondStatus(int state)
        {
            m_autoRespond = state;
            return SetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS;
        }
        public bool I2C_GetAutoRespondStatus(ref int state)
        {
            if (GetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                state = m_autoRespond;
                return true;
            }
            return false;
        }
        public bool I2C_SetWriteTimeout(int writeTimeout)
        {
            m_writeTimeout = (ushort)writeTimeout;
            return SetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS;
        }
        public bool I2C_GetWriteTimeout(ref int writeTimeout)
        {
            if (GetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                writeTimeout = (int)m_writeTimeout;
                return true;
            }
            return false;
        }
        public bool I2C_SetReadTimeout(int readTimeout)
        {
            m_readTimeout = (ushort)readTimeout;
            return this.SetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS;
        }
        public bool I2C_GetReadTimeout(ref int readTimeout)
        {
            if (GetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                readTimeout = (int)m_readTimeout;
                return true;
            }
            return false;
        }
        public bool I2C_SetSclLowTimeoutStatus(int state)
        {
            m_sclLowTimeout = state;
            return SetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS;
        }
        public bool I2C_GetSclLowTimeoutStatus(ref int state)
        {
            if (GetSmbusConfig() == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                state = m_sclLowTimeout;
                return true;
            }
            return false;
        }
        public bool I2C_SetTimeout(int timeout)
        {
            m_responseTimeout = (uint)timeout;
            return SetTimeout() == CP2112_DLL.HID_SMBUS_SUCCESS;
        }
        public bool I2C_GetTimeout(ref int timeout)
        {
            if (GetTimeout() == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                timeout = (int)m_responseTimeout;
                return true;
            }
            return false;
        }
        public bool ReadBytes(byte SlaveAddr, byte offsetAddr, int nBytes, byte[] rdBytes)
        {
            if (ReadAddrI2c(SlaveAddr, offsetAddr, nBytes, rdBytes) == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                //DelayMS(6);
                return true;
            }
            //DelayMS(6);
            return false;
        }
        public bool WriteBytes(byte SlaveAddr, byte offsetAddr, int nBytes, byte[] wtBytes)
        {
            if (WriteAddrI2c(SlaveAddr, offsetAddr, nBytes, wtBytes) == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                //DelayMS(6);
                return true;
            }
            //DelayMS(6);
            return false;
        }
        public bool CurrentReadBytes(byte SlaveAddr, int nBytes, byte[] rdBytes)
        {
            if (ReadI2c(SlaveAddr, nBytes, rdBytes) == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                //DelayMS(6);
                return true;
            }
            //DelayMS(6);
            return false;
        }
        public bool CurrentWriteBytes(byte SlaveAddr, int nBytes, byte[] wtBytes)
        {
            if (WriteI2c(SlaveAddr, nBytes, wtBytes) == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                //DelayMS(6);
                return true;
            }
            //DelayMS(6);
            return false;
        }

        public bool I2C_GetGpioConfig(ref byte direction, ref byte mode, ref byte function)
        {
            byte clkDiv = 0;
            if (GetGpioConfig(ref direction,ref mode,ref function,ref clkDiv) == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                return true;
            }
            return false;
        }

        public bool I2C_SetGpioConfig(byte direction, byte mode, byte function)
        {
            byte clkDiv = 0;
            if (SetGpioConfig(direction, mode, function, clkDiv) == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                return true;
            }
            return false;
        }

        public bool I2C_ReadLatch(ref byte latchValue)
        {
            if (ReadLatch(ref latchValue) == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                return true;
            }
            return false;
        }

        public bool I2C_WriteLatch(byte latchValue)
        {
            if (WriteLatch(latchValue) == CP2112_DLL.HID_SMBUS_SUCCESS)
            {
                return true;
            }
            return false;
        }
    }
}
