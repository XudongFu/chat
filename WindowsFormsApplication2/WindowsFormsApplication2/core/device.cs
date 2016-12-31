using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using WindowsFormsApplication2.core;


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
          Socket socket;
          deviceType type;
       public  device(Socket socket,deviceType type)
        {
            this.socket = socket;
            this.type = type;
        }


        public override bool Equals(object obj)
        {
            device shebei = obj as device;
            return this.socket == shebei.socket && type == shebei.type;
           
        }


        public void sendMessage(communication com)
        {


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
