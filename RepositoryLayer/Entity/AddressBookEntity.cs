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
        public int Id { get; set; }

        //[Required]
        public string Name { get; set; } = string.Empty;

        //[Required]
        //[EmailAddress]
        public string Email { get; set; } = string.Empty;

        //[Required]
        //[MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        //[Required]
        //[MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        //[Required]
        [ForeignKey("Users")]
        public int UserId { get; set; }// Foreign Key
    }
}
