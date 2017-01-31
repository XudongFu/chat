using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using WindowsFormsApplication2.core;
using System.Net.Sockets;

namespace WindowsFormsApplication2.dataEntry
{
 public   enum deviceType
    {
        web,
        android,
        apple,
        pc
    }


 public    class device
    {
        IPEndPoint point;
          deviceType type;
       public  device(IPEndPoint ippoint,deviceType type)
        {
            this.point = ippoint;
            this.type = type;
        }


        public override bool Equals(object obj)
        {
            device shebei = obj as device;
            return this.point == shebei.point && type == shebei.type;
        }


        public void sendMessage(communication com)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<message><action>communication</action><th>"+getTh());
            builder.Append("</th><value><from>"+com.from);
            builder.Append("</from><type>"+com.type);
            builder.Append("</type><to>"+com.to);
            builder.Append("</to><content>"+com.message);
            builder.Append("</content></value></message>");
            SendMessage.getInstance().sendMessage(builder.ToString(), point);
        }

        public int getTh()
        {
            Random p = new Random(DateTime.Now.Millisecond);
            return   p.Next(0, int.MaxValue);
        }

        public   static deviceType getType(string type)
        {
            deviceType shebei;
            switch (type)
            {
                case "web":
                    shebei = deviceType.web;
                    break;
                case "android":
                    shebei = deviceType.android;
                    break;
                case "apple":
                    shebei = deviceType.apple;
                    break;
                case "pc":
                    shebei = deviceType.pc;
                    break;
                default:
                    shebei = deviceType.android;
                    break;
            }
            return shebei;

        }

    }
}
