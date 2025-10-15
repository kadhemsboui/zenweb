using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    public class Companies
    {
        public string code = "";





        [DataMember(Order = 1)]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
       
    }
}