﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBank.Persistence.Dto
{
    public class TransactionHistoryDto
    {
        public String? TransactionType { get; set; }

        public string? BenificaryName { get; set; } = null!;

        public string? SourceAccountNumber { get; set; } = null!;

        public string? DestinationAccountNumber { get; set; } = null!;
        public string? Message { get; set; } = null!;

        public int? TransactionTotal { get; set; }

        public DateTime? ExecutionDate { get; set; }

    }
}
