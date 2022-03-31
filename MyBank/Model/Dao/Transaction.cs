using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBank.Model.Dao
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

		public TransactionType Type { get; set; }

		[ForeignKey("Account")]
		public int SourceAccountId { get; set; }

		[ForeignKey("Account")]
		public int DestinationAccountId { get; set; }

		public string DestinationAccountOwnerName { get; set; }

		public DateTime TransactionExecutionDate { get; set; }

		public int TransactionTotal { get; set; }
	}
}
