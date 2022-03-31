using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBank.ViewModel
{
    public class RegistrationViewModel
    {
        [DisplayName("Felhasználó név")]
        public string UserName { get; set; } = null!;

        [DisplayName("Számlaszám")]
        public string AccountNumber { get; set; } = null!;

        [DisplayName("Ellenőrző pinkód")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
