using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.Payment.PagSeguro.Objects
{
    [Serializable]
    public record PaymentSender
    {
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string PhoneCountry { get; init; }
        public required string PhoneDDD { get; init; }
        public required string Phone { get; init; }
        public int CPF {  get; init; }
    }
}
