using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_OrderRequest
    {
        [DataMember(EmitDefaultValue = false, Order = 1)] public string Message { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)] public string refCmd { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 3)] public string reglement { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 4)] public string idStatus { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 5)] public string IdCompany { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 6)] public string Motif { get; set; }

    }
}