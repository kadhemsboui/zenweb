using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class getListCategorie
    {
        public string Code_categorie = "";
        public string Code_sub_categorie = "";
        public string Name = "";
        public string Status = "";
        public string ErrorReference = "";
        public string ErrorMessage = "";
        public string Valeur_parent = "";
        [DataMember(Order = 1)]
        public string code_categorie
        {
            get { return Code_categorie; }
            set { Code_categorie = value; }
        }
    
        [DataMember(Order = 3)]
        public string code_Parent
        {
            get { return Code_sub_categorie; }
            set { Code_sub_categorie = value; }
        }
        [DataMember(Order = 4)]
        public string name
        {
            get { return Name; }
            set { Name = value; }
        }

   

    }
}