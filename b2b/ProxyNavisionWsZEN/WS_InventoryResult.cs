
using ProxyNavisionWsZEN.API;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_InventoryResult
    {


        public string message = "";
        List<Stock> inventories = null;



        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember(Order = 2)]
        public List<Stock> Stock
        {
            get { return inventories; }
            set { inventories = value; }
        }



    }
}