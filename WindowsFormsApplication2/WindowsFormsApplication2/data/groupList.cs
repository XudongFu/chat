using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.dataEntry;
using WindowsFormsApplication2.core;
namespace WindowsFormsApplication2.data
{

    /// <summary>
    /// 这个类对数据文件自己进行维护
    /// </summary>
    class groupList
    {
        char table = '\t';

        class groupdata
        {
            public groupdata(uint id,string name,DateTime createdTime,string description)
            {
                this.id = id;
                this.name = name;
                this.createdTime = createdTime;
                userId = new List<uint>();
                this.description = description;
            }
            public  uint id;
            public  string name;
            public  DateTime createdTime;
            public  List<uint> userId;
            public  string description;
            public List<communication> comms = new List<communication>();

        }

        string groupPath = "./savedData/group.txt";

        string groupEntryPath = "./savedData/groupEntry.txt";

        Dictionary<uint, groupdata> groups = new Dictionary<uint, groupdata>();

        public groupList()
        {
            praseTxt prase = new praseTxt(groupPath);
            try
            {
                List<string> temp = prase.getNextLine();
                uint id = uint.Parse(temp[0]);
                groups.Add(id, new groupdata(uint.Parse(temp[0]), temp[1],
                     DateTime.Parse(temp[2]),temp[3]));
            }
            catch (Exception e)
            {

            }

            try
            {
                praseTxt groupEntry = new praseTxt(groupEntryPath);
                List<string> strs = groupEntry.getNextLine();

                try {
                    uint groupId = uint.Parse(strs[0]);
                    uint userId = uint.Parse(strs[1]);
                    groups[groupId].userId.Add(userId);
                }
                catch (KeyNotFoundException e) 
                {

                }
            }
            catch (Exception e)
            {

            }
        }


        void saveChangeToFile()
        {
            var groupStream = File.Open(groupPath, FileMode.Create);
            var entryStream = File.Open(groupEntryPath, FileMode.Create);
            foreach (var p in groups.ToArray().OrderBy(o=>o.Key))
            {
                byte[] data = ASCIIEncoding.Unicode.GetBytes(p.Value.id +table+ p.Value.name + table + p.Value.createdTime + table + p.Value.description);
                groupStream.Write(data,0,data.Length);

                foreach (var user in p.Value.userId)
                {
                    byte[] userData = ASCIIEncoding.Unicode.GetBytes(p.Key.ToString()+table+user.ToString());
                    entryStream.Write(userData,0,userData.Length);
                }
            }
            groupStream.Flush();
            entryStream.Flush();
            groupStream.Close();
            entryStream.Close();
            
        }

        void addGroup()
        {

        }

    }
}
