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
        public Thread receiveMessage;
        public Thread solveMessages;
        server ser;
        public ConcurrentQueue<message> messages = new ConcurrentQueue<chat.message>();
        Dictionary<uint, user> onlineUser = new Dictionary<uint, user>();
        listen listen;
        Dictionary<int, Action> operation;


        public manager(server ser)
        {
            this.ser = ser;
            listen = new listen(ser);
            operation = new Dictionary<int, Action>();
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
                            device shebei = new device(mess.point, device.getType(clientType));
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
                    string time =signInInfor.SelectSingleNode("birthday").InnerText;
                    string company = signInInfor.SelectSingleNode("company").InnerText;
                    string colloge = signInInfor.SelectSingleNode("colloge").InnerText;
                    string password= signInInfor.SelectSingleNode("password").InnerText;
                    uint newUserId = userList.getFreeId();
                    mess.sendCarryInfoMessage("<newUserId>"+newUserId+"</newUserId>");

                    operation.Add(mess.th, () =>                  
                    {
                        userManager.addUser(new userdata(newUserId, name, sex, time, "", colloge, "", company, password, 1));
                        ser.showText("用户注册账号成功");

                    });
                    break;
                //用户退出
                case actionConst.signOff:

                    XmlNode offInfor = mess.value;
                    uint userId = uint.Parse(offInfor.SelectSingleNode("id").InnerText);
                    string pass = offInfor.SelectSingleNode("password").InnerText;
                    string client = offInfor.SelectSingleNode("clientType").InnerText;
                    device she = new device(mess.point, device.getType(client));
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
                 //用户发送消息
                case actionConst.communication:

                    communication com = communication.prase(mess.value);
                    ser.showText(com.from+""+com.to+",内容为"+com.message);
                    //user from = onlineUser[com.from];
                    //from.pushComm(com);
                  

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

                case actionConst.confirm:
                    if (operation.ContainsKey(mess.th))
                    {
                        operation[mess.th].Invoke();
                    }
                    break;
                default:
                    ser.showText("得到不能被处理的消息"+mess.ToXml());
                    break;
            }
        }

        public void solve()
        {
            message m = new message(); ;
            if (messages.Count != 0 && messages.TryDequeue(out m))
            {
                solveMessage(m);
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
            receiveMessage.IsBackground = true;
            receiveMessage.Start(this);
            ser.showText("接受以及处理消息线程启动");
        }


        bool UserIsOnline(uint id)
        {
            return false;
        }


    }
}
