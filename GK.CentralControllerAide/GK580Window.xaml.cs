using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;

namespace GK.CentralControllerAide
{
    /// <summary>
    /// GK580Window.xaml 的交互逻辑
    /// </summary>
    public partial class GK580Window : Window
    {
        private RS232Communication rs232Mode;
        private bool hex = false;
        private byte byBaudRate;//波特率
        private byte byParityBit;//校验位
        private string filePath = System.IO.Directory.GetCurrentDirectory() + "\\projector232Codes.xml";
        private LoadXml loadXml;
        private byte[] presetCodes = new byte[16];
        private BackgroundWorker bgwProcessData = new BackgroundWorker();
        private byte[] functionCodes = { 0x31, 0x32, 0x33, 0x34, 0x35, 0x36 };
        private string[] inputCodes = new string[6];

        public GK580Window(RS232Communication rs232Mode)
        {
            InitializeComponent();

            this.rs232Mode = rs232Mode;
            bgwProcessData.WorkerSupportsCancellation = true;
            bgwProcessData.DoWork += new DoWorkEventHandler(bgwProcessData_DoWork);
        }

        private void bgwProcessData_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            switch (rs232Mode.RdType)
            {
                case ReceiveDataType.SetProjectorConfig:
                    WriteProjectorCode(inputCodes);
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
            }

            worker.CancelAsync();

            if (worker.CancellationPending)
            {
                return;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitComboBox();

            checkBox1.IsChecked = true;
        }

        private void InitComboBox()
        {
            //波特率
            comboBox1.Items.Add("9600");
            comboBox1.Items.Add("19200");
            comboBox1.Items.Add("38400");
            comboBox1.Items.Add("115200");

            comboBox1.SelectedIndex = 0;

            //校验位
            comboBox2.Items.Add("None");
            comboBox2.Items.Add("Odd");
            comboBox2.Items.Add("Even");

            comboBox2.SelectedIndex = 0;

            //数据位
            comboBox3.Items.Add("5");
            comboBox3.Items.Add("6");
            comboBox3.Items.Add("7");
            comboBox3.Items.Add("8");

            comboBox3.SelectedIndex = 3;
            comboBox3.IsEnabled = false;

            //停止位
            comboBox4.Items.Add("none");
            comboBox4.Items.Add("1");
            comboBox4.Items.Add("1.5");
            comboBox4.Items.Add("2");

            comboBox4.SelectedIndex = 1;
            comboBox4.IsEnabled = false;
        }

        private void miReconnect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miDisconnect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void miRs232_Click(object sender, RoutedEventArgs e)
        {
            if (miUsb.IsChecked)
            {
                miUsb.IsChecked = false;

                miRs232.IsChecked = true;
            }
        }

        private void miUsb_Click(object sender, RoutedEventArgs e)
        {
            if (miRs232.IsChecked)
            {
                miRs232.IsChecked = false;

                miUsb.IsChecked = true;
            }
        }

        //按十六进制码发送
        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            hex = true;

            if (textBox3.Text != "")
            {
                textBox3.Text = DataDeclaration.String2Hex(textBox3.Text);
            }

            if (textBox4.Text != "")
            {
                textBox4.Text = DataDeclaration.String2Hex(textBox4.Text);
            }

            if (textBox5.Text != "")
            {
                textBox5.Text = DataDeclaration.String2Hex(textBox5.Text);
            }

            if (textBox6.Text != "")
            {
                textBox6.Text = DataDeclaration.String2Hex(textBox6.Text);
            }

            if (textBox7.Text != "")
            {
                textBox7.Text = DataDeclaration.String2Hex(textBox7.Text);
            }

            if (textBox8.Text != "")
            {
                textBox8.Text = DataDeclaration.String2Hex(textBox8.Text);
            }
        }

        //按ASCII码发送
        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            hex = false;

            if (textBox3.Text != "")
            {
                textBox3.Text = DataDeclaration.Hex2String(textBox3.Text);
            }

            if (textBox4.Text != "")
            {
                textBox4.Text = DataDeclaration.Hex2String(textBox4.Text);
            }

            if (textBox5.Text != "")
            {
                textBox5.Text = DataDeclaration.Hex2String(textBox5.Text);
            }

            if (textBox6.Text != "")
            {
                textBox6.Text = DataDeclaration.Hex2String(textBox6.Text);
            }

            if (textBox7.Text != "")
            {
                textBox7.Text = DataDeclaration.Hex2String(textBox7.Text);
            }

