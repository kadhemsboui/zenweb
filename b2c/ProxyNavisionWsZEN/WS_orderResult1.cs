using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_orderResult1
    {
        [DataMember(EmitDefaultValue = false, Order = 1)] public string Colisno { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)] public string Message { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 3)] public string IdStatus { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 4)] public string status { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 5)] public string Amount { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 6)] public string reference { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 7)] public string updatedAt { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 8)] public string Motif { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 9)] public List<ProxyNavisionWsZEN.orderline> orderlines { get; set; }


    }
}