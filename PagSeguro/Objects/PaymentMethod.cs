using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.Payment.PagSeguro.Objects
{
    [Serializable]
    public class PaymentMethod
    {
        public enum ACCEPTED_PAGSEGURO_BRANDS
        {
            AMEX,
            AVISTA,
            AURA,
            BANESECARD,
            BRASILCARD,
            CABAL,
            CARDBAN,
            DINERS,
            DISCOVER,
            ELO,
            FORTBRASIL,
            GRANDCARD,
            HIPER,
            HIPERCARD,
            JCB,
            MAIS,
            MASTERCARD,
            PERSONALCARD,
            PLENOCARD,
            POLICARD,
            SOROCRED,
            UPBRASIL,
            VALECARD,
            VERDECARD,
            VISA
        }

        public string Type { get; set; }
        public ACCEPTED_PAGSEGURO_BRANDS[] Brands { get; set; }

        public PaymentMethod(string type)
        {
            Type = type;
            Brands = [];
        }

        public PaymentMethod(string type, ACCEPTED_PAGSEGURO_BRANDS[] brands)
        {
            Type = type;
            Brands = brands;
        }

        public static PaymentMethod CreateCreditCard()
        {
            return new PaymentMethod("credit_card", []);
        }

        public static PaymentMethod CreateCreditCard(ACCEPTED_PAGSEGURO_BRANDS[] brands)
        {
            return new PaymentMethod("credit_card", brands);
        }

        public static PaymentMethod CreateDebitCard()
        {
            return new PaymentMethod("debit_card", []);
        }

        public static PaymentMethod CreateDebitCard(ACCEPTED_PAGSEGURO_BRANDS[] brands)
        {
            return new PaymentMethod("debit_card", brands);
        }

        public static PaymentMethod CreatePix()
        {
            return new PaymentMethod("pix");
        }

        public static PaymentMethod CreateBoleto()
        {
            return new PaymentMethod("boleto");
        }

        public override string ToString()
        {
            string brands = "";
            foreach (var brand in Brands)
            {
                if(brands != "")
                {
                    brands += ", " + Environment.NewLine;
                }
                brands += "\"" + brand.ToString() + "\"";
            }
            return "{" +
                "\"type\": \"" + Type + "\"," +
                "\"brands\": [" +
                brands + 
                "]" +
                "}";
        }
    }
}
