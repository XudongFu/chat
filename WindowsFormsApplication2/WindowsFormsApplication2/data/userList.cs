using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.dataEntry;
using WindowsFormsApplication2.data;
namespace WindowsFormsApplication2.data.savedData
{
    class userList
    {
        string path = "/.savedData/user.txt";

        Dictionary<uint, userdata> users = new Dictionary<uint, userdata>();

        struct userdata
        {
            public userdata(uint id,string userName,
                string sex,DateTime time,string place,
                string company,string colloge,string sign )
            {
                this.id = id;
                this.userName = userName;
                this.sex = sex;
                this.birthday = time;
                this.place = place;
                this.commany = company;
                this.colloge = colloge;
                this.sign = sign;
            }

            uint id;
            string userName;
            string sex;
            DateTime birthday;
            string place;
            string colloge;
            string sign;
            string commany;

        }

        userList()
        {
            praseTxt prase = new praseTxt(path);

            try
            {
                List<string> temp = prase.getNextLine();
                uint id = uint.Parse(temp[0]);
                users.Add(id,new userdata(uint.Parse( temp[0]), temp[1], 
                    temp[2],DateTime.Parse( temp[3]), temp[4], 
                    temp[5], temp[6], temp[7]));
            }
            catch (Exception e)
            {

            }
        }

        void saveChangeToFile()
        {
            

        }
        void addUser()
        {

        }

         void deleteUser()
        {

        }

         void addGroup()
        {


        }
    }
}
