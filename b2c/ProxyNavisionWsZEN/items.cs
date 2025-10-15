using ProxyNavisionWsZEN.API;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class items
    {
        [DataMember(EmitDefaultValue = false, Order = 1)] public string title { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 2)] public string sku { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 3)] public string created_at { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 4)] public string updated_at { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 5)] public string Famille { get; set; } //non utilisé
        [DataMember(EmitDefaultValue = false, Order = 6)] public string Style { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 7)] public string Groupe { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 8)] public string Ligne { get; set; } //non utilisé
        [DataMember(EmitDefaultValue = false, Order = 9)] public string coupe { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 10)] public string Persona { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 11)] public string DivisionCommerciale { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 12)] public string Fournisseur { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 13)] public string CodeGroupe { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 14)] public string code_saison { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 15)] public string Nom_saison { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 16)] public string Poids { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 17)] public string numerPiece { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 18)] public string GS1 { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 19)] public string DateReception_PSR { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 20)] public string DateReception_TDS { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 21)] public string Message { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 22)] public List<ProxyNavisionWsZEN.Variants> declinaisons { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 23)] public List<Prices> SalesPrice { get; set; }
    }
}
