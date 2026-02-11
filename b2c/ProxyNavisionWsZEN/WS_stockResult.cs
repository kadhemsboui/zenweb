using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_stockResult
    {
        [DataMember(EmitDefaultValue = false, Order = 1)] public string Message { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 2)] public string Store { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 3)] public string barreCode { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 4)] public string Stock_on_sales_order { get; set; }

        [DataMember(EmitDefaultValue = false, Order = 5)] public string Stock_awaiting_delivery { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 6)] public string Received_stock { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 7)] public string Stock_on_purchase_order { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 8)] public string stockAvailable { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 9)] public string all_stock_on_purchase_order { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 10)] public string all_received_stock { get; set; }
        [DataMember(EmitDefaultValue = false, Order = 11)] public string all_stock_on_sales_order { get; set; }

    }
}