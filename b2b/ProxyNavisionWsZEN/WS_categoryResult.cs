using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    public class WS_categoryResult
    {
        public List<getListCategorie> GetListCategorie = new List<getListCategorie>();
        public string message = "";




        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember(Order = 2)]
        public List<getListCategorie> ListCategorie
        {
            get { return GetListCategorie; }
            set { GetListCategorie = value; }
        }

    }
}