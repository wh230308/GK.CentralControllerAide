using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows;

namespace GK.CentralControllerAide
{
    class LoadXml
    {
        private XmlDocument xmlDoc = new XmlDocument();
        private XmlNodeList xnl;
        private string filePath = System.IO.Directory.GetCurrentDirectory() + "\\projector232Codes.xml";
        private ComboBox cmbBrand, cmbModel;
        private ListView listView;

        public LoadXml(ComboBox c1, ComboBox c2, ListView l)
        {
            cmbBrand = c1;
            cmbModel = c2;
            listView = l;
        }

        private void LoadProjectorConfig()
        {
            try
            {
                xmlDoc.Load(filePath);
            }
            catch
            {
                MessageBox.Show("未找到配置文件，加载配置失败！");
                return;
            }
        }

        //加载配置
        public void ShowListViewConfig()
        {
            LoadProjectorConfig();
            xnl = xmlDoc.SelectSingleNode("ConfigList").ChildNodes;

            //遍历xml文件中的配置
            foreach (XmlNode xn in xnl)
            {
                ConfigurationMember member = new ConfigurationMember();
                XmlElement xe = (XmlElement)xn;
                member.Brand = xe.GetAttribute("Brand");

                //对比combobox中已有的品牌，防止重复添加
                if (cmbBrand.Items.Cast<object>().All(x => x.ToString() != member.Brand))
                {
                    cmbBrand.Items.Add(member.Brand);
                }
                member.Model = xe.GetAttribute("Model");

                XmlNodeList xnlSub = xe.ChildNodes;
                foreach (XmlNode xnSub in xnlSub)
                {
                    if (xnSub.Name == "Baudrate")
                    {
                        member.Baudrate = xnSub.InnerText;
                    }

                    if (xnSub.Name == "Parity")
                    {
                        member.Parity = xnSub.InnerText;
                    }

                    if (xnSub.Name == "Databits")
                    {
                        member.Databits = xnSub.InnerText;
                    }

                    if (xnSub.Name == "Stopbits")
                    {
                        member.Stopbits = xnSub.InnerText;
                    }

                    if (xnSub.Name == "Hexadecimal")
                    {
                        member.Hexadecimal = xnSub.InnerText;
                    }

                    if (xnSub.Name == "BootCode")
                    {
                        member.BootCode = xnSub.InnerText;
                    }

                    if (xnSub.Name == "Shutdown")
                    {
                        member.ShutdownCode = xnSub.InnerText;
                    }

                    if (xnSub.Name == "RGB1")
                    {
                        member.RGB1 = xnSub.InnerText;
                    }

                    if (xnSub.Name == "RGB2")
                    {
                        member.RGB2 = xnSub.InnerText;
                    }

                    if (xnSub.Name == "VIDEO")
                    {
                        member.VIDEO = xnSub.InnerText;
                    }

                    if (xnSub.Name == "SVIDEO")
                    {
                        member.SVIDEO = xnSub.InnerText;
                    }
                }

                listView.Items.Add(member);
            }

            //排序
            SortListViewItems();
        }

        // 对listview中的配置列表进行排序
        public void SortListViewItems()
        {
            SortDescriptionCollection sdc = listView.Items.SortDescriptions;
            ListSortDirection sortDirection = ListSortDirection.Ascending;
            if (sdc.Count > 0)
            {
                SortDescription sd = sdc[0];
                sortDirection = (ListSortDirection)(((int)sd.Direction + 1) % 2);
                sdc.Clear();
            }

            sdc.Add(new SortDescription("Brand", ListSortDirection.Ascending));
        }

        //获取指定品牌的所有型号
        public void GetModels()
        {
            cmbModel.Items.Clear();

            //遍历型号属性
            foreach (XmlNode xn in xnl)
            {
                XmlElement xe = (XmlElement)xn;

                if (cmbBrand.SelectedItem.ToString() == xe.GetAttribute("Brand"))
                {
                    cmbModel.Items.Add(xe.GetAttribute("Model"));
                }
            }
        }
    }
}
