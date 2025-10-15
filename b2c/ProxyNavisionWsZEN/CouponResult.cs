using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class CouponResult
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }

        [DataMember(Order = 2)]
        public bool Success { get; set; }
    }
}
