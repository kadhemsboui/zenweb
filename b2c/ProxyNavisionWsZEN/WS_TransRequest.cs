using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]

    public class WS_TransRequest
    {
        [DataMember(EmitDefaultValue = false)] public string date { get; set; }
        [DataMember(EmitDefaultValue = false)] public string points { get; set; }
        [DataMember(EmitDefaultValue = false)] public string orderId { get; set; }
        [DataMember(EmitDefaultValue = false)] public string Type_écriture { get; set; }
        [DataMember(EmitDefaultValue = false)] public string Message { get; set; }

    }
}
