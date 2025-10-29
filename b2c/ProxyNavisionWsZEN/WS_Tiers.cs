using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    public class WS_Tiers
    {
        [DataMember(EmitDefaultValue = false, Order = 1)] public string Code { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 2)] public string Description { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 3)] public string created_at { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 4)] public string updated_at { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 4)] public string Message { get; set; }
    }
}