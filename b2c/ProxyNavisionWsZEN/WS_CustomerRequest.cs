using System.Runtime.Serialization;

[DataContract]
public class WS_CustomerRequest
{
    [DataMember(EmitDefaultValue = false)] public string codeErp { get; set; }
    [DataMember(EmitDefaultValue = false)] public string username { get; set; }
    [DataMember(EmitDefaultValue = false)] public string firstName { get; set; }
    [DataMember(EmitDefaultValue = false)] public string lastName { get; set; }
    [DataMember(EmitDefaultValue = false)] public string email { get; set; }
    [DataMember(EmitDefaultValue = false)] public string phone { get; set; }
    [DataMember(EmitDefaultValue = false)] public string birthday { get; set; }
    [DataMember(EmitDefaultValue = false)] public string gender { get; set; }
    [DataMember(EmitDefaultValue = false)] public string IdCompany { get; set; }
    [DataMember(EmitDefaultValue = false)] public string Message { get; set; }
}