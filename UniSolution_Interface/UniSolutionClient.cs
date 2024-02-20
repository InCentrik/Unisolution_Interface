using System;
using RestSharp;
using Newtonsoft.Json.Linq;
using RestSharp.Authenticators;
using System.Threading;
using System.Collections.Generic;
using System.Net.Http;

namespace InCentrik.UniSolution
{
    public class Client
    {
        private readonly RestClient client;
        public string accessToken = "";
        public int retries = 3;
        public int retryPauseTime = 5000;
        public string username = "";
        public string password = "";
        public string token = "";
        public string token_type = "bearer";

        public Client(string baseUrl, string username, string password)
        {

            RestClientOptions options = new RestClientOptions(baseUrl)
            {
            };
            client = new RestClient(options);
            this.username = username;
            this.password = password;
            this.GetToken();
        }

        public dynamic ExecuteAsyncWithRetries(RestRequest request, CancellationToken token)
        {
            int i = 0;
            while (i <= retries)
            {
                try
                {
                    RestResponse responseResult = client.ExecuteAsync(request,token).Result;
                    if (responseResult.Content.StartsWith("{"))
                    {
                        return JObject.Parse(responseResult.Content);
                    } else
                    {
                        return JArray.Parse(responseResult.Content);
                    }
                }
                catch (Exception err)
                {
                    i++;
                    if (i == 4)
                    {
                        throw err;
                    }
                    Thread.Sleep(retryPauseTime);
                }
            }
            return null;
        }

        public JObject GetToken()
        {

            var request = new RestRequest("/CorporateConnectBack/back/api/token?ignoreErrors=true", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "password");
            request.AddParameter("username", "ERGON");
            request.AddParameter("password", "Ergon@2024");
            request.AddParameter("codEmpresa", "181");
            request.AddParameter("certificado", "undefined");

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            JObject data = ExecuteAsyncWithRetries(request, cancellationTokenSource.Token);
            this.token = data.Value<string>("access_token");

            Console.WriteLine(token);
            return data;
        }

        public JArray GetData()

        {
            var request = new RestRequest("/CorporateConnectBack/back/api/V1/dataConnect/getDados", Method.Post);
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer " + token);
            var body = @"{""action"": ""tnYnq3K0OHX6XbUlpH9LBH9lq3Y0NLlkO2K0F29jr3KhsVTxARYwN3Geq24xDxYFI1CJXxlxN29iNL5zqnX6XzChpJUzq1saNzCkqzCkqcC1qWGwJbUzNMXvBIN0CoX5ARPxARY2OMYoNL9Mr0Ckqb5aN3FxDxYLBHY9""}";
            request.AddStringBody(body, DataFormat.Json);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            JArray data = ExecuteAsyncWithRetries(request, cancellationTokenSource.Token);

            Console.WriteLine(data);
            return data;
        }
    }
}
