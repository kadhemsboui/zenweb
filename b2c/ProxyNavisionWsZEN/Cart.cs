
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    public class Cart
    {
        public string customerCodeErp = "";
        public string location = "";
        public string company = "";

      

        [DataMember(Order = 1)]
        public string CustomerCodeErp
        {
            get { return customerCodeErp; }
            set { customerCodeErp = value; }
        }
        [DataMember(Order = 2)]
        public string Location
        {
            get { return location; }
            set { location = value; }
        }
        [DataMember(Order = 3)]
        public string IdCompany
        {
            get { return company; }
            set { company = value; }
        }

    }
}