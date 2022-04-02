using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBank.ViewModel
{
    public class LoginViewModel
    {
        [DisplayName("Felhasználó név")]
        public string UserName { get; set; } = null!;

        [DisplayName("Számlaszám")]
        public string AccountNumber { get; set; } = null!;

        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DisplayName("Ellenőrző pin")]
        [DataType(DataType.Password)]
        public string PinCode { get; set; } = null!;

        [DisplayName("Minden tranzakció előtt kérjen jelszót")]
        public bool IsSecure { get; set; } = false!;
    }


}
