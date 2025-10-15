using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]

    public class WS_PurchRequest
    {
        [DataMember(EmitDefaultValue = false)] public string ean13 { get; set; }
        [DataMember(EmitDefaultValue = false)] public string reference { get; set; }
        [DataMember(EmitDefaultValue = false)] public string couleur { get; set; }
        [DataMember(EmitDefaultValue = false)] public string taille { get; set; }
        [DataMember(EmitDefaultValue = false)] public string date { get; set; }
        [DataMember(EmitDefaultValue = false)] public string Message { get; set; }

    }
}

