using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {

        private readonly IMapper _mapper;
        private readonly IAddressBookRL _addressBookRL;

        /// <summary>
        /// using depencency injection
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="addressBookRL"></param>
        public AddressBookBL(IMapper mapper, IAddressBookRL addressBookRL)
        {
            _mapper = mapper;
            _addressBookRL = addressBookRL;
        }

        /// <summary>
        /// retrieving all contacts
        /// </summary>
        /// <returns></returns>
        public List<AddressBookEntity> GetAllContactsBL()
        {
            return _addressBookRL.GetAllContactsRL();
        }

        /// <summary>
        /// getting a particular contact by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AddressBookDTO GetContactByIDBL(int id)
        {
            AddressBookEntity addressBookEntity = _addressBookRL.GetContactByIDRL(id);

            return _mapper.Map<AddressBookDTO>(addressBookEntity);
        }

        /// <summary>
        /// creating new contact
        /// </summary>
        /// <param name="createContact"></param>
        /// <returns></returns>
        public CreateContactDTO AddContactBL(AddressBookDTO createContact)
        {
            AddressBookEntity addressBookEntity = _mapper.Map<AddressBookEntity>(createContact);

            AddressBookEntity createdEntity = _addressBookRL.AddContactRL(addressBookEntity);

            return _mapper.Map<CreateContactDTO>(createdEntity);
        }

        /// <summary>
        /// updating a particular contact
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateContact"></param>
        /// <returns></returns>
        public AddressBookDTO UpdateContactByIDBL(int id, AddressBookDTO updateContact)
        {
            AddressBookEntity addressBookEntity = _mapper.Map<AddressBookEntity>(updateContact);

            AddressBookEntity UpdatedEntity = _addressBookRL.UpdateContactByID(id, addressBookEntity);

            return _mapper.Map<AddressBookDTO>(UpdatedEntity);
        }

        /// <summary>
        /// deleting contact by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AddressBookDTO DeleteContactByIDBL(int id)
        {
            AddressBookEntity deletedEntity = _addressBookRL.DeleteContactByID(id);

            return _mapper.Map<AddressBookDTO>(deletedEntity);
        }
    }
}
