using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xstock.lib
{
    public partial class stockcore
    {
        private static readonly string __servname = "X-Stock Core Service";
        private static readonly string __version = "1.0.0.0";
        private bool __status = false;

        private const string creator0 = "creator0";

        private static stockcore __singleton;
        private comp.log.logger __cmp_logger;
        private comp.webapi.webapiserv __cmp_webapiserv;
    }
}
