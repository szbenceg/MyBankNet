using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBank.ViewModel
{
    public class ConformationViewModel
    {
        [DisplayName("Felhasználó név")]
        public string UserName { get; set; } = null!;

        [DisplayName("Jelszó")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
