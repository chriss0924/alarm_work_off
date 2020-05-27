using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Text.RegularExpressions;

using System.IO;

namespace alarm_work_off
{
    public partial class Form1 : Form
    {
        public string strIniFileName = @"D:\\work_off.ini";
        private string strHourValue = "";
        private string strMinuteValue = "";
        private string strSecondValue = "";

        private int iHour = 00;
        private int iMinute = 00;
        private int iSecond = 00;


        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1000; //每秒掃一次
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += new EventHandler(timer1_Tick);

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //設定程式視窗出現位子，該位子已經跟螢幕解析度作比例調整
            //目前設定為視窗開啟時出現在螢幕右上角
            int x = (System.Windows.Forms.SystemInformation.WorkingArea.Width - this.Size.Width);
            int y = (System.Windows.Forms.SystemInformation.WorkingArea.Height - System.Windows.Forms.SystemInformation.WorkingArea.Height);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = (Point)new Size(x, y);

            if(!File.Exists(@"D:\\work_off.ini"))
            {
                // to do
            }

            //取ini值並轉成int
            strHourValue = GetKeyValueString(strIniFileName, "work_off_time", "Hour");
            iHour = Int32.Parse(strHourValue);
            strMinuteValue = GetKeyValueString(strIniFileName, "work_off_time", "Minute");
            iMinute = Int32.Parse(strMinuteValue);
            strSecondValue = GetKeyValueString(strIniFileName, "work_off_time", "Second");
            iSecond = Int32.Parse(strSecondValue);



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(DateTime.Now.Hour == iHour && DateTime.Now.Minute == iMinute && DateTime.Now.Second == iSecond)
            {
                label1.Text = "該下班囉~~~~";
            }
        }

        /// <summary>
        /// read write INI file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static string GetKeyValueString(string fileName, string Section, string Key)
        {
            StringBuilder value = new StringBuilder(255);
            bool hasSection = false;

            //開啟IO串流
            StreamReader sr = new StreamReader(fileName, Encoding.UTF8);
            while (true)
            {
                string s = sr.ReadLine();

                //空值或空字串判斷
                if (s == null || s == "")
                {
                    continue;
                }

                //以;或是#開頭作註解的判斷
                if (Regex.Match(s, @"^(;|#).*$").Success)
                {
                    continue;
                }

                //讀取[Section]
                if (Regex.Match(s, @"^\[.*\]").Success)
                {
                    //判斷Section名稱是否符合
                    if (Regex.Match(s, Section).Success)
                    {
                        hasSection = true;
                    }
                }

                //如果Section存在，才去判斷Key
                if (hasSection)
                {
                    string[] KeyValue = s.Split('=');

                    //判斷Key名稱是否符合
                    if (Regex.Match(KeyValue[0].Trim(), Key).Success)
                    {
                        value.Append(KeyValue[1].Trim());
                        break;
                    }
                }
            }

            //關閉IO串流
            sr.Close();

            return value.ToString();
        }

    }
}
