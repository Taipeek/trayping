using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrayPing
{
    class TrayPingHost
    {
        public readonly String address;
        private int timesFail = 0;
        public readonly int timeout = 1000;
        public readonly int timesFailThreshold = 3;

        public TrayPingHost(string address, int timesFail, int timeout, int timesFailThreshold)
        {
            this.address = address;
            this.timesFail = timesFail;
            this.timeout = timeout;
            this.timesFailThreshold = timesFailThreshold;
        }

        public TrayPingHost(string address)
        {
            this.address = address ?? throw new ArgumentNullException(nameof(address));
        }

        public void resetFails()
        {
            this.timesFail = 0;
        }

        public int getFailsCount()
        {
            return this.timesFail;
        }

        public void fail()
        {
            this.timesFail++;
        }
    }
}
