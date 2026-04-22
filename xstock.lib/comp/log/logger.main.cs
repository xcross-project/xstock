using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xstock.lib.comp.log
{
    internal partial class logger
    {
        public logger() => __constructor_logger();

        public void start() => __start();
        public void stop() => __stop();

        public void log(string intro, string context = const_emptystring, string tag = const_emptystring, level level = level.info, 
            ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black, bool priority = false, bool primed = false)
            => __log(intro, context, tag, level, foreground, background, priority, primed);

        public void log(block block, string tag = const_emptystring, level level = level.info, bool priority = false, bool primed = false)
            => __log(block, tag, level, priority, primed);

        public void log(block[] blocks, string tag = const_emptystring, level level = level.info, bool priority = false, bool primed = false)
            => __log(blocks, tag, level, priority, primed);
    }
}
