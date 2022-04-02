using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBank.ViewModel
{
    public class TransactionViewModel
    {
        [DisplayName("Kedvezményezett számlaszám")]
        public string DestinationAccountNumber { get; set; } = null!;

        [DisplayName("Kedvezményezett neve")]
        public string BenificaryName{ get; set; } = null!;

        [DisplayName("Saját számla kiválasztása")]
        public string SourceAccount { get; set; } = null!;

        [DisplayName("Összeg")]
        public int Amount { get; set; } = 0;

        [DisplayName("Közlemény")]
        [MaxLength(120, ErrorMessage = "A közlemény túl hosszú")]
        public string Message { get; set; } = null!;

    }
}
