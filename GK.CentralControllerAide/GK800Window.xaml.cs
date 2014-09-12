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

namespace GK.CentralControllerAide
{
    /// <summary>
    /// GK800Window.xaml 的交互逻辑
    /// </summary>
    public partial class GK800Window : Window
    {
        private ICommunication communicate;
        private byte byBaudRate;//波特率
        private byte byParityBit;//校验位
        private string filePath = System.IO.Directory.GetCurrentDirectory() + "\\projector232Codes.xml";
        private LoadXml loadXml;
        private byte[] presetCodes = new byte[16];

        public GK800Window()
        {
            InitializeComponent();

            loadXml = new LoadXml(comboBox5, comboBox6, lvwconfiguration);
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

        }

        private void miUsb_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitComboBox();

            loadXml.ShowListViewConfig();
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


        //按十六进制码发送
        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
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

        private void radioButton7_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void radioButton8_Checked(object sender, RoutedEventArgs e)
        {

        }

        //选择波特率
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem.ToString();

            if (value != null)
            {
                switch (value)
                {
                    case "9600":
                        byBaudRate = 0x00;
                        break;
                    case "19200":
                        byBaudRate = 0x01;
                        break;
                    case "38400":
                        byBaudRate = 0x02;
                        break;
                    case "115200":
                        byBaudRate = 0x03;
                        break;
                    default:
                        byBaudRate = 0x00;
                        break;
                }
            }
        }

        //选择校验位
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
            loadXml.GetModels();
        }

        //快速查找
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            if ("" == comboBox5.Text && "" == comboBox6.Text)
            {
                return;
            }

            for (int i = 0; i < lvwconfiguration.Items.Count; i++)
            {
                ConfigurationMember tmp = new ConfigurationMember();
                tmp = (ConfigurationMember)lvwconfiguration.Items[i];

                //按品牌查找
                if (tmp.Brand == comboBox5.Text && comboBox6.Text == "")
                {
                    lvwconfiguration.ScrollIntoView(lvwconfiguration.Items[i]);
                    lvwconfiguration.SelectedIndex = i;
                    lvwconfiguration.Focus();
                    break;
                }

                //按品牌和型号查找
                if (tmp.Brand == comboBox5.Text && tmp.Model == comboBox6.Text)
                {
                    lvwconfiguration.ScrollIntoView(lvwconfiguration.Items[i]);
                    lvwconfiguration.SelectedIndex = i;
                    lvwconfiguration.Focus();
                    break;
                }
            }
        }

        //写入控制码
        private void button8_Click(object sender, RoutedEventArgs e)
        {

        }

        //读取中控配置
        private void button9_Click(object sender, RoutedEventArgs e)
        {

        }

        //数据验证
        private void button10_Click(object sender, RoutedEventArgs e)
        {

        }

        //添加修改型号
        private void button11_Click(object sender, RoutedEventArgs e)
        {

        }

        //保存当前型号
        private void button12_Click(object sender, RoutedEventArgs e)
        {

        }

        //删除当前型号
        private void button13_Click(object sender, RoutedEventArgs e)
        {

        }

        //发送开机命令
        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        //发送关机命令
        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

        //发送RGB1命令
        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }

        //发送RGB2命令
        private void button4_Click(object sender, RoutedEventArgs e)
        {

        }

        //发送VIDEO命令
        private void button5_Click(object sender, RoutedEventArgs e)
        {

        }

        //发送SVIDEO命令
        private void button6_Click(object sender, RoutedEventArgs e)
        {

        }

        #region 开机预置功能选项设置
        //选中开机视频信号到展示台
        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[5] = 0x04;

            radioButton4.IsChecked = false;
            radioButton5.IsChecked = false;
            radioButton6.IsChecked = false;
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

            radioButton4.IsChecked = false;
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

            radioButton4.IsChecked = false;
        }

        //取消开机视频信号到台式机
        private void radioButton3_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[2] = 0x00;
        }

        //选中开机VGA信号到DVD
        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[6] = 0x02;

            radioButton1.IsChecked = false;
            radioButton2.IsChecked = false;
            radioButton3.IsChecked = false;
        }

        //取消开机VGA信号到DVD
        private void radioButton4_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[6] = 0x00;
        }

        //选中开机VGA信号到AV1
        private void radioButton5_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[7] = 0x01;

            radioButton1.IsChecked = false;
        }

        //取消开机VGA信号到AV1
        private void radioButton5_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[7] = 0x00;
        }

        //选中开机VGA信号到AV2
        private void radioButton6_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[10] = 0x20;

            radioButton1.IsChecked = false;
        }

        //取消开机VGA信号到AV2
        private void radioButton6_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[10] = 0x00;
        }

        //设置关机延时
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

        //选中开机降下幕布
        private void checkBox3_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[1] = 0x40;
        }

        //取消开机降下幕布
        private void checkBox3_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[1] = 0x00;
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

        //选中投影机控制与幕布同步
        private void checkBox5_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[8] = 0x80;
        }

        //取消投影机控制与幕布同步
        private void checkBox5_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[8] = 0x00;
        }

        //选中发送1次关机码
        private void checkBox6_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[9] = 0x40;
        }

        //取消发送1次关机码
        private void checkBox6_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[9] = 0x00;
        }

        /*GK800中没有此选项
        //选中开机启动电脑
        private void checkBox7_Checked(object sender, RoutedEventArgs e)
        {
            presetCodes[13] = 0x04;
        }

        //取消开机启动电脑
        private void checkBox7_Unchecked(object sender, RoutedEventArgs e)
        {
            presetCodes[13] = 0x00;
        }
         */
        #endregion

        //写入投影机配置
        private void button14_Click(object sender, RoutedEventArgs e)
        {

        }

        //读取投影机配置
        private void button15_Click(object sender, RoutedEventArgs e)
        {

        }



    }
}
