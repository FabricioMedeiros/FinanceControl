using FinanceControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceControl.Application.DTOs
{
    public class TransactionDto
    {
        public Guid? Id { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime Date { get; set; }
        public string? Description { get; set; }
        public required CategoryDto Category { get; set; }
        public required PaymentMethodDto PaymentMethod { get; set; }

    }
}
