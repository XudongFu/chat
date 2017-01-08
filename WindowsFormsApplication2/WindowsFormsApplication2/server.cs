using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.core;
namespace WindowsFormsApplication2
{
    public partial class server : Form
    {

        manager control;

        delegate void showMessage(string str);

        public server()
        {
            InitializeComponent();
        }

        showMessage show;

        private void inite_Click(object sender, EventArgs e)
        {
            control.initeServer(this);

            control.doIt(this);
        }

        private void server_Load(object sender, EventArgs e)
        {
            control = new manager(this);
            show = showText;
        }

        public  void showText(string str)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(show, str);
            }
            else
                textBox1.Text += str+"\r\n";
        }

    }
}
