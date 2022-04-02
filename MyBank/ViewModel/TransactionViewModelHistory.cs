using MyBank.Model.Dao;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBank.ViewModel
{
    public class TransactionViewModelHistory
    {

        public TransactionType ?TransactionType { get; set; }

        public string ?BenificaryName { get; set; } = null!;

        public string ?SourceAccountNumber { get; set; } = null!;

        public string ?DestinationAccountNumber { get; set; } = null!;
        public string ?Message { get; set; } = null!;

        public int ?TransactionTotal { get; set; }

        public DateTime ?ExecutionDate { get; set; }

        //For conformation

        [DisplayName("Felhasználó név")]
        public string? UserName { get; set; }

        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
