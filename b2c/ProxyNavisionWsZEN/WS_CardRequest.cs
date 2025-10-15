using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_CardRequest
    {
        [DataMember(EmitDefaultValue = false)] public string idCard { get; set; }
        [DataMember(EmitDefaultValue = false)] public string points { get; set; }
        [DataMember(EmitDefaultValue = false)] public string programName { get; set; }
        [DataMember(EmitDefaultValue = false)] public string status { get; set; }
        [DataMember(EmitDefaultValue = false)] public string activationDate { get; set; }
        [DataMember(EmitDefaultValue = false)] public string Message { get; set; }
    }
}