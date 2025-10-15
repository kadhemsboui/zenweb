using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]

    public class Order_list
    {
        public string barcode = "";
        public string quantity = "";
        public string unit_Price = "";
        public string id = "";
        public string location = "";
        public string orderNo = "";
        public string discount = "";
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
        [DataMember(Order = 3)]
        public string Unit_Price
        {
            get { return unit_Price; }
            set { unit_Price = value; }
        }
        [DataMember(Order = 4)]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        [DataMember(Order = 5)]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        [DataMember(Order = 6)]
        public string Discount
        {
            get { return discount; }
            set { discount = value; }
        }

    }
}
