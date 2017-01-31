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
                            onlineUser[id].devices.Distinct();
                        }
                        us.checkNotSendMessage();
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
                    ser.showText(com.from+" 发送消息到 "+com.to+" ,内容为"+com.message);
                    sendMessageToUser(com);
                    communication test = new communication(0, com.from, "user", "接收到来自于服务器的反馈数据");
                    sendMessageToUser(test);
                    ser.showText("有用户向好友发送消息");
                    break;
                //用户向服务器请求数据
                case actionConst.dataRequest:
                    XmlNode request = mess.value;
                    int version = mess.version;
                    uint userFrom = mess.from;
                    StringBuilder builder = new StringBuilder();
                    userdata frominfo = userManager.getUser(userFrom);
                    frominfo.friends.Where(friend =>friend.verison > version).ToList().ForEach(friend =>
                    {
                        builder.Append("<user  condition='"+friend.condition+"'>"+friend.friendId+ "</user>");
                    });
                    mess.sendCarryInfoMessage(builder.ToString());
                    ser.showText("向客户端发送数据为："+builder.ToString());        
                    break;
                case actionConst.userInfoUpdate:
                    XmlNode update = mess.value;
                    StringBuilder sb = new StringBuilder();
                    if (update.FirstChild.Name == "friendId")
                    {
                        uint friendId = uint.Parse( update.FirstChild.InnerText);
                        userdata friend = userManager.getUser(friendId);
                        sb.Append("<message><action>InfoUpdate</action><th>");
                        sb.Append(mess.th);
                        sb.Append("</th><value>");
                        sb.Append(getXMl("name",friend.userName));
                        sb.Append(getXMl("id", friend.id));
                        sb.Append(getXMl("college", friend.colloge));
                        sb.Append(getXMl("company", friend.commany));
                        sb.Append(getXMl("birthDay", friend.birthday));
                        sb.Append(getXMl("where", friend.place));
                        sb.Append(getXMl("sign", friend.sign));
                        sb.Append(getXMl("sex", friend.sex));
                        sb.Append("</value></message> ");
                        mess.sendStringToDevice(sb.ToString());
                        ser.showText(sb.ToString());
                    }
                  else if (update.FirstChild.Name == "groupId")
                    {
                    }

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


        private void sendMessageToUser(communication mess)
        {
            if (onlineUser.ContainsKey(mess.to))
            {
                user u = onlineUser[mess.to];
                u.pushComm(mess);
            }
          
        }
       
        private string getXMl(string name,object o)
        {
            return "<" + name + ">" + o.ToString() + "</" + name + ">";
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
       
    }
}
