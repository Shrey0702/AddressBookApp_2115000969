using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.Context
{
    public class AddressBookDBContext: DbContext
    {

        public AddressBookDBContext(DbContextOptions<AddressBookDBContext> options) : base(options)
        {

        }

        public virtual DbSet<Entity.AddressBookEntity> AddressBook { get; set; }

        public virtual DbSet<Entity.UserEntity> Users { get; set; }
    }
}
