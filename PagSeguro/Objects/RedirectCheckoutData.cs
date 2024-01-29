using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpControls.Payment.PagSeguro.Objects
{
    [Serializable]
    public class RedirectCheckoutData
    {
        public string Id { get; set; }
        public string ReferenceId {  get; set; }
        public string ExpirationDate {  get; set; }
        public string CreatedAt {  get; set; }
        public string Status { get; set; }
        public string[] Customer {  get; set; }
        public bool CustomerModifiable {  get; set; }
        public string[] Items { get; set; }
        public int AdditionalAmount {  get; set; }
        public int DiscountAmount {  get; set; }
        public string[] Shipping {  get; set; }
        public string[] PaymentMethods { get; set; }
        public string[] PaymentMethodsConfigs { get; set; }
        public string SoftDescriptor {  get; set; }
        public string RedirectUrl { get; set; }
        public string ReturnUrl {  get; set; }
        public string[] NotificationUrls { get; set; }
        public string[] PaymentNotificationUrls { get; set; }
        public string PayLink { get; set; }
        public string SelfLink { get; set; }
        public string InactivateLink { get; set; }

        public RedirectCheckoutData(string jsonResponse)
        {
            var responseObj = JsonConvert.DeserializeObject<JObject>(jsonResponse);
            Id = responseObj!.Value<string>("id")!;
            ReferenceId = responseObj!.Value<string>("reference_id")!;
            CreatedAt = responseObj!.Value<string>("created_at")!;
            Status = responseObj!.Value<string>("status")!;
            Customer = []; //TODO: Add this
            CustomerModifiable = responseObj!.Value<bool>("customer_modifiable");
            Items = []; //TODO: Add this
            AdditionalAmount = responseObj!.Value<int>("additional_amount");
            DiscountAmount = responseObj!.Value<int>("discount_amount");
            Shipping = []; //TODO: Add this
            PaymentMethods = []; //TODO: Add this
            PaymentMethodsConfigs = []; //TODO: Add this
            SoftDescriptor = responseObj!.Value<string>("soft_descriptor")!;
            RedirectUrl = responseObj!.Value<string>("redirect_url")!;
            ReturnUrl = ""; //TODO: Add this
            NotificationUrls = []; //TODO: Add this
            PaymentNotificationUrls = []; //TODO: Add this
            var responseLinks = responseObj!.Value<JObject[]>("links")!;
            foreach (var link in responseLinks)
            {
                if(link.Value<string>("rel") == "PAY")
                {
                    PayLink = link.Value<string>("href")!;
                }else if (link.Value<string>("rel") == "SELF")
                {
                    SelfLink = link.Value<string>("href")!;
                }
                else if (link.Value<string>("rel") == "INACTIVATE")
                {
                    InactivateLink = link.Value<string>("href")!;
                }
            }
        }

        public string CheckoutUrl(bool offline = false)
        {
            if (offline)
            {
                return PayLink;
            }
            throw new NotImplementedException("Online CheckoutUrl not implemented...");
        }
    }
}
