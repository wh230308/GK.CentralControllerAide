using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GK.CentralControllerAide
{
    public interface ICommunication
    {
        ReceiveDataType RdType
        {
            get;
            set;
        }

        DeviceModel InitConnection();
        void SendData(byte[] data, int offset, int count);
        void ReceiveData(byte[] data, int offset, int count);
    }
}
