using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]

    public class variants
    {
        public string barcode = "";
        public string couleur = "";
        public string taille= "";
        public string Quantity = "";

        public string stock_disponible = "";
        public string stock_en_attente_de_livraison = "";
        public string stock_receptionné = "";
        public string stock_sur_commande_achat = "";





        [DataMember(Order = 1)]
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }
        [DataMember(Order = 2)]
        public string Couleur
        {
            get { return couleur; }
            set { couleur = value; }
        }
        [DataMember(Order = 3)]
        public string Taille
        {
            get { return taille; }
            set { taille = value; }
        }
        [DataMember(Order = 4)]
        public string Quantity_in_serie_type
        {
            get { return Quantity; }
            set { Quantity = value; }
        }
        [DataMember(Order = 5)]
        public string Stock_disponible
        {
            get { return stock_disponible; }
            set { stock_disponible = value; }
        }
        [DataMember(Order = 6)]
        public string Stock_en_attente_de_livraison
        {
            get { return stock_en_attente_de_livraison; }
            set { stock_en_attente_de_livraison = value; }
        }
        [DataMember(Order = 7)]
        public string Stock_receptionné
        {
            get { return stock_receptionné; }
            set { stock_receptionné = value; }
        }
        [DataMember(Order = 8)]
        public string Stock_sur_commande_achat
        {
            get { return stock_sur_commande_achat; }
            set { stock_sur_commande_achat = value; }
        }
    }
}