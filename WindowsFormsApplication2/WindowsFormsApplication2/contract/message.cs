using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.contract
{
   public  abstract class message
    {

        uint from = 0;
        uint sendTo = 0;
        string actionType;
        string value;
        /// <summary>
        /// 消息的序列号
        /// </summary>
        int th;

        message(string xml)
        {

        }


       public  string getAction()
        {
            return null;
        }




    }
}
