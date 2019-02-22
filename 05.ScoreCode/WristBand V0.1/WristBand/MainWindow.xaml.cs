using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utility.Tool.Controls;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受   
        }

        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, Encoding charset)
        {
            HttpWebRequest request = null;
            //HTTPSQ请求
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            request = WebRequest.Create(url) as HttpWebRequest;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = DefaultUserAgent;
            //如果需要POST数据   
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                    }
                    i++;
                }
                byte[] data = charset.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return request.GetResponse() as HttpWebResponse;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://cpc.aecg.com.cn/TimeCollectorOpenAPI/TagActiveLog/UpdateTagActiveDateTime";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("TagCode", this.No.Text);
            parameters.Add("UpdateStateTime", dateTimePicker1.DateTime.ToString());
            parameters.Add("State", "1"); ;
            HttpWebResponse response = MainWindow.CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值
            Stream stream = response.GetResponseStream();   //获取响应的字符串流
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html
            if(Convert.ToString(html[8]) == "0")
            {
                this.display.Text = "激活成功";
            }
            else
            {
                this.display.Text = "激活失败";
            }
            Console.WriteLine(html);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://cpc.aecg.com.cn/TimeCollectorOpenAPI/TagActiveLog/UpdateTagActiveDateTime";
            Encoding encoding = Encoding.GetEncoding("utf-8");
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("TagCode", this.No.Text);
            parameters.Add("UpdateStateTime", dateTimePicker1.DateTime.ToString());
            parameters.Add("State", "2"); ;
            HttpWebResponse response = MainWindow.CreatePostHttpResponse(url, parameters, encoding);
            //打印返回值
            Stream stream = response.GetResponseStream();   //获取响应的字符串流
            StreamReader sr = new StreamReader(stream); //创建一个stream读取流
            string html = sr.ReadToEnd();   //从头读到尾，放到字符串html
            if (Convert.ToString(html[8]) == "0")
            {
                this.display.Text = "取消激活成功";
            }
            else
            {
                this.display.Text = "取消激活失败";
            }
            Console.WriteLine(html);
        }
    }
}
