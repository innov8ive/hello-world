using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using static WebApplication1.SetuModels;

namespace WebApplication1
{
    public class SetuHelper
    {
        private const string MerchantID = "1079008445958456853";
        private const string ClientID = "b7c2703b-970c-417d-a924-04dfcf508f81";
        private const string Secret = "b55587fa-afd8-48fe-bbb7-aa2ef367264c";
        private const string SetuUrl = "https://uat.setu.co/api/v2/";
        private bool updatedDB = false;

        public string GenerateDeepLink(int amount,string socName,string transactionNote)
        {
            int count = 0;
            string token = Convert.ToString(HttpContext.Current.Session["Token"]);
            if (string.IsNullOrEmpty(token))
            {
                GenerateToken();
                token = Convert.ToString(HttpContext.Current.Session["Token"]);
            }
            string billerBillID = Guid.NewGuid().ToString().Substring(0, 10);
            GenerateDeepinkReq req = new GenerateDeepinkReq();
            req.amount = new Amount() { currencyCode = "INR", value = 100 };
            req.amountExactness = "EXACT";
            req.billerBillID = billerBillID;
            req.name = "TEST LTD";
            req.transactionNote = transactionNote;

            string linkRespnse = HttpCall("payment-links", "POST", Newtonsoft.Json.JsonConvert.SerializeObject(req), token);
            if (!string.IsNullOrEmpty(linkRespnse))
            {
                if (linkRespnse.Contains("Invalid Token") && count <= 0)
                {
                    count++;
                    GenerateToken();
                    return GenerateDeepLink(amount, socName, transactionNote);
                }
                SetuResponse<GenerateDeepinkResponse> setuResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<SetuResponse<GenerateDeepinkResponse>>(linkRespnse);
                if (setuResponse.success && updatedDB)
                {
                    SetuInfoBL setuInfoBL = new SetuInfoBL("Data Source=.;Initial Catalog=TestDB;Integrated Security=True");
                    SetuInfo setuInfo = new SetuInfo();
                    setuInfo.ID = 0;
                    setuInfo.Amount = amount;
                    setuInfo.BillerBillID = billerBillID;
                    setuInfo.LinkCreated = DateTime.Now;
                    setuInfo.PaymentLinkUPIID = setuResponse.data.paymentLink.upiID;
                    setuInfo.PaymentLinkUPILInk = setuResponse.data.paymentLink.upiLink;
                    setuInfo.PaymentNote = transactionNote;
                    setuInfo.PlatformBillID = setuResponse.data.platformBillID;

                    setuInfoBL.Data = setuInfo;
                    setuInfoBL.Update();
                }
                return setuResponse.data.platformBillID + '`' + setuResponse.data.paymentLink.upiID;
            }
            return null;
        }
        public string CheckLinkStatus(string platformBillId)
        {
            int count = 0;
            string token = Convert.ToString(HttpContext.Current.Session["Token"]);
            if (string.IsNullOrEmpty(token))
            {
                GenerateToken();
                token = Convert.ToString(HttpContext.Current.Session["Token"]);
            }
            string linkRespnse = HttpCall("payment-links/" + platformBillId, "GET", string.Empty, token);
            if (!string.IsNullOrEmpty(linkRespnse))
            {
                if (linkRespnse.Contains("Invalid Token") && count <= 0)
                {
                    count++;
                    GenerateToken();
                    return CheckLinkStatus(platformBillId);
                }
                SetuResponse<CheckStatusResponse> setuResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<SetuResponse<CheckStatusResponse>>(linkRespnse);
                if (setuResponse.success && updatedDB)
                {
                    SetuInfoBL setuInfoBL = new SetuInfoBL("Data Source=.;Initial Catalog=TestDB;Integrated Security=True");
                    setuInfoBL.Load(platformBillId);
                    SetuInfo setuInfo = setuInfoBL.Data;
                    setuInfo.LastStatus = setuResponse.data.status;
                    setuInfo.LastStatusRecieved = DateTime.Now;
                    setuInfo.LinkExpired = setuResponse.data.expiresAt;
                    setuInfo.StatusEventId = setuResponse.data.receipt?.id;
                    setuInfoBL.Update();
                }
                return setuResponse.data.status;
            }
            return null;
        }
        public void GenerateToken()
        {
            GenerateTokenReq req = new GenerateTokenReq() { clientID = ClientID, secret = Secret };
            string tokenRespnse = HttpCall("auth/token", "POST", Newtonsoft.Json.JsonConvert.SerializeObject(req), null);
            if (!string.IsNullOrEmpty(tokenRespnse))
            {
                SetuResponse<GenerateTokenResponse> setuResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<SetuResponse<GenerateTokenResponse>>(tokenRespnse);
                HttpContext.Current.Session["Token"] = setuResponse.data.token;
            }
        }
        public void HttpCallWebRequest()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            GenerateTokenReq req = new GenerateTokenReq() { clientID = ClientID, secret = Secret };
            // Create a request using a URL that can receive a post. 
            WebRequest request = WebRequest.Create(SetuUrl+ "auth/token");
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data and convert it to a byte array.
            string postData = Newtonsoft.Json.JsonConvert.SerializeObject(req);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.
            //request.ContentType = "application/x-www-form-urlencoded";
            request.ContentType = "application/json";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        private string HttpCall(string api, string method, string request, string token)
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;

