using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace pro
{
    partial class Form1
    {
        private Button button1;
        private Button button2;
        
        private Label msg;
        private Label uLabel;
        private Label pLabel;
        private TextBox username;
        private TextBox password;
        private NotifyIcon notifyIcon; 
        private ContextMenu notifyiconMnu;
        private ReqUtil reqUtil = new ReqUtil();
        
        protected void login(object sender, EventArgs e)
        {
            FileInfo f = new FileInfo("msg.txt");
            if(!f.Exists)
            {
                StreamWriter w = new StreamWriter("msg.txt");
                w.WriteLine(this.username.Text);
                w.Write(this.password.Text);
                w.Close();
            }

            string getmsg = reqUtil.login(this.username.Text, this.password.Text);
            if(getmsg.Contains("success"))
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
            if(getmsg.Contains("下线成功")) 
            {
                this.msg.Text = "下线成功";
            }
            else
            {
                this.msg.Text = "下线失败";
            }
        }
        protected void initButton() 
        {
            this.button1 = new Button();
            this.button1.Location = new Point(200,200);
            this.button1.Name = "button1";
            this.button1.Text = "登录";
            this.button1.Size = new Size(100,50);
            this.button1.Click += this.login;

            this.button2 = new Button();
            this.button2.Location = new Point(500,200);
            this.button2.Name = "button2";
            this.button2.Text = "登出";
            this.button2.Size = new Size(100,50);
            this.button2.Click += this.logout;

        }
        protected void initLabel()
        {
            this.msg = new Label();
            this.msg.Text = "输出信息";
            this.msg.Location = new Point(300,300);
            this.msg.Size = new Size(200,80);
            this.msg.Font = new Font("黑体",20);

            this.uLabel = new Label();
            this.uLabel.Text = "学号";
            this.uLabel.Location = new Point(150,105);
            
            this.pLabel = new Label();
            this.pLabel.Text = "密码";
            this.pLabel.Location = new Point(400,105);
        }
        protected void initTextBox()
        {
            this.username = new TextBox();
            this.username.Location = new Point(200,100);
            this.username.Size = new Size(160,40);

            this.password = new TextBox();
            this.password.Location = new Point(450,100);
            this.password.Size = new Size(160,40);

            FileInfo f = new FileInfo("msg.txt");
            if(f.Exists)
            {
                StreamReader r = new StreamReader("msg.txt");
                string s = r.ReadToEnd();
                this.username.Text = s.Split("\n")[0];
                this.password.Text = s.Split("\n")[1];
                r.Close();
            }
        }
        protected void initContextMenu()
        {
            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Icon = new Icon("main.ico");
            this.notifyIcon.Text = "scuAutoInternet";
            this.notifyIcon.Visible = true;
            MenuItem[] menuItems = new MenuItem[2];
            menuItems[0] = new MenuItem();
            menuItems[0].Text = "显示主窗口";
            menuItems[0].Click += this.showWin;

            menuItems[1] = new MenuItem();
            menuItems[1].Text = "退出程序";
            menuItems[1].Click += this.selectExit;
            
            this.notifyiconMnu = new ContextMenu(menuItems);
            this.notifyIcon.ContextMenu = notifyiconMnu;
            this.notifyIcon.MouseDoubleClick += this.doubleClick;
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
        protected void doubleClick(object sender, MouseEventArgs e)
        {
            this.notifyIcon.Visible = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Focus();
        }
             
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new Size(800, 450);
            int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Size.Width) / 2;
            int y = (System.Windows.Forms.SystemInformation.WorkingArea.Height - this.Size.Height) / 2;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Location = (Point)new Size(x, y);
            this.Text = "scuAutoInternet";
            this.Icon = new Icon("main.ico");
            
            this.initButton();
            this.initLabel();
            this.initTextBox();
            this.initContextMenu();

            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.msg);
            this.Controls.Add(this.username);
            this.Controls.Add(this.password);
            this.Controls.Add(this.uLabel);
            this.Controls.Add(this.pLabel);
        }

        #endregion
    }
}

