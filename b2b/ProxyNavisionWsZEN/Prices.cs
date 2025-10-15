using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]

    public class Prices
    {
        public string currencyCode = "";
        public string discountPrice = "";
        public string discountPercentage = "";
        public string price = "";









        [DataMember(Order = 1)]
        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }
        [DataMember(Order = 2)]
        public string DiscountPrice
        {
            get { return discountPrice; }
            set { discountPrice = value; }
        }
        [DataMember(Order = 3)]
        public string DiscountPercentage
        {
            get { return discountPercentage; }
            set { discountPercentage = value; }
        }
        [DataMember(Order = 4)]
        public string Price
        {
            get { return price; }
            set { price = value; }
        }
    }
}