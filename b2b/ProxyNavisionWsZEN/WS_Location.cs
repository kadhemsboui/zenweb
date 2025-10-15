using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_Location
    {
        public string nom = "";
        public string description = "";
        public string store = "";
        public string company = "";




        [DataMember(Order = 1)]
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
        [DataMember(Order = 4)]
        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        [DataMember(Order = 4)]
        public string Store
        {
            get { return store; }
            set { store = value; }
        }
    }
}