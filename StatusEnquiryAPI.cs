public class StatusEnquiryAPI
    {

        public class Request
        {
            public string PAY_ID { get; set; }
            public string ORDER_ID { get; set; }
            public string AMOUNT { get; set; }
            public string TXNTYPE { get; set; }
            public string CURRENCY_CODE { get; set; }
            public string HASH { get; set; }
        }

        public class Response
        {
            public string RESPONSE_DATE_TIME { get; set; }
            public string RESPONSE_CODE { get; set; }
            public string TXN_ID { get; set; }
            public string MOP_TYPE { get; set; }
            public string CARD_MASK { get; set; }
            public string ACQ_ID { get; set; }
            public string TXNTYPE { get; set; }
            public string CURRENCY_CODE { get; set; }
            public string RRN { get; set; }
            public string SURCHARGE_FLAG { get; set; }
            public string PAYMENT_TYPE { get; set; }
            public string PG_TXN_MESSAGE { get; set; }
            public string STATUS { get; set; }
            public string PG_REF_NUM { get; set; }
            public string PAY_ID { get; set; }
            public string ORDER_ID { get; set; }
            public string AMOUNT { get; set; }
            public string RESPONSE_MESSAGE { get; set; }
            public string ORIG_TXN_ID { get; set; }
            public string TOTAL_AMOUNT { get; set; }
            public string CUST_NAME { get; set; }
            public string IS_STATUS_FINAL { get; set; }
        }


        const string salt = "ABCD1234";
        const string baseUrl = "https://uat.cashlesso.com/";

        public async Task<Response> TransactStatusAsync(Request request)
        {
            if(request==null)
                throw new ArgumentException("invalid `request` param supplied.");

            var keyValuePairs = request.GetType().GetProperties().Where(x => x.GetValue(request, null) != null).ToDictionary(x => x.Name, y => y.GetValue(request).ToString());

            var SHA256 = SignatureGenerator.Generate(keyValuePairs, salt);

            request.HASH = SHA256;

            var requestPayload = Newtonsoft.Json.JsonConvert.SerializeObject(request);

            Console.WriteLine($"requestPayload : {requestPayload}");

            using (var httpClient = new HttpClient())
            {
                var requestFromServer = await httpClient.PostAsync($"{baseUrl}pgws/transact", new StringContent(requestPayload, Encoding.UTF8, "application/json"));
                requestFromServer.EnsureSuccessStatusCode();
                string requestFromServer_Body = await requestFromServer.Content.ReadAsStringAsync();

                Console.WriteLine($"requestFromServer_Body : {requestFromServer_Body}");

                return JsonConvert.DeserializeObject<Response>(requestFromServer_Body);
            }
        }


        public async Task SampleCall()
        {
            var sampleRequest = new Request { AMOUNT = "1500", CURRENCY_CODE = "356", ORDER_ID = "ORDER00123", PAY_ID = "1001180108120354", TXNTYPE = "STATUS" };

            await TransactStatusAsync(sampleRequest);
        }

    }