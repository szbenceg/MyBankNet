using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBank.Model.Dao
{
    public class Account
    {
        public Account()
        {

        }

        [Key]
        public long Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public int Balance { get; set; }

        public string AccountNumber { get; set; }

        public Boolean IsLocked { get; set; }

        public DateTime Created { get; set; }

    }
}
