public class SignatureGenerater
     {
         public static string Generate(Dictionary<string, string> keyValuePairs, string salt)
         {
             if (keyValuePairs == default(Dictionary<string, string>))
                 throw new ArgumentException("invalid `keyValuePairs` supplied.");
 
             var sortedKeyValueString = string.Join('~', keyValuePairs.OrderBy(x => x.Key).Select(x => string.Concat(x.Key, "=", x.Value)));
             var sortedKeyValueStringWithSalt = string.Concat(sortedKeyValueString, salt);
 
             return SHA256(sortedKeyValueStringWithSalt);
         }
 
         public static string SHA256(string values)
         {
             using (var hash = System.Security.Cryptography.SHA256.Create())
             {
                 Byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(values));
                 StringBuilder Sb = new StringBuilder(64);
                 foreach (Byte b in result)
                     Sb.Append(b.ToString("x2"));
                 return Sb.ToString();
             }
         }
 
 
         public static void DummyExample()
         {
             string sampleJSON = "{ \"PAY_ID\":\"1234567890\", \"ORDER_ID\":\"ABCD123456789\", \"AMOUNT\":\"1000\", \"CURRENCY_CODE\":\"356\", \"RETURN_URL\":\"https://uat.cashlesso.com/pgui/jsp/response.jsp\" }";
             string salt = "ABCD1234";
 
             var keyValuePairFromJSON = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sampleJSON);
 
             Console.WriteLine($"input : {sampleJSON}");
             Console.WriteLine($"salt : {salt}");
             Console.WriteLine($"SHA256 : {Generate(keyValuePairFromJSON, salt)}");
         }
     }