using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpControls.Payment.PagSeguro.Objects;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace SharpControls.Payment.PagSeguro
{
    public class RedirectCheckout : IDisposable
    {
        public static bool DebugMode = false;

        private HttpClient _client;
        private string? _email;
        private string? _token;

        public RedirectCheckout(string email, string token)
        {
            _email = email;
            _token = token;
            _client = new HttpClient();
        }

        private static string GetHost()
        {
            if (DebugMode)
            {
                return "https://sandbox.api.pagseguro.com";
            }
            return "https://api.pagseguro.com";
        }

        public async Task<RedirectCheckoutData> GenerateDigitalCode(PaymentItem[] items, PaymentSender? sender, string reference, DateTime? expirationDate, string redirectUrl, string[] notificationUrls, string[] paymentNotificationUrls, PaymentMethod[] paymentMethods, int discount, int additionalAmount, string softDescriptor, bool senderRequired = false)
        {
            if (senderRequired && sender == null)
            {
                throw new Exception("Sender is required but it is null!");
            }

            var host = GetHost();

            try
            {
                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Post, host);
                msg.Headers.Add("Authorization", "Bearer " + _token);
                msg.Headers.Add("Content-Type", "application/json");
                msg.Headers.Add("Accept", "application/json");


                //ADD CONTENT
                StringContent content = new(
                  "{" +
                  "\"reference_id\": \"" + reference + "\"," +
                  (expirationDate == null ? "" : "\"expiration_date\":" + "\"" + expirationDate.Value.ToString("o", CultureInfo.InvariantCulture) + "\", ") +
                  (sender != null ? "\"customer\": {" +
                  "\"phone\": {" +
                      "\"country\": \"" + sender!.PhoneCountry + "\"," +
                      "\"area\": \"" + sender!.PhoneDDD + "\"," +
                      "\"number\": \"" + sender!.Phone + "\"" +
                    "}," +
                    "\"name\":" + "\"" + sender.Name + "\"," +
                    "\"email\": " + "\"" + sender.Email + "\"," +
                    "\"tax_id\": " + "\"" + sender.CPF + "\"" +
                  "}," : "") + 
                  "\"items\": " + JsonConvert.SerializeObject(items) + "," +
                  (paymentMethods.Length > 0 ? "\"payment_methods\": " + JsonConvert.SerializeObject(paymentMethods) + "," : "") +
                  "\"discount_amount\": " + discount + "," +
                  "\"customer_modifiable\": " + (senderRequired ? "false" : "true") + "," +
                  "\"additional_amount\": " + additionalAmount + "," +
                  "\"soft_descriptor\": " + softDescriptor + "," +
                  "\"redirect_url\": " + redirectUrl + "," +
                  (notificationUrls.Length > 0 ? "\"notification_urls\": " + JsonConvert.SerializeObject(notificationUrls) + "," : "") +
                  (paymentNotificationUrls.Length > 0 ? "\"payment_notification_urls\": " + JsonConvert.SerializeObject(paymentNotificationUrls) : "") +
                  "}"
                  );

                var response = await _client.SendAsync(msg);

                if(response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created)
                {
                    throw new Exception(response.StatusCode + ": " + response.Content.ToString());
                }
                return new RedirectCheckoutData(response.Content.ToString()!);
            }
            catch (Exception ex)
            {
                //TODO: Change this to something else...
                throw ex;
            }
        }

        public string GetCheckoutUrl(string id)
        {
            throw new NotImplementedException();
        }

        public static bool CompareSignature(string token, string payload, string authenticityToken)
        {
            var rawData = token + "-" + payload;
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                var calculatedToken = builder.ToString();
                return calculatedToken == authenticityToken;
            }
        }

        public bool CompareSignature(string payload, string authenticityToken)
        {
            return CompareSignature(_token, payload, authenticityToken);
        }


        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }

                _email = null;
                _token = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
