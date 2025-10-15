
using ProxyNavisionWsZEN.API;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;

namespace ProxyNavisionWsZEN
{
    [DataContract]

    public class Stock
    {
        public string stock_disponible = "";
        public string stock_en_attente_de_livraison = "";
        public string stock_receptionné = "";
        public string stock_sur_commande_achat = ""; 
        [DataMember(Order = 1)]
        public string Stock_disponible
        {
            get { return stock_disponible; }
            set { stock_disponible = value; }
        }
        [DataMember(Order = 2)]
        public string Stock_en_attente_de_livraison
        {
            get { return stock_en_attente_de_livraison; }
            set { stock_en_attente_de_livraison = value; }
        }
        [DataMember(Order = 3)]
        public string Stock_receptionné
        {
            get { return stock_receptionné; }
            set { stock_receptionné = value; }
        }
        [DataMember(Order = 4)]
        public string Stock_sur_commande_achat
        {
            get { return stock_sur_commande_achat; }
            set { stock_sur_commande_achat = value; }
        }
      
    }

}