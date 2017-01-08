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
using WindowsFormsApplication2.dataEntry;
namespace WindowsFormsApplication2.core
{
    public class manager
    {
        /// <summary>
        /// 耗时的操作，需要很多的时间完成
        /// </summary>
        userList userManager;
        groupList groupManager;
        Thread receiveMessage;
        Thread solveMessages;
        server ser;
        ConcurrentQueue<message> messages = new ConcurrentQueue<chat.message>();

        Dictionary<uint, user> onlineUser = new Dictionary<uint, user>();

        listen listen = new listen();

        public manager(server ser)
        {
            this.ser = ser;
        }


        private void inite(server ser)
        {
            userManager = new userList();
            ser.showText("总计用户数量："+userManager.users.Count.ToString());
            groupManager = new groupList();
        }


        private void solveMessage(message mess)
        {
            switch (mess.getAction())
            {
                //用户登录
                case actionConst.signOn:
                    #region
                    XmlNode node = mess.value;
                    uint id = uint.Parse(node.SelectSingleNode("id").InnerText);
                    string passcode = node.SelectSingleNode("password").InnerText;
                    string clientType = node.SelectSingleNode("clientType").InnerText;
                    if (userManager.comfirmUser(id, passcode))
                    {
                        userdata u = userManager.getUser(id);
                        mess.sendConfirm(true);
                        user us = new user(ref userManager, u.id);
                        try
                        {
                            onlineUser.Add(id, us);
                            ser.showText("id为："+id+"上线了");
                        }
                        catch (ArgumentException e)
                        {
                            device shebei = new device(mess.socket, device.getType(clientType));
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
                    string name = signInInfor.SelectSingleNode("name").InnerText;
                    string sex = signInInfor.SelectSingleNode("sex").InnerText;
                    string time =signInInfor.SelectSingleNode("birthDay").InnerText;
                    string company = signInInfor.SelectSingleNode("company").InnerText;
                    string colloge = signInInfor.SelectSingleNode("colloge").InnerText;
                    string password= signInInfor.SelectSingleNode("password").InnerText;
                    userManager.addUser(new userdata( userList.getFreeId(), name, sex, time, "", colloge, "", company,password,1));

                    break;
                //用户退出
                case actionConst.signOff:

                    XmlNode offInfor = mess.value;
                    uint userId = uint.Parse(offInfor.SelectSingleNode("id").InnerText);
                    string pass = offInfor.SelectSingleNode("password").InnerText;
                    string client = offInfor.SelectSingleNode("clientType").InnerText;
                    device she = new device(mess.socket, device.getType(client));
                    bool userIsOnline = false;
                    try {
                        user u = onlineUser[userId];
                        userIsOnline = true;
                    }
                    catch (KeyNotFoundException e)
                    {
                    }
                    if (userManager.comfirmUser(userId, pass) && userIsOnline)
                    {
                        try {
                            onlineUser[userId].devices.Remove(she);
                            if (onlineUser[userId].devices.Count == 0)
                                onlineUser.Remove(userId);
                        }
                        catch (Exception e) {
                        }
                    }

                    break;

                case actionConst.communication:

                    communication com = communication.prase(mess.value);
                    user from = onlineUser[com.from];
                    from.pushComm(com);
                    break;

                case actionConst.dataRequest:
                    XmlNode request = mess.value;
                    int version = mess.version;
                    uint userFrom = mess.from;

                    StringBuilder builder = new StringBuilder();
                    
                    string str = "<message><action>DataAnswer</action ><th >"+mess.th+ "</th><value>";

                    builder.Append(str);

                    userdata frominfo = userManager.getUser(userFrom);

                    frominfo.friends.Where(friend =>friend.verison > version).ToList().ForEach(friend =>
                    {
                        builder.Append("<user  condition='"+friend.condition+"'>"+friend.friendId+ "</user>");

                    });
                    builder.Append("</value></message>");
                    mess.sendStringToDevice(builder.ToString());
                  
                    break;
                default:
                    break;

            }
        }

        private void solve()
        {
            while (true) {
                message m = new message(); ;
                if (messages.Count != 0 && messages.TryDequeue(out m))
                {
                    solveMessage(m);
                }
            }
        }

        public void initeServer(server ser)
        {
            inite(ser);
            ser.showText("服务器加载数据完毕");
            
        }

        public void doIt(server ser)
        {
            receiveMessage = new Thread(new ParameterizedThreadStart(listen.getNextMessage));
            receiveMessage.Start(messages);
            ser.showText("接受消息线程启动");
            solveMessages = new Thread(solve);
            solveMessages.Start();
            ser.showText("处理线程消息启动");            
        }


        bool UserIsOnline(uint id)
        {
            return false;
        }


    }
}
