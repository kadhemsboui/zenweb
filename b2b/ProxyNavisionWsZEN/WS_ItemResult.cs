using ProxyNavisionWsZEN.API;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_ItemResult
    {


        public string message = "";
        List<items> items = null;



        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember(Order = 2)]
        public List<items> Items
        {
            get { return items; }
            set { items = value; }
        }



    }
}