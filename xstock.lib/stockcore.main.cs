using static xstock.lib.comp.log.logger;

namespace xstock.lib
{
    public partial class stockcore
    {
        public stockcore() => __constructor_stockcore();

        public static string title => $"XStock - {__servname} version {__version}";

        public void start() => __start();
        public void stop() => __stop();

        internal static void log(string intro, string context = null, string tag = null, level level = level.info,
            ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black, bool priority = false, bool primed = false)
            => __log(intro, context, tag, level, foreground, background, priority, primed);
        internal static void log(block block, string tag = null, level level = level.info, bool priority = false, bool primed = false)
            => __log(block, tag, level, priority, primed);
        internal static void log(block[] blocks, string tag = null, level level = level.info, bool priority = false, bool primed = false)
            => __log(blocks, tag, level, priority, primed);
    }
}
