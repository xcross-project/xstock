using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace xstock.lib.comp.webapi
{
    internal partial class webapiserv
    {
        public webapiserv() => __constructor_webapiserv();
        public bool status => __status;

        public void start() => __start();
        public async void stop() => __stop();
    }
}
