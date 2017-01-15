using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Net;
using WindowsFormsApplication2.core;
namespace WindowsFormsApplication2.contract
{
   public  class message
    {

        string xmlString;
        string actionType;
        public XmlNode value;
        public IPEndPoint point;
        /// <summary>
        /// 消息的序列号
        /// </summary>
        public  int th;

        public int version;
        public uint from;

        public  message(string xml, IPEndPoint ippoint)
        {
            this.point = ippoint;
            xmlString = xml;
            XmlDocument doc = new  XmlDocument();
            doc.LoadXml(xml);
            XmlNode message = doc.SelectSingleNode("message");
            XmlElement mess = message as XmlElement;
            th = int.Parse(message.SelectSingleNode("th").InnerText);
            actionType = message.SelectSingleNode("action").InnerText;
            value = message.SelectSingleNode("value");

            if (actionType==actionConst.dataRequest)
            {
                from= uint.Parse( message.SelectSingleNode("from").InnerText);
                version= int.Parse(message.SelectSingleNode("version").InnerText);
            }
        }

        public  message()
        {

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

        public void sendCarryInfoMessage(XmlNode value)
        {
            string message = getComfirm(th,value);
            sendStringToDevice(message);
        }

        public void sendCarryInfoMessage(string value)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(@"<message><action>confirm</action><th></th><value>"+value+"</value></message>");
            XmlNode confirm = doc.SelectSingleNode("message");
            XmlNode xuhao = confirm.SelectSingleNode("th");
            xuhao.InnerText = th.ToString();
            sendStringToDevice(confirm.OuterXml);
        }


        private  string getComfirm(int th,XmlNode value)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(@"<confirm><th></th><value></value></confirm>");
            XmlNode confirm = doc.SelectSingleNode("confirm");
            XmlNode xuhao = confirm.SelectSingleNode("th");
            XmlNode valu = confirm.SelectSingleNode("value");
            xuhao.InnerText = th.ToString();
            valu.InnerText = value.InnerText;
            return confirm.OuterXml;
        }


       public  void sendStringToDevice(string message)
        {
            SendMessage.getInstance().sendMessage(message,this.point);
        }


       public   string ToXml()
        {
            return xmlString;
        }

        Boolean getConfirm()
        {
            return true;
        }

    }
}
