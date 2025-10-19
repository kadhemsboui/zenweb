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
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "order/GetOrderReturn/")]
        List<WS_orderResult1> GetOrderreturn(WS_OrderRequest1 request);

        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "order/GetOrder/")]
        List<WS_orderResult1> GetOrder(WS_OrderRequest1 request);



        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddCustomer/")]
        WS_CustomerResult AddCustomer(WS_CustomerRequest request);
        [WebInvoke(Method = "PATCH", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ModifyCustomer/")]
        WS_CustomerResult ModifyCustomer(WS_CustomerRequest request);
        [WebGet(UriTemplate = "GetCompany/", ResponseFormat = WebMessageFormat.Json)]
        WS_CompanyResult GetCompany();
        [WebGet(UriTemplate = "customer/search?IdCompany={IdCompany}&codeErp={codeErp}&phone={phone}",ResponseFormat = WebMessageFormat.Json)]
        List<WS_CustomerRequest> SearchCustomers(string IdCompany, string codeErp,  string phone);
        [WebGet(UriTemplate = "loyalty/card/by-code?idcard={idcard}&codeErp={codeErp}&IdCompany={IdCompany}",ResponseFormat = WebMessageFormat.Json)]
        List<WS_CardRequest> card( string idcard, string codeErp,string IdCompany);
        [WebGet(UriTemplate = "getListCategorie?IdCompany={IdCompany}", ResponseFormat = WebMessageFormat.Json)]
        WS_categoryResult getListCategorie(string IdCompany);
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddImage/")]
        WS_ImageResult AddImage(string ItemNo, string ImageUrl);
        [WebGet(UriTemplate = "loyalty/transactions?idcard={idcard}&IdCompany={IdCompany}",ResponseFormat = WebMessageFormat.Json)]
        List<WS_TransRequest> Gettransaction(string idcard,  string IdCompany);
        [WebGet(UriTemplate = "purchase/history?codeErp={codeErp}&IdCompany={IdCompany}",ResponseFormat = WebMessageFormat.Json)]
        List<WS_PurchRequest> GetPurchase(string codeErp, string IdCompany);
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddOrder/")]
        WS_OrderResult AddOrder(CmdHead CmdHead, DeliveryAddress DeliveryAddress, List<Order_list> Order_list);
        [WebGet(UriTemplate = "GetLocation?IdCompany={IdCompany}", ResponseFormat = WebMessageFormat.Json)]
        WS_LocationResult GetLocation(string IdCompany);
        [WebGet(UriTemplate = "Product/GetProduct?updated_start={updated_start}&IdCompany={IdCompany}&updated_end={updated_end}&created_start={created_start}&created_end={created_end}&reference={reference}", ResponseFormat = WebMessageFormat.Json)]
        List<items> getitem(string reference, string IdCompany, string created_start, string created_end,string updated_start,string updated_end);
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "stock/GetStock/")]
        List<WS_stockResult> GetStock(WS_StockrRequest request);
        [WebInvoke(Method = "PATCH", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "ModifyOrder/")]
        string ModifyOrder(WS_OrderRequest request);
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "Addcardpoint/")]
        string addcardpoint(string idcard, string points, string IdCompany,string DocumentNo);
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "AddTransfertOrder/")]
        string AddTransfertOrder(string IdCompany,string Transfer_from_Code ,string Transfer_to_Code,string OrderNo, List<Transfert_list> Transfert_list);
        [OperationContract]
        [WebInvoke(Method = "POST",
                   UriTemplate = "CreateCoupon",
                   RequestFormat = WebMessageFormat.Json,
                   ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        CouponResult CreateCoupon(CouponRequest request);
        [WebGet(UriTemplate = "GetCoupon?CustomerCodeErp={CustomerCodeErp}&CodeCoupon={CodeCoupon}&phoneNumber={phoneNumber}", ResponseFormat = WebMessageFormat.Json)]
        WS_CouponResult GetCoupon(string CustomerCodeErp, string CodeCoupon, string phoneNumber);
        [WebInvoke(
    Method = "PATCH",
    UriTemplate = "UpdateCoupon",
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json,
    BodyStyle = WebMessageBodyStyle.Bare)]
         ws_result UpdateCoupon(WS_UpdateCouponRequest request);

    }
}
