using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrayPing
{
    public partial class Form1 : Form
    {
        private List<TrayPingHost> hosts = new List<TrayPingHost> { new TrayPingHost("8.8.8.8") };
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            Show();
            Hide();
            runHostChecks();
            Hide();
            Hide();
            Hide();
            Hide();
        }
        private bool allowVisible = false;
        protected override void SetVisibleCore(bool value)
        {
            if (!allowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        private async void runHostChecks()
        {
            new Thread(() =>
            {
                Ping myPing = new Ping();
                while (true)
                {
                    foreach (TrayPingHost host in hosts)
                    {
                        try
                        {
                            PingReply reply = myPing.Send(host.address, host.timeout);
                            if (reply != null)
                            {
                                if (reply.Status == IPStatus.Success)
                                {
                                    if (host.getFailsCount() > host.timesFailThreshold)
                                        notifyIcon1.ShowBalloonTip(1000, host.address + " is up", "The host " + host.address + " is up again.", ToolTipIcon.Info);
                                    host.resetFails();
                                }
                                else
                                {
                                    host.fail();
                                    if (host.getFailsCount() == host.timesFailThreshold)
                                        notifyIcon1.ShowBalloonTip(1000, host.address + " is down", "The host " + host.address + " is down: " + reply.Status.ToString(), ToolTipIcon.Warning);
                                }
                            }
                        }catch(PingException e)
                        {

                        }
                    }
                }
            }).Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.ShowBalloonTip(1000, "Test", "This is just a test", ToolTipIcon.Info);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.allowVisible = true;
            Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
