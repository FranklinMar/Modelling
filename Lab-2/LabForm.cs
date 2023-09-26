using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using Microsoft.Web.WebView2.Core;

namespace Lab_2
{
    public partial class LabForm : Form
    {
        bool Ensure = false;
        // string Filename;
        string StringHTML;
        public LabForm(string HTMLstring)
        {
            StringHTML = HTMLstring;
            InitializeComponent();
            // AllocConsole();
            webView21.EnsureCoreWebView2Async();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void webView21_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            Ensure = true;
            // string myFile = Directory.GetCurrentDirectory() + "\\..\\..\\Chart.html";
            /*string applicationDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            string myFile = Path.Combine(applicationDirectory, Filename);//"..\\..\\Chart.html");

            // webView21.CoreWebView2.Navigate("https://www.google.com");
            string URL = Uri.EscapeUriString(myFile.Replace('\\', '/'));
            // Console.WriteLine($"file:///{URL}");
            // Console.WriteLine("file:///D:/Programs/VS%20Project/MathModeling/Chart.html");
            webView21.CoreWebView2.Navigate($"file:///{URL}");//"file:///D:/Programs/VS%20Project/MathModeling/Chart.html");*/
            webView21.NavigateToString(StringHTML);
        }

        private void webView21_Click(object sender, EventArgs e)
        {

        }

        private void LabForm_Load(object sender, EventArgs e)
        {
            //this.webBrowser1.Url = new Uri("https://www.google.com");
            //this.webBrowser1.AllowWebBrowserDrop = false;
            //this.webBrowser1.Navigate("https://www.google.com");
        }
    }
}
