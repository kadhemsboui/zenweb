using ProxyNavisionWsZEN.API;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_OrderResult
    {
        public string message = "";
        List<Order_list> order_list = null;
        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
      
    }
}