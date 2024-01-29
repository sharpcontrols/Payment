using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.Payment.PagSeguro.Objects
{
    [Serializable]
    public record ShippingInformation
    {
        public required string Road { get; init; }
        public required string Number { get; init; }
        public string? Complement {  get; init; }
        public required string Area { get; init; }
        public required string City { get; init; }
        public required string State { get; init; }
        public required int Cep {  get; init; }

        public required int ShippingType { get; init; }
        public required float ShippingCost { get; init; }
        public required bool AddressRequired { get; init; }
    }
}
