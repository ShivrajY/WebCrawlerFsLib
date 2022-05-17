using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WebCrawlerLib;
namespace WebCrawlerLinkChecker_V._2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if(!(Uri.TryCreate(startUrlTextBox.Text, UriKind.Absolute, out Uri uri)))
            {

            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {

        }
    }
}
