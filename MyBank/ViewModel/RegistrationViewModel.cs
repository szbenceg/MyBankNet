using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBank.ViewModel
{
    public class RegistrationViewModel
    {
        [RegularExpression(@"^[a-z]+$", ErrorMessage = "Csak kisbetűs, éskezet mentes karakterek megengedettek")]
        [DisplayName("Felhasználó név")]
        public string UserName { get; set; } = null!;

        [RegularExpression(@"^([a-zA-Z]|\s)+$", ErrorMessage = "A név számokat, ékezeteket nem tartalmazhat")]
        [DisplayName("Teljes név")]
        public string Name { get; set; } = null!;

        [RegularExpression(@"^(([1-9]){4}-){3}[1-9]{4}$", ErrorMessage = "A számlaszám formátuma nem megfelelő (xxxx-xxxx-xxxx-xxxx)")]
        [DisplayName("Számlaszám")]
        public string AccountNumber { get; set; } = null!;

        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [RegularExpression(@"^([1-9]){6}$", ErrorMessage = "A pin code csak számokat tartalmazhat és 6 hosszú lehet")]
        [DisplayName("Ellenörző pin")]
        [DataType(DataType.Password)]
        public string PinCode { get; set; } = null!;
    }
}
