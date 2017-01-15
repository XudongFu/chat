using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using WindowsFormsApplication2.contract;
namespace WindowsFormsApplication2.core
{
    class SendMessage
    {
        UdpClient client;
        private static SendMessage send = null;


        public UdpClient getUdpClient()
        {
            return client;
        }

        private SendMessage()
        {
            client = new UdpClient(actionConst.port);
        }

        public static SendMessage getInstance()
        {
            if (send == null)
            {
                send = new SendMessage();
                return send;
            }
            else
                return send;
        }

        public void sendMessage(message mess)
        {
            byte[] data = ASCIIEncoding.UTF8.GetBytes(mess.ToXml());
            client.Send(data, data.Length, mess.point);
        }

        public void sendMessage(string str, IPEndPoint point)
        {
            byte[] data = ASCIIEncoding.UTF8.GetBytes(str);
            client.Send(data, data.Length, point);
        }

    }
}
