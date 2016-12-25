using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using chat = WindowsFormsApplication2.contract;
using System.Collections.Concurrent;
using WindowsFormsApplication2.dataEntry;
using WindowsFormsApplication2.data;
using WindowsFormsApplication2.contract;
using System.Xml;
namespace WindowsFormsApplication2.dataEntry
{
    class manager
    {

        /// <summary>
        /// 耗时的操作，需要很多的时间完成
        /// </summary>
        userList userManager;

        groupList groupManager;



        Thread receiveMessage;
        Thread solveMessages;
        

        ConcurrentQueue<message> messages = new ConcurrentQueue<chat.message>();

        ConcurrentDictionary<uint, user> onlineUser = new ConcurrentDictionary<uint, user>();

        ConcurrentDictionary<uint, group> groups = new ConcurrentDictionary<uint, group>();


        void inite()
        {
            userManager = new userList();
            groupManager = new groupList();
        }



        void solveMessage(message mess)
        {
            switch (mess.getAction())
            {
                //用户登录
                case actionConst.signOn:
                    XmlNode node = mess.value;
                    uint id = uint.Parse(node.SelectSingleNode("id").Value);
                    string passcode = node.SelectSingleNode("password").Value;
                    mess.sendConfirm(userManager.comfirmUser(id,passcode));
                    break;
                default:
                    break;

            }
        }




        void mainfunction()
        {
            inite();




        }



    }
}