            var fullUrl = SetuUrl + api;
            var content = new StringContent(request, Encoding.UTF8, "application/json");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("X-Setu-Product-Instance-ID", MerchantID);
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //httpClient.DefaultRequestHeaders.Add("x-api-key", ConfigurationManager.AppSettings("InfinityXApiKey"));
            switch (method)
            {
                case "GET":
                    {
                        response = httpClient.GetAsync(fullUrl).Result;
                        break;
                    }

                case "PUT":
                    {
                        response = httpClient.PutAsync(fullUrl, content).Result;
                        break;
                    }

                case "POST":
                    {
                        response = httpClient.PostAsync(fullUrl, content).Result;
                        break;
                    }

                case "Delete":
                    {
                        response = httpClient.DeleteAsync(fullUrl).Result;
                        break;
                    }

                default:
                    {
                        throw new Exception("Unsupported HTTP Request Method: " + method);
                    }
            }

            if (!response.IsSuccessStatusCode)
            {
                var responseContentString = response.Content.ReadAsStringAsync().Result;
                return response.Content.ReadAsStringAsync().Result;
            }
            else
                return response.Content.ReadAsStringAsync().Result;
        }
    }

    public class SetuModels
    {
        public class GenerateTokenReq
        {
            public string clientID { get; set; }
            public string secret { get; set; }
        }
        public class GenerateTokenResponse
        {
            public int expiresIn { get; set; }
            public string token { get; set; }
        }
        public class AdditionalInfo
        {
            public string UUID { get; set; }
            public string tags { get; set; }
        }

        public class Amount
        {
            public int value { get; set; }
            public string currencyCode { get; set; }
        }

        public class GenerateDeepinkReq
        {
            public string billerBillID { get; set; }
            public Amount amount { get; set; }
            public string amountExactness { get; set; }
            public string name { get; set; }
            public string transactionNote { get; set; }
            public AdditionalInfo additionalInfo { get; set; }
        }
        public class PaymentLink
        {
            public string shortURL { get; set; }
            public string upiID { get; set; }
            public string upiLink { get; set; }
        }

        public class GenerateDeepinkResponse
        {
            public string name { get; set; }
            public PaymentLink paymentLink { get; set; }
            public string platformBillID { get; set; }
        }
        public class Receipt
        {
            public DateTime date { get; set; }
            public string id { get; set; }
        }

        public class CheckStatusResponse
        {
            public Amount amountPaid { get; set; }
            public DateTime createdAt { get; set; }
            public DateTime expiresAt { get; set; }
            public string name { get; set; }
            public string payerVpa { get; set; }
            public PaymentLink paymentLink { get; set; }
            public string platformBillID { get; set; }
            public string billerBillID { get; set; }
            public Receipt receipt { get; set; }
            public string status { get; set; }
            public AdditionalInfo additionalInfo { get; set; }
        }

        public class SetuResponse<T>
        {
            public int status { get; set; }
            public bool success { get; set; }
            public T data { get; set; }
        }
    }
}