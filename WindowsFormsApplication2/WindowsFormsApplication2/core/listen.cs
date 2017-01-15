using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using WindowsFormsApplication2.contract;
using System.Collections.Concurrent;
using WindowsFormsApplication2.core;
namespace WindowsFormsApplication2.dataEntry
{
   public class listen
    {
        UdpClient client;
        server ser;
              
       public  listen(server ser)
        {
            this.ser = ser;
            IPEndPoint local = new IPEndPoint(IPAddress.Any, actionConst.port);
            client = SendMessage.getInstance().getUdpClient();
        }
   
        public void getNextMessage(object m)
        {
            manager man = (manager)m;
            ConcurrentQueue<message> con = man.messages;

            while (true)
            {
                try
                {
                    IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = client.Receive(ref remote);
                    ser.showText("有设备接入");
                    string str = ASCIIEncoding.UTF8.GetString(data);
                    message message = new message(str, remote);
                    con.Enqueue(message);
                    man.solve();
                }
                catch (Exception e)
                {
                }
            }
        }
        
    }
}
