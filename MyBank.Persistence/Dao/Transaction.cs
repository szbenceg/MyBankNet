using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBank.Persistence.Dao
{
	public enum TransactionType
	{
		
		Transfer
	}
	public class Transaction
    {
        public Transaction()
        {

        }

		[Key]
		public int Id { get; set; }
		public TransactionType Type { get; set; }

		[ForeignKey("Account")]
		public int SourceAccountId { get; set; }

		[ForeignKey("Account")]
		public int DestinationAccountId { get; set; }

		public string BenificaryName { get; set; } = null!;

		public DateTime TransactionExecutionDate { get; set; }

		public int TransactionTotal { get; set; }

		public string Message { get; set; }
	}
}
