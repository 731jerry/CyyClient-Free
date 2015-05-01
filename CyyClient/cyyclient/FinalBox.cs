using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CyyClient
{
    public partial class FinalBox : Form
    {
        private string splite = " ";
        List<Lottery> lotterys;
        List<Lottery> backLottery;

        public FinalBox(List<Lottery> lotterys, string splite)
            : this()
        {
            this.splite = splite;
            this.lotterys = lotterys;
            display(lotterys);
        }
        public FinalBox()
        {
            InitializeComponent();
        }

        private bool isFanxiang = false;
        public void display(List<Lottery> lotterys)
        {
            if (!isFanxiang)
            {
                this.Text = "共生成号码： " + lotterys.Count.ToString() + " 注";
                button2.Text = "反向采集";
                isFanxiang = true;
            }
            else
            {
                this.Text = "反向采集号码： " + lotterys.Count.ToString() + " 注";
                button2.Text = "还原查看";
                isFanxiang = false;
            }
            txtOutPut.Clear();
            foreach (Lottery tmp in lotterys)
            {
                txtOutPut.Text += (
                    (new StringBuilder())
                    .Append(tmp[0].ToString("D2")).Append(splite)
                    .Append(tmp[1].ToString("D2")).Append(splite)
                    .Append(tmp[2].ToString("D2")).Append(splite)
                    .Append(tmp[3].ToString("D2")).Append(splite)
                    .Append(tmp[4].ToString("D2")).Append("\r\n").ToString());
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {

                if (!File.Exists(saveFileDialog1.FileName))
                {
                    using (FileStream fs = File.Create(saveFileDialog1.FileName))
                    {
                        string tmp = txtOutPut.Text;

                        /*
                        foreach (string s in listBox1.Items)
                        {
                            tmp += s + "\n";
                        }
                         */

                        byte[] info = new UTF8Encoding(true).GetBytes(tmp);
                        fs.Write(info, 0, info.Length);
                    }
                }
                else
                {
                    MessageBox.Show("该文件已经存在");
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string tmp = txtOutPut.Text;
            /*
            foreach (string s in listBox1.Items)
            {
                tmp += s + "\n";
            }
             */
            Clipboard.SetDataObject(tmp);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //display(null);
            txtOutPut.Clear();
            backLottery = Algorithm11_5.GetBaseLotterys();

            backLottery.RemoveAll(
                    delegate(Lottery lt)
                    {
                        if (lotterys.Contains(lt))
                        {
                            return true;
                        }

                        return false;
                    });
            display(backLottery);

            lotterys = backLottery;

        }
    }
}