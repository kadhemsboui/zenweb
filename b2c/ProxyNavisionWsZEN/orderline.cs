using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class orderline
    {
        [DataMember(EmitDefaultValue = false, Order = 1)] public string barcode { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 2)] public string quantityC { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 3)] public string unitPrice { get; set; }

    }
}