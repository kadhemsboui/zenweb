using System.Runtime.Serialization;
using System.Collections.Generic;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class VariantDetail
    {
        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string code { get; set; }

        [DataMember(Order = 2, EmitDefaultValue = false)]
        public string Composition0 { get; set; }

        [DataMember(Order = 3, EmitDefaultValue = false)]
        public string Composition1 { get; set; }

        [DataMember(Order = 4, EmitDefaultValue = false)]
        public string Composition2 { get; set; }

        [DataMember(Order = 5, EmitDefaultValue = false)]
        public string Composition3 { get; set; }

        [DataMember(Order = 6, EmitDefaultValue = false)]
        public string NGP { get; set; }

        [DataMember(Order = 7, EmitDefaultValue = false)]
        public string name { get; set; }

        [DataMember(Order = 8, EmitDefaultValue = false)]
        public string Taille { get; set; }

        [DataMember(Order = 9, EmitDefaultValue = false)]
        public string code_taille { get; set; }

        [DataMember(Order = 10, EmitDefaultValue = false)]
        public string ean13 { get; set; }

        [DataMember(Order = 11, EmitDefaultValue = false)]
        public string Quantity_in_serie_type { get; set; }
    }
}
