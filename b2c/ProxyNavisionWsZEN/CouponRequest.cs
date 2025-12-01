using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class CouponRequest
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
        public string Price_Group { get; set; }
        [DataMember(Order = 11)]
        public List<CouponProduct> ApplicableProducts { get; set; }
    }

    [DataContract]
    public class CouponProduct
    {
        [DataMember(Order = 1)]
        public string sku { get; set; }
    }
}
