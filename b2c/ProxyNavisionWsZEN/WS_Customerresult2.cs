
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_Customerresult
    {


        public string nom = "";
        public List<WS_Customer> customersList = new List<WS_Customer>();
        public string message = "";




        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember(Order = 2)]
        public List<WS_Customer> CustomersList
        {
            get { return customersList; }
            set { customersList = value; }
        }



    }
}