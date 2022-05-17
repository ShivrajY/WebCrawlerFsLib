using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using static WebCrawlerFsLib.WebCrawler;

namespace WebCrawler
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void logListControl_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

        }
        static string dirName = "Output";
        string allLinksFile = Path.Combine(dirName,"allLinks.csv");
        string goodLinksFile = Path.Combine(dirName,"goodLinks.csv");
        string badLinksFile = Path.Combine(dirName,"badLinks.csv");
        private void MainForm_Load(object sender, EventArgs e)
        {
            Directory.CreateDirectory(dirName);

        }
        CancellationTokenSource cts = null;

        Crawler crawler = null;

        private void DisposeCrawler()
        {
            if (crawler != null)
            {
                ((IDisposable)crawler).Dispose();
            }
        }
        private async void startButton_Click(object sender, EventArgs e)
        {
            if (!Uri.TryCreate(urlTextBox.Text, UriKind.Absolute , out Uri uri))
            {
                MessageBox.Show("Enter Vaild Url");
                return;
            }
            startButton.Enabled = false;

            DisposeCrawler();

            cts = new CancellationTokenSource();
            
            crawler = new Crawler(uri,allLinksFile ,goodLinksFile, badLinksFile, cts.Token, FuncConvert.FromAction<LogData>(Log));
            await crawler.Start();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if(startButton.Enabled == true) return;

            if(crawler!=null)
            {
                cts.Cancel();
                cts = null;
                startButton.Enabled = true;
                stopButton.Enabled = false;
            }
        }
        public void Log(LogData data)
        {
            if(InvokeRequired)
            {
                var x = data;
                _ = BeginInvoke(() => { 
                      logListControl.Items.Add(x.Message);
                      totalLinksLabel.Text = x.LinksInQueue.ToString();
                      checkedLinksLabel.Text = x.CheckedLinks.ToString();
                      logListControl.ScrollToItem(logListControl.Items.Last());
                      if(logListControl.Items.Count > 10000)
                      {
                        logListControl.Items.Clear();
                      }
                    });
            }
            else
            {
                logListControl.Items.Add(data.Message);
                totalLinksLabel.Text = data.LinksInQueue.ToString();
                checkedLinksLabel.Text = data.CheckedLinks.ToString();
            }
        }

        private void openFolderButton_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", dirName);
        }
    }
}
