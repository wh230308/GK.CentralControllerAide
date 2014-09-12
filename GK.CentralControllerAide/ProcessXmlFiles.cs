using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Xml;

namespace GK.CentralControllerAide
{
    /// <summary>
    /// 软件启动时，加载投影机的rs232控制码配置到Listview列表中，保存新增的或修改的某一投影机配
    /// 置到配置文件中，删除投影机配置文件中与Listview列表中对应的某一投影机配置；
    /// 软件启动时，创建软件属性配置文件，若当前输入的投影机配置成功写入中控，则保存当前投影机配置信息，
    /// 当需要控制投影机发出“开机”、“关机”...这些命令时或进行数据校验时，从文件中读取投影机的当前配置信息。
    /// </summary>
    class ProcessXmlFiles
    {
        private string rs232Config = System.IO.Directory.GetCurrentDirectory() + "\\projector232Codes.xml";
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            + @"\CentralControllerAide";
        private string file;
        private XmlDocument xmlDoc = new XmlDocument();

        public ProcessXmlFiles()
        {
            file = path + @"\performance.xml";
            CreateXmlFile();
        }

        private void CreateXmlFile()
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    //创建目录
                    Directory.CreateDirectory(path);

                    //创建文件
                    string file = path + @"\performance.xml";
                    if (!File.Exists(file))
                    {
                        File.Create(file);
                    }
                }
                else
                {
                    //创建文件
                    string file = path + @"\performance.xml";
                    if (!File.Exists(file))
                    {
                        File.Create(file);
                    }
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex + "创建软件属性配置文件失败！");
            }

        }

        private void LoadXmlFile()
        {
            try
            {
                xmlDoc.Load(file);
            }
            catch
            {
                MessageBox.Show("未找到配置文件！");
                return;
            }
        }

        private void LoadConfig2ListView()
        { 
            
        }

        public void AddXmlNode(ConfigurationMember member)
        {
            LoadXmlFile();

            //创建PresentConfig节点
            XmlElement xePresentConfig = xmlDoc.CreateElement("PresentConfig");

            //添加PresentConfig节点的品牌属性
            xePresentConfig.SetAttribute("Brand", member.Brand);

            //添加PresentConfig节点的型号属性
            xePresentConfig.SetAttribute("Model", member.Model);

            //添加PresentConfig节点的十六进制码子节点
            XmlElement xnHexadecimal = xmlDoc.CreateElement("Hexadecimal");
            xnHexadecimal.InnerText = member.Hexadecimal;
            xePresentConfig.AppendChild(xnHexadecimal);

            //添加PresentConfig节点的波特率子节点
            XmlElement xnBaudrate = xmlDoc.CreateElement("Baudrate");
            xnBaudrate.InnerText = member.Baudrate;
            xePresentConfig.AppendChild(xnBaudrate);

            //添加PresentConfig节点的校验位子节点
            XmlElement xnParity = xmlDoc.CreateElement("Paritybit");
            xnParity.InnerText = member.Parity;
            xePresentConfig.AppendChild(xnParity);

            //添加PresentConfig节点的数据位子节点
            XmlElement xnDatabits = xmlDoc.CreateElement("Databits");
            xnDatabits.InnerText = member.Databits;
            xePresentConfig.AppendChild(xnDatabits);

            //添加PresentConfig节点的停止位子节点
            XmlElement xnStopbits = xmlDoc.CreateElement("Stopbit");
            xnStopbits.InnerText = member.Stopbits;
            xePresentConfig.AppendChild(xnStopbits);

            //添加PresentConfig节点的开机码子节点
            XmlElement xnStartingUp = xmlDoc.CreateElement("Boot");
            xnStartingUp.InnerText = member.BootCode;
            xePresentConfig.AppendChild(xnStartingUp);

            //添加PresentConfig节点的关机码子节点
            XmlElement xnShutdown = xmlDoc.CreateElement("Shutdown");
            xnShutdown.InnerText = member.ShutdownCode;
            xePresentConfig.AppendChild(xnShutdown);

            //添加PresentConfig节点的RGB1子节点
            XmlElement xnRGB1 = xmlDoc.CreateElement("RGB1");
            xnRGB1.InnerText = member.RGB1;
            xePresentConfig.AppendChild(xnRGB1);

            //添加PresentConfig节点的RGB2子节点
            XmlElement xnRGB2 = xmlDoc.CreateElement("RGB2");
            xnRGB2.InnerText = member.RGB2;
            xePresentConfig.AppendChild(xnRGB2);

            //添加PresentConfig节点的VIDEO子节点
            XmlElement xnVIDEO = xmlDoc.CreateElement("VIDEO");
            xnVIDEO.InnerText = member.VIDEO;
            xePresentConfig.AppendChild(xnVIDEO);

            //添加PresentConfig节点的SVIDEO子节点
            XmlElement xnSVIDEO = xmlDoc.CreateElement("SVIDEO");
            xnSVIDEO.InnerText = member.SVIDEO;
            xePresentConfig.AppendChild(xnSVIDEO);

            xmlDoc.Save(file);
        }
    }
}
