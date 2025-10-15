using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    public class DeliveryAddress
    {
        public string address = "";
        public string city = "";
        public string currency_Ratios = "";
        public string idPaymentMethod = "";
        public string commandType = "";
        public string location = "";
        public string countryId = "";
        public string firstName = "";
        public string lastName = "";
        public string phoneNumber = "";


        [DataMember(Order = 1)]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
       
        [DataMember(Order = 3)]
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        [DataMember(Order = 4)]
        public string CountryId
        {
            get { return countryId; }
            set { countryId = value; }
        }
        [DataMember(Order = 5)]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        [DataMember(Order = 6)]
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        [DataMember(Order = 7)]
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
    }
}
