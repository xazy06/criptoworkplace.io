using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ExchangerMonitor
{
    public class JsonModel
    {
        [JsonProperty(PropertyName = "exchanger")]
        public JsonItemModel Exchanger { get; set; }
        [JsonProperty(PropertyName = "sale")]
        public JsonItemModel Sale { get; set; }
        [JsonProperty(PropertyName = "token")]
        public JsonItemModel Token { get; set; }
    }

    public class JsonItemModel
    {
        [JsonProperty(PropertyName = "bytecode")]
        public BytecodeModel Bytecode { get; set; }
        [JsonProperty(PropertyName = "abi")]
        public dynamic ABI { get; set; }
    }

    public class BytecodeModel
    {
        [JsonProperty(PropertyName = "linkReferences")]
        public object LinkReferences { get; set; }
        [JsonProperty(PropertyName = "object")]
        public string Object { get; set; }
        [JsonProperty(PropertyName = "opcodes")]
        public string OpCodes { get; set; }
        [JsonProperty(PropertyName = "sourceMap")]
        public string SourceMap { get; set; }

    }
}
