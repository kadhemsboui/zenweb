using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using ProxyNavisionWsZEN.API;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Web.Services.Protocols;
using System.Globalization;
using System.Diagnostics.Eventing.Reader;
using System.Collections.Specialized;
using System.Web;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Data;
using System.Configuration;
using System.Diagnostics;

namespace ProxyNavisionWsZEN
{

    public class Service1 : IService1
    {
        public ws_result UpdateCoupon(WS_UpdateCouponRequest request)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);

                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/"
                                          + request.IdCompany + "/Codeunit/API";

                mobile_Web_Services.UpdateCoupon(request.codeCoupon, request.isActive, request.isUsed);
                ws_result ws_Result = new ws_result();
                ws_Result.Message= "Success";
                return ws_Result;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();

                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                ws_result ws_Result = new ws_result();
                ws_Result.Message = jsonResponse["Message"].ToString();
                return ws_Result;
            
            }
        }

        public WS_CouponResult GetCoupon(string CustomerCodeErp, string CodeCoupon,string phoneNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    phoneNumber = "";
                }

                if (string.IsNullOrEmpty(CustomerCodeErp))
                {
                    CustomerCodeErp = "";
                }
                if (string.IsNullOrEmpty(CodeCoupon))
                {
                    CodeCoupon = "";
                }
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);

                WS_CouponResult CouponResult = new WS_CouponResult();

                // Step 1: Get all companies
                ProxyNavisionWsZEN.API.Company navCompany = new ProxyNavisionWsZEN.API.Company();
                mobile_Web_Services.getcompany(ref navCompany);

                foreach (var company in navCompany.Companies)
                {
                    string idCompany = company.Name;

                    ProxyNavisionWsZEN.API.Coupon navCoupon = new ProxyNavisionWsZEN.API.Coupon();
                    mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/"
                                              + idCompany + "/Codeunit/API";
                    mobile_Web_Services.get_coupon(ref navCoupon, CustomerCodeErp, CodeCoupon, phoneNumber);
                    if (navCoupon.Coupons != null)
                    {
                        foreach (var c in navCoupon.Coupons)
                        {
                            if (!string.IsNullOrEmpty(c.codeCoupon))
                            {
                                var wsCoupon = new WS_Coupon();

                                if (c.Coup!=null)
                                {

                                     wsCoupon = new WS_Coupon
                                    {
                                        CodeCoupon = c.codeCoupon,
                                        CustomerCodeErp = c.CustomerCodeErp,
                                        IdCompany = idCompany,
                                        Type = c.type.ToString(),
                                        Value = c.value,
                                        Validity = c.validity,
                                        Description = c.Description,
                                        IsActive = c.isActive.FirstOrDefault(),
                                        IsUsed = c.isused.FirstOrDefault(),
                                        BarCode = c.Coup.FirstOrDefault()?.BarCode
                                    };
                                }
                                else
                                {
                                     wsCoupon = new WS_Coupon
                                    {
                                        CodeCoupon = c.codeCoupon,
                                        CustomerCodeErp = c.CustomerCodeErp,
                                        IdCompany = idCompany,
                                        Type = c.type.ToString(),
                                        Value = c.value,
                                        Validity = c.validity,
                                        Description = c.Description,
                                        IsActive = c.isActive.FirstOrDefault(),
                                        IsUsed = c.isused.FirstOrDefault(),
                                        BarCode = ""
                                    };
                                }
                                var acceptedFormats = new[] { "MM/dd/yy", "MM/dd/yyyy" };

                                if (DateTime.TryParseExact(wsCoupon.Validity, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                                {
                                    wsCoupon.Validity = parsedDate.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    wsCoupon.Validity = wsCoupon.Validity;
                                }
                                if (c.Line != null)
                                {
                                    // Products
                                    foreach (var line in c.Line)
                                    {
                                        if (line.Sku != "")
                                        { wsCoupon.ApplicableProducts.Add(new WS_Product { sku = line.Sku }); }
                                    }
                                }
                                if (wsCoupon.CodeCoupon != "")

                                { CouponResult.CouponList.Add(wsCoupon); }
                            }
                        }
                    }
                   
                }

                CouponResult.Message = "Success";
                return CouponResult;
            }
            catch (Exception ex)
            {
                WS_CouponResult CouponResult = new WS_CouponResult();
                CouponResult.Message = ex.Message;
                return CouponResult;
            }
        }

        public CouponResult CreateCoupon(CouponRequest request)
        {
            CouponResult result = new CouponResult();
            JObject jsonResponse = new JObject();

            try
            {
                // === 1. Required Field Validations ===
                if (string.IsNullOrWhiteSpace(request.CodeCoupon))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "CodeCoupon est obligatoire.";
                    result.Success = false;
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.CustomerCodeErp))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "CustomerCodeErp est obligatoire.";
                    result.Success = false;
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.Type))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "Type est obligatoire.";
                    result.Success = false;
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.Description))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "Description est obligatoire.";
                    result.Success = false;
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.Value))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "Value est obligatoire.";
                    result.Success = false;
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.IdCompany))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "IdCompany est obligatoire.";
                    result.Success = false;
                    return result;
                }

                // === 2. Validate IsActive and IsUsed explicitly ===
                if (string.IsNullOrWhiteSpace(request.IsActive))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "IsActive est obligatoire.";
                    result.Success = false;
                    return result;
                }

                if (string.IsNullOrWhiteSpace(request.IsUsed))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    result.Message = "IsUsed est obligatoire.";
                    result.Success = false;
                    return result;
                }

                // === 3. Validate ApplicableProducts ===
                List<API.Couponline> lines = new List<API.Couponline>();

                if (request.ApplicableProducts != null && request.ApplicableProducts.Count > 0)
                {
                    foreach (var product in request.ApplicableProducts)
                    {
                        if (string.IsNullOrWhiteSpace(product.sku))
                        {
                            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                            result.Message = "Chaque produit dans ApplicableProducts doit avoir un sku non vide.";
                            result.Success = false;
                            return result;
                        }

                        lines.Add(new API.Couponline
                        {
                            SKU = product.sku
                        });
                    }
                }
                else
                {
                    // Empty product list → pass <SKU>NULL</SKU> in XMLPort
                    lines.Add(new API.Couponline
                    {
                        SKU = ""
                    });
                }

                API.Couponlines couponXmlPort = new API.Couponlines
                {
                    Couponline = lines.ToArray()
                };

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);

                mobile_Web_Services.Url = $"https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/{request.IdCompany}/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                // === 6. Call create_coupon SOAP Method ===
                string soapResponse = mobile_Web_Services.create_coupon(
                    request.CodeCoupon,
                    request.Description,
                    request.CustomerCodeErp,
                    Convert.ToDecimal(request.Value.Replace(".", ",")),
                    request.Type,
                    request.IsActive,
                    request.Validity,
                    request.IsUsed,
                    couponXmlPort
                );

                result.Message = soapResponse ?? "Success";
                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                result.Success = false;
                return result;
            }
        }



        public string AddTransfertOrder(string IdCompany, string Transfer_from_Code, string Transfer_to_Code, string OrderNo,List<Transfert_list> Transfert_list)
        {
            try
            {
                JObject jsonResponse = new JObject();



                var request = OperationContext.Current.RequestContext.RequestMessage;

                if ((IdCompany == null) || (IdCompany == ""))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "IdCompany obligatoire";

                }
                if ((Transfer_from_Code == null) || (Transfer_from_Code == ""))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "Transfer_from_Code obligatoire";

                }
                if ((Transfer_to_Code == null) || (Transfer_to_Code == ""))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "Transfer_to_Code obligatoire";

                }


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                int i =0;
                if (Transfert_list.Count == 0){
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "Transfert_list obligatoire";

                }
                API.TransferLine TransferLinerXmlPort = new API.TransferLine();
                List<API.TransferLines> lines = new List<API.TransferLines>();

                if (Transfert_list != null)
                {
                    foreach (var list in Transfert_list)
                    {
                        i = i + 10000;

                        API.TransferLines lineXmlPort = new API.TransferLines
                        {
                            Barcode = list.Barcode,
                            Quantity = list.Quantity,
                            LineNo= i.ToString()


                        };
                        lines.Add(lineXmlPort);

                    }

                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "Transfert_list obligatoire";

                }
                TransferLinerXmlPort.TransferLines = lines.ToArray();




                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                string stores = "";

                string result=mobile_Web_Services.addordertransfert(Transfer_from_Code, Transfer_to_Code,ref  TransferLinerXmlPort, OrderNo);

                return result;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }

                return jsonResponse["Message"].ToString();
            }
        }
        public List<WS_orderResult1> GetOrderreturn(WS_OrderRequest1 Orderrequest)
        {
            try
            {
                JObject jsonResponse = new JObject();
                List<WS_orderResult1> ItemResult = new List<WS_orderResult1>();

                var request = OperationContext.Current.RequestContext.RequestMessage;
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

                if (Orderrequest.IdCompany == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";
                    jsonResponse["Message"] = "Veuillez spécifier l'IdCompany.";
                    WS_orderResult1 items = new WS_orderResult1();
                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;
                }

                if (((Orderrequest.updatedstart == "") && (Orderrequest.updatedend != "")) ||
                    ((Orderrequest.updatedstart != "") && (Orderrequest.updatedend == "")))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";
                    jsonResponse["Message"] = "Vous devez spécifier à la fois created_end et created_start.";
                    WS_orderResult1 items = new WS_orderResult1();
                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);

                ProxyNavisionWsZEN.API.sheaders ItemsXML = new ProxyNavisionWsZEN.API.sheaders();
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + Orderrequest.IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                string references = "";
                foreach (var reference in Orderrequest.references)
                {
                    if (references == "")
                    {
                        references = reference;
                    }
                    else
                    {
                        references = references + "|" + reference;
                    }
                }

                mobile_Web_Services.getorderreturnb2c(references, Orderrequest.updatedstart, Orderrequest.updatedend, ref ItemsXML);

                for (int i = 0; i < ItemsXML.sheader.Count(); i++)
                {
                    if (ItemsXML.sheader.ElementAt(i).@ref != "")
                    {
                        ProxyNavisionWsZEN.WS_orderResult1 items = new ProxyNavisionWsZEN.WS_orderResult1();
                        items.reference = ItemsXML.sheader.ElementAt(i).@ref;
                        items.Amount = ItemsXML.sheader.ElementAt(i).Amount_TTC;
                        items.IdStatus= ItemsXML.sheader.ElementAt(i).IdStatus;
                        items.status = ItemsXML.sheader.ElementAt(i).status;
                        items.updatedAt = ItemsXML.sheader.ElementAt(i).updatedAt;

                        var acceptedDateTimeFormats = new[] { "MM/dd/yy hh:mm tt", "MM/dd/yyyy hh:mm tt" };
                        if (DateTime.TryParseExact(items.updatedAt, acceptedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
                        {
                            items.updatedAt = parsedDateTime.ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            items.updatedAt = items.updatedAt;
                        }

                        items.orderlines = new List<ProxyNavisionWsZEN.orderline>();
                        if (ItemsXML.sheader.ElementAt(i)?.sline != null)
                        {
                            for (int j = 0; j < ItemsXML.sheader.ElementAt(i).sline.Count(); j++)
                            {
                                ProxyNavisionWsZEN.orderline orderline = new ProxyNavisionWsZEN.orderline();
                                orderline.barcode = ItemsXML.sheader.ElementAt(i).sline.ElementAt(j).barcode;
                                orderline.quantityC = ItemsXML.sheader.ElementAt(i).sline.ElementAt(j).quantity;
                                orderline.unitPrice = ItemsXML.sheader.ElementAt(i).sline.ElementAt(j).unitprice;
                                items.orderlines.Add(orderline);
                            }
                        }

                        ItemResult.Add(items);
                    }
                }

                return ItemResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }

                List<WS_orderResult1> ItemResult = new List<WS_orderResult1>();
                WS_orderResult1 items = new WS_orderResult1();
                items.Message = jsonResponse["Message"].ToString();
                ItemResult.Add(items);
                return ItemResult;
            }
        }
        public List<WS_orderResult1> GetOrder(WS_OrderRequest1 Orderrequest)
        {
            try
            {
                JObject jsonResponse = new JObject();
                List<WS_orderResult1> ItemResult = new List<WS_orderResult1>();

                var request = OperationContext.Current.RequestContext.RequestMessage;
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

                if (Orderrequest.IdCompany == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";
                    jsonResponse["Message"] = "Veuillez spécifier l'IdCompany.";
                    WS_orderResult1 items = new WS_orderResult1();
                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;
                }

                if (((Orderrequest.updatedstart == "") && (Orderrequest.updatedend != "")) ||
                    ((Orderrequest.updatedstart != "") && (Orderrequest.updatedend == "")))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";
                    jsonResponse["Message"] = "Vous devez spécifier à la fois created_end et created_start.";
                    WS_orderResult1 items = new WS_orderResult1();
                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);

                ProxyNavisionWsZEN.API.sheaders ItemsXML = new ProxyNavisionWsZEN.API.sheaders();
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + Orderrequest.IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                string references = "";
                foreach (var reference in Orderrequest.references)
                {
                    if (references == "")
                    {
                        references = reference;
                    }
                    else
                    {
                        references = references + "|" + reference;
                    }
                }

                mobile_Web_Services.getorder(references, Orderrequest.updatedstart, Orderrequest.updatedend, ref ItemsXML);

                for (int i = 0; i < ItemsXML.sheader.Count(); i++)
                {
                    if (ItemsXML.sheader.ElementAt(i).@ref != "")
                    {
                        ProxyNavisionWsZEN.WS_orderResult1 items = new ProxyNavisionWsZEN.WS_orderResult1();

                        //items.Colisno = ItemsXML.sheader.ElementAt(i).Colisno;

                        items.IdStatus = ItemsXML.sheader.ElementAt(i).IdStatus;
                        items.reference = ItemsXML.sheader.ElementAt(i).@ref;
                        items.status = ItemsXML.sheader.ElementAt(i).status;
                        items.Amount = ItemsXML.sheader.ElementAt(i).Amount_TTC;
                        items.updatedAt = ItemsXML.sheader.ElementAt(i).updatedAt;
                        items.Motif = ItemsXML.sheader.ElementAt(i).Motif;

                        var acceptedDateTimeFormats = new[] { "MM/dd/yy hh:mm tt", "MM/dd/yyyy hh:mm tt" };
                        if (DateTime.TryParseExact(items.updatedAt, acceptedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
                        {
                            items.updatedAt = parsedDateTime.ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            items.updatedAt = items.updatedAt;
                        }

                        items.orderlines = new List<ProxyNavisionWsZEN.orderline>();
                        if (ItemsXML.sheader.ElementAt(i)?.sline != null)
                        {
                            for (int j = 0; j < ItemsXML.sheader.ElementAt(i).sline.Count(); j++)
                            {
                                ProxyNavisionWsZEN.orderline orderline = new ProxyNavisionWsZEN.orderline();
                                orderline.barcode = ItemsXML.sheader.ElementAt(i).sline.ElementAt(j).barcode;
                                orderline.quantityC = ItemsXML.sheader.ElementAt(i).sline.ElementAt(j).quantity;
                                orderline.unitPrice = ItemsXML.sheader.ElementAt(i).sline.ElementAt(j).unitprice;
                                items.orderlines.Add(orderline);
                            }
                        }
                        ItemResult.Add(items);
                    }
                }

                return ItemResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }

                List<WS_orderResult1> ItemResult = new List<WS_orderResult1>();
                WS_orderResult1 items = new WS_orderResult1();
                items.Message = jsonResponse["Message"].ToString();
                ItemResult.Add(items);
                return ItemResult;
            }
        }
        public string ModifyOrder(WS_OrderRequest Orderrequest)
        {
            try
            {
                JObject jsonResponse = new JObject();

                List<WS_stockResult> ItemResult = new List<WS_stockResult>();


                var request = OperationContext.Current.RequestContext.RequestMessage;

                if (Orderrequest.IdCompany == null)
                {

                    return jsonResponse["Message"].ToString();

                }

                

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);


                ProxyNavisionWsZEN.API.Inventoriesb2c ItemsXML = new ProxyNavisionWsZEN.API.Inventoriesb2c();
                if (Orderrequest.Motif==null)
                {
                    Orderrequest.Motif = "";
                }

                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + Orderrequest.IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                string stores = "";
                
                mobile_Web_Services.updatorder(Orderrequest.refCmd, Orderrequest.reglement, Orderrequest.idStatus, Orderrequest.Motif);
                string company = mobile_Web_Services.getCompanyesp();
                
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + company + "/Codeunit/API";
                mobile_Web_Services.updatorder(Orderrequest.refCmd, Orderrequest.reglement, Orderrequest.idStatus, Orderrequest.Motif);

                return "Success";
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                
                return jsonResponse["Message"].ToString();
            }
        }
        public string addcardpoint(string idcard, string points, string IdCompany, string DocumentNo)
        {
            try
            {
                JObject jsonResponse = new JObject();

                List<WS_stockResult> ItemResult = new List<WS_stockResult>();


                var request = OperationContext.Current.RequestContext.RequestMessage;

                if ((IdCompany == null)||(IdCompany == ""))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "IdCompany obligatoire";

                }
                if ((points == null) || (points == ""))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "points obligatoire";

                }
                if ((idcard == null) || (idcard == ""))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "idcard obligatoire";

                }
                if ((DocumentNo == null) || (DocumentNo == ""))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;


                    return "DocumentNo obligatoire";

                }


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);


                ProxyNavisionWsZEN.API.Inventoriesb2c ItemsXML = new ProxyNavisionWsZEN.API.Inventoriesb2c();
             

                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                string stores = "";

                mobile_Web_Services.Addransaction(idcard, points.Replace(",","."),  DocumentNo);

                return "Success";
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }

                return jsonResponse["Message"].ToString();
            }
        }
        public List<WS_stockResult> GetStock(WS_StockrRequest Stockrequest)
        {
            try
            {
                JObject jsonResponse = new JObject();

                List<WS_stockResult> ItemResult = new List<WS_stockResult>();


                var request = OperationContext.Current.RequestContext.RequestMessage;
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                
                if (Stockrequest.IdCompany == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Veuillez spécifier l'IdCompany.";
                    WS_stockResult items = new WS_stockResult();

                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;

                }
               
                if (((Stockrequest.updatedstart == "") && (Stockrequest.updatedend != "")) || ((Stockrequest.updatedstart != "") && (Stockrequest.updatedend == "")))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Vous devez spécifier à la fois created_end et created_start.";
                    WS_stockResult items = new WS_stockResult();

                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;
                }
               
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);


                ProxyNavisionWsZEN.API.Inventoriesb2c ItemsXML = new ProxyNavisionWsZEN.API.Inventoriesb2c();


                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + Stockrequest.IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                foreach (var store in Stockrequest.Stores)
                {
                    
                    

                
                    foreach (var barreCode in Stockrequest.barreCodes)
                    {
                        mobile_Web_Services.getStockB2C(ref ItemsXML, Stockrequest.updatedstart, Stockrequest.updatedend, barreCode, store);


                        for (int i = 0; i < ItemsXML.Inventoryb2c.Count(); i++)
                        {
                            if (ItemsXML.Inventoryb2c.ElementAt(i).barcode.FirstOrDefault() != "")
                            {

                                    ProxyNavisionWsZEN.WS_stockResult items = new ProxyNavisionWsZEN.WS_stockResult();

                                    ProxyNavisionWsZEN.Variants Variants = new ProxyNavisionWsZEN.Variants();
                                    items.barreCode = ItemsXML.Inventoryb2c.ElementAt(i).barcode.FirstOrDefault();

                                    items.Store = store;
                                    items.stockAvailable = ItemsXML.Inventoryb2c.ElementAt(i).stock_disponible.FirstOrDefault();


                                    ItemResult.Add(items);
                            

                            }
                        }

                    }
                }
                return ItemResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                List<WS_stockResult> ItemResult = new List<WS_stockResult>();
                WS_stockResult items = new WS_stockResult();
                items.Message = jsonResponse["Message"].ToString();
                ItemResult.Add(items);
                return ItemResult;
            }
        }
        public List<items> getitem(string reference, string IdCompany, string created_start, string created_end, string updated_start, string updated_end)
        {
            try
            {
                JObject jsonResponse = new JObject();

                List<items> ItemResult = new List<items>();


                var request = OperationContext.Current.RequestContext.RequestMessage;
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                var rawUrl = httpRequest.QueryString; // mais QueryString n'existe pas ici

                // => Donc on récupère l'Uri complète
                var uri = request.Headers.To;
                var query = uri.Query; // ex: "?phone=123&username=john"

                // Parse manuelle de la query string
                var queryParams = System.Web.HttpUtility.ParseQueryString(query);

                if (queryParams.Count == 0)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;

                    items items = new items();

                    items.Message = "Au moins un paramètre est requis pour la recherche.";
                    ItemResult.Add(items);
                    return ItemResult;

                }
                var validParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "reference" ,"IdCompany","created_start","created_end","updated_start","updated_end"
                };

                // Vérifie les clés
                foreach (string key in queryParams)
                {
                    if (!validParams.Contains(key))
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        jsonResponse["Status"] = "400";

                        jsonResponse["Message"] = $"Paramètre invalide : '{key}'";
                        items items = new items();

                        items.Message = jsonResponse["Message"].ToString();
                        ItemResult.Add(items);
                        return ItemResult;
                    }
                }
                if (IdCompany == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Veuillez spécifier l'IdCompany.";
                    items items = new items();

                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;

                }
                if(reference==null)
                {
                    reference = "";
                }
                if (created_start == null)
                {
                    created_start = "";
                }
                if (created_end == null)
                {
                    created_end = "";
                }
                if (updated_start == null)
                {
                    updated_start = "";
                }
                if (updated_end == null)
                {
                    updated_end = "";
                }
                if (((created_end=="")&&(created_start!=""))|| ((created_end != "") && (created_start == "")))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Vous devez spécifier à la fois created_end et created_start.";
                    items items = new items();

                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;
                }
                if (((updated_start == "") && (updated_end != "")) || ((updated_start != "") && (updated_end == "")))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Vous devez spécifier à la fois updated_start et updated_end.";
                    items items = new items();

                    items.Message = jsonResponse["Message"].ToString();
                    ItemResult.Add(items);
                    return ItemResult;
                }
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
            

                ProxyNavisionWsZEN.API.Itemsb2c ItemsXML = new ProxyNavisionWsZEN.API.Itemsb2c();
              

                    mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                mobile_Web_Services.getItemb2c(ref ItemsXML, reference, created_start, created_end, updated_start, updated_end);


                for (int i = 0; i < ItemsXML.Itemb2c.Count(); i++)
                {
                    if (ItemsXML.Itemb2c.ElementAt(i).No != "")
                    {
                        ProxyNavisionWsZEN.items items = new ProxyNavisionWsZEN.items();
                        items.sku = ItemsXML.Itemb2c.ElementAt(i).No;
                        items.title = ItemsXML.Itemb2c.ElementAt(i).Description;
                        items.code_saison = ItemsXML.Itemb2c.ElementAt(i).code_saison;
                        items.Nom_saison = ItemsXML.Itemb2c.ElementAt(i).saison.FirstOrDefault();

                        //items.division = ItemsXML.Itemb2c.ElementAt(i).division;

                        items.Groupe = ItemsXML.Itemb2c.ElementAt(i).famille;
                        items.Style = ItemsXML.Itemb2c.ElementAt(i).sexe;
                        items.Famille = ItemsXML.Itemb2c.ElementAt(i).sexe;
                        items.coupe = ItemsXML.Itemb2c.ElementAt(i).Coupe;
                        items.Persona = ItemsXML.Itemb2c.ElementAt(i).Persona;
                        items.DivisionCommerciale = ItemsXML.Itemb2c.ElementAt(i).DivisionCommerciale;
                        items.CodeGroupe = ItemsXML.Itemb2c.ElementAt(i).CodeGroupe;
                        items.Ligne = ItemsXML.Itemb2c.ElementAt(i).Groupe;
                        items.GS1 = ItemsXML.Itemb2c.ElementAt(i).GS1;

                        items.numerPiece = ItemsXML.Itemb2c.ElementAt(i).numerPiece.ToString() ;

                        //items.definition = ItemsXML.Itemb2c.ElementAt(i).definition;
                        //items.Poids = ItemsXML.Itemb2c.ElementAt(i).Poids;
                        //items.CodeMarque = ItemsXML.Itemb2c.ElementAt(i).code_marque;
                        //items.Sexe = ItemsXML.Itemb2c.ElementAt(i).sexe; ;
                        //items.SerieType = ItemsXML.Itemb2c.ElementAt(i).serieType.FirstOrDefault();
                        items.Fournisseur = ItemsXML.Itemb2c.ElementAt(i).Fournisseur.FirstOrDefault();
                        items.created_at = ItemsXML.Itemb2c.ElementAt(i).created_at;
                        items.updated_at = ItemsXML.Itemb2c.ElementAt(i).updated_at;

                        items.DateReception_PSR= ItemsXML.Itemb2c.ElementAt(i).DateReception_PSR;
                        items.DateReception_TDS= ItemsXML.Itemb2c.ElementAt(i).DateReception_TDS;
                        items.date_injection = ItemsXML.Itemb2c.ElementAt(i).date_injection;


                        var acceptedDateTimeFormats0 = new[] { "MM/dd/yy hh:mm tt", "MM/dd/yyyy hh:mm tt" };
                        var acceptedDateTimeFormats1 = new[] { "MM/dd/yy hh:mm tt", "MM/dd/yyyy hh:mm tt" };


                        if (DateTime.TryParseExact(items.DateReception_PSR, acceptedDateTimeFormats0, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime0))
                        {
                            // Format: yyyy-dd-MM hh:mm tt
                            items.DateReception_PSR = parsedDateTime0.ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            items.DateReception_PSR = items.DateReception_PSR; // fallback to original if parsing fails
                        }
                        if (DateTime.TryParseExact(items.DateReception_TDS, acceptedDateTimeFormats1, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime1))
                        {
                            // Format: yyyy-dd-MM hh:mm tt
                            items.DateReception_TDS = parsedDateTime1.ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            items.DateReception_TDS = items.DateReception_TDS; // fallback to original if parsing fails
                        }
                        var acceptedFormats5 = new[] { "MM/dd/yy", "MM/dd/yyyy" };


                        if (DateTime.TryParseExact(items.date_injection, acceptedFormats5, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate100))
                        {
                            items.date_injection = parsedDate100.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            items.date_injection = items.date_injection;
                        }
                        var acceptedFormats = new[] { "MM/dd/yy", "MM/dd/yyyy" };


                        if (DateTime.TryParseExact(items.created_at, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        {
                            items.created_at = parsedDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            items.created_at = items.created_at;
                        }

                        items.updated_at = ItemsXML.Itemb2c.ElementAt(i).updated_at;
                        var acceptedDateTimeFormats = new[] { "MM/dd/yy hh:mm tt", "MM/dd/yyyy hh:mm tt" };

                        items.updated_at = ItemsXML.Itemb2c.ElementAt(i).updated_at;

                        if (DateTime.TryParseExact(items.updated_at, acceptedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
                        {
                            // Format: yyyy-dd-MM hh:mm tt
                            items.updated_at = parsedDateTime.ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            items.updated_at = items.updated_at; // fallback to original if parsing fails
                        }

                        //if (DateTime.TryParseExact(items.updated_at, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        //{
                        //    items.updated_at = parsedDate.ToString("yyyy-MM-dd");
                        //}
                        //else
                        //{
                        //    items.updated_at = items.updated_at;
                        //}
                        items.declinaisons = new List<ProxyNavisionWsZEN.Variants>();
                        items.SalesPrice = new List<ProxyNavisionWsZEN.Prices>();


                        var variantsDict = new Dictionary<string, ProxyNavisionWsZEN.Variants>();
                        if (ItemsXML.Itemb2c.ElementAt(i)?.Variants != null)
                        {

                            for (int j = 0; j < ItemsXML.Itemb2c.ElementAt(i).Variants.Count(); j++)
                            {
                                var rawVariant = ItemsXML.Itemb2c.ElementAt(i).Variants.ElementAt(j);

                                // if this color group doesn't exist yet, create it
                                if (!variantsDict.TryGetValue(rawVariant.codeCouleur, out var variantsGroup))
                                {
                                    variantsGroup = new ProxyNavisionWsZEN.Variants
                                    {
                                        codeCouleur = rawVariant.codeCouleur,
                                        Couleur = rawVariant.Couleur.FirstOrDefault()
                                    };
                                    variantsDict[rawVariant.codeCouleur] = variantsGroup;
                                }

                                // create a detail entry
                                var detail = new ProxyNavisionWsZEN.VariantDetail
                                {
                                    code = rawVariant.code,
                                    name = rawVariant.Description,
                                    Taille = rawVariant.Taille.FirstOrDefault(),
                                    code_taille = rawVariant.code_taille,
                                    ean13 = rawVariant.Barcode,
                                    Quantity_in_serie_type = rawVariant.Quantity_in_serie_type.FirstOrDefault(),
                                    Composition0 = rawVariant.Composition0,
                                    Composition1 = rawVariant.Composition1,
                                    Composition2 = rawVariant.Composition2,
                                    Composition3 = rawVariant.Composition3,
                                    NGP = rawVariant.NGP
                                };

                                // add detail under the correct color group
                                variantsGroup.variants.Add(detail);
                            }
                        }
                        // finally add all grouped variants
                        items.declinaisons = variantsDict.Values.ToList();

                        if (ItemsXML.Itemb2c.ElementAt(i)?.SalesPrice != null)
                        {
                            for (int k = 0; k < ItemsXML.Itemb2c.ElementAt(i).SalesPrice.Count(); k++)
                            {
                                ProxyNavisionWsZEN.Prices prices = new ProxyNavisionWsZEN.Prices();
                                if (ItemsXML.Itemb2c.ElementAt(i).SalesPrice.ElementAt(k).PriceTTC.FirstOrDefault() != "")
                                {
                                    prices.CurrencyCode = ItemsXML.Itemb2c.ElementAt(i).SalesPrice.ElementAt(k).CurrencyCode.FirstOrDefault();
                                    prices.DiscountPrice = ItemsXML.Itemb2c.ElementAt(i).SalesPrice.ElementAt(k).DiscountPrice.FirstOrDefault();
                                    prices.DiscountPercentage = ItemsXML.Itemb2c.ElementAt(i).SalesPrice.ElementAt(k).DiscountPercentage.FirstOrDefault();
                                    prices.PriceTTC = ItemsXML.Itemb2c.ElementAt(i).SalesPrice.ElementAt(k).PriceTTC.FirstOrDefault();
                                    prices.PriceHT = ItemsXML.Itemb2c.ElementAt(i).SalesPrice.ElementAt(k).PriceHT.FirstOrDefault();
                                    prices.prix_negoce = ItemsXML.Itemb2c.ElementAt(i).SalesPrice.ElementAt(k).prix_negoce.FirstOrDefault();

                                    items.SalesPrice.Add(prices);
                                }
                                else
                                {
                                    prices.CurrencyCode = "TND";
                                    prices.DiscountPrice = "0";
                                    prices.DiscountPercentage = "0";
                                    prices.PriceTTC = "0";
                                    prices.PriceHT = "0";


                                    items.SalesPrice.Add(prices);
                                }


                            }
                        }
                        else

                        {
                            ProxyNavisionWsZEN.Prices prices = new ProxyNavisionWsZEN.Prices();

                            prices.CurrencyCode = "TND";
                            prices.DiscountPrice = "0";
                            prices.DiscountPercentage = "0";
                            prices.PriceTTC = "0";
                            prices.PriceHT = "0";


                            items.SalesPrice.Add(prices);

                        }
                        ItemResult.Add(items);



                    }
                }
                return ItemResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                List<items> ItemResult = new List<items>();
                items items = new items();
                items.Message = jsonResponse["Message"].ToString();
                ItemResult.Add(items);
                return ItemResult;
            }
        }


        public WS_CompanyResult GetCompany()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/ZEDD/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                Company navCompany = new Company();
                mobile_Web_Services.getcompany(ref navCompany);
                WS_CompanyResult CompanyResult = new WS_CompanyResult();
                List<WS_Company> Companies = new List<WS_Company>();
                for (int i = 0; i < navCompany.Companies.Count(); i++)
                {
                    WS_Company Company = new WS_Company();
                    Company.Code = navCompany.Companies.ElementAt(i).Name;
                    Company.Description = navCompany.Companies.ElementAt(i).Display_Name;
                    Companies.Add(Company);
                }
                CompanyResult.Message = "Sucsess";
                CompanyResult.companiesList = Companies;

                return CompanyResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                WS_CompanyResult CompanyResult = new WS_CompanyResult();

                CompanyResult.Message = jsonResponse["Message"].ToString();
                return CompanyResult;
            }

        }
        public WS_CustomerResult AddCustomer(WS_CustomerRequest request)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WS_CustomerResult Customer =new WS_CustomerResult();
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/"+ request.IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                Customer.codeErp = mobile_Web_Services.AddOrModifyCustomer("", request.firstName+" "+request.lastName, request.email, request.phone, request.birthday, request.gender,false);
                Customer.status = "Success";
                Customer.message = "Client ajouté avec succès";
                return Customer;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                WS_CustomerResult Customer = new WS_CustomerResult();

                Customer.message = jsonResponse["Message"].ToString();
                Customer.status = "Error";
                return Customer;
            }
        }
        public List<WS_CustomerRequest> SearchCustomers(string IdCompany, string codeErp ,string phone)
        {
            try
            {
                JObject jsonResponse = new JObject();

                List<WS_CustomerRequest> CustomerRequests = new List<WS_CustomerRequest>();


                var request = OperationContext.Current.RequestContext.RequestMessage;
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                var rawUrl = httpRequest.QueryString; // mais QueryString n'existe pas ici

                // => Donc on récupère l'Uri complète
                var uri = request.Headers.To;
                var query = uri.Query; // ex: "?phone=123&username=john"

                // Parse manuelle de la query string
                var queryParams = System.Web.HttpUtility.ParseQueryString(query);

                if (queryParams.Count == 0)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;

                    WS_CustomerRequest CustomerRequest = new WS_CustomerRequest();

                    CustomerRequest.Message = "Au moins un paramètre est requis pour la recherche.";
                    CustomerRequests.Add(CustomerRequest);
                    return CustomerRequests;
                 
                }
                var validParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "codeErp" ,"phone","IdCompany"
                };

                // Vérifie les clés
                foreach (string key in queryParams)
                {
                    if (!validParams.Contains(key))
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        jsonResponse["Status"] = "400";

                        jsonResponse["Message"] = $"Paramètre invalide : '{key}'";
                        WS_CustomerRequest CustomerRequest = new WS_CustomerRequest();

                        CustomerRequest.Message = jsonResponse["Message"].ToString();
                        CustomerRequests.Add(CustomerRequest);
                        return CustomerRequests;
                    }
                }
                if (IdCompany == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Veuillez spécifier l'IdCompany.";
                    WS_CustomerRequest CustomerRequest = new WS_CustomerRequest();

                    CustomerRequest.Message = jsonResponse["Message"].ToString();
                    CustomerRequests.Add(CustomerRequest);
                    return CustomerRequests;
                  
                }
                if (codeErp == null)
                {
                    codeErp = "";
                }
                if (phone == null)
                {
                    phone = "";
                }
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/"+IdCompany+"/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                Contact navContact = new Contact();
                mobile_Web_Services.getcontact(codeErp, phone, ref navContact);
                WS_CustomerRequest CompanyResult = new WS_CustomerRequest();
                for (int i = 0; i < navContact.Contacts.Count(); i++)
                {
                    WS_CustomerRequest CustomerRequest = new WS_CustomerRequest();
                    CustomerRequest.phone = navContact.Contacts.ElementAt(i).phone;
                    CustomerRequest.username = navContact.Contacts.ElementAt(i).username;
                    CustomerRequest.email = navContact.Contacts.ElementAt(i).email;
                    CustomerRequest.firstName = navContact.Contacts.ElementAt(i).firstName;
                    CustomerRequest.lastName = navContact.Contacts.ElementAt(i).lastName;
                    CustomerRequest.IdCompany = IdCompany;
                    var rawBirthday = navContact.Contacts.ElementAt(i).birthday;
                    var acceptedFormats = new[] { "MM/dd/yy", "MM/dd/yyyy" };

                    if (DateTime.TryParseExact(rawBirthday, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                    {
                        CustomerRequest.birthday = parsedDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        CustomerRequest.birthday = rawBirthday; 
                    }
                    CustomerRequest.gender = navContact.Contacts.ElementAt(i).gender.FirstOrDefault();
                    CustomerRequest.codeErp = navContact.Contacts.ElementAt(i).codeErp;
                    if (navContact.Contacts.ElementAt(i).codeErp != "")
                    { CustomerRequests.Add(CustomerRequest); }
                }

                return CustomerRequests;


            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;

                    jsonResponse["Message"] = argumentException.Message;
                    jsonResponse["Status"] = "400";

                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Message"] = soapException.Message;
                    jsonResponse["Status"] = "400";


                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Message"] = error.Message;
                    jsonResponse["Status"] = "500";


                }
                WS_CustomerRequest CustomerRequest = new WS_CustomerRequest();

                CustomerRequest.Message = jsonResponse["Message"].ToString();
                List<WS_CustomerRequest> CustomerRequests = new List<WS_CustomerRequest>();
                CustomerRequests.Add(CustomerRequest);
                return CustomerRequests;

            }
        }

        public List<WS_CardRequest> card(string idcard, string codeErp,string IdCompany)
        {
            try
            {
                JObject jsonResponse = new JObject();

                var request = OperationContext.Current.RequestContext.RequestMessage;
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                var rawUrl = httpRequest.QueryString; // mais QueryString n'existe pas ici

                // => Donc on récupère l'Uri complète
                var uri = request.Headers.To;
                var query = uri.Query; 
                var queryParams = System.Web.HttpUtility.ParseQueryString(query);
                List<WS_CardRequest> CustomerRequests = new List<WS_CardRequest>();

                if (queryParams.Count == 0)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Au moins un paramètre est requis pour la recherche.";
                    WS_CardRequest CustomerRequest = new WS_CardRequest();

                    CustomerRequest.Message = jsonResponse["Message"].ToString();
                    CustomerRequests.Add(CustomerRequest);
                    return CustomerRequests;
                }
                var validParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "codeErp" ,"idcard","IdCompany"
                };

                // Vérifie les clés
                foreach (string key in queryParams)
                {
                    if (!validParams.Contains(key))
                    {
                            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                            jsonResponse["Status"] = "400";

                            jsonResponse["Message"] = $"Paramètre invalide : '{key}'";
                            WS_CardRequest CustomerRequest = new WS_CardRequest();

                            CustomerRequest.Message = jsonResponse["Message"].ToString();
                            CustomerRequests.Add(CustomerRequest);
                            return CustomerRequests;
                    }
                }
                if (IdCompany == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    WS_CardRequest CustomerRequest = new WS_CardRequest();

                    CustomerRequest.Message = "Veuillez spécifier l'IdCompany";
                    CustomerRequests.Add(CustomerRequest);
                    return CustomerRequests;

                    
                }
                if (codeErp == null)
                {
                    codeErp = "";
                }
                if (idcard == null)
                {
                    idcard = "";
                }
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                Card navCard = new Card();
                mobile_Web_Services.getCard(idcard, codeErp, ref navCard);
                for (int i = 0; i < navCard.Cards.Count(); i++)
                {
                    WS_CardRequest CustomerRequest = new WS_CardRequest();
                    CustomerRequest.idCard = navCard.Cards.ElementAt(i).idCard;
                    CustomerRequest.points = navCard.Cards.ElementAt(i).points.FirstOrDefault();
                    CustomerRequest.programName = navCard.Cards.ElementAt(i).programName;
                    CustomerRequest.status = navCard.Cards.ElementAt(i).Status;
                    var rawactivationDate = navCard.Cards.ElementAt(i).activationDate.FirstOrDefault();
                    var acceptedFormats = new[] { "MM/dd/yy", "MM/dd/yyyy" };

                    if (DateTime.TryParseExact(rawactivationDate, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                    {
                        CustomerRequest.activationDate = parsedDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        CustomerRequest.activationDate = rawactivationDate;
                    }

                    CustomerRequests.Add(CustomerRequest);
                }

                return CustomerRequests;


            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Message"] = soapException.Message;
                    jsonResponse["Status"] = "400";


                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";

                    jsonResponse["Message"] = error.Message;

                }
                WS_CardRequest CustomerRequest = new WS_CardRequest();

                CustomerRequest.Message = jsonResponse["Message"].ToString();
                List<WS_CardRequest> CustomerRequests = new List<WS_CardRequest>();
                CustomerRequests.Add(CustomerRequest);
                return CustomerRequests;

            }
        }
        public List<WS_TransRequest> Gettransaction(string idcard, string IdCompany)
        {
            try
            {
                JObject jsonResponse = new JObject();

                var request = OperationContext.Current.RequestContext.RequestMessage;
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                var rawUrl = httpRequest.QueryString; // mais QueryString n'existe pas ici

                // => Donc on récupère l'Uri complète
                var uri = request.Headers.To;
                var query = uri.Query;
                var queryParams = System.Web.HttpUtility.ParseQueryString(query);
                List<WS_TransRequest> CustomerRequests = new List<WS_TransRequest>();

                if (idcard == null) 
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "idcard est onligatoire.";
                    WS_TransRequest CustomerRequest = new WS_TransRequest();

                    CustomerRequest.Message = jsonResponse["Message"].ToString();
                    CustomerRequests.Add(CustomerRequest);
                    return CustomerRequests;
                }
                if (IdCompany == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "IdCompany est onligatoire.";
                    WS_TransRequest CustomerRequest = new WS_TransRequest();

                    CustomerRequest.Message = jsonResponse["Message"].ToString();
                    CustomerRequests.Add(CustomerRequest);
                    return CustomerRequests;
                }
                var validParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "idcard","IdCompany"
                };

                // Vérifie les clés
                foreach (string key in queryParams)
                {
                    if (!validParams.Contains(key))
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        jsonResponse["Status"] = "400";

                        jsonResponse["Message"] = $"Paramètre invalide : '{key}'";
                        WS_TransRequest CustomerRequest = new WS_TransRequest();

                        CustomerRequest.Message = jsonResponse["Message"].ToString();
                        CustomerRequests.Add(CustomerRequest);
                        return CustomerRequests;
                    }
                }
                
                
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                transaction navCard = new transaction();
                mobile_Web_Services.getransaction(idcard,  ref navCard);
                for (int i = 0; i < navCard.transactions.Count(); i++)
                {
                    WS_TransRequest CustomerRequest = new WS_TransRequest();
                    CustomerRequest.date = navCard.transactions.ElementAt(i).date;
                    CustomerRequest.points = navCard.transactions.ElementAt(i).points;
                    CustomerRequest.orderId = navCard.transactions.ElementAt(i).orderId;
                    CustomerRequest.Type_écriture = navCard.transactions.ElementAt(i).typeecriture;
                    var rawactivationDate = navCard.transactions.ElementAt(i).date;
                    var acceptedFormats = new[] { "MM/dd/yy", "MM/dd/yyyy" };

                    if (DateTime.TryParseExact(rawactivationDate, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                    {
                        CustomerRequest.date = parsedDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        CustomerRequest.date = rawactivationDate;
                    }

                    CustomerRequests.Add(CustomerRequest);
                }

                return CustomerRequests;


            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Message"] = soapException.Message;
                    jsonResponse["Status"] = "400";


                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";

                    jsonResponse["Message"] = error.Message;

                }
                WS_TransRequest CustomerRequest = new WS_TransRequest();

                CustomerRequest.Message = jsonResponse["Message"].ToString();
                List<WS_TransRequest> CustomerRequests = new List<WS_TransRequest>();
                CustomerRequests.Add(CustomerRequest);
                return CustomerRequests;

            }
        }
        public List<WS_PurchRequest> GetPurchase(string codeErp, string IdCompany)
        {
            try
            {
                JObject jsonResponse = new JObject();

                var request = OperationContext.Current.RequestContext.RequestMessage;
                var httpRequest = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
                var rawUrl = httpRequest.QueryString; // mais QueryString n'existe pas ici

                // => Donc on récupère l'Uri complète
                var uri = request.Headers.To;
                var query = uri.Query;
                var queryParams = System.Web.HttpUtility.ParseQueryString(query);
                List<WS_PurchRequest> CustomerRequests = new List<WS_PurchRequest>();

                if (codeErp == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "idcard est onligatoire.";
                    WS_PurchRequest CustomerRequest = new WS_PurchRequest();

                    CustomerRequest.Message = jsonResponse["Message"].ToString();
                    CustomerRequests.Add(CustomerRequest);
                    return CustomerRequests;
                }
                if (IdCompany == null)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "IdCompany est onligatoire.";
                    WS_PurchRequest CustomerRequest = new WS_PurchRequest();

                    CustomerRequest.Message = jsonResponse["Message"].ToString();
                    CustomerRequests.Add(CustomerRequest);
                    return CustomerRequests;
                }
                var validParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "codeErp","IdCompany"
                };

                // Vérifie les clés
                foreach (string key in queryParams)
                {
                    if (!validParams.Contains(key))
                    {
                        WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                        jsonResponse["Status"] = "400";

                        jsonResponse["Message"] = $"Paramètre invalide : '{key}'";
                        WS_PurchRequest CustomerRequest = new WS_PurchRequest();

                        CustomerRequest.Message = jsonResponse["Message"].ToString();
                        CustomerRequests.Add(CustomerRequest);
                        return CustomerRequests;
                    }
                }


                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                Sales navCard = new Sales();
                mobile_Web_Services.getSales(codeErp, ref navCard);
                for (int i = 0; i < navCard.Sales_Entry.Count(); i++)
                {
                    WS_PurchRequest CustomerRequest = new WS_PurchRequest();
                    
                    CustomerRequest.reference = navCard.Sales_Entry.ElementAt(i).reference;
                    CustomerRequest.ean13 = navCard.Sales_Entry.ElementAt(i).Variants[0].ean13;
                    CustomerRequest.couleur = navCard.Sales_Entry.ElementAt(i).Variants[0].Couleur.FirstOrDefault();
                    CustomerRequest.taille = navCard.Sales_Entry.ElementAt(i).Variants[0].Taille.FirstOrDefault();

                    var rawactivationDate = navCard.Sales_Entry.ElementAt(i).date;
                    var acceptedFormats = new[] { "MM/dd/yy", "MM/dd/yyyy" };

                    if (DateTime.TryParseExact(rawactivationDate, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                    {
                        CustomerRequest.date = parsedDate.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        CustomerRequest.date = rawactivationDate;
                    }
                    if (CustomerRequest.reference != "") {
                        CustomerRequests.Add(CustomerRequest);
                    }
                }

                return CustomerRequests;


            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Message"] = soapException.Message;
                    jsonResponse["Status"] = "400";


                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";

                    jsonResponse["Message"] = error.Message;

                }
                WS_PurchRequest CustomerRequest = new WS_PurchRequest();

                CustomerRequest.Message = jsonResponse["Message"].ToString();
                List<WS_PurchRequest> CustomerRequests = new List<WS_PurchRequest>();
                CustomerRequests.Add(CustomerRequest);
                return CustomerRequests;

            }
        }


        public WS_CustomerResult ModifyCustomer(WS_CustomerRequest request)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WS_CustomerResult Customer = new WS_CustomerResult();
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + request.IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                Customer.codeErp = mobile_Web_Services.AddOrModifyCustomer(request.codeErp,request.firstName+" "+request.lastName, request.email, request.phone, request.birthday, request.gender, true);
                Customer.status = "Success";
                Customer.message = "Client mis à jour avec succès";
                return Customer;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                WS_CustomerResult Customer = new WS_CustomerResult();

                Customer.message = jsonResponse["Message"].ToString();
                Customer.status = "Error";
                return Customer;
            }
        }



        
        public WS_OrderResult AddOrder(CmdHead CmdHead, DeliveryAddress DeliveryAddress, List<Order_list> Order_list)
        {
            try
            {
                if (CmdHead.orderNo == null)
                {
                    CmdHead.orderNo = "NULL";
                }
              
                if (CmdHead.TiersColisNo == null)
                {
                    CmdHead.TiersColisNo = "NULL";
                }
                if (CmdHead.Currency == null)
                {
                    CmdHead.Currency = "NULL";
                }
                if (CmdHead.Currency_Ratio == null)
                {
                    CmdHead.Currency_Ratio = "1";
                }
                if (CmdHead.Currency_Ratio == null)
                {
                    CmdHead.Currency_Ratio = "1";
                }
                if (CmdHead.Date == null)
                {
                    CmdHead.Date = "";
                }
                if (DeliveryAddress==null)
                {
                    DeliveryAddress= new DeliveryAddress();
                    DeliveryAddress.Address = "NULL";
                    DeliveryAddress.City = "";
                    DeliveryAddress.CountryId = "";
                    DeliveryAddress.FirstName = "";
                    DeliveryAddress.LastName = "";
                    DeliveryAddress.phoneNumber = "";

                }
                else { 
                if (DeliveryAddress.Address == null)
                {
                    DeliveryAddress.Address = "NULL";
                }
                if (DeliveryAddress.City == null)
                {
                    DeliveryAddress.City = "";
                }
                if (DeliveryAddress.CountryId == null)
                {
                    DeliveryAddress.CountryId = "";
                }
                if (DeliveryAddress.FirstName == null)
                {
                    DeliveryAddress.FirstName = "";
                }
                if (DeliveryAddress.LastName == null)
                {
                    DeliveryAddress.LastName = "";
                }
                if (DeliveryAddress.phoneNumber == null)
                {
                    DeliveryAddress.phoneNumber = "";
                }
                }
                CmdHead.Currency_Ratio = CmdHead.Currency_Ratio.Replace(",", ".");

                if (CmdHead.Currency_Ratio=="")
                {
                    CmdHead.Currency_Ratio = "1";
                }

                int TotalRecords = 0;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                WS_OrderResult WS_orderResult = new WS_OrderResult();
                API.Order OrderXmlPort = new API.Order();
                List<API.Orders> lines = new List<API.Orders>();
                int id = 0;
                if (Order_list!=null) {
                    foreach (var order in Order_list)
                    {
                        if (order.Barcode == null)
                        {
                            order.Barcode = "NULL";
                        }
                        if (order.Quantity == null)
                        {
                            order.Quantity = "0";
                        }
                        if (order.Unit_Price == null)
                        {
                            order.Unit_Price = "0";
                        }
                        if (order.Id == null)
                        {
                            order.Id = "NULL";
                        }
                        if (order.Location == null)
                        {
                            order.Location = "NULL";
                        }
                        if (order.Discount == null)
                        {
                            order.Discount = "0";
                        }
                        if (order.Motif == null)
                        {
                            order.Motif = "";
                        }

                        order.Unit_Price = order.Unit_Price.Replace(",", ".");
                        if (CmdHead.commandType != "order")
                        {
                            if ((order.Id == null) || (order.Id == "NULL"))
                            {
                                id = id + 1;
                                order.Id = id.ToString();

                            }
                        }
                        API.Orders lineXmlPort = new API.Orders
                        {
                            Barcode = order.Barcode,
                            Quantity = order.Quantity,
                            Unit_Price = order.Unit_Price,
                            ID = order.Id,
                            location = CmdHead.Location,
                            OrderNo = CmdHead.orderNo,
                            Discount = order.Discount,
                            motif = order.Motif


                        };

                        lines.Add(lineXmlPort);

                    }
                   
                }
                else
                {
                    API.Orders lineXmlPort = new API.Orders
                        {
                            Barcode = "NULL",
                            Quantity = "0",
                            Unit_Price = "0",
                            ID = "NULL",
                            location = "NULL",
                            OrderNo = CmdHead.orderNo,
                            Discount = "0"


                        };
                    lines.Add(lineXmlPort);
                }
                OrderXmlPort.Orders = lines.ToArray();
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/"+ CmdHead.IdCompany+"/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                if ((CmdHead.commandType != "order")&&(CmdHead.commandType != "orderReturn"))
                {
                    JObject jsonResponse = new JObject();

                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;

                    WS_orderResult.Message = "commandType doit étre soit order soit orderReturn";
                    return WS_orderResult;
                }
                if (CmdHead.idStore==null)
                {
                    CmdHead.idStore = "";
                }
                if (CmdHead.DeliveryType == null)
                {
                    CmdHead.DeliveryType = "";
                }
                if (CmdHead.Remise_Coupon == null)
                {
                    CmdHead.Remise_Coupon = "0";
                }

                if (CmdHead.commandType== "order")
                {

                    //mobile_Web_Services.AddOrModifyOrderB2C(CmdHead.TiersColisNo, CmdHead.CustomerCodeErp, CmdHead.OrderNo, CmdHead.Currency, CmdHead.Currency_Ratio, DeliveryAddress.Address, false, CmdHead.IdPaymentMethod, CmdHead.Date, DeliveryAddress.City, DeliveryAddress.CountryId, DeliveryAddress.FirstName, DeliveryAddress.LastName, DeliveryAddress.PhoneNumber, ref OrderXmlPort, CmdHead.DeliveryType, CmdHead.idStore, CmdHead.Remise_Coupon);
                    WS_orderResult.Message = mobile_Web_Services.AddOrModifyOrderB2C(CmdHead.TiersColisNo, CmdHead.CustomerCodeErp, CmdHead.OrderNo, CmdHead.Currency, CmdHead.Currency_Ratio, DeliveryAddress.Address, false, CmdHead.IdPaymentMethod, CmdHead.Date, DeliveryAddress.City, DeliveryAddress.CountryId, DeliveryAddress.FirstName, DeliveryAddress.LastName, DeliveryAddress.PhoneNumber, ref OrderXmlPort, CmdHead.DeliveryType, CmdHead.idStore, CmdHead.Remise_Coupon);

                    string company = mobile_Web_Services.getCompanyesp();
                    string location = mobile_Web_Services.getLocationesp();
                    string customer = mobile_Web_Services.getCustomeresp();
                    foreach (var order in OrderXmlPort.Orders)
                    {
                        order.location = location;
                        order.Unit_Price = "0";
                    }
                    mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + company + "/Codeunit/API";
                    mobile_Web_Services.AddOrModifyOrder(CmdHead.TiersColisNo, customer, CmdHead.OrderNo, CmdHead.Currency, CmdHead.Currency_Ratio, DeliveryAddress.Address, CmdHead.IdCompany, false, ref OrderXmlPort, "");

                }
                else
                {
                    WS_orderResult.Message = mobile_Web_Services.AddOrModifySalesReturnB2C(CmdHead.TiersColisNo, CmdHead.CustomerCodeErp, CmdHead.OrderNo, CmdHead.Currency, CmdHead.Currency_Ratio, DeliveryAddress.Address, false, CmdHead.IdPaymentMethod, CmdHead.Date, DeliveryAddress.City, DeliveryAddress.CountryId, DeliveryAddress.FirstName, DeliveryAddress.LastName, DeliveryAddress.PhoneNumber, CmdHead.refundType, CmdHead.rib, ref OrderXmlPort);

                }
                //WS_orderResult.Message = "Success";
                return WS_orderResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                WS_OrderResult WS_orderResult = new WS_OrderResult();

                WS_orderResult.Message = jsonResponse["Message"].ToString();
                return WS_orderResult;
            }
        }
        public WS_ImageResult AddImage(string ItemNo, string ImageUrl)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                WS_ImageResult WS_imageResult = new WS_ImageResult();

                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/ESP/Codeunit/API";

                mobile_Web_Services.Timeout = 1000000000;

                mobile_Web_Services.AddImage(ItemNo, ImageUrl);
               
                WS_imageResult.Message = "Success";
                return WS_imageResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                WS_ImageResult WS_imageResult = new WS_ImageResult();

                WS_imageResult.Message = jsonResponse["Message"].ToString();
                return WS_imageResult;
            }
        }
        public WS_LocationResult GetLocation(string IdCompany)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                ProxyNavisionWsZEN.API.Location navLocation = new ProxyNavisionWsZEN.API.Location();

                WS_LocationResult LocationResult = new WS_LocationResult();
                List<WS_Location> Locations = new List<WS_Location>();
              
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + IdCompany + "/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                mobile_Web_Services.getlocation(ref navLocation);
                for (int i = 0; i < navLocation.Locations.Count(); i++)
                {
                    WS_Location Location = new WS_Location();
                    Location.Code = navLocation.Locations.ElementAt(i).code;
                    Location.Description = navLocation.Locations.ElementAt(i).Name;
                    Location.Store = navLocation.Locations.ElementAt(i).storef.FirstOrDefault();
                    Location.Company = IdCompany;
                    Locations.Add(Location);
                }
                
                LocationResult.Message = "Sucsess";
                LocationResult.LocationList = Locations;

                return LocationResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                WS_LocationResult LocationResult = new WS_LocationResult();

                LocationResult.Message = jsonResponse["Message"].ToString();
                return LocationResult;
            }
        }
        public WS_categoryResult getListCategorie(string IdCompany)
        {
            try
            {
                int TotalRecords = 0;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                WS_categoryResult WS_categoryResult = new WS_categoryResult();

                Category navCategory = new Category();
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/"+ IdCompany+"/Codeunit/API";

                mobile_Web_Services.getCategory(ref navCategory);
                List<getListCategorie> items = new List<getListCategorie>();
                for (int i = 0; i < navCategory.Categories.Count(); i++)
                {
                    getListCategorie item = new getListCategorie();
                    item.code_categorie = navCategory.Categories.ElementAt(i).Code.Replace("\"", "");
                    item.code_Parent = "" + navCategory.Categories.ElementAt(i).valeur_parent.Replace("\"", "");
                    item.name = "" + navCategory.Categories.ElementAt(i).valeur.Replace("\"", "");
                    items.Add(item);
                }
                Categoryparent Categoryparent = new Categoryparent();
                mobile_Web_Services.getParentcategory(ref Categoryparent);
                for (int i = 0; i < Categoryparent.Categoriesparent.Count(); i++)
                {
                    getListCategorie item = new getListCategorie();
                    item.code_categorie = Categoryparent.Categoriesparent.ElementAt(i).Code.Replace("\"", "");
                    item.code_Parent = "" + Categoryparent.Categoriesparent.ElementAt(i).valeur_parent.Replace("\"", "");
                    item.name = "" + Categoryparent.Categoriesparent.ElementAt(i).valeur.Replace("\"", "");
                    items.Add(item);
                }
                Parent Parent = new Parent();
                mobile_Web_Services.getParent(ref Parent);
                for (int i = 0; i < Parent.Parents.Count(); i++)
                {
                    getListCategorie item = new getListCategorie();
                    item.code_categorie = Parent.Parents.ElementAt(i).Code.Replace("\"", "");
                    item.code_Parent = "0";
                    item.name = "" + Parent.Parents.ElementAt(i).valeur.Replace("\"", "");
                    items.Add(item);
                }
                WS_categoryResult.Message = "Success";
                WS_categoryResult.GetListCategorie = items;
                return WS_categoryResult;
            }
            catch (Exception error)
            {
                JObject jsonResponse = new JObject();
                if (error is ArgumentException argumentException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = argumentException.Message;
                }
                else if (error is SoapException soapException)
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = soapException.Message;
                }
                else
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                    jsonResponse["Status"] = "500";
                    jsonResponse["Message"] = error.Message;
                }
                WS_categoryResult WS_categoryResult = new WS_categoryResult();

                WS_categoryResult.Message = jsonResponse["Message"].ToString();
                return WS_categoryResult;
            }
        }
        public static string GetAccessToken()
        {
            using (var client = new HttpClient())
            {
                var tokenEndpoint = "https://login.microsoftonline.com/e18fb4b5-9142-4516-a5f8-8de91c4e5681/oauth2/v2.0/token";
                var clientId = "3ecaaffa-ede3-4c98-8a80-80755473fb7a";
                var clientSecret = "UwJ8Q~n_b10U8AgXOLcm4a.Cpc8eVDBgUpbzodlf";
                var scope = "https://api.businesscentral.dynamics.com/.default";

                var request = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint);
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"client_id", clientId},
            {"scope", scope},
            {"client_secret", clientSecret},
            {"grant_type", "client_credentials"}
        });

                var response = client.SendAsync(request).Result;

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Token request failed: " + response.ReasonPhrase);

                var content = response.Content.ReadAsStringAsync().Result;
                var token = JsonDocument.Parse(content).RootElement.GetProperty("access_token").GetString();

                return token;
            }
        }




    }
}

