using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class AddressBookRL : IAddressBookRL
    {
        private readonly AddressBookDBContext _dbContext;
        public AddressBookRL(AddressBookDBContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
