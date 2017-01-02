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

        public server()
        {
            InitializeComponent();
            control = new manager();
        }

        private void inite_Click(object sender, EventArgs e)
        {
            control.initeServer();
            control.doIt();
        }





    }
}
