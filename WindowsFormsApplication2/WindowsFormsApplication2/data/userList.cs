﻿using System;
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

   public  class userdata
    {
        public userdata(uint id, string userName,
            string sex, DateTime time, string place,
            string company, string colloge, string sign, string pass)
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
        }
        public uint id;
        public string userName;
        public string sex;
        public DateTime birthday;
        public string place;
        public string colloge;
        public string sign;
        public string commany;
        public string password;
        /// <summary>
        /// 接受的消息缓存
        /// </summary>
        public Queue<communication> notReadMess = new Queue<communication>();

        public List<communication> readMess = new List<communication>();



    }

    public  class userList
    {
        string path = "/.savedData/user.txt";

        Dictionary<uint, userdata> users = new Dictionary<uint, userdata>();
        
       public  userList()
        {
            praseTxt prase = new praseTxt(path);

            try
            {
                List<string> temp = prase.getNextLine();
                uint id = uint.Parse(temp[0]);
                users.Add(id,new userdata(uint.Parse( temp[0]), temp[1], 
                    temp[2],DateTime.Parse( temp[3]), temp[4], 
                    temp[5], temp[6], temp[7],temp[8]));
            }
            catch (Exception e)
            {
            }
            
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

        public bool comfirmUser(uint id,string password)
        {
            try {
                if (users[id].password == password)
                    return true;
                return false;
            }
            catch (KeyNotFoundException e)
            {
                return false;
            }
        }

       

        char table = '\t';
        public  void saveChangeToFile()
        {
            var userStream = File.Open(path, FileMode.Create);
            foreach (var x in users.ToArray() )
            {
                byte[] data = ASCIIEncoding.Unicode.GetBytes(
                    x.Value.id.ToString() + table
                    + x.Value.userName + table
                    + x.Value.sex + table
                    + x.Value.birthday + table
                    + x.Value.place + table
                    + x.Value.colloge + table
                    + x.Value.sign + table
                    + x.Value.commany + table
                    + x.Value.password);
                userStream.Write(data,0,data.Length);

            }
            userStream.Flush();
            userStream.Close();
        }
        public  void addUser(uint id,string userName,string sex,DateTime birthday,
            string place,string colloge,string sign,string commany)
        {

        }

        public   void deleteUser(uint id)
        {

        }

         public  void addGroup()
        {


        }


        public static uint getFreeId()
        {
            return 0;
        }

    }
}
