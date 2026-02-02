/*=========================================================================
| (c) 2006-2007  Total Phase, Inc.
|--------------------------------------------------------------------------
| Project : Aardvark Sample Code
| File    : aaspi_eeprom.cs
|--------------------------------------------------------------------------
| Perform simple read and write operations to an SPI EEPROM device.
|--------------------------------------------------------------------------
| Redistribution and use of this file in source and binary forms, with
| or without modification, are permitted.
|
| THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
| "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
| LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
| FOR A PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE
| COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
| INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
| BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
| LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
| CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
| LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
| ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
| POSSIBILITY OF SUCH DAMAGE.
 ========================================================================*/

using System;
using TotalPhase;

namespace AardvarkLibrary
{
    public class Aardvark_Device
    {
        public const int PAGE_SIZE = 129;
        public const int MAX_TARGET_ADDRESS_SIZE = 16;
        private static int handle = 0;
        private static byte m_ackAddress = 48;
        private static int m_bitRate = 800;
        private static int m_responseTimeout = 150;  // ms

        public uint deviceNum;

        //public bool GetNumDevices(ref uint numDevices)
        //{
        //    ushort[] ports = new ushort[16];
        //    uint[] uniqueIds = new uint[16];
        //    int numElem = 16;
        //    numDevices = 0;

        //    // Find all the attached devices
        //    int count = AardvarkApi.aa_find_devices_ext(numElem, ports, numElem, uniqueIds);

        //    if (count > numElem) count = numElem;

        //    // Print the information on each device
        //    for (int i = 0; i < count; i++)
        //    {
        //        // Determine if the device is in-use
        //        if ((ports[i] & AardvarkApi.AA_PORT_NOT_FREE) == 0)
        //        {
        //            numDevices++;
        //        }
        //    }
        //    return true;
        //}

