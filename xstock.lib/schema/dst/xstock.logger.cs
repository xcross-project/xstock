using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xstock.lib.schema.dst
{
    internal class xstocklogger
    {
        private const string confpath = "conf";
        private const string confile = "xstock.logger.conf";

        private static IConfiguration __configures;
        private static string __workpath;

        static xstocklogger() {
            __workpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, confpath);
            __configures = new ConfigurationBuilder()
                .SetBasePath(__workpath)
                .AddJsonFile(confile, optional: false, reloadOnChange: true)
                .Build();
        }

        public static bool enable
        => __configures
            .GetSection("enable")
            .Get<bool>();

        public static string storage
        => __configures
            .GetSection("storage")
            .Get<string>();

        public static int buffer
        => __configures
            .GetSection("buffer")
            .Get<int>();

        public static string[] filters
        => __configures
            .GetSection("filters")
            .Get<string[]>();

    }
}
