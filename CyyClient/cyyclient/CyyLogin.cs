using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace CyyClient
{
    public partial class CyyLogin : Form
    {
        private CyyDatabase CyyDb;

        //public DataTable dtUserInfo { get; private set; }

        public UserInfo userInfo { get; private set; }

        public CyyLogin()
        {
            LinkDatabase();
            InitializeComponent();
        }

        #region 连接数据库
        private void LinkDatabase()
        {
            CyyDb = CyyDatabase.GetDatabase();

            try
            {
                CyyDb.DbOpen();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("无法连接服务器，请重启软件" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    Application.Exit();
                }

            }
        }
        #endregion

        private void CyyLogin_Load(object sender, EventArgs e)
        {
            this.ActiveControl = Acc;
            Acc.Text = "";
            Psw.Text = "";
            LoadCyy.Enabled = true;

            DirectoryInfo TheFolder = new DirectoryInfo(System.Environment.CurrentDirectory + @"\config");
            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                if (NextFile.Extension.Equals(".cyy"))
                {
                    Acc.Items.Add(NextFile.Name.Substring(0, NextFile.Name.Length - 4));
                }
            }
        }

        private void LoadCyy_Click(object sender, EventArgs e)
        {
            if (Owner is CyyMain)
            {
                //bool loginstate = CyyDb.UserLogin(Acc.Text, Psw.Text);

                // dtUserInfo = CyyDb.UserLogin(Acc.Text, Psw.Text);

                userInfo = new UserInfo(CyyDb.UserLogin(Acc.Text, Psw.Text));

                bool loginstate = userInfo.Count > 0 ? true : false;

                if (!loginstate)
                {
                    MessageBox.Show("如果您是标准会员，请到官网 www.caiyingying.com 查看每日随机密码！", "帐号或者密码错误");
                    loginstate = false;
                }
                else if (userInfo["state"].ToString().Equals("OVER"))
                {
                    MessageBox.Show("软件已经到期，请续费!");
                    loginstate = false;
                }
                else
                {
                    if (CyyDb.CheckCountOfSameOnlineUserLogin(userInfo["user_name"].ToString()) == 0)
                    {
                        loginstate = true;
                        CyyDb.InserUserOnline(userInfo);

                        UserLicense ul = new UserLicense();
                        this.Hide();
                        ul.ShowDialog(this);

                        //(CyyMain)Owner.Show();

                        //(Owner as CyyMain).Visible = true;
                        //Close();
                    }
                    else
                    {
                        MessageBox.Show("系统检测到您已登录，请勿重复登录！", "提示消息");
                    }
                }
                (Owner as CyyMain).LoginState = loginstate;
            }
        }

        private void weblink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.caiyingying.com");
        }

        private void RemPsw_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Acc_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void Psw_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                LoadCyy.PerformClick();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.caiyingying.com/register.php");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.caiyingying.com/");
        }
    }
}