        public bool GetNumDevices(ref uint numDevices)
        {
            ushort[] ports = new ushort[16];
            int numElem = 16;
            numDevices = 0;

            // Find all the attached devices
            int num_devices = AardvarkApi.aa_find_devices(numElem, ports);

            if (num_devices > numElem) num_devices = numElem;

            // Print the information on each device
            for (int i = 0; i < num_devices; i++)
            {
                // Determine if the device is in-use
                if ((ports[i] & AardvarkApi.AA_PORT_NOT_FREE) == 0)
                {
                    numDevices++;
                }
            }
            return true;
        }
        private bool OpenDevice(uint deviceIndex)
        {
            int bus_timeout = 0;
            int bus_bitRate = 0;
            ushort[] ports = new ushort[16];
            int numElem = 16;
            if (IsOpen() == false)
            {
                // Find all the attached devices
                int num_devices = AardvarkApi.aa_find_devices(numElem, ports);

                if (num_devices > 0)
                {
                    if (num_devices > numElem) num_devices = numElem;

                    // Print the information on each device
                    for (int i = 0; i < num_devices; i++)
                    {
                        // Determine if the device is in-use
                        if ((ports[i] & AardvarkApi.AA_PORT_NOT_FREE) == 0)
                        {
                            if (i == deviceIndex)
                            {
                                handle = AardvarkApi.aa_open((int)deviceIndex);
                                if (handle <= 0)
                                {
                                    handle = 0;
                                    return false;
                                }

                                // Ensure that the I2C subsystem is enabled
                                AardvarkApi.aa_configure(handle, AardvarkConfig.AA_CONFIG_SPI_I2C);

                                // Enable the I2C bus pullup resistors (2.2k resistors).
                                // This command is only effective on v2.0 hardware or greater.
                                // The pullup resistors on the v1.02 hardware are enabled by default.
                                AardvarkApi.aa_i2c_pullup(handle, AardvarkApi.AA_I2C_PULLUP_BOTH);

                                // Power the EEPROM using the Aardvark adapter's power supply.
                                // This command is only effective on v2.0 hardware or greater.
                                // The power pins on the v1.02 hardware are not enabled by default.
                                AardvarkApi.aa_target_power(handle, AardvarkApi.AA_TARGET_POWER_BOTH);

                                // Set the bitrate
                                bus_bitRate = AardvarkApi.aa_i2c_bitrate(handle, m_bitRate);

                                // Set the bus lock timeout
                                bus_timeout = AardvarkApi.aa_i2c_bus_timeout(handle, (ushort)m_responseTimeout);
                                if ((bus_bitRate != m_bitRate) || (bus_timeout != m_responseTimeout))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsOpen()
        {
            if (handle != 0)
            {
                return true;
            }
            return false;
        }

        private bool CloseDevice()
        {
            if (AardvarkApi.aa_close(handle) > 0)
            {
                handle = 0;
                return false;
            }
            else
            {
                handle = 0;
                return true;
            }
        }

        private bool SetSmbusConfig()
        {
            int bus_timeout = 0;
            int bus_bitRate = 0;
            // Make sure that the device is opened
            if (IsOpen() == false)
            {
                return false;
            }
            else
            {
                // Set the bitrate
                bus_bitRate = AardvarkApi.aa_i2c_bitrate(handle, m_bitRate);

                // Set the bus lock timeout
                bus_timeout = AardvarkApi.aa_i2c_bus_timeout(handle, (ushort)m_responseTimeout);
                if ((bus_bitRate != m_bitRate) || (bus_timeout != m_responseTimeout))
                {
                    return false;
                }
            }
            return true;
        }

        private bool GetSmbusConfig()
        {
            if (IsOpen() == false)
            {
                return false;
            }
            else
            {
                // Set the bitrate
                m_bitRate = AardvarkApi.aa_i2c_bitrate(handle, m_bitRate);

                // Set the bus lock timeout
                m_responseTimeout = AardvarkApi.aa_i2c_bus_timeout(handle, (ushort)m_responseTimeout);
            }
            return true;
        }

        private bool SetTimeout()
        {
            if (IsOpen() == false)
            {
                return false;
            }
            else
            {
                // Set the bus lock timeout
                AardvarkApi.aa_i2c_bus_timeout(handle, (ushort)m_responseTimeout);
            }
            return true;
        }

        private bool GetTimeout()
        {
            if (IsOpen() == false)
            {
                return false;
            }
            else
            {
                // Set the bus lock timeout
                m_responseTimeout = AardvarkApi.aa_i2c_bus_timeout(handle, (ushort)m_responseTimeout);
            }
            return true;
        }

        private bool ReadAddrI2c(byte slaveAddress, int OffsetAddr, int numBytesToRead, byte[] readBytes)
        {
            byte[] buffer = new byte[PAGE_SIZE];
            byte[] targetAddress = new byte[MAX_TARGET_ADDRESS_SIZE];
            byte targetAddressSize = 0;
            ushort numRead = 0;
            byte numBytesRead = 0;
            Array.Clear(readBytes, 0, numBytesToRead);
            int i = OffsetAddr;

            do
            {
                targetAddress[targetAddressSize++] = (byte)(i % 256);
                i = i / 256;
                if (targetAddressSize == MAX_TARGET_ADDRESS_SIZE)
                {
                    break;
                }
            } while (i > 0);

            targetAddress[0] = (byte)OffsetAddr;

            // Write the address
            AardvarkApi.aa_i2c_write(handle, (ushort)(slaveAddress >> 1), AardvarkI2cFlags.AA_I2C_NO_STOP, (ushort)targetAddressSize, targetAddress);

            while (numBytesToRead > 0)
            {
                int readLen = numBytesToRead;

                if (numBytesToRead >= PAGE_SIZE)
                {
                    readLen = PAGE_SIZE;
                }

                numBytesRead = (byte)AardvarkApi.aa_i2c_read(handle, (ushort)(slaveAddress >> 1), AardvarkI2cFlags.AA_I2C_NO_FLAGS, (ushort)readLen, buffer);
                if (numBytesRead != readLen)
                {
                    return false;
                }
                Array.Copy(buffer, 0, readBytes, numRead, numBytesRead);
                numRead += numBytesRead;

                numBytesToRead -= readLen;
                numRead += (ushort)readLen;
            }
            return true;
        }
        private bool WriteAddrI2c(byte slaveAddress, byte OffsetAddr, int numBytesToWrite, byte[] WriteBytes)
        {
            byte[] buffer = new byte[PAGE_SIZE];
            byte writeAddr = OffsetAddr;
            int numWrite = 0;
            int count;
            while (numBytesToWrite > 0)
            {
                int writeLen = numBytesToWrite;

                if (numBytesToWrite >= PAGE_SIZE)
                {
                    writeLen = PAGE_SIZE - 1;
                }

                buffer[0] = writeAddr;
                Array.Copy(WriteBytes, numWrite, buffer, 1, writeLen);
                // Write the address and data
                count = AardvarkApi.aa_i2c_write(handle, (ushort)(slaveAddress >> 1), AardvarkI2cFlags.AA_I2C_NO_FLAGS, (ushort)(writeLen + 1), buffer);
                if (count != (writeLen + 1))
                {
                    return false;
                }
                numBytesToWrite -= writeLen;
                numWrite += writeLen;
                writeAddr += (byte)writeLen;
            }
            return true;
        }
        private bool ReadI2c(byte slaveAddress, int numBytesToRead, byte[] readBytes)
        {
            byte[] buffer = new byte[PAGE_SIZE];
            ushort numRead = 0;
            byte numBytesRead = 0;
            Array.Clear(readBytes, 0, numBytesToRead);

            while (numBytesToRead > 0)
            {
                int readLen = numBytesToRead;

                if (numBytesToRead >= PAGE_SIZE)
                {
                    readLen = PAGE_SIZE;
                }

                numBytesRead = (byte)AardvarkApi.aa_i2c_read(handle, (ushort)(slaveAddress >> 1), AardvarkI2cFlags.AA_I2C_NO_FLAGS, (ushort)readLen, buffer);
                if (numBytesRead != readLen)
                {
                    return false;
                }
                Array.Copy(buffer, 0, readBytes, numRead, numBytesRead);
                numRead += numBytesRead;

                numBytesToRead -= readLen;
                numRead += (ushort)readLen;
            }
            return true;
        }
        private bool WriteI2c(byte slaveAddress, int numBytesToWrite, byte[] WriteBytes)
        {
            byte[] buffer = new byte[PAGE_SIZE];
            int numWrite = 0;
            int count;
            while (numBytesToWrite > 0)
            {
                int writeLen = numBytesToWrite;

                if (numBytesToWrite >= PAGE_SIZE)
                {
                    writeLen = PAGE_SIZE;
                }

                Array.Copy(WriteBytes, numWrite, buffer, 0, writeLen);
                // Write the address and data
                count = AardvarkApi.aa_i2c_write(handle, (ushort)(slaveAddress >> 1), AardvarkI2cFlags.AA_I2C_NO_FLAGS, (ushort)(writeLen), buffer);
                if (count != writeLen)
                {
                    return false;
                }
                numBytesToWrite -= writeLen;
                numWrite += writeLen;
            }
            return true;
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
            if (OpenDevice(deviceNum))
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
            m_bitRate = rate;
            return SetSmbusConfig();
        }
        public bool I2C_GetRate(ref int rate)
        {
            if (GetSmbusConfig())
            {
                rate = m_bitRate;
                return true;
            }
            return false;
        }
        public bool I2C_SetAddress(byte address)
        {
            m_ackAddress = address;
            return SetSmbusConfig();
        }
        public bool I2C_GetAddress(ref byte address)
        {
            if (GetSmbusConfig())
            {
                address = m_ackAddress;
                return true;
            }
            return false;
        }

        public bool I2C_SetTimeout(int timeout)
        {
            m_responseTimeout = timeout;
            return SetTimeout();
        }
        public bool I2C_GetTimeout(ref int timeout)
        {
            if (GetTimeout())
            {
                timeout = m_responseTimeout;
                return true;
            }
            return false;
        }
        public bool ReadBytes(byte SlaveAddr, byte offsetAddr, int nBytes, byte[] rdBytes)
        {
            if (ReadAddrI2c(SlaveAddr, offsetAddr, nBytes, rdBytes))
            {
                return true;
            }
            return false;
        }
        public bool WriteBytes(byte SlaveAddr, byte offsetAddr, int nBytes, byte[] wtBytes)
        {
            if (WriteAddrI2c(SlaveAddr, offsetAddr, nBytes, wtBytes))
            {
                return true;
            }
            return false;
        }
        public bool CurrentReadBytes(byte SlaveAddr, int nBytes, byte[] rdBytes)
        {
            if (ReadI2c(SlaveAddr, nBytes, rdBytes))
            {
                return true;
            }
            return false;
        }
        public bool CurrentWriteBytes(byte SlaveAddr, int nBytes, byte[] wtBytes)
        {
            if (WriteI2c(SlaveAddr, nBytes, wtBytes))
            {
                return true;
            }
            return false;
        }
    } 
}   
