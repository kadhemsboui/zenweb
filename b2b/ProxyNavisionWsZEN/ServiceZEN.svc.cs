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

namespace ProxyNavisionWsZEN
{
 public class Service1 : IService1
    {
        
        
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
                WS_CompanyResult CompanyResult = new WS_CompanyResult();

                CompanyResult.Message = jsonResponse["Message"].ToString();
                return CompanyResult;
            }

        }
        
        public WS_ItemResult getitem(string No, string Division_commerciale, List<Companies> Companies, List<Locations> Locations, string updated_start, string updated_end, string created_start, string created_end)
        {
            try
            {
                WS_ItemResult ItemResult = new WS_ItemResult();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WS_CustomerResult Customer = new WS_CustomerResult();
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                string magasin = "";
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
                foreach (var Line in Locations)
                {
                    if (magasin=="")
                    {
                        magasin = Line.code;
                    }
                    else
                    {
                        magasin = magasin + "|" + Line.code;
                    }

                }
                JObject jsonResponse = new JObject();

                ItemResult.Items = new List<ProxyNavisionWsZEN.items>();
                if (((created_end == "") && (created_start != "")) || ((created_end != "") && (created_start == "")))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Vous devez spécifier à la fois created_end et created_start.";
                    items items = new items();

                    ItemResult.Message = jsonResponse["Message"].ToString();
                    return ItemResult;
                }
                if (((updated_end == "") && (updated_start != "")) || ((updated_end != "") && (updated_start == "")))
                {
                    WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.BadRequest;
                    jsonResponse["Status"] = "400";

                    jsonResponse["Message"] = "Vous devez spécifier à la fois updated_end et updated_start.";
                    items items = new items();

                    ItemResult.Message = jsonResponse["Message"].ToString();
                    return ItemResult;
                }
                ProxyNavisionWsZEN.API.Items ItemsXML = new ProxyNavisionWsZEN.API.Items();
                foreach (var company in Companies.GroupBy(c => c.code).Select(g => g.First()))
                {
                   
                    mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + company.code + "/Codeunit/API";
                    mobile_Web_Services.Timeout = 1000000000;

                    mobile_Web_Services.getItem(ref ItemsXML, No,magasin, Division_commerciale, created_start, created_end, updated_start, updated_end);
             
                
                    for (int i = 0; i < ItemsXML.Item.Count(); i++)
                    {
                        if (ItemsXML.Item.ElementAt(i).No != "") { 
                            ProxyNavisionWsZEN.items items =new ProxyNavisionWsZEN.items();
                            items.No= ItemsXML.Item.ElementAt(i).No;
                            items.Description = ItemsXML.Item.ElementAt(i).Description;
                            items.Barcode = ItemsXML.Item.ElementAt(i).Barcode.FirstOrDefault();
                            items.Category = ItemsXML.Item.ElementAt(i).DescriptionCategory.FirstOrDefault();
                            items.Code_marque = ItemsXML.Item.ElementAt(i).code_marque;
                            items.Sexe= ItemsXML.Item.ElementAt(i).sexe; ;
                            items.Serie_type= ItemsXML.Item.ElementAt(i).serieType.FirstOrDefault();
                            items.Variants = new List<ProxyNavisionWsZEN.variants>();
                            items.SalesPrice = new List<ProxyNavisionWsZEN.Prices>();
                            items.created_at = ItemsXML.Item.ElementAt(i).created_at;
                            items.updated_at = ItemsXML.Item.ElementAt(i).updated_at;
                            var acceptedFormats = new[] { "MM/dd/yy", "MM/dd/yyyy" };

                            if (DateTime.TryParseExact(items.created_at, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                            {
                                items.created_at = parsedDate.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                items.created_at = items.created_at;
                            }
                            var acceptedDateTimeFormats = new[] { "MM/dd/yy hh:mm tt", "MM/dd/yyyy hh:mm tt" };

                            items.updated_at = ItemsXML.Item.ElementAt(i).updated_at;

                            if (DateTime.TryParseExact(items.updated_at, acceptedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
                            {
                                // Format: yyyy-dd-MM hh:mm tt
                                items.updated_at = parsedDateTime.ToString("yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                items.updated_at = items.updated_at; // fallback to original if parsing fails
                            }
                            if (ItemsXML.Item.ElementAt(i)?.Variants != null)
                            {

                                for (int j = 0; j < ItemsXML.Item.ElementAt(i).Variants.Count(); j++)
                                {
                                    ProxyNavisionWsZEN.variants Variants = new ProxyNavisionWsZEN.variants();

                                    Variants.Barcode = ItemsXML.Item.ElementAt(i).Variants.ElementAt(j).Barcode;
                                    Variants.Couleur = ItemsXML.Item.ElementAt(i).Variants.ElementAt(j).Couleur.FirstOrDefault();
                                    Variants.Taille = ItemsXML.Item.ElementAt(i).Variants.ElementAt(j).Taille.FirstOrDefault();
                                    Variants.Quantity_in_serie_type = ItemsXML.Item.ElementAt(i).Variants.ElementAt(j).Quantity_in_serie_type.FirstOrDefault();
                                    Variants.Stock_disponible = ItemsXML.Item.ElementAt(i).Variants.ElementAt(j).stock_disponible.FirstOrDefault();
                                    Variants.Stock_en_attente_de_livraison = ItemsXML.Item.ElementAt(i).Variants.ElementAt(j).Stock_en_attente_de_livraison.FirstOrDefault();

                                    Variants.Stock_receptionné = ItemsXML.Item.ElementAt(i).Variants.ElementAt(j).Stock_receptionné.FirstOrDefault();

                                    Variants.Stock_sur_commande_achat = ItemsXML.Item.ElementAt(i).Variants.ElementAt(j).Stock_sur_commande_achat.FirstOrDefault();

                                    items.Variants.Add(Variants);
                                }
                            }
                            if (ItemsXML.Item.ElementAt(i)?.SalesPrice != null)
                            {
                                for (int k = 0; k < ItemsXML.Item.ElementAt(i).SalesPrice.Count(); k++)
                                {
                                    ProxyNavisionWsZEN.Prices prices = new ProxyNavisionWsZEN.Prices();
                                    if (ItemsXML.Item.ElementAt(i).SalesPrice.ElementAt(k).Price.FirstOrDefault() != "")
                                    {
                                        prices.CurrencyCode = ItemsXML.Item.ElementAt(i).SalesPrice.ElementAt(k).CurrencyCode.FirstOrDefault();
                                        prices.DiscountPrice = ItemsXML.Item.ElementAt(i).SalesPrice.ElementAt(k).DiscountPrice.FirstOrDefault();
                                        prices.DiscountPercentage = ItemsXML.Item.ElementAt(i).SalesPrice.ElementAt(k).DiscountPercentage.FirstOrDefault();
                                        prices.Price = ItemsXML.Item.ElementAt(i).SalesPrice.ElementAt(k).Price.FirstOrDefault();

                                        items.SalesPrice.Add(prices);
                                    }
                                    else
                                    {
                                        prices.CurrencyCode = "TND";
                                        prices.DiscountPrice = "0";
                                        prices.DiscountPercentage = "0";
                                        prices.Price = "0";

                                        items.SalesPrice.Add(prices);
                                    }
                                }
                            }

                            ItemResult.Items.Add(items);


                        }


                    }
                }
                ItemResult.Message = "Success";
                return ItemResult;
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
                WS_ItemResult ItemResult = new WS_ItemResult();

                ItemResult.Message = jsonResponse["Message"].ToString();
                return ItemResult;
            }
        }
        public WS_InventoryResult getstock(string barcode, string Division_commerciale, List<Companies> Companies, List<Locations> Locations)
        {
            try
            {
                WS_InventoryResult InventoryResult = new WS_InventoryResult();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                WS_CustomerResult Customer = new WS_CustomerResult();
                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                string magasin = "";
                foreach (var Line in Locations)
                {
                    if (magasin == "")
                    {
                        magasin = Line.code;
                    }
                    else
                    {
                        magasin = magasin + "|" + Line.code;
                    }

                }
                InventoryResult.Stock = new List<ProxyNavisionWsZEN.Stock>();

                ProxyNavisionWsZEN.API.Inventories ItemsXML = new ProxyNavisionWsZEN.API.Inventories();
                foreach (var company in Companies.GroupBy(c => c.code).Select(g => g.First()))
                {

                    mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/" + company.code + "/Codeunit/API";
                    mobile_Web_Services.Timeout = 1000000000;

                    mobile_Web_Services.getStock(ref ItemsXML, barcode, magasin, Division_commerciale);


                    for (int i = 0; i < ItemsXML.Inventory.Count(); i++)
                    {
                        
                            ProxyNavisionWsZEN.Stock items = new ProxyNavisionWsZEN.Stock();
                            items.Stock_disponible = ItemsXML.Inventory.ElementAt(i).stock_disponible.FirstOrDefault();
                        items.Stock_en_attente_de_livraison = ItemsXML.Inventory.ElementAt(i).Stock_en_attente_de_livraison.FirstOrDefault();

                        items.Stock_receptionné = ItemsXML.Inventory.ElementAt(i).Stock_receptionné.FirstOrDefault();

                        items.Stock_sur_commande_achat = ItemsXML.Inventory.ElementAt(i).Stock_sur_commande_achat.FirstOrDefault();


                        items.Stock_sur_commande_vente= ItemsXML.Inventory.ElementAt(i).Stock_sur_commande_vente.FirstOrDefault();


                        InventoryResult.Stock.Add(items);


                        


                    }
                }
                InventoryResult.Message = "Success";
                return InventoryResult;
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
                WS_InventoryResult ItemResult = new WS_InventoryResult();

                ItemResult.Message = jsonResponse["Message"].ToString();
                return ItemResult;
            }
        }
        public WS_Customerresult GetCustomer()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/ESP/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                Customer navCustomer = new Customer();
                mobile_Web_Services.getCustomer(ref navCustomer);
                WS_Customerresult CustomerResult = new WS_Customerresult();
                List<WS_Customer> Customers = new List<WS_Customer>();
                for (int i = 0; i < navCustomer.Customers.Count(); i++)
                {
                    WS_Customer Customer = new WS_Customer();
                    Customer.Code = navCustomer.Customers.ElementAt(i).code;
                    Customer.Name = navCustomer.Customers.ElementAt(i).Name;
                    Customer.Currency = navCustomer.Customers.ElementAt(i).Currency;
                    Customer.Type = navCustomer.Customers.ElementAt(i).Type;
                    Customer.Mail = navCustomer.Customers.ElementAt(i).EMail;
                    Customer.Phone = navCustomer.Customers.ElementAt(i).Phone;
                    Customer.Address = navCustomer.Customers.ElementAt(i).Address;
                    Customer.Address2 = navCustomer.Customers.ElementAt(i).Address2;
                    Customer.Posting_Group = navCustomer.Customers.ElementAt(i).Posting_Group;
                    Customer.City = navCustomer.Customers.ElementAt(i).City;
                    Customer.Code_postal = navCustomer.Customers.ElementAt(i).Code_postal;
                    var acceptedDateTimeFormats = new[] { "MM/dd/yy", "MM/dd/yyyy" };

                    Customer.Date_Created = navCustomer.Customers.ElementAt(i).Date_Created;

                    if (DateTime.TryParseExact(Customer.Date_Created, acceptedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
                    {
                        Customer.Date_Created = parsedDateTime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    
                    var acceptedDateTimeFormats2 = new[] { "MM/dd/yy", "MM/dd/yyyy" };

                    Customer.Last_Date_Modified = navCustomer.Customers.ElementAt(i).Last_Date_Modified;

                    if (DateTime.TryParseExact(Customer.Last_Date_Modified, acceptedDateTimeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime2))
                    {
                        Customer.Last_Date_Modified = parsedDateTime2.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }

                    Customer.pays = navCustomer.Customers.ElementAt(i).pays.FirstOrDefault();
                    Customers.Add(Customer);
                }
                CustomerResult.Message = "Sucsess";
                CustomerResult.CustomersList = Customers;

                return CustomerResult;
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
                WS_Customerresult CustomerResult = new WS_Customerresult();

                CustomerResult.Message = jsonResponse["Message"].ToString();
                return CustomerResult;
            }

        }
        public WS_LocationResult GetLocation(List<Companies> Companies)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                ProxyNavisionWsZEN.API.Location navLocation = new ProxyNavisionWsZEN.API.Location();

                WS_LocationResult LocationResult = new WS_LocationResult();
                List<WS_Location> Locations = new List<WS_Location>();
                foreach (var company in Companies.GroupBy(c => c.code).Select(g => g.First()))
                {
                    mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/"+company.Code+"/Codeunit/API";
                    mobile_Web_Services.Timeout = 1000000000;

                    mobile_Web_Services.getlocation(ref navLocation);
                    for (int i = 0; i < navLocation.Locations.Count(); i++)
                    {
                        WS_Location Location = new WS_Location();
                        Location.Code = navLocation.Locations.ElementAt(i).code;
                        Location.Description = navLocation.Locations.ElementAt(i).Name;
                        Location.Store= navLocation.Locations.ElementAt(i).storef.FirstOrDefault();
                        Location.Company = company.Code;

                        Locations.Add(Location);
                    }
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
                WS_LocationResult LocationResult = new WS_LocationResult();

                LocationResult.Message = jsonResponse["Message"].ToString();
                return LocationResult;
            }
        }
        public WS_categoryResult getListCategorie()
        {
            try
            {
                int TotalRecords = 0;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                WS_categoryResult WS_categoryResult = new WS_categoryResult();

                Category navCategory = new Category();
                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/ESP/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

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
                WS_categoryResult WS_categoryResult = new WS_categoryResult();

                WS_categoryResult.Message = jsonResponse["Message"].ToString();
                return WS_categoryResult;
            }
        }
        public WS_OrderResult AddOrder(string orderNo, string customerNo, string colisno, string currency, string currency_Ratio, string address, List<Order_list> Order_list)
        {
            try
            {
                if (orderNo == null) {
                    orderNo = "NULL";
                }
                if (customerNo == null)
                {
                    customerNo = "NULL";
                }
                if (colisno == null)
                {
                    colisno = "NULL";
                }
                if (currency == null)
                {
                    currency = "NULL";
                }
                if (currency_Ratio == null)
                {
                    currency_Ratio = "0";
                }
                if (address == null)
                {
                    address = "NULL";
                }
                currency_Ratio = currency_Ratio.Replace(",", ".");



                int TotalRecords = 0;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                WS_OrderResult WS_orderResult = new WS_OrderResult();
                API.Order OrderXmlPort = new API.Order();
                List< API.Orders > lines = new List<API.Orders>();

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

                    order.Quantity= order.Quantity.Replace(",", ".");
                    order.Unit_Price = order.Unit_Price.Replace(",", ".");

                    API.Orders lineXmlPort = new API.Orders
                    {
                        Barcode = order.Barcode,
                        Quantity = order.Quantity,
                        Unit_Price = order.Unit_Price,
                        ID = order.Id,
                        location = order.Location,
                        OrderNo = orderNo,
                        Discount=order.Discount,
                        motif=""


                    };
                    lines.Add(lineXmlPort);

                }
                OrderXmlPort.Orders = lines.ToArray();

                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/ESP/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                mobile_Web_Services.AddOrModifyOrder(colisno, customerNo, orderNo, currency, currency_Ratio, address,"", false,ref OrderXmlPort,"");

                WS_orderResult.Message="Success";
                return WS_orderResult;
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
                WS_OrderResult WS_orderResult = new WS_OrderResult();

                WS_orderResult.Message = jsonResponse["Message"].ToString();
                return WS_orderResult;
            }
        }
        public WS_OrderResult ModifyOrder(string orderNo, string customerNo, string colisno, string currency, string currency_Ratio, string address, List<Order_list> Order_list)
        {
            try
            {
                if (orderNo == null)
                {
                    orderNo = "NULL";
                }
                if (customerNo == null)
                {
                    customerNo = "NULL";
                }
                if (colisno == null)
                {
                    colisno = "NULL";
                }
                if (currency == null)
                {
                    currency = "NULL";
                }
                if (currency_Ratio == null)
                {
                    currency_Ratio = "0";
                }
                if (address == null)
                {
                    address = "NULL";
                }
                currency_Ratio = currency_Ratio.Replace(",", ".");



                int TotalRecords = 0;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                WS_OrderResult WS_orderResult = new WS_OrderResult();
                API.Order OrderXmlPort = new API.Order();
                List<API.Orders> lines = new List<API.Orders>();
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

                        order.Quantity = order.Quantity.Replace(",", ".");
                        order.Unit_Price = order.Unit_Price.Replace(",", ".");

                        API.Orders lineXmlPort = new API.Orders
                        {
                            Barcode = order.Barcode,
                            Quantity = order.Quantity,
                            Unit_Price = order.Unit_Price,
                            ID = order.Id,
                            location = order.Location,
                            OrderNo = orderNo,
                            Discount = order.Discount,  
                            motif=""


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
                            OrderNo = orderNo,
                            Discount = "0",
                            motif=""


                        };
                    lines.Add(lineXmlPort);
                }
                OrderXmlPort.Orders = lines.ToArray();

                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/ESP/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;

                mobile_Web_Services.AddOrModifyOrder(colisno, customerNo, orderNo, currency, currency_Ratio, address,"" ,true, ref OrderXmlPort,"");

                WS_orderResult.Message = "Success";
                return WS_orderResult;
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
                WS_ImageResult WS_imageResult = new WS_ImageResult();

                WS_imageResult.Message = jsonResponse["Message"].ToString();
                return WS_imageResult;
            }
        }
        public WS_Status ModifyStatus(string OrderNo, string Status)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                string token = GetAccessToken();
                AuthenticatedAPIClient mobile_Web_Services = new AuthenticatedAPIClient(token);
                WS_Status WS_status = new WS_Status();

                mobile_Web_Services.Url = "https://api.businesscentral.dynamics.com/v2.0/e18fb4b5-9142-4516-a5f8-8de91c4e5681/ZEDD_DEV/WS/ESP/Codeunit/API";
                mobile_Web_Services.Timeout = 1000000000;


                mobile_Web_Services.UpdateStatus(Status,OrderNo);

                WS_status.Message = "Success";
                return WS_status;
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
                WS_Status WS_status = new WS_Status();

                WS_status.Message = jsonResponse["Message"].ToString();
                return WS_status;
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

