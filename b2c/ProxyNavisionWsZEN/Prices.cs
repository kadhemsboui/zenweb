using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class Prices
    {
        [DataMember(Order = 1, EmitDefaultValue = true)] public string CurrencyCode { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = true)] public string PriceHT { get; set; }
        [DataMember(Order = 3, EmitDefaultValue = true)] public string PriceTTC { get; set; }
        [DataMember(Order = 4, EmitDefaultValue = true)] public string DiscountPrice { get; set; }
        [DataMember(Order = 5, EmitDefaultValue = true)] public string DiscountPercentage { get; set; }
                [DataMember(Order = 6, EmitDefaultValue = true)] public string prix_negoce { get; set; }

        
        

    }
}
