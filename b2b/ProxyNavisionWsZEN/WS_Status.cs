using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_Status
    {
       
            public string message = "";
            [DataMember(Order = 1)]
            public string Message
            {
                get { return message; }
                set { message = value; }
            }

        
    }
}

