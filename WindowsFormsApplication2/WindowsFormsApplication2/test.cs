using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.data;
namespace WindowsFormsApplication2
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }
        userList list = new userList();
        private void test_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            userdata user = new userdata(0,"fuxudong","男",DateTime.Now.ToString(),"深圳","kingdee","ahu","free","pass");
            showText( list.addUser(user).ToString());
            list.saveChangeToFile();

        }



        void showText(string str)
        {

            textBox1.Text += "\r\n"+str;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            userdata date= list.getUser(799);
            showText(date.id.ToString());

            showText(date.userName);

            showText(date.password);
        }
    }
}
