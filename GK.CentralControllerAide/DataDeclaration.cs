using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.CentralControllerAide
{
    /// <summary>
    /// 设备型号
    /// </summary>
    public enum DeviceModel
    {
        None,
        ModelGK580,
        ModelGK600,
        ModelGK800
    }

    /// <summary>
    /// 接收的数据类型
    /// </summary>
    public enum ReceiveDataType
    {
        None,
        GetDeviceModel,
        SetProjectorConfig,
        GetProjectorConfig,
        SetPresetCode,
        GetPresetCode,
        SetNetworkConfig,
        GetNetworkConfig,
        VerifyData,
    }

    public class DataDeclaration
    {
        //获取中控型号 model?
        internal static readonly byte[] DeviceModel = {0x24, 0x00, 0x00, 0x6d, 0x6f, 0x64, 0x65,
                                                       0x6c, 0x3f, 0x5e, 0x24};
        //读取投影机配置 tyj232kzs
        internal static readonly byte[] ProjectorConfig = {0x24, 0x00, 0x00, 0x74, 0x79, 0x6a, 0x32,
                                                             0x33, 0x32, 0x6b, 0x7a, 0x73, 0x5e, 0x24};
        //读取开机预置信息 zkqpzxxss
        internal static readonly byte[] PresetInfo = {0x24, 0x00, 0x00, 0x7a, 0x6b, 0x71, 0x70,
                                                            0x7a, 0x78, 0x78, 0x73, 0x73, 0x5e, 0x24};

        //读取网络信息 net?                                                     
        internal static readonly byte[] NetworkInfo = { 0x24, 0x00, 0x00, 0x6e, 0x65, 0x74, 0x3f,
                                                             0x5e, 0x24 };

        /// <summary>
        /// 字符转换成相应的十进制整数
        /// </summary>
        internal static int Char2Integer(char data)
        {
            int nCount = 0;

            if (data == '0' || data == '1' || data == '2' || data == '3' || data == '4' || data == '5' || data == '6' || data == '7'
                || data == '8' || data == '9')
            {
                nCount = data - '0';
            }
            else if (data == 'a' || data == 'b' || data == 'c' || data == 'd' || data == 'e' || data == 'f')
            {
                nCount = data - 'a' + 10;
            }

            else if (data == 'A' || data == 'B' || data == 'C' || data == 'D' || data == 'E' || data == 'F')
            {
                nCount = data - 'A' + 10;
            }

            return nCount;
        }

        /// <summary>
        /// 字符串转十六进制
        /// </summary>
        internal static string String2Hex(string inputdata)
        {
            byte[] data = new byte[100];

            ASCIIEncoding ae = new ASCIIEncoding();
            data = ae.GetBytes(inputdata);
            inputdata = null;

            foreach (byte d in data)
            {
                inputdata += Convert.ToString(d, 16) + ' ';
            }

            return inputdata;
        }

        /// <summary>
        /// 十六进制转字符串
        /// </summary>
        internal static string Hex2String(string inputData)
        {
            int index = 0;
            byte[] data = new byte[100];

            for (int i = 0; i < inputData.Length - 1; i++)
            {
                if (inputData[i] != ' ' && inputData[i + 1] != ' ')
                {
                    data[index++] = (byte)((Char2Integer(inputData[i])) * 16 + Char2Integer(inputData[i + 1]));
                }
            }

            inputData = null;
            ASCIIEncoding ae = new ASCIIEncoding();
            inputData = ae.GetString(data, 0, index);

            return inputData;
        }

        /// <summary>
        /// 获取byte数组
        /// </summary>
        /// <param name="inputCodes">输入的投影机控制码</param>
        /// <param name="hex">十六进制码标志，true则inputCodes为十六进制码，false则inputCodes为ASCII码</param>
        /// <param name="length">有效数据的个数</param>
        /// <returns>转换后的byte数组</returns>
        internal static byte[] GetByteArray(string inputCodes, bool hex, ref int length)
        {
            byte[] data = new byte[100];
            int count = 0;

            if (inputCodes != null)
            {
                if (hex)
                {
                    //十六进制字符
                    for (int i = 0; i < inputCodes.Length - 1; i++)
                    {
                        if (inputCodes[i] != ' ' && inputCodes[i + 1] != ' ')
                        {
                            data[count++] = (byte)(Char2Integer(inputCodes[i]) * 16 + Char2Integer(inputCodes[i + 1]));
                        }
                    }

                    length = count;
                }
                else
                {
                    //ASCII字符
                    ASCIIEncoding ae = new ASCIIEncoding();
                    data = ae.GetBytes(inputCodes);

                    length = inputCodes.Length;
                }
            }
            else
            {
                length = 0;
                data = null;
            }

            return data;
        }


        /// <summary>
        /// 格式化投影机控制码，为其加上头码，功能码，数据长度，波特率，校验位，尾码
        /// </summary>
        /// <param name="function">功能码字节</param>
        /// <param name="baudrate">波特率字节</param>
        /// <param name="paritybit">校验位字节</param>
        /// <param name="inputCodes">输入的投影机控制码</param>
        /// <param name="hex">十六进制码标志，true则inputCodes为十六进制码，false则inputCodes为ASCII码</param>
        /// <returns>格式化好的控制码byte数组</returns>
        internal static byte[] SetDataFormat(byte function, byte baudrate,
            byte paritybit, string inputCodes, bool hex)
        {
            int length = 0;
            byte[] tmp = GetByteArray(inputCodes, hex, ref length);
            byte[] data = new byte[length + 7];

            data[0] = 0x24;
            data[1] = function;
            data[2] = (byte)(length + 2);
            data[3] = baudrate;
            data[4] = paritybit;

            if (tmp != null)
            {
                for (int i = 0; i < length; i++)
                {
                    data[i + 5] = tmp[i];
                }
            }

            data[length + 5] = 0x5e;
            data[length + 6] = 0x24;


            return data;
        }

    }
}
