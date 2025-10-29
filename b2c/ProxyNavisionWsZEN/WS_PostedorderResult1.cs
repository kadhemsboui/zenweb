using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_PostedorderResult1
    {

        [DataMember(EmitDefaultValue = false, Order = 2)] public string Message { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 5)] public string Amount { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 6)] public string reference { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 7)] public string updatedAt { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 8)] public string Motif { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 9)] public List<ProxyNavisionWsZEN.orderline> orderlines { get; set; }
    }
}