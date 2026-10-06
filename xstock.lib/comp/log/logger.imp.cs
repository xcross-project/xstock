using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace xstock.lib.comp.log
{
    internal partial class logger
    {

        
        private void __constructor_logger() {
            __status = false;
            __con_logsqueue_priority = new ConcurrentQueue<block[]>();
            __con_logsqueue = new ConcurrentQueue<block[]>();
        }
        private void __start() {
            if (__status) return;
            __status = true;


            (__thd_logprocessing = new Thread(
                new ThreadStart(__thdmtd_logprocessing))
            { IsBackground = true }).Start();
        }

        private void __stop()
        {
            if (!__status) return;
            __status = false;
            try {
                __thd_logprocessing.Join();
            } catch { }
            __thd_logprocessing = null;
        }


        private void __log(string intro, string context = const_emptystring, string tag = const_emptystring, level level = level.info,
            ConsoleColor foreground = ConsoleColor.White, ConsoleColor background = ConsoleColor.Black, bool priority = false, bool primed = false) {
            block[] __enqueueblocks = new block[] {
                new block(intro, context, tag, level, foreground, background, primed)
            };
            __log(__enqueueblocks, tag, level, priority, primed);
        }


        private void __log(block block, string tag = const_emptystring, level level = level.info, bool priority = false, bool primed = false)
            => __log(new block[] { block }, tag, level, priority, primed);

        private void __log(block[] blocks, string tag = const_emptystring, level level = level.info, bool priority = false, bool primed = false) {
            for(var i = 0x00; i < blocks.Length; i++) {
                blocks[i].tag = tag;
                blocks[i].level = level;
                blocks[i].primed = primed;
                blocks[i].timestamp = DateTime.Now;
            }
            if (priority)
                __con_logsqueue_priority.Enqueue(blocks);
            else
                __con_logsqueue.Enqueue(blocks);
        }

        private void __thdmtd_logprocessing()
        {
            StringBuilder __logbuilder = new StringBuilder();
            while (__status) {
                if (!__con_logsqueue_priority.IsEmpty || !__con_logsqueue.IsEmpty) {
                    block[] __dequeueblocks = null;
                    bool __priority = false;
                    if (!__con_logsqueue_priority.IsEmpty)
                    {
                        __priority = true;
                        __dequeueblocks = __con_logsqueue_priority.TryDequeue(out __dequeueblocks) ? __dequeueblocks : null;
                    } else if (!__con_logsqueue.IsEmpty)
                        __dequeueblocks = __con_logsqueue.TryDequeue(out __dequeueblocks) ? __dequeueblocks : null;

                    if(null != __dequeueblocks) {
                        Console.ForegroundColor = const_default_foreground;
                        Console.BackgroundColor = const_default_background;
                        if (__priority) { 
                            __colorwrite(" PRIORITY ", ConsoleColor.Black, ConsoleColor.Red);
                            __logbuilder.Append("[PRIORITY]");
                        }
                        if (!__dequeueblocks[0x00].primed) {
                            __colorwrite("[");
                            __colorwrite(__dequeueblocks[0x00].timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff"), ConsoleColor.Yellow);
                            __colorwrite("]");
                            __logbuilder.Append($"[{__dequeueblocks[0x00].timestamp:yyyy-MM-dd HH:mm:ss.fff}]");

                            switch (__dequeueblocks[0x00].level) {
                                case level.trace:
                                    __colorwrite($" {level.trace.ToString().ToUpper()} ", ConsoleColor.Black, ConsoleColor.Blue);
                                    __logbuilder.Append($"[{level.trace.ToString().ToUpper()}]");
                                    break;
                                case level.debug:
                                    __colorwrite($" {level.debug.ToString().ToUpper()} ", ConsoleColor.Black, ConsoleColor.Cyan);
                                    __logbuilder.Append($"[{level.debug.ToString().ToUpper()}]");
                                    break;
                                    break;
                                case level.warn:
                                    __colorwrite($" {level.warn.ToString().ToUpper()} ", ConsoleColor.Black, ConsoleColor.Yellow);
                                    __logbuilder.Append($"[{level.warn.ToString().ToUpper()}]");
                                    break;
                                case level.error:
                                    __colorwrite($" {level.error.ToString().ToUpper()} ", ConsoleColor.Black, ConsoleColor.Red);
                                    __logbuilder.Append($"[{level.error.ToString().ToUpper()}]");
                                    break;
                                case level.fatal:
                                    __colorwrite($" {level.fatal.ToString().ToUpper()} ", ConsoleColor.Black, ConsoleColor.Magenta);
                                    __logbuilder.Append($"[{level.fatal.ToString().ToUpper()}]");
                                    break;
                                case level.info:
                                default:
                                    __colorwrite($" {level.info.ToString().ToUpper()} ", ConsoleColor.Black, ConsoleColor.White);
                                    __logbuilder.Append($"[{level.info.ToString().ToUpper()}]");
                                    break;
                            }
                            if (!string.IsNullOrEmpty(__dequeueblocks[0x00].tag))
                            {
                                __colorwrite($" {__dequeueblocks[0x00].tag} ", ConsoleColor.Black, ConsoleColor.White);
                                __logbuilder.Append($"[{__dequeueblocks[0x00].tag}]");
                            }
                        }
                        __colorwrite(__dequeueblocks[0x00].primed ? string.Empty : " ");
                        for (int i = 0x00; i < __dequeueblocks.Length; i++) {
                            var __block = __dequeueblocks[i];
                            string __intro = $"{__block.intro}{(i < __dequeueblocks.Length - 0x01 ? string.Empty : "\n")}";
                            __colorwrite(__intro, __block.foreground, __block.background);
                            __logbuilder.Append(__intro);
                        }
                        if (__dequeueblocks.Length == 0x01 && !string.IsNullOrEmpty(__dequeueblocks[0x00].context)) {
                            __colorwrite($"{__dequeueblocks[0x00].context}\n");
                            __logbuilder.Append($"{__dequeueblocks[0x00].context}\n");
                        }

                        if (__logbuilder.Length >= schema.dst.xstocklogger.buffer) { 
                            File.AppendAllText(schema.dst.xstocklogger.storage, __logbuilder.ToString(), Encoding.UTF8);
                            __logbuilder.Clear();
                        }
                    }
                } else Thread.Sleep(const_sleepinterval);
            }

            __stop();
        }

        private void __colorwrite(string text, ConsoleColor foreground = const_default_foreground, ConsoleColor background = const_default_background) {
            if (
                true
                //OperatingSystem.IsWindows() //该方法已兼容linux环境，无需特别拼接echo
                ) {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                Console.Write(text);
                Console.ForegroundColor = const_default_foreground;
                Console.BackgroundColor = const_default_background;
            }
            else if(OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
            {
                StringBuilder __textbuilder = new StringBuilder();
                switch (foreground)
                {
                    case ConsoleColor.Black:
                        __textbuilder.Append($"\\033[{(int)forecolor.black}");
                        break;
                    case ConsoleColor.Red:
                    case ConsoleColor.DarkRed:
                        __textbuilder.Append($"\\033[{(int)forecolor.red}");
                        break;
                    case ConsoleColor.Green:
                    case ConsoleColor.DarkGreen:
                        __textbuilder.Append($"\\033[{(int)forecolor.green}");
                        break;
                    case ConsoleColor.Yellow:
                    case ConsoleColor.DarkYellow:
                        __textbuilder.Append($"\\033[{(int)forecolor.yellow}");
                        break;
                    case ConsoleColor.Blue:
                    case ConsoleColor.DarkBlue:
                        __textbuilder.Append($"\\033[{(int)forecolor.blue}");
                        break;
                    case ConsoleColor.Magenta:
                    case ConsoleColor.DarkMagenta:
                        __textbuilder.Append($"\\033[{(int)forecolor.purple}");
                        break;
                    case ConsoleColor.Cyan:
                    case ConsoleColor.DarkCyan:
                        __textbuilder.Append($"\\033[{(int)forecolor.cyan}");
                        break;
                    case ConsoleColor.White:
                    case ConsoleColor.Gray:
                    case ConsoleColor.DarkGray:
                        __textbuilder.Append($"\\033[{(int)forecolor.white}");
                        break;
                    default:
                        __textbuilder.Append($"\\033[{(int)forecolor._default}");
                        break;
                }
                __textbuilder.Append(";");
                switch (background)
                {
                    case ConsoleColor.Black:
                        __textbuilder.Append($"{(int)backcolor.black}m");
                        break;
                    case ConsoleColor.Red:
                    case ConsoleColor.DarkRed:
                        __textbuilder.Append($"{(int)backcolor.red}m");
                        break;
                    case ConsoleColor.Green:
                    case ConsoleColor.DarkGreen:
                        __textbuilder.Append($"{(int)backcolor.green}m");
                        break;
                    case ConsoleColor.Yellow:
                    case ConsoleColor.DarkYellow:
                        __textbuilder.Append($"{(int)backcolor.yellow}m");
                        break;
                    case ConsoleColor.Blue:
                    case ConsoleColor.DarkBlue:
                        __textbuilder.Append($"{(int)backcolor.blue}m");
                        break;
                    case ConsoleColor.Magenta:
                    case ConsoleColor.DarkMagenta:
                        __textbuilder.Append($"{(int)backcolor.purple}m");
                        break;
                    case ConsoleColor.Cyan:
                    case ConsoleColor.DarkCyan:
                        __textbuilder.Append($"{(int)backcolor.cyan}m");
                        break;
                    case ConsoleColor.White:
                    case ConsoleColor.Gray:
                    case ConsoleColor.DarkGray:
                        __textbuilder.Append($"{(int)backcolor.white}m");
                        break;
                    default:
                        __textbuilder.Append($"{(int)backcolor._default}m");
                        break;
                }
                __textbuilder.Append(text);
                __textbuilder.Append(const_colorend);

                var __echoproc = Process.Start(new ProcessStartInfo() {
                    FileName = "/usr/bin/echo",
                    Arguments = $"-e \"{Regex.Replace(__textbuilder.ToString(), "\"", "\\\"")}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    //RedirectStandardOutput = true,
                    RedirectStandardError = true,
                });
                __echoproc.WaitForExit();
            }
        }
    }
}
