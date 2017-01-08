using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.core;
using WindowsFormsApplication2.data;

namespace WindowsFormsApplication2.dataEntry
{
 public   class user
    {
        public  List<device> devices = new List<device>();
        userList list;
        uint id;
        /// <summary>
        /// 接受的消息集合
        /// 发送的消息集合怎么办
        /// </summary>
       
        public  user(ref userList list,uint id)
        {
            this.list = list;
            this.id = id;
        }

        public  void pushComm(communication comm)
        {
            if (devices.Count == 0)
            {
                list.getUser(id).notReadMess.Enqueue(comm);
            }
            else
            {
                while (list.getUser(id).notReadMess.Count!=0)
                {
                    communication temp = list.getUser(id).notReadMess.Dequeue();
                    sendCommToDev(temp);
                    list.getUser(id).readMess.Add(temp);
                }
                sendCommToDev(comm);
                list.getUser(id).readMess.Add(comm);
            }          
        }

        private void sendCommToDev(communication com)
        {
            foreach (device de in devices)
            {
                de.sendMessage(com);
            }
        }



        
    }
}
