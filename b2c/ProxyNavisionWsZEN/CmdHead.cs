using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    public class CmdHead
    {
        public string colisno = "";
        public string customerCodeErp = "";
        public string orderNo = "";
        public string currency_Ratios = "";
        public string idPaymentMethod = "";
        public string commandType = "";
        public string location = "";
        public string idCompany = "";
        public string currency = "";
        public string date = "";
        public string deliveryType = "";
        public string idStore = "";

        public string RefundType = "";

        public string Rib = "";
        public string remise_Coupon = "";


        [DataMember(Order = 1)]
        public string TiersColisNo
        {
            get { return colisno; }
            set { colisno = value; }
        }
        [DataMember(Order = 2)]
        public string CustomerCodeErp
        {
            get { return customerCodeErp; }
            set { customerCodeErp = value; }
        }
        [DataMember(Order = 3)]
        public string OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        [DataMember(Order = 4)]
        public string Currency_Ratio
        {
            get { return currency_Ratios; }
            set { currency_Ratios = value; }
        }
        [DataMember(Order = 5)]
        public string IdPaymentMethod
        {
            get { return idPaymentMethod; }
            set { idPaymentMethod = value; }
        }
        [DataMember(Order = 6)]
        public string CommandType
        {
            get { return commandType; }
            set { commandType = value; }
        }
        [DataMember(Order = 7)]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        [DataMember(Order = 8)]
        public string IdCompany
        {
            get { return idCompany; }
            set { idCompany = value; }
        }
        [DataMember(Order = 9)]
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        [DataMember(Order = 10)]
        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        [DataMember(Order = 11)]
        public string DeliveryType
        {
            get { return deliveryType; }
            set { deliveryType = value; }
        }
        [DataMember(Order = 12)]
        public string IdStore
        {
            get { return idStore; }
            set { idStore = value; }
        }
        [DataMember(Order = 13)]
        public string refundType
        {
            get { return RefundType; }
            set { RefundType = value; }
        }
        [DataMember(Order = 14)]
        public string rib
        {
            get { return Rib; }
            set { Rib = value; }
        }
        [DataMember(Order = 15)]
        public string Remise_Coupon
        {
            get { return remise_Coupon; }
            set { remise_Coupon = value; }
        }
    }
}