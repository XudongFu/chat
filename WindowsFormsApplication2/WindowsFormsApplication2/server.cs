using System;
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
            try
            {
                control.initeServer(this);
                control.doIt(this);
            }
            catch (Exception p)
            {
                MessageBox.Show(p.Message);
                this.Close();
            }
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

        private void server_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (control.receiveMessage != null && control.receiveMessage.IsAlive)
            {
                control.receiveMessage.Abort();
            }
        }

        private void server_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (control.receiveMessage!=null && control.receiveMessage.IsAlive)
            {
                control.receiveMessage.Abort();
            }
          
        }
    }
}
