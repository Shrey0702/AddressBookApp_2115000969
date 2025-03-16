using ModelLayer.DTO;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IAddressBookBL
    {
        Task<List<AddressBookEntity>> GetAllContactsBL(int UserId);

        Task<AddressBookDTO> GetContactByIDBL(int id, int UserId);

        Task<AddressBookDTO> UpdateContactByIDBL(int id, AddressBookDTO updateContact, int UserId);

        Task<CreateContactDTO> AddContactBL(AddressBookDTO createContact, int userId);

        Task<AddressBookDTO> DeleteContactByIDBL(int id, int UserId);
    }
}
