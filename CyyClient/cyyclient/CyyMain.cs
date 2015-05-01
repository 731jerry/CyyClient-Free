using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using Skybound.Gecko;
using System.Drawing.Drawing2D;
using ControlExs;
using System.Net;

//using mshtml;
//using SHDocVw;

namespace CyyClient
{
    public partial class CyyMain : ControlExs.FormEx
    {
        #region Public setting
        public int topbarHeight = 120;
        public float productVersion = 2.2f;
        public int mainPageHeight = 580;

        #endregion

        #region Override

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.ExStyle |= (int)WindowStyle.WS_CLIPCHILDREN;
                }
                return cp;
            }
        }

        /*
         protected override void OnPaint(PaintEventArgs e)
         {
             base.OnPaint(e);
           //  DrawFromAlphaMainPart(this, e.Graphics);
             //DrawControlPanel(this.tabPage1, this.tabPage1.CreateGraphics());
         }
         */
        #endregion

        #region Private

        public static void cyyMainExit()
        {
            CyyDatabase.cyyDB.DeleteDateNow(USERID);
            Application.Exit();
        }

        public static void DrawControlPanel(TabPage tabPage, Graphics g)
        {
            Color[] colors = 
            {
                Color.FromArgb(5, Color.Black),
                Color.FromArgb(30, Color.Black),
                Color.FromArgb(145, Color.Black),
                Color.FromArgb(150, Color.Black),
                Color.FromArgb(30, Color.Black),
                Color.FromArgb(5, Color.Black)
            };

            float[] pos = 
            {
                0.0f,
                0.04f,
                0.10f,
                0.90f,
                0.97f,
                1.0f      
            };

            ColorBlend colorBlend = new ColorBlend(6);
            colorBlend.Colors = colors;
            colorBlend.Positions = pos;

            RectangleF destRect = new RectangleF(0, 0, tabPage.Width, tabPage.Height);
            using (LinearGradientBrush lBrush = new LinearGradientBrush(destRect, colors[0], colors[5], LinearGradientMode.Vertical))
            {
                lBrush.InterpolationColors = colorBlend;
                g.FillRectangle(lBrush, destRect);
            }
        }
        /// <summary>
        /// 绘制窗体主体部分白色透明层
        /// </summary>
        /// <param name="form"></param>
        /// <param name="g"></param>
        public static void DrawFromAlphaMainPart(Form form, Graphics g)
        {
            /*
            Color[] colors = 
            {
                Color.FromArgb(5, Color.White),
                Color.FromArgb(30, Color.White),
                Color.FromArgb(145, Color.White),
                Color.FromArgb(150, Color.White),
                Color.FromArgb(30, Color.White),
                Color.FromArgb(5, Color.White)
            };
            */

            Color[] colors = 
            {
                Color.FromArgb(0, Color.White),
                Color.FromArgb(0, Color.White),
                Color.FromArgb(0, Color.White),
                Color.FromArgb(0, Color.White),
                Color.FromArgb(0, Color.White),
                Color.FromArgb(0, Color.White)
            };

            float[] pos = 
            {
                0.0f,
                0.04f,
                0.10f,
                0.90f,
                0.97f,
                1.0f      
            };

            ColorBlend colorBlend = new ColorBlend(6);
            colorBlend.Colors = colors;
            colorBlend.Positions = pos;

            RectangleF destRect = new RectangleF(0, 0, form.Width, form.Height);
            using (LinearGradientBrush lBrush = new LinearGradientBrush(destRect, colors[0], colors[5], LinearGradientMode.Vertical))
            {
                lBrush.InterpolationColors = colorBlend;
                g.FillRectangle(lBrush, destRect);
            }
        }


        private void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        #endregion

        public static string USERID { get; set; }

        GeckoWebBrowser Browser = new GeckoWebBrowser();

        private int[] nums = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private CyyDatabase CyyDb = CyyDatabase.GetDatabase();
        private CyyLogin cyyLogin;

        /*
        List<TwoNum> SelectTwoNums = new List<TwoNum>();
        List<TwoNum> NotSelectTwoNums = new List<TwoNum>();
        */

        Microsoft.Win32.RegistryKey productKey;

        public static string productKeyNameString = ""; // 彩盈盈
        public static string productKeyVersionNameString = ""; // 基础版
        public static string proudctVersionString = ""; // 2.9

        public static string currentIP = "127.0.0.1";
        public static string currentAddress = "本地";

        List<string> LiangMaZuHeSelecteds = new List<string>();

        Algorithm11_5 A11_5 = new Algorithm11_5();

        #region 表格编辑前 data值
        private string preDataOfLotteryInfo;
        #endregion

        #region 往期彩票信息
        public List<LotteryInfo> LotteryInfos { get; private set; }
        #endregion

        #region 当前彩票类型
        private CurrentLotteryTypeInfo currentLotteryTypeInfo { get; set; }
        private class CurrentLotteryTypeInfo
        {
            private LotteryType lotteryType;
            public LotteryType Lottery_Type
            {
                get
                {
                    return lotteryType;
                }
                set
                {
                    lotteryType = value;

                    FileName = System.Environment.CurrentDirectory + @"\data\" + USERID + lotteryType.ToString() + @".dll";
                }
            }
            public string FileName { get; private set; }
            public CurrentLotteryTypeInfo(LotteryType lotteryType)
            {
                Lottery_Type = lotteryType;
            }
        }
        #endregion

        #region 彩票分隔符
        public string LotterySplite { get; set; }
        #endregion

        #region 登录状态
        private bool loginstate = false;
        public bool LoginState
        {
            set
            {
                loginstate = value;
            }

            get
            {
                return loginstate;
            }
        }
        #endregion


        public int back = -1;

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            back = 0;
            cyyLogin.Show(this);
            this.Hide();
            cyyLogin.LoadCyy.PerformClick();
        }

        #region 显示登录窗口
        private void CyyMain_VisibleChanged(object sender, EventArgs e)
        {
            if (cyyLogin != null)
            {
                if (back == 0)
                {
                    back = 1;
                }
                else
                    if (back == 1)
                    {
                        // back = false;
                        Visible = true;

                    }
                    else
                    {
                        try
                        {
                            cyyLogin.ShowDialog(this);
                        }
                        catch (Exception exc)
                        {
                            CyyClose();
                        }

                        if (!LoginState)
                        {
                            CyyClose();
                        }
                        else
                        {
                            //lblUser.Text = CyyDb.userInfo["UserName"].ToString();
                            lblSHOWS.Text = CyyDb.GetShows();
                            lblSHOWS2.Text = lblSHOWS.Text;

                            //this.ShowIcon = false;

                            tmrShows.Enabled = true;

                            if (lblSHOWS.Width < panel17.Width)
                            {
                                lblSHOWS2.Left = lblSHOWS.Left + panel17.Width;
                            }
                            else
                            {
                                lblSHOWS2.Left = lblSHOWS.Left + lblSHOWS.Width;
                            }

                            tmrLoginDate.Enabled = true;
                            tmrUpdate.Enabled = true;

                            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
                            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                            //this.MaximizeBox = false;

                        }

                    }
            }
        }
        #endregion

        public void readCertainFile(string certainFileName)
        {
            using (FileStream fs = File.OpenRead(certainFileName))
            {
                byte[] b = new byte[fs.Length];
                UTF8Encoding temp = new UTF8Encoding(true);
                StringBuilder sb = new StringBuilder();
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    sb.Append(temp.GetString(b));
                }

                string datas = sb.ToString();

                /*string[] datasArr = datas.Split('\n');
                string[] tmpArr;

                List<LotteryInfo> lis = new List<LotteryInfo>();

                foreach (string tmp in datasArr)
                {
                    tmpArr = tmp.Split(',');
                    lis.Add(new LotteryInfo { Day = tmpArr[0], Data = tmpArr[1] });
                }
                
                */
                string[] datasArr = datas.Split('\n');
                List<LotteryInfo> lis = new List<LotteryInfo>();

                for (int i = 0; i < datasArr.Length; i++)
                {

                    if (i % 2 == 0)
                    {
                        lis.Add(new LotteryInfo { Day = datasArr[i], Data = datasArr[i + 1] });
                    }
                }



                LotteryInfos = lis;
            }
        }
        public CyyMain()
            : base()
        {
            cyyLogin = new CyyLogin();
            currentLotteryTypeInfo = new CurrentLotteryTypeInfo(LotteryType.GD_11_5);
            InitializeComponent();

            //Browser.Parent = pnlcccc;
            //Browser.Dock = DockStyle.Fill;

            /*
            Browser1.Parent = tp1;
            Browser1.Dock = DockStyle.Fill;

            Browser2.Parent = tp2;
            Browser2.Dock = DockStyle.Fill;

            Browser3.Parent = tp3;
            Browser3.Dock = DockStyle.Fill;

            Browser4.Parent = tp4;
            Browser4.Dock = DockStyle.Fill;

            Browser5.Parent = tp5;
            Browser5.Dock = DockStyle.Fill;

            Browser6.Parent = tp6;
            Browser6.Dock = DockStyle.Fill;

            Browser7.Parent = tp7;
            Browser7.Dock = DockStyle.Fill;

            Browser8.Parent = tp8;
            Browser8.Dock = DockStyle.Fill;

            Browser9.Parent = tp9;
            Browser9.Dock = DockStyle.Fill;

            Browser10.Parent = tp10;
            Browser10.Dock = DockStyle.Fill;

            Browser11.Parent = tp11;
            Browser11.Dock = DockStyle.Fill;

            Browser12.Parent = tp12;
            Browser12.Dock = DockStyle.Fill;

            Browser13.Parent = tp13;
            Browser13.Dock = DockStyle.Fill;

            Browser14.Parent = tp14;
            Browser14.Dock = DockStyle.Fill;
            */

        }

        #region 关闭窗口
        public void CyyClose()
        {
            CyyDb.DbClose();
            Close();
        }
        #endregion


        private void CyyMain_Load(object sender, EventArgs e)
        {
            productKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\彩盈盈彩票做号系统\");
            try
            {
                productKeyNameString = productKey.GetValue("DisplayName").ToString();
                productKeyVersionNameString = productKey.GetValue("VersionNameFree").ToString(); ;
                proudctVersionString = productKey.GetValue("DisplayVersionFree").ToString();

                cyyLogin.Text = productKeyNameString + "(" + productKeyVersionNameString + ")v" + proudctVersionString;

                productNameLabel.Text = productKeyVersionNameString + " v" + proudctVersionString;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            //tabPage17.Parent = null;

            CbbLotterySplite.SelectedIndex = 0;
            //cbbLotteryTypes.SelectedIndex = 0;

            cbbBeiCheng.SelectedIndex = 0;
            cbbBeiJia.SelectedIndex = 0;

            ClickEvent(grpBileCode, new EventHandler(this.chkD1_01_Click), "chkD1_");
            ClickEvent(grpBileCode, new EventHandler(this.chkD2_01_Click), "chkD2_");
            ClickEvent(grpBileCode, new EventHandler(this.chkD3_01_Click), "chkD3_");
            ClickEvent(grpBileCode, new EventHandler(this.chkD4_01_Click), "chkD4_");
            ClickEvent(grpBileCode, new EventHandler(this.chkD5_01_Click), "chkD5_");

            // 设置初始tab page
            //SetTabState(button2, true);

            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2;
            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2;
            /*
            clr0 = true;
            clr1 = true;
            clr2 = true;
            clr3 = true;
            clr4 = true;
            clr5 = true;
            clr6 = true;
            clr7 = true;
            clr8 = true;
            clr9 = true;
            clr10 = true;
            clr11 = true;
            clr12 = true;
            clr13 = true;
            clr14 = true;
            clr15 = true;
            */
            panel2.Height = mainPageHeight;
        }


        private void tmrAutoRefresh_Tick(object sender, EventArgs e)
        {
            //if (!cbbLotteryTypes.Enabled)
            //{
            //btnGetLotteryInfo_Click(sender, e);
            //}
        }
        private void ClickEvent(GroupBox grp, EventHandler handler, string preName)
        {
            foreach (Control cc in grp.Controls)
            {
                if (cc is CheckBox)
                {
                    CheckBox cb = (CheckBox)cc;
                    if (cb.Name.StartsWith(preName))
                    {
                        cb.Click += handler;
                    }
                }
            }
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(sender.GetType().ToString());
        }


        private bool BaseNumAllBool = true;
        private void BaseNumAll_CheckedChanged(object sender, EventArgs e)
        {/*
            BaseNum1.Checked = BaseNumAll.Checked;
            BaseNum2.Checked = BaseNumAll.Checked;
            BaseNum3.Checked = BaseNumAll.Checked;
            BaseNum4.Checked = BaseNumAll.Checked;
            BaseNum5.Checked = BaseNumAll.Checked;
            BaseNum6.Checked = BaseNumAll.Checked;
            BaseNum7.Checked = BaseNumAll.Checked;
            BaseNum8.Checked = BaseNumAll.Checked;
            BaseNum9.Checked = BaseNumAll.Checked;
            BaseNum10.Checked = BaseNumAll.Checked;
            BaseNum11.Checked = BaseNumAll.Checked;
             */
            //
            if (BaseNumAllBool)
            {
                BaseNumAllBool = false;
                SetCheckboxState(grpBaseNums, false);
            }
            else
            {
                SetCheckboxState(grpBaseNums, true);
                BaseNumAllBool = true;
            }

        }
        /*
        private void cbbLotteryTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbLotteryTypes.SelectedIndex)
            {
                default:
                case 0:
                    currentLotteryTypeInfo.Lottery_Type = LotteryType.SD_11_5;
                    break;
                case 1:
                    currentLotteryTypeInfo.Lottery_Type = LotteryType.GD_11_5;
                    break;
                case 2:
                    currentLotteryTypeInfo.Lottery_Type = LotteryType.JX_11_5;
                    break;
                case 3:
                    currentLotteryTypeInfo.Lottery_Type = LotteryType.CQ_11_5;
                    break;
                case 4:
                    currentLotteryTypeInfo.Lottery_Type = LotteryType.JS_11_5;
                    break;
                case 5:
                    currentLotteryTypeInfo.Lottery_Type = LotteryType.ZJ_11_5;
                    break;
                case 6:
                    currentLotteryTypeInfo.Lottery_Type = LotteryType.SH_11_5;
                    break;

            }

            if (LoginState)
            {
                if (!File.Exists(currentLotteryTypeInfo.FileName))
                {
                    btnGetLotteryInfo_Click(sender, e);
                }
                else
                {
                    ReadLotteryInfosFromFile();

                    dgvLotteryInfos.Rows.Clear();

                    foreach (LotteryInfo tmp in LotteryInfos)
                    {
                        dgvLotteryInfos.Rows.Add(tmp.Day, tmp.Data);
                    }
                }

                if (LotteryInfos.Count > 0)
                {
                    txtLotteryDates.Text = (int.Parse(LotteryInfos[0].Day) + 1).ToString();
                }
            }
        }
        */
        private void CbbLotterySplite_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CbbLotterySplite.SelectedIndex)
            {
                default:
                case 0:
                    LotterySplite = " ";
                    break;
                case 1:
                    LotterySplite = "，";
                    break;

                case 2:
                    LotterySplite = ",";
                    break;
                case 3:
                    LotterySplite = "；";
                    break;
                case 4:
                    LotterySplite = ";";
                    break;
            }
        }

        #region 读取文件 往期彩票信息

        private void ReadLotteryInfosFromFile()
        {
            using (FileStream fs = File.OpenRead(currentLotteryTypeInfo.FileName))
            {
                byte[] b = new byte[fs.Length];
                UTF8Encoding temp = new UTF8Encoding(true);
                StringBuilder sb = new StringBuilder();
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    sb.Append(temp.GetString(b));
                }

                string datas = sb.ToString();
                /*

                string[] datasArr = datas.Split('\n');
                string[] tmpArr;

                List<LotteryInfo> lis = new List<LotteryInfo>();

                foreach (string tmp in datasArr)
                {
                    tmpArr = tmp.Split(',');
                    lis.Add(new LotteryInfo { Day = tmpArr[0], Data = tmpArr[1] });
                }

                 */

                string[] datasArr = datas.Split('\n');
                List<LotteryInfo> lis = new List<LotteryInfo>();

                for (int i = 0; i < datasArr.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        lis.Add(new LotteryInfo { Day = datasArr[i], Data = datasArr[i + 1] });
                    }
                }




                LotteryInfos = lis;
            }
        }

        #endregion

        private void SoundPlay(string filename)
        {
            System.Media.SoundPlayer media = new System.Media.SoundPlayer(filename);
            media.Play();
        }

        #region 保存文件 往期彩票信息

        private void AddText(FileStream fs, List<LotteryInfo> cpdatas)
        {
            if (cpdatas.Count != 0)
            {
                string splite = ",";
                string wrap = "\n";
                StringBuilder sb = new StringBuilder();
                foreach (LotteryInfo lotteryInfo in cpdatas)
                {
                    sb.Append(lotteryInfo.Day + splite + lotteryInfo.Data + wrap);
                }

                sb.Remove(sb.Length - 1, 1);

                string tmp = sb.ToString();


                byte[] info = new UTF8Encoding(true).GetBytes(tmp);
                fs.Write(info, 0, info.Length);
            }
            else
            {
                MessageBox.Show("数据库中没有彩种数据");
            }
        }

        private void AddTextChart(FileStream fs, List<LotteryInfo> cpdatas)
        {
            if (cpdatas.Count != 0)
            {
                string wrap = "\n";
                StringBuilder sb = new StringBuilder();
                foreach (LotteryInfo lotteryInfo in cpdatas)
                {
                    sb.Append(lotteryInfo.Day + wrap + lotteryInfo.Data + wrap);
                }

                sb.Remove(sb.Length - 1, 1);

                string tmp = sb.ToString();


                byte[] info = new UTF8Encoding(true).GetBytes(tmp);
                fs.Write(info, 0, info.Length);
            }
            else
            {
                MessageBox.Show("数据库中没有彩种数据");
            }
        }


        private void SaveLotteryInfosToFile()
        {
            //  if (!File.Exists(currentLotteryTypeInfo.FileName))
            //  {
            using (FileStream fs = File.Create(currentLotteryTypeInfo.FileName))
            {
                AddText(fs, LotteryInfos);
            }

            using (FileStream fs = File.Create(
                System.Environment.CurrentDirectory + @"\data\" + USERID + currentLotteryTypeInfo.Lottery_Type.ToString() + @".dll"))
            {
                AddTextChart(fs, LotteryInfos);
            }
            //   }
        }

        #endregion


        private void SetCheckState(GroupBox grp, int count, string autoPreName)
        {
            foreach (Control cc in grp.Controls)
            {
                if (cc is CheckBox)
                {

                    CheckBox cb = (CheckBox)cc;
                    if (cb.Name.StartsWith(autoPreName))
                    {
                        cb.Enabled = true;

                        //if(!cb.Checked){
                        //    //cb.Checked = true;
                        //} else {
                        //    cb.Checked = true;
                        //}
                        cb.Enabled = false;
                    }

                }
            }

            foreach (Control cc in grp.Controls)
            {

                if (cc is CheckBox)
                {

                    CheckBox cb = (CheckBox)cc;

                    if (cb.Name.StartsWith(autoPreName))
                    {

                        string Nindex = cb.Name.Substring(autoPreName.Length, cb.Name.Length - autoPreName.Length);

                        if (int.Parse(Nindex) <= count)
                        {
                            cb.Enabled = true;

                            if (count == 0 && int.Parse(Nindex) == 0)
                            {
                                cb.Enabled = false;
                                cb.Checked = false;
                            }

                        }
                        else
                        {
                            cb.Enabled = false;
                            cb.Checked = false;
                        }
                    }

                }
            }
        }


        private void SetCheckState(GroupBox grp, string selPreName, string autoProName)
        {
            int count = 0;
            foreach (Control cc in grp.Controls)
            {
                if (cc is CheckBox)
                {

                    CheckBox cb = (CheckBox)cc;

                    if (cb.Name.StartsWith(selPreName) && cb.Checked)
                    {
                        count++;
                    }
                }

                if (cc is CheckBox)
                {

                    CheckBox cb = (CheckBox)cc;

                    if (cb.Name.StartsWith(autoProName))
                    {
                        cb.Enabled = false;
                    }
                }
            }

            foreach (Control cc in grp.Controls)
            {

                if (cc is CheckBox)
                {

                    CheckBox cb = (CheckBox)cc;

                    if (cb.Name.StartsWith(autoProName))
                    {

                        string Nindex = cb.Name.Substring(autoProName.Length, cb.Name.Length - autoProName.Length);



                        if (int.Parse(Nindex) <= count)
                        {
                            cb.Enabled = true;
                            //cb.Checked = true;

                            if (count == 0 && int.Parse(Nindex) == 0)
                            {
                                cb.Enabled = false;
                                cb.Checked = false;
                            }
                        }
                        else
                        {
                            cb.Checked = false;
                            cb.Enabled = false;
                        }
                    }
                }
            }
        }

        private bool SetCheckState(GroupBox grp, int count, object sender)
        {
            int ccc = 0;
            foreach (Control cc in grp.Controls)
            {
                if (cc is CheckBox)
                {

                    CheckBox cb = (CheckBox)cc;

                    if (cb.Checked)
                    {
                        ccc++;
                    }
                }
            }

            if (ccc > count)
            {
                if (sender is CheckBox)
                {
                    (sender as CheckBox).Checked = false;
                }
            }


            if (ccc != 0)
            {
                return true;
            }

            return false;
        }

        private bool SetCheckState(GroupBox grp, int count, object sender, string preName)
        {
            int ccc = 0;
            foreach (Control cc in grp.Controls)
            {
                if (cc is CheckBox)
                {

                    CheckBox cb = (CheckBox)cc;

                    if (cb.Checked && cb.Name.StartsWith(preName))
                    {
                        ccc++;
                    }
                }
            }

            if (ccc > count)
            {
                if (sender is CheckBox)
                {
                    (sender as CheckBox).Checked = false;
                }
            }


            if (ccc != 0)
            {
                return true;
            }

            return false;
        }

        // 基础号码 -
        private void SetBaseNums()
        {
            int length = 0;
            int[] baseNums = new int[11];

            int notLength = 0;
            int[] notBaseNums = new int[11];
            if (grpBaseNums.HasChildren)
            {
                foreach (Control cc in grpBaseNums.Controls)
                {
                    if (cc is CheckBox)
                    {
                        CheckBox cb = (CheckBox)cc;
                        string cbIndex = cb.Name.Substring(7, cb.Name.Length - 7); //BaseNumX

                        if (!cbIndex.Equals("All"))
                        {
                            if (cb.Checked)
                            {
                                baseNums[length] = int.Parse(cbIndex);
                                length++;
                            }
                            else
                            {
                                notBaseNums[notLength] = int.Parse(cbIndex);
                                notLength++;

                            }
                        }
                    }
                }

                A11_5.NotBaseNums = AlgorithmTools.GetSubArray(notBaseNums, notLength);
                A11_5.BaseNums = AlgorithmTools.GetSubArray(baseNums, length);
            }
        }

        private int[] GetCheckedArray(GroupBox grp, string checkBoxPreName, bool bCheckState, int defaultArrLength = 15)
        {

            int[] temp = new int[defaultArrLength];
            int length = 0;

            foreach (Control cc in grp.Controls)
            {
                if (cc is CheckBox)
                {
                    CheckBox cb = (CheckBox)cc;
                    //if (cb.Checked) //Jerry
                    //{
                    if (cb.Name.StartsWith(checkBoxPreName)) //Jerry
                    {
                        string cbIndex = cb.Name.Substring(checkBoxPreName.Length, cb.Name.Length - checkBoxPreName.Length); //chkRePassBaseNumX

                        if (!Regex.IsMatch(cbIndex, "^[0-9]+$"))
                        {
                            continue;
                        }

                        if (cb.Checked == bCheckState && cb.Name.Substring(0, checkBoxPreName.Length).Equals(checkBoxPreName))
                        {
                            temp[length] = int.Parse(cbIndex);
                            length++;
                        }

                    }
                    //}
                }
            }


            return AlgorithmTools.GetSubArray(temp, length);
        }

        // 胆码列表 -
        private void SetBileCodes()
        {
            List<BileCode> bileCodes = new List<BileCode>();
            for (int i = 0; i < 5; i++)
            {
                int[] bileCode = GetCheckedArray(grpBileCode, "chkD" + (i + 1).ToString() + "_", true);
                int[] appearCounts = GetCheckedArray(grpBileCode, "chkD" + Char.ToString((char)('A' + i)) + "_", true);
                int[] notAppearCounts = GetCheckedArray(grpBileCode, "chkD" + Char.ToString((char)('A' + i)) + "_", false);
                BileCode bc =
                    new BileCode()
                    {
                        IsSelect = bileCode.Length == 0 ? false : true,
                        _BileCode = bileCode,
                        AppearCounts = appearCounts,
                        NotAppearCounts = notAppearCounts,
                    };

                bileCodes.Add(bc);
            }

            A11_5.BileCodes = bileCodes;
        }

        // 综合属性 -
        private void SetSynthesizedAttribute()
        {
            SynthesizedAttribute syattr = new SynthesizedAttribute()
            {
                EvenCounts = GetCheckedArray(grpEven, "chkEven", true),
                SmallCounts = GetCheckedArray(grpSmall, "chkSmall", true),
                SumCounts = GetCheckedArray(grpSum, "chkSum", true),
                LinkedCounts = GetCheckedArray(grpLinked, "chkLink", true),
                AppearCounts = GetCheckedArray(grpSynthesizedAttribute, "chkSA", true)
            };

            A11_5._SynthesizedAttribute = syattr;
        }

        // 平衡指数 -
        private void SetBalanceIndex()
        {
            List<BalanceState> balanceStates = new List<BalanceState>();
            //List<AppearState> appearStates = new List<AppearState>();

            if (chkLeftMore.Checked)
            {
                balanceStates.Add(BalanceState.LeftMore);
            }

            if (chkRightMore.Checked)
            {
                balanceStates.Add(BalanceState.RightMore);
            }

            if (chkLeftEqualRight.Checked)
            {
                balanceStates.Add(BalanceState.Equal);
            }

            /*
            if (chkAppear0.Checked)
            {
                appearStates.Add(AppearState.NotAppear);
            }

            if (chkAppear1.Checked)
            {
                appearStates.Add(AppearState.Appear);
            }
             */


            BalanceIndex bi = new BalanceIndex()
            {
                BalanceStates = balanceStates
            };



            A11_5._BalanceIndex = bi;
        }

        // 和值 -
        private void SetSumOfLotterys()
        {
            A11_5.SumOfLotterys = GetCheckedArray(grpHezhi, "chkHezhi", true, 40);
        }

        // 合值 -
        private void SetSmallBitValue()
        {
            A11_5.SmallBitValue = GetCheckedArray(grpHezhidown, "chkHezhidown", true);
        }

        // 跨度 -
        private void SetSpans()
        {
            A11_5.Spans = GetCheckedArray(grpKuadu, "chkGuadu", true);
        }


        // 朱佳峰
        // 两码差 -
        private void SetTwoNumDiss()
        {
            List<TwoNumDis> tnd = new List<TwoNumDis>();
            tnd.Add(new TwoNumDis()
            {
                _TwoNumDis = GetCheckedArray(grpLiangmacha, "chkLiangmacha1_", true),
                AppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian1_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian1_", false),
            });
            tnd.Add(new TwoNumDis()
            {
                _TwoNumDis = GetCheckedArray(grpLiangmacha, "chkLiangmacha2_", true),
                AppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian2_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian2_", false),
            });
            tnd.Add(new TwoNumDis()
            {
                _TwoNumDis = GetCheckedArray(grpLiangmacha, "chkLiangmacha3_", true),
                AppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian3_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian3_", false),
            });
            tnd.Add(new TwoNumDis()
            {
                _TwoNumDis = GetCheckedArray(grpLiangmacha, "chkLiangmacha4_", true),
                AppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian4_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian4_", false),
            });
            tnd.Add(new TwoNumDis()
            {
                _TwoNumDis = GetCheckedArray(grpLiangmacha, "chkLiangmacha5_", true),
                AppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian5_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmacha, "chkLiangmachaChuxian5_", false),
            });

            A11_5.TwoNumDiss = tnd;
        }

        // 差临值 -
        private void SetTwoNumDissCounts()
        {
            A11_5.TwoNumDissCounts = GetCheckedArray(grpLingchazhi, "chkLingchazhi", true);
        }

        // 两码和 -
        private void SetTwoNumPluss()
        {
            List<TwoNumPlus> tnp = new List<TwoNumPlus>();
            tnp.Add(new TwoNumPlus()
            {
                _TwoNumPlus = GetCheckedArray(grpLiangmahe, "chkLiangmahe1_", true, 210),
                AppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian1_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian1_", false)
            });
            tnp.Add(new TwoNumPlus()
            {
                _TwoNumPlus = GetCheckedArray(grpLiangmahe, "chkLiangmahe2_", true, 210),
                AppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian2_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian2_", false)
            });
            tnp.Add(new TwoNumPlus()
            {
                _TwoNumPlus = GetCheckedArray(grpLiangmahe, "chkLiangmahe3_", true, 210),
                AppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian3_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian3_", false)
            });
            tnp.Add(new TwoNumPlus()
            {
                _TwoNumPlus = GetCheckedArray(grpLiangmahe, "chkLiangmahe4_", true, 210),
                AppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian4_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian4_", false)
            });
            tnp.Add(new TwoNumPlus()
            {
                _TwoNumPlus = GetCheckedArray(grpLiangmahe, "chkLiangmahe5_", true, 210),
                AppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian5_", true),
                NotAppearCounts = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian5_", false)
            });

            A11_5._TwoNumPluss = tnp;

            // 出现个数
            //A11_5.TwoNumAppears = GetCheckedArray(grpLiangmahe, "chkLiangmaheChuxian", true);
        }


        // 连号轨迹
        private void SetLianHaoGuiJi()
        {
            A11_5._LianHaoGuiJi = new LianHaoGuiJi()
            {
                guiji = GetCheckedArray(grpLianhaoguiji, "chkLianhao", true),
                /*
                AppearCounts = GetCheckedArray(grpLianhaoguiji, "chkLianhaoChuxian", true),
                 */
            };
        }

        FinalBox final;
        private void Generate11_5_Click(object sender, EventArgs e)
        {
            if (final != null)
            {
                final.Close();
            }

            SetBaseNums();  //基础号码
            //SetReNoAndPassNo(); //重号传号
            SetBileCodes(); //胆码
            SetSynthesizedAttribute();//综合属性
            //SetSixLeft(); // 前后比例
            SetBalanceIndex(); // 设置平衡指数
            //SetMaxLinkNum(); // 设置集临个数 、溃临个数
            SetLianHaoGuiJi();// 连号轨迹
            //SetFaucetAndPterisPrimer(); // 设置龙头凤尾质数，和数
            //SetFaucetAndPterisEven();//设置龙头凤尾 单数、双数
            //SetFaucet012();//龙头 012路
            //SetPteris012();//凤尾 012路
            //SetLinkCounts();//设置临码号
            SetSumOfLotterys();// 设置和值
            SetSmallBitValue();//设置合值
            SetSpans();//设置跨度
            //SetMaxMinusSmallMinus4s();//设置临码和
            //SetMaxNearestNumDiss();//设置最大临码距离
            //SetSmallerBigerLengths();//设置反编球距离
            //SetSmallBiggerLenAddMaxNearestDiss();//设置边临和
            //SetHeadTailMaxSkip();//首尾邻码最大间距
            SetTwoNumDiss(); // 两码差
            //SetSkipNum(); // 跨码
            //SetSkipBitDis(); // 隔位差
            //SettwoNums(); // 两码组合
            SetTwoNumDissCounts(); //差临值
            //SetNearSkipCount(); //邻码间距
            //SetLocateIndexNum(); //定位组选
            //SetCountsOf012(); // 012路 
            SetTwoNumPluss();// 两码和
            //Setrate012s(); // 012路比例
            //SetkillNears(); // 断临
            //SetSkipBitSum(); // 隔位合

            //SetAiData(); //智能数据
            //SetAIValues(); // 智能值
            //SetAIBalance(); //智能平衡

            A11_5.ReGetBaseLotterys();
            A11_5.Calc();

            string splieString = "";
            switch (CbbLotterySplite.SelectedIndex)
            {
                default:
                case 0:
                    splieString = " ";
                    break;
                case 1:
                    splieString = "，";
                    break;
                case 2:
                    splieString = ",";
                    break;
                case 3:
                    splieString = "；";
                    break;
                case 4:
                    splieString = ";";
                    break;
            }


            final = new FinalBox(A11_5.Lotterys, splieString);
            final.Show();
        }


        private void btnD1Clear_Click(object sender, EventArgs e)
        {
            chkD1_01.Checked = false;
            chkD1_02.Checked = false;
            chkD1_03.Checked = false;
            chkD1_04.Checked = false;
            chkD1_05.Checked = false;
            chkD1_06.Checked = false;
            chkD1_07.Checked = false;
            chkD1_08.Checked = false;
            chkD1_09.Checked = false;
            chkD1_10.Checked = false;
            chkD1_11.Checked = false;

            chkDA_0.Checked = false;
            chkDA_1.Checked = false;
            chkDA_2.Checked = false;
            chkDA_3.Checked = false;
            chkDA_4.Checked = false;
            chkDA_5.Checked = false;

            chkDA_0.Enabled = false;
            chkDA_1.Enabled = false;
            chkDA_2.Enabled = false;
            chkDA_3.Enabled = false;
            chkDA_4.Enabled = false;
            chkDA_5.Enabled = false;
        }

        private void btnD2Clear_Click(object sender, EventArgs e)
        {
            chkD2_01.Checked = false;
            chkD2_02.Checked = false;
            chkD2_03.Checked = false;
            chkD2_04.Checked = false;
            chkD2_05.Checked = false;
            chkD2_06.Checked = false;
            chkD2_07.Checked = false;
            chkD2_08.Checked = false;
            chkD2_09.Checked = false;
            chkD2_10.Checked = false;
            chkD2_11.Checked = false;

            chkDB_0.Checked = false;
            chkDB_1.Checked = false;
            chkDB_2.Checked = false;
            chkDB_3.Checked = false;
            chkDB_4.Checked = false;
            chkDB_5.Checked = false;

            chkDB_0.Enabled = false;
            chkDB_1.Enabled = false;
            chkDB_2.Enabled = false;
            chkDB_3.Enabled = false;
            chkDB_4.Enabled = false;
            chkDB_5.Enabled = false;
        }

        private void btnD3Clear_Click(object sender, EventArgs e)
        {
            chkD3_01.Checked = false;
            chkD3_02.Checked = false;
            chkD3_03.Checked = false;
            chkD3_04.Checked = false;
            chkD3_05.Checked = false;
            chkD3_06.Checked = false;
            chkD3_07.Checked = false;
            chkD3_08.Checked = false;
            chkD3_09.Checked = false;
            chkD3_10.Checked = false;
            chkD3_11.Checked = false;

            chkDC_0.Checked = false;
            chkDC_1.Checked = false;
            chkDC_2.Checked = false;
            chkDC_3.Checked = false;
            chkDC_4.Checked = false;
            chkDC_5.Checked = false;

            chkDC_0.Enabled = false;
            chkDC_1.Enabled = false;
            chkDC_2.Enabled = false;
            chkDC_3.Enabled = false;
            chkDC_4.Enabled = false;
            chkDC_5.Enabled = false;
        }

        private void btnSAClear_Click(object sender, EventArgs e)
        {
            chkEven0.Checked = false;
            chkEven1.Checked = false;
            chkEven2.Checked = false;
            chkEven3.Checked = false;
            chkEven4.Checked = false;
            chkEven5.Checked = false;
            chkSmall0.Checked = false;
            chkSmall1.Checked = false;
            chkSmall2.Checked = false;
            chkSmall3.Checked = false;
            chkSmall4.Checked = false;
            chkSmall5.Checked = false;
            chkSum0.Checked = false;
            chkSum1.Checked = false;
            chkSum2.Checked = false;
            chkSum3.Checked = false;
            chkSum4.Checked = false;
            chkSum5.Checked = false;
            chkLink0.Checked = false;
            chkLink1.Checked = false;
            chkLink2.Checked = false;
            chkLink3.Checked = false;
            chkLink4.Checked = false;

            chkSA0.Checked = false;
            chkSA1.Checked = false;
            chkSA2.Checked = false;
            chkSA3.Checked = false;
            chkSA4.Checked = false;

            chkSA0.Enabled = false;
            chkSA1.Enabled = false;
            chkSA2.Enabled = false;
            chkSA3.Enabled = false;
            chkSA4.Enabled = false;
        }


        private void btnBalanceClear_Click(object sender, EventArgs e)
        {
            chkLeftEqualRight.Checked = false;
            chkLeftMore.Checked = false;
            chkRightMore.Checked = false;

            /*
            chkAppear0.Checked = false;
            chkAppear1.Checked = false;

            chkAppear0.Enabled = false;
            chkAppear1.Enabled = false;
             */
        }


        private void btnCalc_Click(object sender, EventArgs e)
        {

            int condition = -1;

            if (chkLiRunLv.Checked)
            {
                condition = 0;
            }
            else if (chkLiRun.Checked)
            {
                condition = 1;
            }
            else if (chkGeCheng.Checked)
            {
                condition = 2;
            }
            else if (chkGeJia.Checked)
            {
                condition = 3;
            }


            List<AlgorithmTools.BeiLvCalc> bcs = AlgorithmTools.CalcBeiLv(
                int.Parse(txtDanZhuJinE.Text), int.Parse(txtZhongJiangJinE.Text), int.Parse(txtJiHuaTouZhu.Text),
                int.Parse(txtQiShiBeiLv.Text), int.Parse(txtZhuiHaoQiShu.Text), condition,
                int.Parse(txtLiRun.Text), int.Parse(txtLiRunLv.Text), int.Parse(txtQiCheng.Text), int.Parse(cbbBeiCheng.Text), int.Parse(txtQiJia.Text), int.Parse(cbbBeiJia.Text));

            dgvCalc.Rows.Clear();
            for (int i = 0; i < bcs.Count; i++)
            {
                dgvCalc.Rows.Add((i + 1)
                    , bcs[i].BeiLv
                    , bcs[i].ZhuShu
                    , bcs[i].TouRu
                    , bcs[i].LeiJi
                    , bcs[i].ZhongJiang
                    , bcs[i].LiRun
                    , Math.Round(bcs[i].LiRunLv * 100).ToString() + "%"
                    );
            }
        }

        private void chkLiRun_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLiRun.Checked)
            {
                chkLiRunLv.Checked = !chkLiRun.Checked;
                chkGeCheng.Checked = !chkLiRun.Checked;
                chkGeJia.Checked = !chkLiRun.Checked;
            }

        }

        private void chkLiRunLv_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLiRunLv.Checked)
            {
                chkLiRun.Checked = !chkLiRunLv.Checked;
                chkGeCheng.Checked = !chkLiRunLv.Checked;
                chkGeJia.Checked = !chkLiRunLv.Checked;
            }

        }

        private void chkGeCheng_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGeCheng.Checked)
            {
                chkLiRun.Checked = !chkGeCheng.Checked;
                chkLiRunLv.Checked = !chkGeCheng.Checked;
                chkGeJia.Checked = !chkGeCheng.Checked;
            }

        }

        private void chkGeJia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGeJia.Checked)
            {
                chkLiRun.Checked = !chkGeJia.Checked;
                chkLiRunLv.Checked = !chkGeJia.Checked;
                chkGeCheng.Checked = !chkGeJia.Checked;
            }

        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (CyyDb.CheckCountOfSameOnlineUserLogin(USERID) <= 0)
            {
                tmrUpdate.Enabled = false;
                if (MessageBox.Show("系统检测到您的软件出现异常,请重新登录!", "警告", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    CyyMain.cyyMainExit();
                }
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkLianhaoChuxianQing_Click(object sender, EventArgs e)
        {
            chkLianhao0.Checked = false;
            chkLianhao1.Checked = false;
            chkLianhao2.Checked = false;

            /*
            chkLianhaoChuxian0.Checked = false;
            chkLianhaoChuxian1.Checked = false;

            chkLianhaoChuxian0.Enabled = false;
            chkLianhaoChuxian1.Enabled = false;
             */
        }

        private void chkGuaduQing_Click(object sender, EventArgs e)
        {
            SetCheckboxState(grpKuadu, false);
        }



        private void SetCheckboxState(GroupBox grp, bool b)
        {

            foreach (Control cc in grp.Controls)
            {
                if (cc is CheckBox)
                {
                    CheckBox cb = (CheckBox)cc;

                    cb.Checked = b;
                }
            }

        }

        private void chkHezhidownQing_Click(object sender, EventArgs e)
        {
            SetCheckboxState(grpHezhidown, false);
        }

        private void bntLiangmachaChuxianQing1_Click(object sender, EventArgs e)
        {

            chkLiangmacha1_1.Checked = false;
            chkLiangmacha1_2.Checked = false;
            chkLiangmacha1_3.Checked = false;
            chkLiangmacha1_4.Checked = false;
            chkLiangmacha1_5.Checked = false;
            chkLiangmacha1_6.Checked = false;
            chkLiangmacha1_7.Checked = false;
            chkLiangmacha1_8.Checked = false;
            chkLiangmacha1_9.Checked = false;
            chkLiangmacha1_10.Checked = false;

            chkLiangmachaChuxian1_0.Checked = false;
            chkLiangmachaChuxian1_1.Checked = false;
            chkLiangmachaChuxian1_2.Checked = false;
            chkLiangmachaChuxian1_3.Checked = false;
            chkLiangmachaChuxian1_4.Checked = false;
            chkLiangmachaChuxian1_5.Checked = false;
        }

        private void chkLingchazhiQing_Click(object sender, EventArgs e)
        {
            SetCheckboxState(grpLingchazhi, false);
        }

        // 断临
        /*
        private void bntDuanlingQing_Click(object sender, EventArgs e)
        {
            SetCheckboxState(grpDuanling, false);
        }
         */

        private void chkLiangmaheChuxianQing_Click(object sender, EventArgs e)
        {
            chkLiangmahe1_3.Checked = false;
            chkLiangmahe1_4.Checked = false;
            chkLiangmahe1_5.Checked = false;
            chkLiangmahe1_6.Checked = false;
            chkLiangmahe1_7.Checked = false;
            chkLiangmahe1_8.Checked = false;
            chkLiangmahe1_9.Checked = false;
            chkLiangmahe1_10.Checked = false;
            chkLiangmahe1_11.Checked = false;
            chkLiangmahe1_12.Checked = false;
            chkLiangmahe1_13.Checked = false;
            chkLiangmahe1_14.Checked = false;
            chkLiangmahe1_15.Checked = false;
            chkLiangmahe1_16.Checked = false;
            chkLiangmahe1_17.Checked = false;
            chkLiangmahe1_18.Checked = false;
            chkLiangmahe1_19.Checked = false;
            chkLiangmahe1_20.Checked = false;
            chkLiangmahe1_21.Checked = false;

            chkLiangmaheChuxian1_0.Checked = false;
            chkLiangmaheChuxian1_1.Checked = false;
            chkLiangmaheChuxian1_2.Checked = false;
            chkLiangmaheChuxian1_3.Checked = false;
            chkLiangmaheChuxian1_4.Checked = false;
            chkLiangmaheChuxian1_5.Checked = false;

            chkLiangmaheChuxian1_0.Enabled = false;
            chkLiangmaheChuxian1_1.Enabled = false;
            chkLiangmaheChuxian1_2.Enabled = false;
            chkLiangmaheChuxian1_3.Enabled = false;
            chkLiangmaheChuxian1_4.Enabled = false;
            chkLiangmaheChuxian1_5.Enabled = false;
        }

        private void chkD1_01_Click(object sender, EventArgs e)
        {
            SetCheckState(grpBileCode, "chkD1_", "chkDA_");
            //SetCheckState(grpBileCode, 5, sender, "chkD1_");
        }

        private void chkD2_01_Click(object sender, EventArgs e)
        {
            SetCheckState(grpBileCode, "chkD2_", "chkDB_");
            //SetCheckState(grpBileCode, 5, sender, "chkD2_");
        }

        private void chkD3_01_Click(object sender, EventArgs e)
        {
            SetCheckState(grpBileCode, "chkD3_", "chkDC_");
            //SetCheckState(grpBileCode, 5, sender, "chkD3_");
        }

        private void chkD4_01_Click(object sender, EventArgs e)
        {
            SetCheckState(grpBileCode, "chkD4_", "chkDD_");
            //SetCheckState(grpBileCode, 5, sender, "chkD4_");
        }

        private void chkD5_01_Click(object sender, EventArgs e)
        {
            SetCheckState(grpBileCode, "chkD5_", "chkDE_");
            //SetCheckState(grpBileCode, 5, sender, "chkD5_");
        }

        private void chkLeftMore_Click(object sender, EventArgs e)
        {
            int count = (chkLeftMore.Checked ? 1 : 0)
                + (chkRightMore.Checked ? 1 : 0) + (chkLeftEqualRight.Checked ? 1 : 0);


            SetCheckState(grpBalanceIndex, count, "chkAppear");


        }

        private void chkLianhao0_Click(object sender, EventArgs e)
        {
            int count = (chkLianhao0.Checked ? 1 : 0)
    + (chkLianhao1.Checked ? 1 : 0) + (chkLianhao2.Checked ? 1 : 0);


            SetCheckState(grpLianhaoguiji, count, "chkLianhaoChuxian");
        }

        private void chkSmall0_Click(object sender, EventArgs e)
        {
            int count = ((chkSmall0.Checked || chkSmall1.Checked || chkSmall2.Checked || chkSmall3.Checked || chkSmall4.Checked || chkSmall5.Checked) ? 1 : 0)
                + ((chkEven0.Checked || chkEven1.Checked || chkEven2.Checked || chkEven3.Checked || chkEven4.Checked || chkEven5.Checked) ? 1 : 0)
                + ((chkSum0.Checked || chkSum1.Checked || chkSum2.Checked || chkSum3.Checked || chkSum4.Checked || chkSum5.Checked) ? 1 : 0)
                + ((chkLink0.Checked || chkLink1.Checked || chkLink2.Checked || chkLink3.Checked || chkLink4.Checked) ? 1 : 0);

            SetCheckState(grpSynthesizedAttribute, count, "chkSA");
        }

        private void chkLiangmacha1_0_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmacha, "chkLiangmacha1_", "chkLiangmachaChuxian1_");
            // SetCheckState(grpLiangmacha, 5, sender, "chkLiangmachaChuxian1_");
        }

        private void chkLiangmacha2_0_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmacha, "chkLiangmacha2_", "chkLiangmachaChuxian2_");
            // SetCheckState(grpLiangmacha, 5, sender, "chkLiangmachaChuxian2_");
        }

        private void chkLiangmacha3_0_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmacha, "chkLiangmacha3_", "chkLiangmachaChuxian3_");
            //SetCheckState(grpLiangmacha, 5, sender, "chkLiangmachaChuxian3_");
        }

        private void chkLiangmacha4_0_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmacha, "chkLiangmacha4_", "chkLiangmachaChuxian4_");
            //SetCheckState(grpLiangmacha, 5, sender, "chkLiangmachaChuxian4_");
        }

        private void chkLiangmacha5_0_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmacha, "chkLiangmacha5_", "chkLiangmachaChuxian5_");
            //SetCheckState(grpLiangmacha, 5, sender, "chkLiangmachaChuxian5_");
        }

        private void chkLiangmahe1_3_Click(object sender, EventArgs e)
        {
            /*
            int count =
            ((chkLiangmahe1_3.Checked || chkLiangmahe1_4.Checked || chkLiangmahe1_5.Checked || chkLiangmahe1_6.Checked || chkLiangmahe1_7.Checked || chkLiangmahe1_8.Checked || chkLiangmahe1_9.Checked || chkLiangmahe1_10.Checked || chkLiangmahe1_11.Checked || chkLiangmahe1_12.Checked || chkLiangmahe1_13.Checked || chkLiangmahe1_14.Checked || chkLiangmahe1_15.Checked || chkLiangmahe1_16.Checked || chkLiangmahe1_17.Checked || chkLiangmahe1_18.Checked || chkLiangmahe1_19.Checked || chkLiangmahe1_20.Checked || chkLiangmahe1_21.Checked) ? 1 : 0)
            + ((chkLiangmahe2_3.Checked || chkLiangmahe2_4.Checked || chkLiangmahe2_5.Checked || chkLiangmahe2_6.Checked || chkLiangmahe2_7.Checked || chkLiangmahe2_8.Checked || chkLiangmahe2_9.Checked || chkLiangmahe2_10.Checked || chkLiangmahe2_11.Checked || chkLiangmahe2_12.Checked || chkLiangmahe2_13.Checked || chkLiangmahe2_14.Checked || chkLiangmahe2_15.Checked || chkLiangmahe2_16.Checked || chkLiangmahe2_17.Checked || chkLiangmahe2_18.Checked || chkLiangmahe2_19.Checked || chkLiangmahe2_20.Checked || chkLiangmahe2_21.Checked) ? 1 : 0)
            + ((chkLiangmahe3_3.Checked || chkLiangmahe3_4.Checked || chkLiangmahe3_5.Checked || chkLiangmahe3_6.Checked || chkLiangmahe3_7.Checked || chkLiangmahe3_8.Checked || chkLiangmahe3_9.Checked || chkLiangmahe3_10.Checked || chkLiangmahe3_11.Checked || chkLiangmahe3_12.Checked || chkLiangmahe3_13.Checked || chkLiangmahe3_14.Checked || chkLiangmahe3_15.Checked || chkLiangmahe3_16.Checked || chkLiangmahe3_17.Checked || chkLiangmahe3_18.Checked || chkLiangmahe3_19.Checked || chkLiangmahe3_20.Checked || chkLiangmahe3_21.Checked) ? 1 : 0)
            + ((chkLiangmahe4_3.Checked || chkLiangmahe4_4.Checked || chkLiangmahe4_5.Checked || chkLiangmahe4_6.Checked || chkLiangmahe4_7.Checked || chkLiangmahe4_8.Checked || chkLiangmahe4_9.Checked || chkLiangmahe4_10.Checked || chkLiangmahe4_11.Checked || chkLiangmahe4_12.Checked || chkLiangmahe4_13.Checked || chkLiangmahe4_14.Checked || chkLiangmahe4_15.Checked || chkLiangmahe4_16.Checked || chkLiangmahe4_17.Checked || chkLiangmahe4_18.Checked || chkLiangmahe4_19.Checked || chkLiangmahe4_20.Checked || chkLiangmahe4_21.Checked) ? 1 : 0)
            + ((chkLiangmahe5_3.Checked || chkLiangmahe5_4.Checked || chkLiangmahe5_5.Checked || chkLiangmahe5_6.Checked || chkLiangmahe5_7.Checked || chkLiangmahe5_8.Checked || chkLiangmahe5_9.Checked || chkLiangmahe5_10.Checked || chkLiangmahe5_11.Checked || chkLiangmahe5_12.Checked || chkLiangmahe5_13.Checked || chkLiangmahe5_14.Checked || chkLiangmahe5_15.Checked || chkLiangmahe5_16.Checked || chkLiangmahe5_17.Checked || chkLiangmahe5_18.Checked || chkLiangmahe5_19.Checked || chkLiangmahe5_20.Checked || chkLiangmahe5_21.Checked) ? 1 : 0);
           
            SetCheckState(grpLiangmahe, count, "chkLiangmaheChuxian"); */

            SetCheckState(grpLiangmahe, "chkLiangmahe1_", "chkLiangmaheChuxian1_");
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            chkLiangmaheChuxianQing1.PerformClick();
            //chkLiangmaheChuxianQing2.PerformClick();
            //chkLiangmaheChuxianQing3.PerformClick();
            //chkLiangmaheChuxianQing4.PerformClick();
            //chkLiangmaheChuxianQing5.PerformClick();
            //button8.PerformClick();
            //btnRePassBaseNumClear_Click(sender, e);
            //btnReNoCountClear_Click(sender, e);
            //btnPassSmallCount_Click(sender, e);
            //btnPassBigCount_Click(sender, e);
            btnD1Clear_Click(sender, e);
            btnD2Clear_Click(sender, e);
            btnD3Clear_Click(sender, e);
            //btnD4Clear_Click(sender, e);
            //btnD5Clear_Click(sender, e);
            btnSAClear_Click(sender, e);
            //chkLeftRightClear_Click(sender, e);
            btnBalanceClear_Click(sender, e);
            //btnLinkNumClear_Click(sender, e);
            //btnNotLinkNumClear_Click(sender, e);
            chkLianhaoChuxianQing_Click(sender, e);
            //chkLingmahaoChuxianQing_Click(sender, e);
            //btnLongfengZhiheQing_Click(sender, e);
            //btnLongfengDanshuangChuxianQing_Click(sender, e);
            chkGuaduQing_Click(sender, e);
            //chkLingmaheQing_Click(sender, e);
            chkHezhidownQing_Click(sender, e);
            bntLiangmachaChuxianQing1_Click(sender, e);
            //bntLiangmachaChuxianQing2_Click(sender, e);
            //bntLiangmachaChuxianQing3_Click(sender, e);
            //bntLiangmachaChuxianQing4_Click(sender, e);
            //bntLiangmachaChuxianQing5_Click(sender, e);
            //chkFanbianqiujuliQing_Click(sender, e);
            //bntZuidalingmakuajuQing_Click(sender, e);
            //bntBianlingheQing_Click(sender, e);
            //chkKuamaChuxianQing_Click(sender, e);
            //chkLingmajianjuChuxianQing_Click(sender, e);
            //chkShouweilingmazuidajianjuQing_Click(sender, e);
            chkLingchazhiQing_Click(sender, e);
            //bntDuanlingQing_Click(sender, e); // 断临
            chkLiangmaheChuxianQing_Click(sender, e);
            //bnt012lugeshuQing_Click(sender, e);
            //bnt012lubiliChuxianQing_Click(sender, e);
            //chkKuaisupipeiQing_Click(sender, e);
            //bntGeweiheChuxianQing_Click(sender, e);
            //bntGeweichaChuxianQing_Click(sender, e);
            //bntGeweihefengxuzuxuanChuxianQing_Click(sender, e);
            //bntGeweichafengxuzuxuanChuxianQing_Click(sender, e);
            //chkZhinengShujuChuxianQing_Click(sender, e);
            //bntZhinengzhiAQing_Click(sender, e);
            //bntZhinengzhiQing_Click(sender, e);
            //bntZhinengpinghengAQing_Click(sender, e);
            //bntZhinengpinghengBQing_Click(sender, e);
            //bntZhinengpinghengCQing_Click(sender, e);
            //bntZhinengpinghengChuxianQing_Click(sender, e);
            //
            SetCheckboxState(grpHezhi, false);
            //ClearDingweizuxuan();
            //SetCheckboxState(grpLongtou012Lu, false);
            //SetCheckboxState(grpFengwei012Lu, false);

        }

        private bool chkKuamaSelectAllBool = false;

        private bool chkShouweilingmazuidajianjuSelectAllBool = false;

        private bool checkBox234Bool = false;
        private void checkBox234_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox234Bool)
            {
                checkBox234Bool = true;
                SetCheckboxState(grpHezhi, true);

            }
            else
            {
                SetCheckboxState(grpHezhi, false);
                checkBox234Bool = false;
            }
        }

        private bool chkZhinengASelectAllBool = false;
        private bool chkZhinengBSelectAllBool = false;

        private bool chkZhinengCSelectAllBool = false;

        private bool chkZhinengDSelectAllBool = false;

        private void tabControl2_DrawItem(object sender, DrawItemEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Near;
            sf.Alignment = StringAlignment.Near;

            e.Graphics.DrawString(((TabControl)sender).TabPages[e.Index].Text,
             System.Windows.Forms.SystemInformation.MenuFont,
             new SolidBrush(Color.Black),
             e.Bounds, sf);
        }

        private void SetTabState(Button bt, bool isSelected)
        {
            if (isSelected)
            {
                bt.FlatAppearance.BorderColor = Color.RoyalBlue;
                bt.FlatAppearance.BorderSize = 1;
                //bt.BackColor = Color.Gray;
                bt.ForeColor = Color.RoyalBlue;
            }
            else
            {
                bt.FlatAppearance.BorderColor = Color.Gray;
                bt.FlatAppearance.BorderSize = 1;
                //bt.BackColor = Color.White;
                bt.ForeColor = Color.Black;
            }
        }
        [DllImport("CyyChartDll.dll", EntryPoint = "GetBaseChart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetBaseChart(string usrName, string lotteryType);

        [DllImport("CyyChartDll.dll", EntryPoint = "ZongHeChart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZongHeChart(string usrName, string lotteryType);

        [DllImport("CyyChartDll.dll", EntryPoint = "LiangMaHeCha", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LiangMaHeCha(string usrName, string lotteryType);

        [DllImport("CyyChartDll.dll", EntryPoint = "HeZhiChart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HeZhiChart(string usrName, string lotteryType);

        [DllImport("CyyChartDll.dll", EntryPoint = "HeZhi2Chart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void HeZhi2Chart(string usrName, string lotteryType);

        [DllImport("CyyChartDll.dll", EntryPoint = "JiLinChart", CallingConvention = CallingConvention.Cdecl)]
        public static extern void JiLinChart(string usrName, string lotteryType);

        private void tabControl3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void viewErrorChart(GeckoWebBrowser wb)
        {
            wb.Navigate(System.Environment.CurrentDirectory + @"\chart\error.html");
        }


        private List<Lottery> GetCPDllInfos(string userId, string cptype, List<string> days)
        {
            readCertainFile(System.Environment.CurrentDirectory + @"\data\" + userId + cptype + ".dll");



            List<Lottery> lotterys = new List<Lottery>();

            foreach (LotteryInfo li in LotteryInfos)
            {
                lotterys.Add(new Lottery(li.Data.ToString()));
                days.Add(li.Day.ToString());
            }

            return lotterys;
        }


        object CurrentSender = null;
        int btnIndex = -1;

        string currentHtml = "error.html";

        private void DeleteFile(string filename)
        {
            string fn = System.Environment.CurrentDirectory + @"\chart\" + filename;
            if (File.Exists(fn))
            {
                if (!filename.Equals("error.html"))
                {
                    File.Delete(fn);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0: //选项条件

                    break;
                default:
                    tabControlWarning(tabControl1);
                    break;

            }
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tcMain.SelectedIndex)
            {
                case 0: //11选5

                    break;
                case 8: //倍率计算器

                    break;
                default:
                    //tabControlWarning(tcMain);
                    MessageBox.Show("基础版暂未开放此功能，敬请期待！", "友情提示");
                    tcMain.SelectedIndex = 0;
                    break;

            }
        }

        private void tabControlWarning(TabControl tb)
        {
            DialogResult result = MessageBox.Show("此版本功能有限，是否需要下载功能更全的版本？", "友情提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("http://www.caiyingying.com/down.php");
            }
            else
            {
            }
            tb.SelectedIndex = 0;
        }

        private void tmrLoginDate_Tick(object sender, EventArgs e)
        {
            CyyDb.UpdateOnlineUserData(USERID);
        }

        private void CyyMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (loginstate)
            {
                if (!USERID.Equals(""))
                {
                    CyyDb.DeleteDateNow(USERID);
                }
                else
                {

                }
            }
        }

        private void chkD1_01_Paint(object sender, PaintEventArgs e)
        {

        }


        private TwoNum GetTwoNum(string chkNames, string preName)
        {
            string twonums = chkNames.Substring(preName.Length, chkNames.Length - preName.Length);
            string[] strs = twonums.Split('_');

            return new TwoNum() { Num1 = int.Parse(strs[0]), Num2 = int.Parse(strs[1]) };
        }

        private void tmrShows_Tick(object sender, EventArgs e)
        {
            lblSHOWS.Left--;
            lblSHOWS2.Left--;

            if (lblSHOWS.Left < -lblSHOWS.Width)
            {
                if (lblSHOWS.Width < panel17.Width)
                    lblSHOWS.Left = panel17.Width + lblSHOWS2.Left;
                else
                    lblSHOWS.Left = lblSHOWS.Width + lblSHOWS2.Left;
            }

            if (lblSHOWS2.Left < -lblSHOWS2.Width)
            {
                if (lblSHOWS2.Width < panel17.Width)
                    lblSHOWS2.Left = panel17.Width + lblSHOWS.Left;
                else
                    lblSHOWS2.Left = lblSHOWS2.Width + lblSHOWS.Left;
            }

        }

        private void panel18_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void CyyMain_Resize(object sender, EventArgs e)
        {
            panel2.Height = this.Height - topbarHeight;
            //this.OnSizeChanged(e);
            //this.UpdateSystemButtonRect();
            //this.OnPaint((PaintEventArgs)e);

            /*
            if (CurrentSender != null) {
                (CurrentSender as Button).PerformClick();
            }
             */
        }

        private void CyyMain_ResizeEnd(object sender, EventArgs e)
        {
            //this.UpdateSystemButtonRect();
            //this.OnSizeChanged(e);
            //this.OnPaint((PaintEventArgs)e);
        }

        private void CyyMain_MaximumSizeChanged(object sender, EventArgs e)
        {
            //panel2.Height = this.Height - topbarHeight;
            this.UpdateSystemButtonRect();
        }

        // 比较版本是否需要更新
        private bool compareVersion(string currentVersion, string newVersion, int versionBit)
        {
            string[] currentVersionArray = currentVersion.Split('.');
            string[] newVersionArray = newVersion.Split('.');

            for (int i = 0; i < currentVersionArray.Length; i++)
            {
                if (currentVersionArray[i].Length < versionBit)
                {
                    currentVersionArray[i] = "0" + currentVersionArray[i];
                }
                if (newVersionArray[i].Length < versionBit)
                {
                    newVersionArray[i] = "0" + newVersionArray[i];
                }
            }
            string currentVersionString = currentVersionArray[0] + currentVersionArray[1] + currentVersionArray[2];
            string newVersionString = newVersionArray[0] + newVersionArray[1] + newVersionArray[2];
            if (int.Parse(currentVersionString) < int.Parse(newVersionString))
            {
                return true;
            }
            return false;
        }


        private string getOnlineFile(string fileUrl)
        {

            WebClient wcClient = new WebClient();

            long fileLength = 0;

            string updateFileUrl = fileUrl;

            WebRequest webReq = WebRequest.Create(updateFileUrl);
            WebResponse webRes = null;
            Stream srm = null;
            StreamReader srmReader = null;
            string ss = "";
            try
            {
                webRes = webReq.GetResponse();
                fileLength = webRes.ContentLength;

                srm = webRes.GetResponseStream();
                srmReader = new StreamReader(srm);

                byte[] bufferbyte = new byte[fileLength];
                int allByte = (int)bufferbyte.Length;
                int startByte = 0;
                while (fileLength > 0)
                {
                    Application.DoEvents();
                    int downByte = srm.Read(bufferbyte, startByte, allByte);
                    if (downByte == 0) { break; };
                    startByte += downByte;
                    allByte -= downByte;

                    float part = (float)startByte / 1024;
                    float total = (float)bufferbyte.Length / 1024;
                    int percent = Convert.ToInt32((part / total) * 100);

                }
                ss = Encoding.Default.GetString(bufferbyte).Trim();

                srm.Close();
                srmReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取信息异常: " + ex.Message);
                //throw;
                return "";
            }
            return ss;
        }

        private void tmr_updateVersion_Tick(object sender, EventArgs e)
        {
            /*
            if (loginstate)
            {
                tmr_updateVersion.Enabled = false;
            }
            else
            {
                return;
            }
            */
            tmr_updateVersion.Enabled = false;
            string ss = getOnlineFile(@"http://www.caiyingying.com/products/cyy/free/Version.txt");

            /*
            string ssss = string.Empty;

            using (FileStream fszz = File.OpenRead(System.Environment.CurrentDirectory + @"\Version.txt"))
            {
                byte[] bytes = new byte[fszz.Length];
                fszz.Read(bytes, 0, bytes.Length);

                ssss = Encoding.Default.GetString(bytes);
            }
             */
            if (!ss.Equals(""))
            {
                bool ok = false;
                if (!proudctVersionString.Equals(""))
                {
                    string localVersionString = "";
                    if (proudctVersionString.Split('.').Length == 2)
                    {
                        localVersionString = proudctVersionString + ".0";
                    }
                    else
                    {
                        localVersionString = proudctVersionString;
                    }

                    if (compareVersion(localVersionString, ss, 2))
                    {
                        ok = true;
                        /* using (FileStream fs = new FileStream(System.Environment.CurrentDirectory + @"\Version.txt", FileMode.OpenOrCreate, FileAccess.Write))
                        / {
                             fs.Write(bufferbyte, 0, bufferbyte.Length);
                         }  
                         */
                    }

                    //fs.Close();
                    if (ok)
                    {
                        string updateLog = getOnlineFile(@"http://www.caiyingying.com/products/cyy/free/updateLog.txt");

                        //if (!st.Equals(""))
                        //{
                        //    UpdateNotification unf = new UpdateNotification("彩盈盈做号系统有新版本了(" + productKeyNameString + ss + ")，是否需要更新？", st, @"http://www.caiyingying.com/products/cyy/full/" + productKeyNameString + productKeyVersionNameString + @"v" + ss + @".exe");
                        //    unf.ShowDialog();
                        //}

                        if (!updateLog.Equals(""))
                        {
                            update ud = new update(@"http://www.caiyingying.com/products/cyy/free/" + productKeyNameString + productKeyVersionNameString + @"v" + ss + ".exe", ss, updateLog);
                            ud.ShowDialog(this);
                        }

                        /*
                        if (MessageBox.Show("彩盈盈做号系统有新版本了(" + productKeyNameString + ss + ")，是否需要更新？", "软件更新", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            //Visible = false;
                            //彩盈盈彩票做号系统精华版2.7.exe
                            update ud = new update(@"http://www.caiyingying.com/products/cyy/free/" + productKeyNameString + productKeyVersionNameString + ss + ".exe");

                            ud.ShowDialog(this);
                        }
                        */

                        /*
                        DialogResult resault = MessageBox.Show("确定退出？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Information,MessageBoxButtons.button1);
                        if (resault == DialogResult.OK)
                        {
                            this.Close();
                        }
                         */
                    }
                }

            }
        }


        public void getExe(string name)
        {


        }

        private void chkLiangmahe2_21_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmahe, "chkLiangmahe2_", "chkLiangmaheChuxian2_");
        }

        private void chkLiangmahe3_21_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmahe, "chkLiangmahe3_", "chkLiangmaheChuxian3_");
        }

        private void chkLiangmahe4_21_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmahe, "chkLiangmahe4_", "chkLiangmaheChuxian4_");
        }

        private void chkLiangmahe5_21_Click(object sender, EventArgs e)
        {
            SetCheckState(grpLiangmahe, "chkLiangmahe5_", "chkLiangmaheChuxian5_");
        }

        private void chkLiangmahe1_12_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void SetChecked(GroupBox grp, bool chkState, string checkBoxPreName, Func<int, bool> function)
        {
            foreach (Control cc in grp.Controls)
            {
                if (cc is CheckBox)
                {
                    CheckBox cb = (CheckBox)cc;

                    if (cb.Name.StartsWith(checkBoxPreName))
                    {
                        string cbIndex = cb.Name.Substring(checkBoxPreName.Length, cb.Name.Length - checkBoxPreName.Length); //chkRePassBaseNumX

                        if (!Regex.IsMatch(cbIndex, "^[0-9]+$"))
                        {
                            continue;
                        }
                        int index = Convert.ToInt32(cbIndex);

                        if (function(index))
                        {
                            cb.Checked = chkState;
                        }
                    }
                }
            }

        }

        private void chk2_CheckedChanged(object sender, EventArgs e)
        {
            SetChecked(grpHezhi, chkHe2.Checked, "chkHezhi",
                (n) => { if (n % 2 == 1) { return true; } return false; });

        }

        private void chkHe3_CheckedChanged(object sender, EventArgs e)
        {

            SetChecked(grpHezhi, chkHe3.Checked, "chkHezhi",
                (n) => { if (n % 2 == 0) { return true; } return false; });


        }

        private void chkHe4_CheckedChanged(object sender, EventArgs e)
        {
            SetChecked(grpHezhi, chkHe4.Checked, "chkHezhi",
                (n) =>
                {
                    bool isprime = true;
                    // 判断是否为质数
                    for (int i = 2; i <= n / 2; i++)
                    {
                        if ((n % i) == 0)
                        {
                            isprime = false;
                        }
                    }

                    return isprime;
                });

        }

        private void chkHe5_CheckedChanged(object sender, EventArgs e)
        {
            SetChecked(grpHezhi, chkHe5.Checked, "chkHezhi",
                    (n) =>
                    {
                        bool isprime = true;

                        for (int i = 2; i <= n / 2; i++)
                        {
                            if ((n % i) == 0)
                            {
                                isprime = false;
                            }
                        }

                        return !isprime;
                    });
        }

        private void chkHe6_CheckedChanged(object sender, EventArgs e)
        {

            SetChecked(grpHezhi, chkHe6.Checked, "chkHezhi",
                   (n) =>
                   {
                       return true;
                   });

        }

        private void chkHeQing_CheckedChanged(object sender, EventArgs e)
        {
            SetCheckboxState(grpHezhi, false);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SetCheckboxState(grpHezhi, false);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://cyyrj.taobao.com/?v=1");
        }


    }
}
