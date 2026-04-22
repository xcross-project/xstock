using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xstock.lib.schema.dst
{

    internal class xstockwebapi
    {
        private const string confpath = "conf";
        private const string confile = "xstock.webapi.conf";

        private static IConfiguration __configures;
        private static string __workpath;

        static xstockwebapi()
        {
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
        public static class settings
        {

            public static string bindurls
            => __configures
                .GetSection("settings:bindurls")
                .Get<string>();

            public static bool staticenable
            => __configures
                .GetSection("settings:openapi:staticenable")
                .Get<bool>();
            public static string staticroute
            => __configures
                .GetSection("settings:openapi:staticroute")
                .Get<string>();

            public static class openapi
            {
                public static bool swaggerswitch
                => __configures
                    .GetSection("settings:openapi:swaggerswitch")
                    .Get<bool>();

                public static bool showlogging
                => __configures
                    .GetSection("settings:openapi:showlogging")
                    .Get<bool>();


                public class document
                {
                    public string name { get; set; }
                    public string version { get; set; }
                    public string title { get; set; }
                    public string description { get; set; }
                    public string termsofservice { get; set; }
                }
                public class securitydefinition
                {
                    public string key { get; set; }
                    public string description { get; set; }
                    public string name { get; set; }
                    public string bearerformat { get; set; }
                    public string scheme { get; set; }
                }



                public static document[] documents
                => __configures
                    .GetSection("settings:openapi:documents")
                    .Get<document[]>();

                public static string injectionxmlcommentsfile
                => __configures
                    .GetSection("settings:openapi:injectionxmlcommentsfile")
                    .Get<string>();

                public static securitydefinition[] securitydefinitions
                => __configures
                    .GetSection("settings:openapi:securitydefinitions")
                    .Get<securitydefinition[]>();
            }

            public class cor
            {
                public string policy { get; set; }
                public string[] trustorigins { get; set; }

                public int expiresdays { get; set; }
                public string secretkey { get; set; }
                public string tokenissuer { get; set; }
                public string tokenaudience { get; set; }
            }

            public static cor[] cors
                => __configures.GetSection("settings:cors").Get<cor[]>();
        }
    }
}
