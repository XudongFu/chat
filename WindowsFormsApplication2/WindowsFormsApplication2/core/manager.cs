using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chat = WindowsFormsApplication2.contract;
using System.Collections.Concurrent;
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
                    #region
                    XmlNode node = mess.value;
                    uint id = uint.Parse(node.SelectSingleNode("id").Value);
                    string passcode = node.SelectSingleNode("password").Value;
                    if (userManager.comfirmUser(id, passcode))
                    {
                        user us = userManager.getUser(id);

                        //需要对us的设备进行添加

                        mess.sendConfirm(true);
                        onlineUser.TryAdd(id,us);
                    }
                    else
                    {
                        mess.sendConfirm(false);
                    }

                    break;
                    #endregion
                case actionConst.signIn:
                    XmlNode signInInfor = mess.value;
                    string name = signInInfor.SelectSingleNode("name").Value;
                    string sex= signInInfor.SelectSingleNode("sex").Value;
                    DateTime time = DateTime.Parse( signInInfor.SelectSingleNode("birthDay").Value);
                    string company= signInInfor.SelectSingleNode("company").Value;
                    string colloge= signInInfor.SelectSingleNode("colloge").Value;
                    userManager.addUser(userManager.getFreeId(), name, sex,time,"", colloge,"", company);

                    break;
                case actionConst.signOff:

                    break;

                case actionConst.communication:

                    XmlNode comminfo = mess.value;
                    uint fromId = uint.Parse(comminfo.SelectSingleNode("from").Value);
                    try {
                      user us=  onlineUser[fromId];

                    }
                    catch (KeyNotFoundException e)
                    {

                    }
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
