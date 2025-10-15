using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ProxyNavisionWsZEN
{
    [DataContract]
    public class WS_LocationResult
    {
        public string message = "";

        public List<WS_Location> locationList = new List<WS_Location>();




        [DataMember(Order = 1)]
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        [DataMember(Order = 2)]
        public List<WS_Location> LocationList
        {
            get { return locationList; }
            set { locationList = value; }
        }
    }
}