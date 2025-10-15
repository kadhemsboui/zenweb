using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_OrderRequest1
    {
        [DataMember(EmitDefaultValue = false)] public List<string> references { get; set; }
        [DataMember(EmitDefaultValue = false)] public string IdCompany { get; set; }
        [DataMember(EmitDefaultValue = false)] public string updatedstart { get; set; }
        [DataMember(EmitDefaultValue = false)] public string updatedend { get; set; }


    }
}