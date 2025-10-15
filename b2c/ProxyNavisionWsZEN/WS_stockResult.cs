using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_stockResult
    {
        [DataMember(EmitDefaultValue = false, Order = 1)] public string Message { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)] public string Store { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 3)] public string barreCode { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 4)] public string stockAvailable { get; set; }

    }
}