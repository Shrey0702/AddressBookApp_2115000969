using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class AddressBookEntity
    {
        [Key]
        public int AddressID { get; set; }
        [Required]
        public string PersonName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public int Zip { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual UserEntity User { get; set; } = null!;
    }
}
