using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_CustomerResult
    {


        public string result = "";
        public string message = "";



        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember(Order = 1)]
        public string CustomerNo
        {
            get { return result; }
            set { result = value; }
        }



    }
}