            if (textBox8.Text != "")
            {
                textBox8.Text = DataDeclaration.Hex2String(textBox8.Text);
            }
        }

   
        //选择波特率
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem.ToString();
            
            if(value != null)
            {
                switch (value)
                {
                    case "9600":
                        byBaudRate = 0x01;
                        break;
                    case "19200":
                        byBaudRate = 0x02;
                        break;
                    case "38400":
                        byBaudRate = 0x03;
                        break;
                    case "115200":
                        byBaudRate = 0x04;
                        break;
                    default:
                        byBaudRate = 0x01;
                        break;
                }
            }
        }

        private void comboBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem.ToString();

            if (value != null)
            {
                switch (value)
                {
                    case "None":
                        byParityBit = 0x00;
                        break;
                    case "Odd":
                        byBaudRate = 0x01;
                        break;
                    case "Even":
                        byBaudRate = 0x02;
                        break;
                    default:
                        byBaudRate = 0x00;
                        break;
                }
            }
        }

        #region 十六进制码选中时屏蔽非16进制字符

        private void ShieldCharacter(KeyEventArgs e)
        {
            if (checkBox1.IsChecked == true)
            {
                if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.A && e.Key <= Key.F || e.Key == Key.Tab)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            ShieldCharacter(e);
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            ShieldCharacter(e);
        }

        private void textBox5_KeyDown(object sender, KeyEventArgs e)
        {
            ShieldCharacter(e);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            ShieldCharacter(e);
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            ShieldCharacter(e);
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            ShieldCharacter(e);
        }
        #endregion

        private void lvwconfiguration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfigurationMember member = (ConfigurationMember)lvwconfiguration.SelectedItem;

            if (null != member)
            {
                textBox1.Text = member.Brand;
                textBox2.Text = member.Model;
                checkBox1.IsChecked = (member.Hexadecimal == "Yes" ? true : false);
                comboBox1.SelectedItem = member.Baudrate;
                comboBox2.SelectedItem = member.Parity;
                comboBox3.SelectedItem = member.Databits;
                comboBox4.SelectedItem = member.Stopbits;
                textBox3.Text = member.BootCode;
                textBox4.Text = member.ShutdownCode;
                textBox5.Text = member.RGB1;
                textBox6.Text = member.RGB2;
                textBox7.Text = member.VIDEO;
                textBox8.Text = member.SVIDEO;
            }
        }

        private void comboBox5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {

        }

        //写入控制码
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            if (textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "" && textBox6.Text == ""
                    && textBox7.Text == "" && textBox8.Text == "")
            {
                MessageBox.Show("投影机控制码不能为空!");

                return;
            }

            GetInputCodesArray();

            if (miRs232.IsChecked == true)
            {
                rs232Mode.RdType = ReceiveDataType.SetProjectorConfig;
            }
            else
            {

            }

            //启动子线程，在子线程函数里循环写入控制码
            if (!bgwProcessData.IsBusy)
            {
                bgwProcessData.RunWorkerAsync();
            }
        }

        private void GetInputCodesArray()
        {
            if (textBox3.Text != "")
            {
                inputCodes[0] = textBox3.Text;
            }

            if (textBox4.Text != "")
            {
                inputCodes[1] = textBox4.Text;
            }

            if (textBox5.Text != "")
            {
                inputCodes[2] = textBox5.Text;
            }

            if (textBox6.Text != "")
            {
                inputCodes[3] = textBox6.Text;
            }

            if (textBox7.Text != "")
            {
                inputCodes[4] = textBox7.Text;
            }

            if (textBox8.Text != "")
            {
                inputCodes[5] = textBox8.Text;
            }
        }

        private void WriteProjectorCode(string[] inputCodes)
        {
            for (int i = 0; i < 6; i++)
            {
                byte[] data = DataDeclaration.SetDataFormat(functionCodes[i], byBaudRate, byParityBit,
                    inputCodes[i], hex);
                rs232Mode.SendData(data, 0, data.Length);

                Thread.Sleep(100);
            }
        }

        private void WriteProjectorCode(byte function, string inputCodes)
        {
            byte[] data = DataDeclaration.SetDataFormat(function, byBaudRate, byParityBit,
                inputCodes, hex);
            rs232Mode.SendData(data, 0, data.Length);

            Thread.Sleep(100);
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {

        }

        //选中开机视频信号到展示台
        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[5] = 0x04;
        }

        //取消开机视频信号到展示台
        private void radioButton1_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[5] = 0x04;
        }

        //选中开机视频信号到笔记本
        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[3] = 0x10;
        }

        //取消开机视频信号到笔记本
        private void radioButton2_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[3] = 0x00;
        }

        //选中开机视频信号到台式机
        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[2] = 0x20;
        }

        //取消开机视频信号到台式机
        private void radioButton3_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[2] = 0x00;
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (slider1.Value == 0)
            {
                //延时0分钟
                presetCodes[14] = 0x02;

                presetCodes[11] = 0x00;
                presetCodes[12] = 0x00;
                presetCodes[15] = 0x00;
            }
            else if (slider1.Value == 3.33)
            {
                //延时1分钟
                presetCodes[15] = 0x01;

                presetCodes[11] = 0x00;
                presetCodes[12] = 0x00;
                presetCodes[14] = 0x00;
            }
            else if (slider1.Value == 6.66)
            {
                //延时2分钟
                presetCodes[11] = 0x10;

                presetCodes[12] = 0x00;
                presetCodes[14] = 0x00;
                presetCodes[15] = 0x00;
            }
            else if (slider1.Value == 9.99)
            {
                //延时3分钟
                presetCodes[12] = 0x08;

                presetCodes[11] = 0x00;
                presetCodes[14] = 0x00;
                presetCodes[15] = 0x00;
            }
        }

        //选中开机打开投影机
        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[0] = 0x80;
        }

        //取消开机打开投影机
        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[0] = 0x00;
        }

        //选中发送1次关机码
        private void checkBox3_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[9] = 0x40;
        }

        //取消发送1次关机码
        private void checkBox3_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[9] = 0x00;
        }

        //选中开机静音
        private void checkBox4_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[4] = 0x08;
        }

        //取消开机静音
        private void checkBox4_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[4] = 0x00;
        }

        private void button14_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button15_Click(object sender, RoutedEventArgs e)
        {

        }


        
    }
}
