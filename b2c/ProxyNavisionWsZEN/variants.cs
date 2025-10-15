using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class Variants
    {
        [DataMember(Order = 1, EmitDefaultValue = false)] public string codeCouleur { get; set; }
        [DataMember(Order = 2, EmitDefaultValue = false)] public string Couleur { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public List<VariantDetail> variants { get; set; } = new List<VariantDetail>();


    }
}
