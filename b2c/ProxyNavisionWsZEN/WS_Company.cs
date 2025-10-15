using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_Company
    {


        public string nom = "";
        public string description = "";
        public string message = "";



        [DataMember(Order =1)]
        public string Code
        {
            get { return nom; }
            set { nom = value; }
        }
        [DataMember(Order = 2)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }



    }
}