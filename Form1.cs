using System;
using System.Windows.Forms;
using System.Net.Http;
using Microsoft.Win32;

namespace pro
{
    
    class ReqUtil
    {
        private HttpClient client;

        public ReqUtil()
        {
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("User-Agent","Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36");
        }
        public string getLoginMsg()
        {
            string url = "http://192.168.2.135";
            var response=this.client.GetAsync(url);
            try
            {
                var text=response.Result.Content.ReadAsStringAsync();
                var s = text.Result;
                var m = System.Text.RegularExpressions.Regex.Match(s,@"href='(\S+)'");
                return m.Groups[1].ToString().Split('?')[1];
            }
            catch (System.Exception)
            {
                return "你已在线";
                throw;
            }
        }
        public string login(string username, string password)
        {
            string p = this.getLoginMsg();
            if(p == "你已在线") {
                return p;
            }

            string url = "http://192.168.2.135/eportal/InterFace.do?method=login";
            System.Collections.Generic.Dictionary<string, string> data = 
            new System.Collections.Generic.Dictionary<string, string>();

            data.Add("userId",username);
            data.Add("password",password);
            data.Add("service","internet");
            data.Add("queryString",p);
            data.Add("operatorPwd","");
            data.Add("operatorUserId","");
            data.Add("validcode","");
            data.Add("passwordEncrypt","false");

            FormUrlEncodedContent formdata = new FormUrlEncodedContent(data);
            var res = this.client.PostAsync(url,formdata);
            var text=res.Result.Content.ReadAsStringAsync();
            return text.Result;
        }
        public string getLogoutMsg()
        {
            string url = "http://192.168.2.135/eportal/InterFace.do?method=getOnlineUserInfo";
            try
            {
                var res = this.client.GetAsync(url);
                var text=res.Result.Content.ReadAsStringAsync();
                var m = System.Text.RegularExpressions.Regex.Match(text.Result,"\"userIndex\":\"(\\w+)\"");
                return m.Groups[1].ToString();
            }
            catch (System.Exception)
            {
                return "已处于下线";
                throw;
            }
        }
        public string logout()
        {
            string url = "http://192.168.2.135/eportal/InterFace.do?method=logout";
            string userIndex = this.getLogoutMsg();
            if(userIndex == "已处于下线") {
                return userIndex;
            }
            System.Collections.Generic.Dictionary<string, string> data = 
            new System.Collections.Generic.Dictionary<string, string>();
            data.Add("userIndex", userIndex);

            FormUrlEncodedContent formdata = new FormUrlEncodedContent(data);

            var res = this.client.PostAsync(url,formdata);
            var text=res.Result.Content.ReadAsStringAsync();
            return text.Result;
        }
    }
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosing += this.Form1_FormClosing;
            SystemEvents.SessionEnding += this.systemEvents_SessionEnding;
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
            if(logoutmsg != "已处于下线")
            {
                this.reqUtil.logout();
            }
            e.Cancel = false;
        }
    }
}
