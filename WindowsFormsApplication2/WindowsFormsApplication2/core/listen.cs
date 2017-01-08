using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using WindowsFormsApplication2.contract;
using System.Collections.Concurrent;

namespace WindowsFormsApplication2.dataEntry
{
   public class listen:IDisposable
    {
        Socket receiveSocket;

       public  listen()
        {
            receiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint local = new IPEndPoint(IPAddress.Any, actionConst.port);
            receiveSocket.Bind(local);
            receiveSocket.Listen(500);
        }
       public   void getNextMessage(object queue)
        {
            ConcurrentQueue<message> con=(ConcurrentQueue<message>)queue;
            Socket remote = receiveSocket.Accept();
            byte[] data = new byte[2048];
            remote.Receive(data);
            string str = ASCIIEncoding.UTF8.GetString(data);
            message message = new message(str,remote);
            con.Enqueue(message);
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
                receiveSocket.Close();
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
