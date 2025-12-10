
using ProxyNavisionWsZEN.API;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_CartResult
    {
        public string message = "";
        public string discountAmount = "";
        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember(Order = 2)]
        public string DiscountAmount
        {
            get { return discountAmount; }
            set { discountAmount = value; }
        }
        [DataMember(Order = 3)]
        public List<CartResultLine> CartLines { get; set; } = new List<CartResultLine>();

    }
    [DataContract]
    public class CartResultLine
    {
        [DataMember(Order = 1)]
        public string Barcode { get; set; }
        [DataMember(Order = 2)]
        public string Amount { get; set; }
   
        [DataMember(Order = 3)]
        public string DiscountPercentage { get; set; }

    }
}