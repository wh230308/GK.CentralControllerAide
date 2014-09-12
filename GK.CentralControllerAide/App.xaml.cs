using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace GK.CentralControllerAide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DeviceModel model = DeviceModel.None;

        //默认采用rs232方式通讯
        public RS232Communication rs232Mode = new RS232Communication();

        public App()
        {
            model = rs232Mode.InitConnection();
        }

        internal void StartWindow(object sender, StartupEventArgs e)
        {
            //switch (model)
            //{
            //    case DeviceModel.None:
            //        Application.Current.Shutdown();
            //        break;
            //    case DeviceModel.ModelGK580:
                    GK580Window gk580 = new GK580Window(rs232Mode);
                    gk580.Show();
                //    break;
                //case DeviceModel.ModelGK600:
                //    //GK600Window gk600 = new GK600Window();
                //    //gk600.Show();
                //    break;
                //case DeviceModel.ModelGK800:
                //    //GK800Window gk800 = new GK800Window();
                //    //gk800.Show();
                //    break;
            //}
        }
    }
}
