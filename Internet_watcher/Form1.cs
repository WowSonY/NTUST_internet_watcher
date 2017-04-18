using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Web;
using System.IO;
using System.Threading;
using Microsoft.Win32;

namespace Internet_watcher
{
    public partial class Form1 : Form
    {
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        public NotifyIcon notifyIcon = new NotifyIcon();
        public string download , upload , total;
        public string currentTime;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            getData();
            Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
            myTimer.Tick += new EventHandler(getDataAndInitial);
            // Sets the timer interval to 3 min.
            myTimer.Interval = 5*60*1000;
            myTimer.Start();
        }
        protected override void OnResize(EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Initial();
                //通知欄顯示Icon
                if (notifyIcon != null)
                {
                    this.ShowInTaskbar = false;
                    //隱藏程式本身的視窗
                    //this.Visible = false;
                    this.Hide();
                    notifyIcon.Visible = true;
                    notifyIcon.ShowBalloonTip(1000, "監控IP", textBoxIP.Text , ToolTipIcon.Info);
                }
            }
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (notifyIcon != null)
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            }
        }
        
        private void Initial()
        {
            getData();
            currentTime = System.DateTime.Now.ToString();
            //設定通知欄提示的文字
            //notifyIcon.BalloonTipText = "Still running";
            //設定通知欄在滑鼠移至Icon上的要顯示的文字
            notifyIcon.Text = total;
            //notifyIcon.Text = currentTime;
            //決定一個Logo
            notifyIcon.Icon = (System.Drawing.Icon)(Properties.Resources.NTUST_LOGO);
            //設定按下Icon發生的事件
            notifyIcon.Click += (sender, e) => {
                //取消再通知欄顯示Icon
                notifyIcon.Visible = false;
                //顯示在工具列
                this.ShowInTaskbar = true;
                //顯示程式的視窗
                this.Show();
                this.WindowState = FormWindowState.Normal;
            };
            
            //設定右鍵選單
            //宣告一個選單的容器
            ContextMenu contextMenu = new ContextMenu();
            //宣告選單項目
            MenuItem notifyIconMenuItem1 = new MenuItem();
            //可以設定是否可勾選
            //notifyIconMenuItem1.Checked = true;
            //在NotifyIcon中的頁籤，順序用
            notifyIconMenuItem1.Index = 1;
            //設定顯示的文字，後面的(S&)代表使用者按S鍵也可以觸發Click事件!
            notifyIconMenuItem1.Text = "關閉";
            //設定按下後的事情
            notifyIconMenuItem1.Click += (sender, e) => {
                Application.Exit();
            };
            //將MenuItem加入到ContextMenu容器中!
            contextMenu.MenuItems.Add(notifyIconMenuItem1);
            //設定notifyIcon的選單內容等於剛剛宣告的選單容器ContextMen;
            notifyIcon.ContextMenu = contextMenu;

        }
        private void getDataAndInitial(object sender, EventArgs e)
        {
            getData();
            if (notifyIcon != null) {
                notifyIcon.Text = total;
            }
        }
        private void getData()
        {
            try
            {
                CookieContainer cookieContainer = new CookieContainer();

                ///////////////////////////////////////////////////
                // 資料來源 = http://www.cnblogs.com/anjou/archive/2006/12/25/602943.html
                ///////////////////////////////////////////////////                
                // 设置打开页面的参数
                string URI = "https://network.ntust.edu.tw/flowstatistical.aspx";

                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;
                string postString = "__EVENTTARGET=&__EVENTARGUMENT=&__LASTFOCUS=&__VIEWSTATE=84RBIYUJY3i0gKCsLFT8nh1260vT%2BTi7FduSLtwCwPqTSYACkiYvlFjMFyt%2BQXipVtmdmLXdzv8NfY8il2LPMHKNBR1f4d2kxwsiVLbbeTGvC%2BfMTSTdjIb2YrQLOKttytRlPw0QF4iDrdcQnmhNMMGM6eO3uuH8hdR8RwvBfj9yrkM0oL5tERktvtqsnIK8QwBPKqppE412hkQRKSjNngC0NdwT%2Bxi1NRj1dIOBLq2VGg8gjXkcX3SQZV9UaHlJmmzZSQbld5L4W6OV%2F0vos%2F4wYVkMug4DYxZKtPb20kej59itSg6CqEVEYrou2Bf83NFhMRnxiB5M78QfLTOQCpkzQ3PfbTVbEcudJvMXLou3G0EOGhUToeNHTLSPKbYe8RJWr7bGDkYyurQFYrkcW8uDLfnltk9izezTr7S2eE%2B5iSHQAdmXioZ9qQu2c7utG6%2Ftqd%2FXucizTwVXkFNEXp%2B%2FZkfHWvqz9hr1U%2FnPoXov5%2BHt%2F1boL%2F1ZxPNMZPk58uDBmBGVR%2FnSRFBv14SPgpDQfOghZwZ8r1lma%2FYj04KQQeqoPq27FzzH9Uwoia0TzzynP3DMLssFrFSewKjpAmi%2FExa8uNx0R0%2FhYOmzA5shuXnD5p2cOSS9LPHXY10uyVENhnEzk5ycMJ6tX14P7mQD5LQUI59FPRakmdTDxrpj0eLB47x5Wev11h1%2BQlHbBnsxjq1naWvGQ%2BpX8XK4yj4lkqXSPVfgpaWOgsVSZu4vWEC6V%2BX98%2FM%2B2ZZKmdjOzEpZn2x4YnZBUSifR6ZIkZhys5X5uRv8N6z1m5PDQxduygSWiK1D8T3GYJiB21RIn6wU1aHLDX1uH1Y9epAZaZADgYS9%2F6AaZmIc4TCTQIVegzOkfdvOq2y%2BOk%2FW92b9mvhebkMrypgCUKISwSNgR4lM%2BFNv2tPliCqEu1c2ueI77OU0EfCfR0SaJCGg91MMWT0Y%2Bj0lSKPTrE5K26KpRjROoGRHmpHQJKAvy83FrkRt9jb4R89U%2FjrD3iZ7cH7K8AENcMZ5t5N6ojEsEH66W8uiF%2F4AxjqV78HyojeXq2EW3aceOSdCfmtXYC0NgMRywBAqGCgFarey0%2FnfMp1L6OvD%2BFyeZPE3XYu1Hg4c40FGB50UCOJSETPmEptpVQi1&__VIEWSTATEGENERATOR=67237148&__EVENTVALIDATION=lQ0OI8ildgG32DbRWLhhZpcOleLdmvqMC%2BR7%2Bf74ciKXBR7a3%2F%2BNyBnS%2FvS2uolNlhjuT7gbnzSCvTkiZIemd3ihnBcpljXNZk9xvOtlS2VDEfAdmfGpbJnLlaMW1q3GjLoz03rwdNwLuBrJi6lrrG9ENUTT6777jMdAXFDf0DW0M8S5zjMOIWZNijfNkMIuIrYUBJ5iRZNzzzt9VBl5s%2BcTwdp5FA6XAf26xn7rc%2FoUDtPagFVf3H55EYmI%2FEBbKzmtKlwuC9Ioyh5kPQipG2S7wZ8t7xP8q6yr3HuoGPxDH8vINFBXl6XtZygiHmpyd904fOK0c95mFukogNmBjDUhqfi%2FXma3UVai2JFqQLi%2F1VFTcjttKE9RXrgSpr4LXDXQASQX2%2FJTtd3jgIWgjpMKubeW34rjBvyLY0PE4YWkFeX%2BVADPEphpe9lMlmpXLrM9XjWYeLg3TrCadgbPxGRWpIENJjkS8JVzPww0EOvQ803asPijXnA968sNp98capiCtnPNH1ipl8XEV2iWKyC6E8rsPprROlmvtlH%2B05NZw5JYQeNlubSmN7kF54uv9pq1O6sswKVostlhVp8%2BXlPbOjeKyYwiM202A%2BUhHzNoXV369HW3L%2FqllDXXJe%2FK8xTh%2BOduoYdtsbKtSJVFCinM%2FYs1vpRrelr%2FyXhrWKLmQqwOXpTrI5FjkZVtEV1HPkqQbyfVOdZ3NKi5vtqIpEgMe8jl%2FDEhzUtE1A60MgfCY0WI6Be%2F%2B4QtrHifxL9w%2Ba%2BUfUhgPPn9AnMYXYc8YjI3DOAvfHO6z2Ferhlky4m81fALYKGEulobakcZMxz50uzuVA%2Bw1jIivlLAD3RRhJ17zo3MGr5yNKJvshqpEx22Woc6GRbPMy3j5lvk6nWlBFcRQppzJIxI%2FvnB%2BNTwgZq39NIC1%2FMegCy4kCHpCIcSol%2BHtk14uOIpUwlJGaAFWYb6M6ERCHsl86BEveqfScx7vbJjY7bTQS2ASwfo91e6m%2FQZAXRT4IN8IXya4GrK%2BogLFw%2F8rDk%2B%2BAJmCYOP2zwVl5z%2FAac0tHWv9TEeU0cpzAI9RL26Nar6%2BtXIE5IDgfrthvROeW5KGUeyfHqXIQyDwEm%2FtQcCFQiWiGUd47p%2F1rxF1GN0pSU95jasl0fvzlWCqctD2drq1fW%2FuGlhrPPVaCbewxyPUpsAXhypWTFjJf4Re030lRH1n773zQ8M&ctl00%24ContentPlaceHolder1%24txtip=" + textBoxIP.Text + " & ctl00%24ContentPlaceHolder1%24dlyear=" + currentTime.Year.ToString() + "&ctl00%24ContentPlaceHolder1%24dlmonth=" + currentTime.Month.ToString() + "&ctl00%24ContentPlaceHolder1%24dlday=" + currentTime.Day.ToString() + "&ctl00%24ContentPlaceHolder1%24dlcunit=1048576&ctl00%24ContentPlaceHolder1%24btnview=%E6%AA%A2%E8%A6%9624%E5%B0%8F%E6%99%82%E6%B5%81%E9%87%8F";

                // 将提交的字符串数据转换成字节数组
                byte[] postData = Encoding.ASCII.GetBytes(postString);

                // 设置提交的相关参数
                HttpWebRequest request = WebRequest.Create(URI) as HttpWebRequest;
                request.Method = "POST";
                request.KeepAlive = false;
                request.ContentType = "application/x-www-form-urlencoded";
                request.CookieContainer = cookieContainer;
                request.ContentLength = postData.Length;

                // 提交请求数据
                System.IO.Stream outputStream = request.GetRequestStream();
                outputStream.Write(postData, 0, postData.Length);
                outputStream.Close();

                // 接收返回的页面
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                System.IO.Stream responseStream = response.GetResponseStream();
                System.IO.StreamReader reader = new System.IO.StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
                string srcString = reader.ReadToEnd();

                string[] stringSeparators_source = new string[] { "<td>", "</tr>","<table>" };
                string[] result = srcString.Split(stringSeparators_source, StringSplitOptions.RemoveEmptyEntries);
                download = "下載：" + result[3].Replace(" ", "").Replace("</td>", "").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                upload = "上傳：" + result[4].Replace(" ", "").Replace("</td>", "").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                total = "總計：" + result[5].Replace(" ", "").Replace("</td>", "").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                label_download.Text = download;
                label_upload.Text = upload;
                label_total.Text = total;

                string totalNum_s = result[5].Replace(" ", "").Replace("</td>", "").Replace("</tr><tr>", "").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("(M)", "").Replace(",","");
                int totalNum = 0;
                if (totalNum_s != "") {
                    try {
                        totalNum = Int32.Parse(totalNum_s);
                    }
                    catch (Exception e)
                    {

                    }
                }
                if( totalNum > 4500)
                {
                    //notifyIcon.ShowBalloonTip(1000, "流量警告！", "目前流量" + total, ToolTipIcon.Info);
                    MessageBox.Show("目前IP：" + textBoxIP.Text +"\r"+"目前流量" + total, "流量警告！");
                }


            }
            catch (WebException we)
            {
                string msg = we.Message;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.RunWhenOn = checkBox1.Checked;
            Properties.Settings.Default.Save();
            SetAutoRun(Application.ExecutablePath, checkBox1.Checked);
        }
        //reference = http://fanli7.net/a/bianchengyuyan/csharp/2011/0921/129713.html
        public static void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!File.Exists(fileName))
                {
                    throw new Exception("木有這個文件，搞什麼搞");
                }
                string name = fileName.Substring(fileName.LastIndexOf(@"\") + 1);
                reg = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (reg == null)
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                if (isAutoRun)
                {
                    reg.SetValue(name, fileName);
                    MessageBox.Show("下次開機將自動啟動！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    reg.SetValue(name, false);
            }
            catch (Exception ex) { }
            finally
            {
                if (reg != null)
                {
                    reg.Close();
                }
            }
        }
        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            getData();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://network.ntust.edu.tw/");
        }
    }
}
