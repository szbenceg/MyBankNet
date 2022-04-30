using MyBank.Persistence.Dto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Desktop.ViewModel
{
    public class TransactionViewModel
    {
        public String? TransactionType { get; set; }

        public string? BenificaryName { get; set; } = null!;

        public string? SourceAccountNumber { get; set; } = null!;

        public string? DestinationAccountNumber { get; set; } = null!;
        public string? Message { get; set; } = null!;

        public String? TransactionTotal { get; set; }

        public String? ExecutionDate { get; set; }

        public static explicit operator TransactionViewModel(TransactionHistoryDto dto) => new TransactionViewModel
        {
           BenificaryName = dto.BenificaryName,
           TransactionType = dto.TransactionType,
           TransactionTotal = dto.TransactionTotal?.ToString("N", new CultureInfo("hu-HU")) + " HUF",
           SourceAccountNumber = dto.SourceAccountNumber,
           DestinationAccountNumber = dto.DestinationAccountNumber,
           Message = dto.Message,
           ExecutionDate = dto.ExecutionDate?.ToString("yyyy-MM-dd HH:mm:ss")
        };
    }
}
