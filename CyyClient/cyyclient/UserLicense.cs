using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace CyyClient
{
    public partial class UserLicense : Form
    {
        private bool ok = false;
        public UserLicense()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            ((Owner as CyyLogin).Owner as CyyMain).LoginState = false;

            //Close();
            CyyMain.cyyMainExit();
            //Application.Exit();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ok = true;

            if (((Owner as CyyLogin).Owner as CyyMain).back == 1) {
                ((Owner as CyyLogin).Owner as CyyMain).Visible = true;
            }
            Close();
        }

        /*
        private string GetLocateByIp(string ip) {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://www.ip138.com/ips138.asp?ip=" + ip + @"&action=2");
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();


            string pattern="<li>";
            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);

            MatchCollection matches = rgx.Matches(retString);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                    retString = match.Value;
            }

            


            myStreamReader.Close();
            myResponseStream.Close();

            return retString;

        }
         */

        private void UserLicense_Load(object sender, EventArgs e)
        {
            this.ActiveControl = btnOk;
            lblUserName.Text = (Owner as CyyLogin).userInfo["nickname"].ToString();
            string ip = (Owner as CyyLogin).userInfo["UserLogined_ip"].ToString();

            ((Owner as CyyLogin).Owner as CyyMain).lblUser.Text = lblUserName.Text;

            webBrowser1.Navigate(System.Environment.CurrentDirectory + @"\config\lic.htm");

            //((Owner as CyyLogin).Owner as CyyMain).USERID = (Owner as CyyLogin).dtUserInfo.Rows[0]["UserNumber"].ToString();
            CyyMain.USERID = (Owner as CyyLogin).userInfo["user_name"].ToString();
            txtCurrentIP.Text = CyyMain.currentIP;
            txtCurrentAddress.Text = CyyMain.currentAddress;
        }

        private void UserLicense_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ok)
            {
                //((Owner as CyyLogin).Owner as CyyMain).LoginState = false;
                btnExit.PerformClick();
            }

            
        }
    }
}
