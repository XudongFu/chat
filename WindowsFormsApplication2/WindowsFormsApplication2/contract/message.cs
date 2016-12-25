using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace WindowsFormsApplication2.contract
{
   public  abstract class message
    {
        

        string actionType;
        public XmlNode value;
        public  Socket socket;
        /// <summary>
        /// 消息的序列号
        /// </summary>
        public  int th;

        message(string xml,Socket socket)
        {
            this.socket = socket;
            XmlDocument doc = new  XmlDocument();
            doc.LoadXml(xml);
            XmlNode message = doc.SelectSingleNode("message");
            th = int.Parse(message.SelectSingleNode("th").Value);
            actionType = message.SelectSingleNode("actione").Value;
            value = message.SelectSingleNode("value");
        }


        public  string getAction()
        {
           return actionType;
        }

       

        public  void sendConfirm(bool success)
        {
            if (success)
            {
                string succ = @"<success></success>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(succ);
                string message=getComfirm(th,doc.SelectSingleNode("success"));
                sendStringToDevice(message);
            }
            else
            {
                string succ = @"<fail></fail>";
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(succ);
                string message = getComfirm(th, doc.SelectSingleNode("fail"));
                sendStringToDevice(message);

            }
        }

        private  string getComfirm(int th,XmlNode value)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(@"<confirm><th></th><value></value></confirm>");

            XmlNode confirm = doc.SelectSingleNode("cpnfirm");
            XmlNode xuhao = confirm.SelectSingleNode("th");
            XmlNode valu = confirm.SelectSingleNode("value");
            xuhao.Value = th.ToString();
            valu.AppendChild(value);
            return confirm.ToString();

        }


        void sendStringToDevice(string message)
        {
            if (socket.Connected)
            {
                byte[] data = ASCIIEncoding.Unicode.GetBytes(message);
                socket.Send(data);
            }

        }


       public  abstract string ToXml();

    }
}
