using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpControls.Payment.PagSeguro.Objects
{
    [Serializable]
    public record PaymentItem
    {
        public required string ReferenceId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public int Quantity { get; init; }
        public int UnitAmount { get; init; }
    }
}
