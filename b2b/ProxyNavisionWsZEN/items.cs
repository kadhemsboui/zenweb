using ProxyNavisionWsZEN.API;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;

namespace ProxyNavisionWsZEN
{
    [DataContract]

    public class items
    {
        public string code = "";
        public string description = "";
        public string category = "";
        public string sub_Category = "";
        public string code_marque="";
        public string série_type = "";
        public string barcode = "";
        public string sexe = "";


        List<variants> variants = null;
        List<Prices> salesPrice = null;





        [DataMember(Order = 1)]
        public string No
        {
            get { return code; }
            set { code = value; }
        }
        [DataMember(Order = 2)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        [DataMember(Order = 3)]
        public string Category
        {
            get { return category; }
            set { category = value; }
        }
      
        [DataMember(Order = 5)]
        public string Code_marque
        {
            get { return code_marque; }
            set { code_marque = value; }
        }
        [DataMember(Order = 6)]
        public string Serie_type
        {
            get { return série_type; }
            set { série_type = value; }
        }
        [DataMember(Order = 7)]
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }
        [DataMember(Order = 8)]
        public string Sexe
        {
            get { return sexe; }
            set { sexe = value; }
        }
        [DataMember(EmitDefaultValue = false, Order = 9)] public string created_at { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 10)] public string updated_at { get; set; }

        [DataMember(Order = 11)]
        public List<variants> Variants
        {
            get { return variants; }
            set { variants = value; }
        }
        [DataMember(Order = 12)]
        public List<Prices> SalesPrice
        {
            get { return salesPrice; }
            set { salesPrice = value; }
        }
    }

}