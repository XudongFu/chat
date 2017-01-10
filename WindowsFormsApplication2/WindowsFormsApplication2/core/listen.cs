using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using WindowsFormsApplication2.contract;
using System.Collections.Concurrent;
using WindowsFormsApplication2.core;
namespace WindowsFormsApplication2.dataEntry
{
   public class listen:IDisposable
    {
        UdpClient client;
        server ser;
       
       public  listen(server ser)
        {
            this.ser = ser;
            IPEndPoint local = new IPEndPoint(IPAddress.Any, actionConst.port);
            client = SendMessage.getInstance().getUdpClient();
        }
   
        public void getNextMessage(object queue)
        {
            try
            {
                ConcurrentQueue<message> con = (ConcurrentQueue<message>)queue;
                IPEndPoint remote = new IPEndPoint(IPAddress.Any,0);
                byte[] data= client.Receive(ref remote);
                ser.showText("有设备接入");
                string str = ASCIIEncoding.UTF8.GetString(data);
                message message = new message(str, remote);
                con.Enqueue(message);
            }
            catch (Exception e)
            {

            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
        
    }
}
