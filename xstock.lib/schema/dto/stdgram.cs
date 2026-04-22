using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace xstock.lib.schema.dto
{
    public class stdgram<T>
    {
        internal const string const_emptystring = "";

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? id { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? sign { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? code { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public T? data { get; set; }
        public long stamp { get; set; }

        #region public method implements
        public stdgram(T data, string? sign = const_emptystring, int? code = null, string id = null, long timestamp = 0x00)
        { __constructor_stdgram(data, sign, code, id, timestamp); }

        public static stdgram<T> parse(string json) { return __stdgram_parse<T>(json); }

        public string tostring() { return __stdgram_tostring(); }

        #endregion 

        #region private method implements
        private void __constructor_stdgram(T? data, string? sign, int? code, string id = null, long timestamp = 0x00)
        {
            if (null != data) this.data = data;
            if (!string.IsNullOrEmpty(sign)) this.sign = sign;
            if (code.HasValue) this.code = code.Value;
            if (string.IsNullOrEmpty(id)) this.id = Guid.NewGuid().ToString("N");
            this.stamp = timestamp != 0x00 ? comm.common.datatimetounixtimestamp(DateTime.Now) : timestamp;
        }

        private static stdgram<T> __stdgram_parse<T>(string json)
        {
            stdgram<T> __stdgram = null;
            if (!string.IsNullOrEmpty(json))
                try { __stdgram = JsonSerializer.Deserialize<stdgram<T>>(json); }
                catch (Exception ex) { }
            return __stdgram;
        }

        private string __stdgram_tostring()
           => JsonSerializer.Serialize(this, typeof(stdgram<T>),
                new JsonSerializerOptions {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });
        

        #endregion

    }
}
