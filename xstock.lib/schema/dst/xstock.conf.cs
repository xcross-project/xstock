using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xstock.lib.schema.dst
{
    internal class xstockconf
    {
        private const string confpath = "conf";
        private const string confile = "xstock.conf";

        private static IConfiguration __configures;
        private static string __workpath;

        static xstockconf()
        {
            __workpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, confpath);
            __configures = new ConfigurationBuilder()
                .SetBasePath(__workpath)
                .AddJsonFile(confile, optional: false, reloadOnChange: true)
                .Build();
        }

        public static string storage
        => __configures
            .GetSection("storage")
            .Get<string>();

        public static bool remotedetect
        => __configures
            .GetSection("remotedetect")
            .Get<bool>();

        public static string[] domains
        => __configures
            .GetSection("domains")
            .Get<string[]>();
    }
}
