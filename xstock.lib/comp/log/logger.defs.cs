using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace xstock.lib.comp.log
{
    internal partial class logger
    {
        private const string const_emptystring = "";
        private const string const_colorend = "\\033[0m\\c";
        private const int const_sleepinterval = 0x3e8;
        private const ConsoleColor const_default_foreground = ConsoleColor.White;
        private const ConsoleColor const_default_background = ConsoleColor.Black;

        private enum forecolor
        {
            black = 0x1e,
            red = 0x1f,
            green = 0x20,
            yellow = 0x21,
            blue = 0x22,
            purple = 0x23,
            cyan = 0x24,
            white = 0x25,
            _default = 0x27,
        }
        private enum backcolor { 
            black = 0x28,
            red = 0x29,
            green = 0x2a,
            yellow = 0x2b,
            blue = 0x2c,
            purple = 0x2d,
            cyan = 0x2e,
            white = 0x2f,
            _default = 0x31
        }

        public enum level {
            trace = 0x00,
            debug = 0x01,
            info = 0x02,
            warn = 0x03,
            error = 0x04,
            fatal = 0x05,
        }

        internal class block
        {
            public block(string intro, string context = const_emptystring, string tag = const_emptystring, level level = level.info, 
                ConsoleColor foreground = const_default_foreground, ConsoleColor background = const_default_background, bool primed = false) {
                this.intro = intro;
                if (!string.IsNullOrEmpty(context)) this.context = context;
                if (!string.IsNullOrEmpty(tag)) this.tag = tag;
                this.level = level;
                this.foreground = foreground;
                this.background = background;
                this.primed = primed;
            }
            public string intro { get; set; }
            public string context { get; set; } = string.Empty;
            public string tag { get; set; } = string.Empty;

            public level level { get; set; }
            public ConsoleColor foreground { get; set; }
            public ConsoleColor background { get; set; }
            public DateTime timestamp { get; set; } = DateTime.Now;
            public bool primed { get; set; } = false;
        }

        private bool __status;

        private ConcurrentQueue<block[]> __con_logsqueue;
        private ConcurrentQueue<block[]> __con_logsqueue_priority;

        private Thread __thd_logprocessing;
    }
}
