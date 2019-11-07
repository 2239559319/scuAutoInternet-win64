using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace autoInternet
{

    public partial class Form1 : Form
    {
        public ReqUtil reqUtil = new ReqUtil();
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += this.Form1_FormClosing;
            SystemEvents.SessionEnding += this.systemEvents_SessionEnding;
            FileInfo f = new FileInfo("msg.txt");
            if (f.Exists)
            {
                StreamReader r = new StreamReader("msg.txt");
                string s = r.ReadToEnd();
                this.username.Text = s.Split('\n')[0];
                this.password.Text = s.Split('\n')[1];
                r.Close();
            }

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.notifyIcon.Visible = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Focus();
        }
        protected void selectExit(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.Close();
            this.Dispose(true);
        }
        protected void showWin(object sender, EventArgs e)
        {
            this.notifyIcon.Visible = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Focus();
        }
        protected void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
            this.Hide();
            this.notifyIcon.Visible = true;
            return;
        }
        protected void systemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            string logoutmsg = this.reqUtil.getLogoutMsg();
            if (logoutmsg != "已处于下线")
            {
                this.reqUtil.logout();
            }
            e.Cancel = false;
        }
        protected void login(object sender, EventArgs e)
        {
            FileInfo f = new FileInfo("./msg.txt");
            if (!f.Exists)
            {
                StreamWriter w = new StreamWriter("./msg.txt");
                w.WriteLine(this.username.Text);
                w.Write(this.password.Text);
                w.Close();
            }

            string getmsg = reqUtil.login(this.username.Text, this.password.Text);
            if (getmsg.Contains("success"))
            {
                this.msg.Text = "登录成功";
            }
            else
            {
                this.msg.Text = "登录失败";
            }
        }
        protected void logout(object sender, EventArgs e)
        {
            string getmsg = reqUtil.logout();
            if (getmsg.Contains("下线成功"))
            {
                this.msg.Text = "下线成功";
            }
            else
            {
                this.msg.Text = "下线失败";
            }
        }

    }
}
