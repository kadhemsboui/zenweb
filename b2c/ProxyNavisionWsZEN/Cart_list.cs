

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]

    public class Cart_list
    {
        public string barcode = "";
        public string quantity = "";
  

        [DataMember(Order = 1)]
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }
        [DataMember(Order = 2)]
        public string Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }
      
    }
}
