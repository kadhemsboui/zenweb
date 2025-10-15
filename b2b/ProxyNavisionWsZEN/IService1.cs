using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ProxyNavisionWsZEN
{
  
    [ServiceContract]
    public interface IService1
    {
        


        [WebGet(UriTemplate = "GetCompany/", ResponseFormat = WebMessageFormat.Json)]
        WS_CompanyResult GetCompany();
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "getitem/")]
        WS_ItemResult getitem(string No, string Division_commerciale, List<Companies> Companies, List<Locations> Locations,string updated_start,string updated_end,string created_start,string created_end);
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "getlocation/")]
        WS_LocationResult GetLocation(List<Companies> Companies);
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "getstock/")]
        WS_InventoryResult getstock(string Barcode, string Division_commerciale, List<Companies> Companies, List<Locations> Locations);
        [WebGet(UriTemplate = "GetCustomer/", ResponseFormat = WebMessageFormat.Json)]
        WS_Customerresult GetCustomer();
        [WebGet(UriTemplate = "getListCategorie", ResponseFormat = WebMessageFormat.Json)]
        WS_categoryResult getListCategorie();
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddOrder/")]
        WS_OrderResult AddOrder(string orderNo, string customerNo, string colisno, string currency, string currency_Ratio, string address, List<Order_list> Order_list);
        [WebInvoke(Method = "PATCH", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ModifyOrder/")]
        WS_OrderResult ModifyOrder(string orderNo, string customerNo, string colisno, string currency, string currency_Ratio, string address, List<Order_list> Order_list);
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddImage/")]
        WS_ImageResult AddImage(string ItemNo, string ImageUrl);
        [WebInvoke(Method = "PATCH", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "UpdateStatus/")]
        WS_Status ModifyStatus(string OrderNo, string Status);

    }



}
