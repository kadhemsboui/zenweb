using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_CompanyResult
    {


        public string nom = "";
        public List<WS_Company> companiesList= new List<WS_Company>();
        public string message = "";



      
        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember(Order = 2)]
        public List<WS_Company> CompaniesList
        {
            get { return companiesList; }
            set { companiesList = value; }
        }



    }
}