using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Windows;

namespace GK.CentralControllerAide
{
    public class RS232Communication
    {
        private static SerialPort sptRS232 = new SerialPort();
        private DeviceModel model = DeviceModel.None;
        private List<byte> receivdData = new List<byte>();
        private ReceiveDataType rdType = ReceiveDataType.None;

        public ReceiveDataType RdType
        {
            get
            {
                return rdType;
            }

            set
            {
                rdType = value;
            }
        }

        ~RS232Communication()
        {
            Close();
        }

        public DeviceModel InitConnection()
        {
            //遍历计算机中的串口
            foreach (string portName in SerialPort.GetPortNames())
            {
                //首先确认串口关闭
                Close();

                sptRS232.PortName = portName;
                //sptRS232.PortName = "COM2";

                //配置串口
                sptRS232.BaudRate = 9600;
                sptRS232.Parity = Parity.None;
                sptRS232.DataBits = 8;
                sptRS232.StopBits = StopBits.One;
                sptRS232.ReceivedBytesThreshold = 1;
                sptRS232.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                sptRS232.DtrEnable = true;
                sptRS232.RtsEnable = true;
                sptRS232.ReadTimeout = 1000;

                try
                {
                    sptRS232.Open();

                    if (sptRS232.IsOpen)
                    {
                        //获取中控型号
                        GetDeviceModel();

                        //延时50ms等待中控回应
                        Thread.Sleep(100);

                        if (model == DeviceModel.ModelGK580 || model == DeviceModel.ModelGK600
                            || model == DeviceModel.ModelGK800)
                        {
                            break;
                        }
                        else
                        {
                            //若未查询到中控型号，关闭当前串口，设置下一个串口
                            sptRS232.Close();

                            continue;
                        }
                    }

                }
                catch
                {
                }
            }

            return model;
        }

        private void GetDeviceModel()
        {
            rdType = ReceiveDataType.GetDeviceModel;

            SendData(DataDeclaration.DeviceModel, 0, DataDeclaration.DeviceModel.Length);       
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //延时50ms等待数据到达接收缓冲区
            Thread.Sleep(50);

            byte[] data = new byte[sptRS232.BytesToRead];
            ReceiveData(data, 0, data.Length);

            //将每次接收的数据放入list中，并判断数据是否完整
            receivdData.AddRange(data);

            if (receivdData[0] == 0x24 && receivdData[receivdData.Count - 2] == 0x5e
                && receivdData[receivdData.Count - 1] == 0x24)
            {
                switch (rdType)
                {
                    case ReceiveDataType.GetDeviceModel:
                        #region
                        //GK580
                        if (receivdData[3] == 0x47 && receivdData[4] == 0x4b &&
                            receivdData[5] == 0x35 && receivdData[6] == 0x38 &&
                            receivdData[7] == 0x30)
                        {
                            model = DeviceModel.ModelGK580;
                        }
                        //GK600
                        else if (receivdData[3] == 0x47 && receivdData[4] == 0x4b &&
                            receivdData[5] == 0x36 && receivdData[6] == 0x30 &&
                            receivdData[7] == 0x30)
                        {
                            model = DeviceModel.ModelGK600;
                        }
                        //GK800
                        else if (receivdData[3] == 0x47 && receivdData[4] == 0x4b &&
                            receivdData[5] == 0x38 && receivdData[6] == 0x30 &&
                            receivdData[7] == 0x30)
                        {
                            model = DeviceModel.ModelGK800;
                        }
                        //未找到中控型号
                        else
                        {
                            model = DeviceModel.None;
                        }
                        #endregion
                        //清空list
                        receivdData.Clear();
                        break;
                    case ReceiveDataType.SetProjectorConfig:
                        break;
                    case ReceiveDataType.GetProjectorConfig:
                        break;
                    case ReceiveDataType.SetPresetCode:
                        break;
                    case ReceiveDataType.GetPresetCode:
                        break;
                    case ReceiveDataType.SetNetworkConfig:
                        break;
                    case ReceiveDataType.GetNetworkConfig:
                        break;
                    case ReceiveDataType.VerifyData:
                        break;
                }
            }
        }

        public void SendData(byte[] data, int offset, int count)
        {
            if (sptRS232.IsOpen)
            {
                sptRS232.Write(data, offset, count);
            }
        }

        public void ReceiveData(byte[] data, int offset, int count)
        {
            if (sptRS232.IsOpen)
            {
                sptRS232.Read(data, offset, count);
            }
        }

        private void Close()
        {
            if (sptRS232.IsOpen)
            {
                sptRS232.Close();
            }

        }
    }
}
