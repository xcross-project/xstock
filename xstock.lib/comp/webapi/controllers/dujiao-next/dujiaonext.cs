using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using xstock.lib.comm;
using xlogblock = xstock.lib.comp.log.logger.block;

namespace xstock.lib.comp.webapi.controllers.dujiao_next
{
    [ApiController]
    [Route("dujiao-next/api/v1/upstream")]
    public class dujiaonext : ControllerBase
    {
        private const string __empty = "";

        [AcceptVerbs("GET", "POST")]
        [Route(__empty)]
        public string upstream() => ping();

        [HttpPost]
        [Route("ping")]
        public string ping() => JsonSerializer.Serialize(new
        {
            ok = true,
            site_name = "xstock",
            protocol_version = "1.0",
            user_id = 1,
            balance = "10000.00",
            currency = "CNY",
            member_level = new
            {
                id = 1,
                name = new {
                    zhcn = "VIP",
                    en = "vip"
                },
                slug = "vip",
                icon = ""
            }
        });

        [HttpGet]
        [Route("categories")]
        public string categories() => JsonSerializer.Serialize(new { 
            ok = true,
            categories = new[] {
                new { 
                    id = 1,
                    parent_id = 0,
                    slug = "google",
                    name = new { en = "gmail" },
                    icon = "",
                    sort_order = 0
                }
            }
        });

        [HttpGet]
        [Route("products")]
        public string productslist() => JsonSerializer.Serialize(new { 
            ok = true,
            items = new[] { 
              new { 
                id = 1,
                slug = "gmail",
                title = new { en = "gmail" },
                description = new { en = "this is product gmail" },
                images = new [] {
                    "https://goblin.express/uploads/product/2026/04/6fdc5bc2-d1d5-4ff2-9385-6ccdfade4cbb.webp"
                },
                tags = new [] { "hot" },
                price_amount = "7.90",
                original_price = "9.90",
                member_price = "7.90",
                fulfillment_type = "auto",
                is_active = true,
                category_id = 1,
                skus = new [] {
                    new {
                        id = 1,
                        sku_code = "GMAIL",
                        price_amount = "7.90",
                        original_price = "9.9",
                        member_price = "7.90",
                        stock_status = "in_stock",
                        stock_quantity = 10000,
                        is_active = true,
                    },
                },
                created_at = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ"),
                updated_at = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ"),
              },
            },
            total = 1,
            page = 1,
            page_size = 20,
        });

        [HttpGet]
        [Route("products/{id}")]
        public string producbyid(string id) => JsonSerializer.Serialize(new { 
            ok = true,
            product = new {
                id = id,
                slug = "gmail",
                title = new { en = "gmail" },
                description = new { en = "this is product gmail" },
                images = new[] {
                    "https://goblin.express/uploads/product/2026/04/6fdc5bc2-d1d5-4ff2-9385-6ccdfade4cbb.webp"
                },
                tags = new[] { "hot" },
                price_amount = "7.90",
                original_price = "9.90",
                member_price = "7.90",
                fulfillment_type = "auto",
                is_active = true,
                category_id = 1,
                skus = new [] {
                    new {
                        id = 1,
                        sku_code = "GMAIL",
                        price_amount = "7.90",
                        original_price = "9.9",
                        member_price = "7.90",
                        stock_status = "in_stock",
                        stock_quantity = 10000,
                        is_active = true,
                    },
                },
                created_at = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ"),
                updated_at = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ"),
            }
        });

        [HttpPost]
        [Route("orders")]
        public string orderslist() => JsonSerializer.Serialize(new { 
            ok = true,
            order_id = $"DJ{DateTime.Now.ToString("yyyyMMddHHmmssff")}CD",
            status = "paid",
            amount = "9.90",
            currency = "CNY",
        });

        [HttpGet]
        [Route("orders/{id}")]
        public string ordersbyid(string id)
        {
            ThreadPool.QueueUserWorkItem(o => {
                const string __ahost = "https://goblin.express";
                const string __apath = "/api/v1/upstream/callback";
                const string __apikey = "66459cf2c7bd6123e89819a1c35f425f932615a2659d74440ce24913eb200941";
                const string __apisecret = "e57de940bae0d710d26ca9d4a70a3996f68b8079c461408b7b267cc5582fc67ae75f6f54dc5103ba88f36c24db0f0c6cc9fe8d02f7291e4253baa0fc4d38b08c";

                Thread.Sleep(0x3e8 * 0x0a);
                string __timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                string __body = JsonSerializer.Serialize(new {
                    order_id = 1,
                    order_no = $"DJ{DateTime.Now.ToString("yyyyMMddHHmmssff")}CD",
                    downstream_order_no = o as string,
                    status = "completed",
                    fulfillment = new {
                        type = "auto",
                        status = "delivered",
                        payload = "DianaBattagliaomldh@centcol.us|Phat3479\r\nLanniVujcrtn@centcol.us|Phat3479",
                        delivered_at = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    },
                    timestamp = __timestamp,
                });
                string __sign = __signature("POST",
                    __apath,
                    __timestamp, __body, __apisecret);
                bool __httpret = false;
                string __response = http.request(new http.requestparam(
                    $"{__ahost}{__apath}", 
                    HttpMethod.Post, __body, new Dictionary<string, string> {
                        { "Dujiao-Next-Api-Key", __apikey },
                        { "Dujiao-Next-Timestamp", __timestamp },
                        { "Dujiao-Next-Signature", __sign },
                    },
                    http.requestparam.contenttype_application_json), out __httpret);
                stockcore.log(new[] { 
                    new xlogblock("dujiao-next callback to ", foreground: ConsoleColor.Yellow),
                    new xlogblock($"{__ahost}{__apath}", foreground: ConsoleColor.Cyan),
                    new xlogblock($" with body:\n{__body}", foreground: ConsoleColor.Yellow),
                });
            }, id);
            return JsonSerializer.Serialize(new
            {
                ok = true,
                order_id = 101,
                order_no = $"DJ{DateTime.Now.ToString("yyyyMMddHHmmssff")}CD",
                status = "completed",
                amount = "9.90",
                currency = "CNY",
                items = new[] {
                new {
                    product_id = 1,
                    sku_id = 1,
                    title = new { en = "gmail" },
                    quantity = 1,
                    unit_price = "9.90",
                    total_price = "9.90",
                    fulfillment_type = "auto",
                }
            },
                fulfillment = new
                {
                    type = "auto",
                    status = "delivered",
                    payload = "ABCD-EFGH-1234-5678",
                    delivered_at = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ"),
                }
            });
        }

        [HttpPost]
        [Route("orders/{id}/cancel")]
        public string ordercancel() => JsonSerializer.Serialize(new { 
            ok = true,
            order_id = 1,
            order_no = $"DJ{DateTime.Now.ToString("yyyyMMddHHmmssff")}CD",
            status = "canceled",
        });


        #region

        private string __signature(string method, string path, string timestamp, string content, string apisecret)
            => comm.common.hmacksha256(
                $"{method}\n{path}\n{timestamp}\n{comm.common.md5crypto(content)}", 
                common.stringtohex(apisecret)).ToLower();


        #endregion
    }
}
