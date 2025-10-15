using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_CustomerResult
    {



        [DataMember(Order = 1)]
        public string status
        {
            get;
            set;
        }
        [DataMember(Order = 2)]
        public string message
        {
            get;
            set;
        }
        [DataMember(Order = 3)]
        public string codeErp
        {
            get;
            set;
        }


    }
}