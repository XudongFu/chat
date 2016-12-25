using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.dataEntry;
namespace WindowsFormsApplication2.data
{

    /// <summary>
    /// 这个类对数据文件自己进行维护
    /// </summary>
    class groupList
    {
        struct groupdata
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

        }

        void addGroup()
        {

        }

    }
}
