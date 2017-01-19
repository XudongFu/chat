using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.dataEntry;
using WindowsFormsApplication2.data;
using WindowsFormsApplication2.core;
using System.IO;
namespace WindowsFormsApplication2.data
{

    public class friendList
    {
        public uint userId;
        public uint friendId;
        public int verison;
        public string condition;

        public  friendList(uint uid,uint fid,int ver,string con)
        {
            userId = uid;
            friendId = fid;
            verison = ver;
            condition = con;
        }
    }

    public class friendInfo
    {
        public uint friendId;
        public string condition;
        public int verison;
    }


    /// <summary>
    /// 
    /// </summary>
    public class userdata
    {
        public userdata(uint id, string userName,
            string sex, string time, string place,
            string company, string colloge, string sign, string pass,int version)
        {
            this.id = id;
            this.userName = userName;
            this.sex = sex;
            this.birthday = time;
            this.place = place;
            this.commany = company;
            this.colloge = colloge;
            this.sign = sign;
            password = pass;
            this.version = version;
        }
        public uint id;
        public string userName;
        public string sex;
        public string birthday;
        public string place;
        public string colloge;
        public string sign;
        public string commany;
        public string password;
        public int version;

        /// <summary>
        /// 接受的消息缓存
        /// </summary>
        public Queue<communication> notReadMess = new Queue<communication>();

        public List<communication> readMess = new List<communication>();

        public List<friendInfo> friends = new List<friendInfo>();

    }

    public class userList
    {
        string path = @"C:\个人文件\chat\WindowsFormsApplication2\WindowsFormsApplication2\data\savedData\user.txt";
        string fls = @"C:\个人文件\chat\WindowsFormsApplication2\WindowsFormsApplication2\data\savedData\friends.txt";

        //string path = @".\savedData\user.txt";
        //string fls = @".\savedData\friends.txt";

        public  Dictionary<uint, userdata> users = new Dictionary<uint, userdata>();

        public List<friendList> friendls = new List<friendList>();

        public userList()
        {
            praseTxt praseFriend = new praseTxt(fls);
            try
            {
                while (true)
                {
                    List<string> temp = praseFriend.getNextLine();
                    uint userId = uint.Parse(temp[0]);
                    uint friend = uint.Parse(temp[1]);
                    int version = int.Parse(temp[2]);
                    string condition = temp[3];
                    friendls.Add(new friendList(userId, friend, version, condition));
                }
            }
            catch (EndOfStreamException e)
            {

            }
            praseFriend.close();
            
            praseTxt prase = new praseTxt(path);
            try
            {
                while(true)
                {
                    List<string> temp = prase.getNextLine();
                    uint id = uint.Parse(temp[0]);
                    userdata udata = new userdata(uint.Parse(temp[0]), temp[1],
                    temp[2], temp[3], temp[4],
                    temp[5], temp[6], temp[7], temp[8], int.Parse(temp[9]));
                    udata.friends.AddRange(friendls.Where(p=>p.userId==id).Select(p=>
                    {
                        friendInfo info = new friendInfo();
                        info.condition = p.condition;
                        info.friendId = p.friendId;
                        info.verison = p.verison;
                        return info;
                    }
                    ).ToList());
                    udata.friends.AddRange(friendls.Where(p => p.friendId == id).Select(p =>
                    {
                        friendInfo info = new friendInfo();
                        info.condition = p.condition;
                        info.friendId = p.friendId;
                        info.verison = p.verison;
                        return info;
                    }
                 ).ToList());

                    users.Add(id, udata);
                }
            }
            catch (EndOfStreamException e)
            {
            }
            prase.close();




        }

        public userdata getUser(uint id)
        {
            if (users.ContainsKey(id))
            {
                return users[id];
            }
            else
            {
                return null;
            }
        }

        public bool comfirmUser(uint id, string password)
        {
            try
            {
                if (users[id].password == password)
                    return true;
                return false;
            }
            catch (KeyNotFoundException e)
            {
                return false;
            }
        }



        void initeUserFriend()
        {
           
        }


        char table = '\t';
        public void saveChangeToFile()
        {
            var userStream = File.Open(path, FileMode.Create);
            foreach (var x in users.ToArray())
            {
                byte[] data = ASCIIEncoding.UTF8.GetBytes(
                    x.Value.id.ToString() + table
                    + x.Value.userName + table
                    + x.Value.sex + table
                    + x.Value.birthday + table
                    + x.Value.place + table
                     + x.Value.commany + table
                    + x.Value.colloge + table
                    + x.Value.sign + table
                    + x.Value.password+ table
                    +x.Value.version+ "\r\n");
                userStream.Write(data, 0, data.Length);

            }
            userStream.Flush();
            userStream.Close();
        }
        public uint addUser(userdata user)
        {
            uint id=getFreeId();
            user.id = id;
            users.Add(id, user);
            return id;
        }

        public void addGroup()
        {


        }


        public void addFriend(uint userId, uint friendId)
        {

        }



        public static uint getFreeId()
        {
            return (uint)(DateTime.Now.Day * DateTime.Now.Millisecond * DateTime.Now.Day);
        }

    }
}