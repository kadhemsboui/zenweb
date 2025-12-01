using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

[DataContract]
public class WS_CouponResult
{
    [DataMember(Order = 1)]
    public string Message { get; set; }

    [DataMember(Order = 2)]
    public List<WS_Coupon> CouponList { get; set; } = new List<WS_Coupon>();
}

[DataContract]
public class WS_Coupon
{
    [DataMember(Order = 1)]
    public string CodeCoupon { get; set; }

    [DataMember(Order = 2)]
    public string CustomerCodeErp { get; set; }

    [DataMember(Order = 3)]
    public string IdCompany { get; set; }

    [DataMember(Order = 4)]
    public string Type { get; set; }

    [DataMember(Order = 5)]
    public string Value { get; set; }

    [DataMember(Order = 6)]
    public string Validity { get; set; }

    [DataMember(Order = 7)]
    public string Description { get; set; }

    [DataMember(Order = 8)]
    public string IsActive { get; set; }

    [DataMember(Order = 9)]
    public string IsUsed { get; set; }

    [DataMember(Order = 10)]
    public string BarCode { get; set; }  // first barcode if exists
    [DataMember(Order = 11)]
    public string Price_Group { get; set; }
    [DataMember(Order = 12)]
    public List<WS_Product> ApplicableProducts { get; set; } = new List<WS_Product>();
}

[DataContract]
public class WS_Product
{
    [DataMember(Order = 1)]
    public string sku { get; set; }
}
