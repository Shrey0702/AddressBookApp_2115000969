using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IAddressBookRL
    {
        List<AddressBookEntity> GetAllContactsRL(int UserId);

        AddressBookEntity GetContactByIDRL(int id, int UserId);

        AddressBookEntity UpdateContactByID(int id, AddressBookEntity addressBookEntity, int UserId);

        public Task<AddressBookEntity> AddContactRL(AddressBookEntity addressBookEntity);
        AddressBookEntity DeleteContactByID(int id, int UserId);
    }
}
