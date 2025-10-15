using System.Runtime.Serialization;

[DataContract]
public class WS_UpdateCouponRequest
{
    [DataMember(Order = 1)]
    public string codeCoupon { get; set; }

    [DataMember(Order = 2)]
    public string isActive { get; set; }

    [DataMember(Order = 3)]
    public string isUsed { get; set; }
    [DataMember(Order = 4)]
    public string IdCompany { get; set; }
}
