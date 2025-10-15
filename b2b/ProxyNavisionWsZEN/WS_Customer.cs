
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_Customer
    {


        public string code = "";
        public string name = "";
        public string mail = "";
        public string phone = "";
        public string address = "";
        public string address2 = "";
        public string posting_Group = "";
        public string matricule_fiscal = "";
        public string city = "";
        public string code_postal = "";
        public string date_Created = "";
        public string last_Date_Modified = "";
        public string pays = "";
        public string currency = "";
        public string role = "";




        [DataMember(Order = 1)]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        [DataMember(Order = 2)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [DataMember(Order = 3)]
        public string Mail
        {
            get { return mail; }
            set { mail = value; }
        }
        [DataMember(Order = 4)]
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        [DataMember(Order = 5)]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        [DataMember(Order = 6)]
        public string Address2
        {
            get { return address; }
            set { address = value; }
        }
        [DataMember(Order = 7)]
        public string Posting_Group
        {
            get { return posting_Group; }
            set { posting_Group = value; }
        }
        [DataMember(Order = 8)]
        public string Matricule_fiscal
        {
            get { return matricule_fiscal; }
            set { matricule_fiscal = value; }
        }
        [DataMember(Order = 9)]
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        [DataMember(Order = 10)]
        public string Code_postal
        {
            get { return code_postal; }
            set { code_postal = value; }
        }
        [DataMember(Order = 11)]
        public string Date_Created
        {
            get { return date_Created; }
            set { date_Created = value; }
        }
        [DataMember(Order = 12)]
        public string Last_Date_Modified
        {
            get { return last_Date_Modified; }
            set { last_Date_Modified = value; }
        }
        [DataMember(Order = 13)]
        public string Pays
        {
            get { return pays; }
            set { pays = value; }
        }
        [DataMember(Order = 14)]
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        [DataMember(Order = 15)]
        public string Type
        {
            get { return role; }
            set { role = value; }
        }
    }
}