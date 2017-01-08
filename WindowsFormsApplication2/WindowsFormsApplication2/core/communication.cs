using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
namespace WindowsFormsApplication2.core
{
  
 public    class communication
    {
        public  uint from;
        public  string type;
        public  uint to;
        public  string message;

        /// <summary>
        /// 表示消息的序列，用于同步不同设备之间的消息传送
        /// </summary>
        public uint th;

        public communication(uint from,uint to,string type,string mess)
        {
            this.from = from;
            this.to = to;
            this.type = type;
            this.message = mess;

        }

        public static communication prase(XmlNode comminfo)
        {
            uint fromId = uint.Parse(comminfo.SelectSingleNode("from").InnerText);
            uint to = uint.Parse(comminfo.SelectSingleNode("to").InnerText);
            string type = comminfo.SelectSingleNode("type").InnerText;
            string value = comminfo.SelectSingleNode("message").InnerText;
            communication com = new communication(fromId, to, type, value);
            return com;
        }


    }
}
