using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_StockrRequest
    {
        [DataMember(EmitDefaultValue = false)] public List<string> Stores { get; set; }
        [DataMember(EmitDefaultValue = false)] public string IdCompany { get; set; }
        [DataMember(EmitDefaultValue = false)] public string updatedstart { get; set; }
        [DataMember(EmitDefaultValue = false)] public string updatedend { get; set; }
        [DataMember(EmitDefaultValue = false)] public List<string> barreCodes { get; set; }
    }
}