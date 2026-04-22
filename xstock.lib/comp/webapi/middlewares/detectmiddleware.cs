using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xblock = xstock.lib.comp.log.logger.block;

namespace xstock.lib.comp.webapi.middlewares
{

    internal class detectmiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (schema.dst.xstockconf.remotedetect)
            {
                stockcore.log(new[] {
                    new xblock("path: "),
                    new xblock("[", foreground: ConsoleColor.Black, background: ConsoleColor.Yellow),
                    new xblock($"{context.Request.Path}", foreground: ConsoleColor.White, background: ConsoleColor.Green),
                    new xblock("]", foreground: ConsoleColor.Black, background: ConsoleColor.Yellow),
                    new xblock($" requested.\n{$"user agent: {context.Request.Headers["user-agent"]}"}")
                }, level: log.logger.level.trace, priority: true);
            }

            await next(context);
        }
    }
}
