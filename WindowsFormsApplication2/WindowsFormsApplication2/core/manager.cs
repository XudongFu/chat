using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using chat = WindowsFormsApplication2.contract;
using System.Collections.Concurrent;
using WindowsFormsApplication2.data;
using WindowsFormsApplication2.contract;
using System.Xml;
using WindowsFormsApplication2.core;
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

        Dictionary<uint, user> onlineUser = new Dictionary<uint, user>();

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
                    string clientType = node.SelectSingleNode("clientType").Value;
                    if (userManager.comfirmUser(id, passcode))
                    {
                        user us = userManager.getUser(id);
                        mess.sendConfirm(true);

                        try
                        {
                            onlineUser.Add(id, us);
                        }
                        catch (ArgumentException e)
                        {
                            device shebei = new device(mess.socket,device.getType(clientType));
                            onlineUser[id].devices.Add(shebei);
                        }
                    }
                    else
                    {
                        mess.sendConfirm(false);
                    }
                    break;
                    #endregion

                    //用户注册
                case actionConst.signIn:
                    XmlNode signInInfor = mess.value;
                    string name = signInInfor.SelectSingleNode("name").Value;
                    string sex= signInInfor.SelectSingleNode("sex").Value;
                    DateTime time = DateTime.Parse( signInInfor.SelectSingleNode("birthDay").Value);
                    string company= signInInfor.SelectSingleNode("company").Value;
                    string colloge= signInInfor.SelectSingleNode("colloge").Value;
                    userManager.addUser(userList.getFreeId(), name, sex,time,"", colloge,"", company);

                    break;
                    //用户退出
                case actionConst.signOff:

                    XmlNode offInfor = mess.value;
                    uint userId = uint.Parse(offInfor.SelectSingleNode("id").Value);
                    string pass = offInfor.SelectSingleNode("password").Value;
                    string client = offInfor.SelectSingleNode("clientType").Value;
                    device she = new device(mess.socket,device.getType(client));
                    bool userIsOnline = false;
                    try {
                        user u = onlineUser[userId];
                        userIsOnline = true;
                    }
                    catch(KeyNotFoundException e)
                    {

                    }
                    if (userManager.comfirmUser(userId, pass) && userIsOnline)
                    {
                        try
                        {
                            onlineUser[userId].devices.Remove(she);
                            if (onlineUser[userId].devices.Count == 0)
                                onlineUser.Remove(userId);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                   
                    break;

                case actionConst.communication:                   
                    communication com = communication.prase(mess.value);

                    if (onlineUser.ContainsKey(com.from))
                    {

                    }
                    else
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


        bool UserIsOnline(uint id)
        {
            return false;
        }


    }
}
