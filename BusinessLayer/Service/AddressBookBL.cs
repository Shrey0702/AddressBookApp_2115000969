using AutoMapper;
using BusinessLayer.Interface;
using ModelLayer.DTO;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class AddressBookBL : IAddressBookBL
    {
        private readonly IMapper _mapper;
        private readonly IAddressBookRL _addressBookRL;
        private readonly RedisCacheService _cacheService;

        /// <summary>
        /// Using dependency injection
        /// </summary>
        public AddressBookBL(IMapper mapper, IAddressBookRL addressBookRL, RedisCacheService cacheService)
        {
            _mapper = mapper;
            _addressBookRL = addressBookRL;
            _cacheService = cacheService;
        }

        /// <summary>
        /// Retrieving all contacts with caching
        /// </summary>
        public async Task<List<AddressBookEntity>> GetAllContactsBL(int UserId)
        {
            string cacheKey = "AddressBook_AllContacts";

            // Check cache first
            var cachedData = await _cacheService.GetCacheAsync<List<AddressBookEntity>>(cacheKey);
            if (cachedData != null)
            {
                Console.WriteLine("Cache Hit - Returning data from Redis");
                return cachedData;
            }

            // Fetch from DB if not in cache
            var contacts = _addressBookRL.GetAllContactsRL(UserId);

            // Store in cache for 5 minutes
            await _cacheService.SetCacheAsync(cacheKey, contacts, TimeSpan.FromMinutes(5));

            Console.WriteLine("Cache Miss - Fetched from DB and stored in Redis");
            return contacts;
        }

        /// <summary>
        /// Getting a particular contact by ID with caching
        /// </summary>
        public async Task<AddressBookDTO> GetContactByIDBL(int id, int UserId)
        {
            string cacheKey = $"AddressBook_Contact_{id}";

            var cachedContact = await _cacheService.GetCacheAsync<AddressBookEntity>(cacheKey);
            if (cachedContact != null)
            {
                Console.WriteLine($"Cache Hit - Contact {id} returned from Redis");
                return _mapper.Map<AddressBookDTO>(cachedContact);
            }

            var contact = _addressBookRL.GetContactByIDRL(id, UserId);
            if (contact != null)
            {
                await _cacheService.SetCacheAsync(cacheKey, contact, TimeSpan.FromMinutes(10));
                Console.WriteLine($"Cache Miss - Contact {id} fetched from DB and stored in Redis");
            }

            return _mapper.Map<AddressBookDTO>(contact);
        }

        /// <summary>
        /// Creating a new contact and clearing cache
        /// </summary>
        public async Task<CreateContactDTO> AddContactBL(AddressBookDTO createContact, int userId)
        {
            // Map DTO to Entity
            AddressBookEntity addressBookEntity = _mapper.Map<AddressBookEntity>(createContact);
            addressBookEntity.UserId = userId;  // Ensure the UserId is set

            // Save contact using the Repository Layer
            AddressBookEntity createdEntity = await _addressBookRL.AddContactRL(addressBookEntity);

            // Clear cached list after adding a new contact
            try
            {
                await _cacheService.RemoveCacheAsync("AddressBook_AllContacts");
            }
            catch (Exception ex)
            {
                // Log error but don't break request
                Console.WriteLine($"Cache removal failed: {ex.Message}");
            }

            // Map the result back to DTO and return
            return _mapper.Map<CreateContactDTO>(createdEntity);
        }


        /// <summary>
        /// Updating a particular contact and clearing cache
        /// </summary>
        public async Task<AddressBookDTO> UpdateContactByIDBL(int id, AddressBookDTO updateContact, int UserId)
        {
            AddressBookEntity addressBookEntity = _mapper.Map<AddressBookEntity>(updateContact);
            AddressBookEntity updatedEntity = _addressBookRL.UpdateContactByID(id, addressBookEntity, UserId);

            // Clear cache for both the specific contact and full list
            await _cacheService.RemoveCacheAsync("AddressBook_AllContacts");
            await _cacheService.RemoveCacheAsync($"AddressBook_Contact_{id}");

            return _mapper.Map<AddressBookDTO>(updatedEntity);
        }

        /// <summary>
        /// Deleting a contact and clearing cache
        /// </summary>
        public async Task<AddressBookDTO> DeleteContactByIDBL(int id, int UserId)
        {
            AddressBookEntity deletedEntity = _addressBookRL.DeleteContactByID(id, UserId);

            // Clear cache for both the specific contact and full list
            await _cacheService.RemoveCacheAsync("AddressBook_AllContacts");
            await _cacheService.RemoveCacheAsync($"AddressBook_Contact_{id}");

            return _mapper.Map<AddressBookDTO>(deletedEntity);
        }
    }
}
