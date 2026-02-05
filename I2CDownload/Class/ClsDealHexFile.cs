using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace I2CDownload
{
    class ClsDealHexFile
    {
        //成功加载，则返回值为文件内容，否则为空
        public string LoadHexFile(string sfile)
        {
            string strFileContent = "";
            StreamReader reader = new StreamReader(sfile);
            strFileContent = reader.ReadToEnd();
            return strFileContent;
        }

        public bool DecodeHexFile(string strContent, ref byte[,] bytFlashSector, int iSectorSize)
        {
            //bytFlashSector = new byte[128, SectorSize];////保存要烧录的数据，以512Byte为1页保存，可以保存128*512/1024=64K
            string strFileContent = strContent;
            string strFileTemp = "";
            //Dim filehand As Integer
            int i, j, k;
            string s;
            string Lstr;
            //Dim ChipType As Integer
            int LL;
            int AAAA;
            int CC;
            int TT;
            int Sector;//扇区
            int col;//列
            int sum;

            for (i = 0; i < bytFlashSector.GetUpperBound(0) + 1; i++)
            {
                for (j = 0; j < bytFlashSector.GetUpperBound(1) + 1; j++)
                {
                    bytFlashSector[i, j] = 0xFF;
                }
            }
            int MaxFlashAddr = (bytFlashSector.GetUpperBound(0) + 1) * iSectorSize - 1;
            //
            strFileTemp = strFileContent.Replace("\r\n", ";");
            if (strFileTemp.LastIndexOf(";") == strFileTemp.Length - 1)//文件最后为";"
            {
                strFileTemp = strFileTemp.Substring(0, strFileTemp.Length - 1);
            }
            string[] a = strFileTemp.Split(';');
            int LineLen;// 一行的长度
            for (i = 0; i < a.GetUpperBound(0) + 1; i++)
            {
                Lstr = a[i];
                //Hex格式：：llaaaatt[dd...]cc
                sum = 0;
                if (Lstr.Equals(":00000001FF"))
                {
                    break;
                }
                s = Lstr.Substring(1, 2);//
                LL = HexToD(s); //数据长度域
                LineLen = LL * 2 + 10 + 1;//本行长度("：llaaaatt[dd...]cc")
                if (Lstr.Length != LineLen)//文件有误
                {
                    return false;
                }
                s = Lstr.Substring(3, 4);
                AAAA = HexToD(s);//地址域
                s = Lstr.Substring(7, 2);
                TT = HexToD(s); //记录类型的域
                //00 – 数据记录
                //01 – 文件结束记录
                //02 – 扩展段地址记录
                //04 – 扩展线性地址记录
                s = Lstr.Substring(Lstr.Length - 2, 2);
                CC = HexToD(s);//校验和域
                if (AAAA > MaxFlashAddr)
                {
                    //Countnum = 0
                    //MsgBox("超长的HEX文件", vbCritical)
                    return false;
                }
                sum = LL + (int)(AAAA / 256) + (AAAA % 256) + TT + CC;
                if (TT == 0)//数据记录
                {
                    for (k = 0; k < LL; k++)
                    {
                        s = Lstr.Substring(9 + 2 * k, 2);
                        Sector = (int)((AAAA + k) / iSectorSize);
                        col = (AAAA + k) % iSectorSize;
                        bytFlashSector[Sector, col] = (byte)HexToD(s);
                        sum = sum + bytFlashSector[Sector, col];
                    }
                    sum = (sum & 0xFF);
                    if (sum != 0)
                    {
                        //Countnum = 0
                        //MsgBox("非法的HEX文件", vbCritical)
                        return false;
                    }
                }

            }
            return true;
        }

        //iFlashSize Flash大小,单位:K
        public string ConvetHexFileToBinFile(string strContent, int iFlashSize, int iSectorSize, int iLineDisplayByteNum)
        {
            string Str = "";
            int iFlashSector = iFlashSize * 1024 / iSectorSize;
            byte[,] bytFlashSector = new byte[iFlashSector, iSectorSize];

            if (DecodeHexFile(strContent, ref bytFlashSector, iSectorSize) == false) return Str;
            
            if (iLineDisplayByteNum < 10) iLineDisplayByteNum = 10;
            StringBuilder strB = new StringBuilder();
            int row = bytFlashSector.GetUpperBound(0) + 1;//第0维个数
            int col = bytFlashSector.GetUpperBound(1) + 1;//第1维个数

            strB.Remove(0, strB.Length);
            int intIndex = 0;
            strB.Append(DecToH(intIndex, 8) + ": ");
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    strB.Append(DecToH(bytFlashSector[i, j], 2));

                    if ((intIndex + 1) % iLineDisplayByteNum != 0)
                    {
                        strB.Append(" ");
                    }
                    else
                    {
                        strB.Append("\r\n");
                        strB.Append(DecToH(intIndex + 1, 8) + ": ");
                    }
                    intIndex++;
                }
            }
            Str = strB.ToString();
            return Str;
        }

        //iFlashSize Flash大小,单位:K
        public string ConvetArrayToBinFile(byte[,] bytFlashData, int iFlashSize, int iLineDisplayByteNum)
        {
            int intIndex;
            string Str = "";
            int i, j;
            //byte[,] bytFlashData = new byte[1, 1];
            //if (DecodeHexFile(strContent, ref bytFlashData) == false) return Str;
            int iFlashPage = iFlashSize * 1024 / 512;
            byte[,] bytFlashPage = new byte[iFlashPage, 512];
            for (i = 0; i < bytFlashPage.GetUpperBound(0) + 1; i++)
            {
                for (j = 0; j < bytFlashPage.GetUpperBound(1) + 1; j++)
                {
                    bytFlashPage[i, j] = bytFlashData[i, j];
                }
            }
            if (iLineDisplayByteNum < 10) iLineDisplayByteNum = 10;
            StringBuilder strB = new StringBuilder();
            int row = bytFlashPage.GetUpperBound(0) + 1;//第0维个数
            int col = bytFlashPage.GetUpperBound(1) + 1;//第1维个数

            strB.Remove(0, strB.Length);
            intIndex = 0;
            strB.Append(DecToH(intIndex, 8) + ": ");
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    strB.Append(DecToH(bytFlashPage[i, j], 2));
                    //if ((intIndex + 1) % 16 != 0)
                    if ((intIndex + 1) % iLineDisplayByteNum != 0)
                    {
                        strB.Append(" ");
                    }
                    else
                    {
                        strB.Append("\r\n");
                        strB.Append(DecToH(intIndex + 1, 8) + ": ");
                    }
                    intIndex++;
                }
            }
            Str = strB.ToString();
            return Str;
        }
        //iFlashSize Flash大小,单位:K
        public string ConvetArrayToBinFile(byte[] bytFlashData, int iFlashSize, int iLineDisplayByteNum, byte bitAddSize)
        {
            int intLen = Math.Min(iFlashSize * 1024, bytFlashData.Length);
            int intIndex = 0;
            StringBuilder strB = new StringBuilder();
            strB.Append(DecToH(intIndex, bitAddSize) + ": ");
            for (int i = 0; i < intLen; i++)
            {
                strB.Append(DecToH(bytFlashData[i], 2));
                if ((intIndex + 1) % iLineDisplayByteNum != 0)
                {
                    strB.Append(" ");
                }
                else
                {
                    strB.Append("\r\n");
                    if ((intIndex + 1) < intLen)
                    {
                        strB.Append(DecToH(intIndex + 1, bitAddSize) + ": ");
                    }                    
                }
                intIndex++;
            }
            return strB.ToString();
        }
        //
        public byte[] GetBytesFromBinFile(string strContent)
        {
            int iStartAddr;
            int iMaxAddr = 0;
            int i;
            byte[] bytTemp = new byte[1];
            string[] strTemp = System.Text.RegularExpressions.Regex.Split(strContent, "\r\n");
            for (i = 0; i < strTemp.Length; i++)//找出最大地址
            {
                string[] a = System.Text.RegularExpressions.Regex.Split(strTemp[i], " ");
                if (a.Length < 2) continue;
                iStartAddr = HexToD(a[0].Substring(0, a[0].Length - 1));
                if (a[1].Equals("")) continue;
                if (iMaxAddr < iStartAddr + a.Length - 1) iMaxAddr = iStartAddr + a.Length - 2;
            }
            //System.Collections.ArrayList list = new System.Collections.ArrayList();
            bytTemp = new byte[iMaxAddr + 1];
            for (i = 0; i < bytTemp.Length; i++)
            {
                bytTemp[i] = 0xFF;
            }
            for (i = 0; i < strTemp.Length; i++)
            {
                string[] a = System.Text.RegularExpressions.Regex.Split(strTemp[i], " ");
                if (a.Length < 2) continue;
                iStartAddr = HexToD(a[0].Substring(0, a[0].Length - 1));
                if (a[1].Equals("")) continue;
                for (int j = 1; j < a.Length; j++)
                {
                    bytTemp[iStartAddr + j - 1] = (byte)HexToD(a[j]);
                }
            }
            return bytTemp;
        }
        public string ConvetBinFileToHexFile(byte[] bytData)
        {
            //bytData的地址从0开始，且连续
            //Hex格式：：llaaaatt[dd...]cc
            int LL, AAAA, CC, TT;
            int i;
            int addr, addr_max;
            bool bNeedSave;
            string strTemp;
            string strSave;
            //If SaveFilePath = "" Then Exit Function
            StringBuilder SB = new StringBuilder();

            addr = 0;
            addr_max = bytData.Length;
            do
            {
                bNeedSave = false;
                CC = 0;
                LL = 16;//数据长度域
                CC = CC + LL;
                AAAA = addr; //地址域
                CC = CC + (int)(AAAA / 256);
                CC = CC + (int)(AAAA % 256);
                TT = 0;//数据记录 , 记录类型的域
                CC = CC + TT;
                strTemp = ":" + DecToH(LL, 2) + DecToH(AAAA, 4) + DecToH(TT, 2);

                for (i = 0; i < 16; i++)
                {
                    CC = CC + bytData[i + addr];
                    strTemp = strTemp + DecToH(bytData[i + addr], 2);
                    if ((bytData[i + addr] != 0xFF)) //每16字节保存一行，只有全部为&HFF的时候才不需要保存
                    {
                        bNeedSave = true;
                    }
                }
                CC = CC & 0xFF;

                if ((256 - CC) == 256)
                {
                    //Dim test As Integer = 1;
                }

                CC = (256 - CC) & 0xFF;
                if (bNeedSave == true)
                {
                    strTemp = strTemp + DecToH(CC, 2) + "\r\n";
                    SB.Append(strTemp);
                }
                addr = addr + 16;
            }
            while (addr < addr_max);
            SB.Append(":00000001FF");//结束
            strSave = SB.ToString();
            //Dim file As New System.IO.StreamWriter(SaveFilePath)
            //file.WriteLine(strSave)
            //file.Close()

            return strSave;
        }

        public int HexToD(string x)
        {
            int iTemp;
            try
            {
                iTemp = Convert.ToInt32(x, 16);
            }
            catch
            {
                iTemp = 0;
            }
            return iTemp;
        }
        public string DecToH(int x, int len)
        {
            string str = Convert.ToString(x, 16).ToUpper();
            int i;
            int iTemp;
            if (str.Length < len)
            {
                iTemp = len - str.Length;
                for (i = 0; i < iTemp; i++)
                {
                    str = "0" + str;
                }
            }
            return str;
        }

    }
}
