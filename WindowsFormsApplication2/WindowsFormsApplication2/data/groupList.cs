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

        string path = "./savedData/user.txt";

        Dictionary<uint, group> groups = new Dictionary<uint, group>();

        groupList()
        {
            praseTxt txt = new praseTxt(path);

        }


        void saveChangeToFile()
        {

        }

        void addGroup()
        {

        }

    }
}
