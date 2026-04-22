using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static xstock.lib.comp.log.logger;
using xlogger = xstock.lib.comp.log.logger;

namespace xstock.lib
{
    public partial class stockcore
    {
        private void __constructor_stockcore()
        {
            __singleton = this;

        }
        private void __start()
        {
            if (__status) return;

            __cmp_logger = new xlogger();
            __cmp_logger.start();

            __cmp_logger.log(new xlogger.block[] {
                new xlogger.block(" XPay ", foreground: ConsoleColor.Black, background: ConsoleColor.Cyan),
                new xlogger.block($" - "),
                new xlogger.block(__servname, foreground: ConsoleColor.Cyan),
                new xlogger.block($" version "),
                new xlogger.block($"{__version}\n", foreground: ConsoleColor.Green)
            }, primed: true);


            __cmp_webapiserv = new comp.webapi.webapiserv();
            __cmp_webapiserv.start();
        }
        private async void __stop()
        {
            if (!__status) return;

            __cmp_webapiserv.stop();
        }

        #region log
        private static void __log(string intro, string context = null, string tag = null, level level = level.info,
            ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black, bool priority = false, bool primed = false)
            => __singleton?.__cmp_logger.log(intro, context, tag, level, foreground, background, priority, primed);
        private static void __log(block block, string tag = null, level level = level.info, bool priority = false, bool primed = false)
            => __singleton?.__cmp_logger.log(block, tag, level, priority, primed);
        private static void __log(block[] blocks, string tag = null, level level = level.info, bool priority = false, bool primed = false)
            => __singleton?.__cmp_logger.log(blocks, tag, level, priority, primed);
        #endregion
    }
}
