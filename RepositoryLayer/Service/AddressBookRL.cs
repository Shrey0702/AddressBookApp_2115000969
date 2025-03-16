using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
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
        /// <summary>
        /// Creating object of AddressBookDBContext
        /// </summary>
        private readonly AddressBookDBContext _dbContext;
        public AddressBookRL(AddressBookDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public List<AddressBookEntity> GetAllContactsRL(int UserId)
        {
            return _dbContext.AddressBook
        .AsNoTracking()
        .Where(contact => contact.UserId == UserId)
        .ToList();
        }


        public AddressBookEntity GetContactByIDRL(int id, int UserId)
        {
            var addressBookEntity = _dbContext.AddressBook.FirstOrDefault(a => a.Id == id && a.UserId == UserId);

            return addressBookEntity;
        }


        public async Task<AddressBookEntity> AddContactRL(AddressBookEntity addressBookEntity)
        {
            // Add the entity to the database
            _dbContext.AddressBook.Add(addressBookEntity);
            await _dbContext.SaveChangesAsync(); // Use async for better performance

            return addressBookEntity;
        }


        public AddressBookEntity UpdateContactByID(int id, AddressBookEntity addressBookEntity, int UserId)
        {
            var entity = _dbContext.AddressBook.FirstOrDefault(a => a.Id == id);
            if (entity == null)
            {
                return null; // it is working fine with authorisation/authentication
                             // it's just in response it is sending updated value but in reality
                             // if not authorised it is not letting other person to update database so database is safe
            }
            // later need to modify the response only other things are fixed
            if (entity.UserId == UserId)
            {
                entity.Address = addressBookEntity.Address;
                entity.PhoneNumber = addressBookEntity.PhoneNumber;
                entity.Email = addressBookEntity.Email;
                entity.Name = addressBookEntity.Name;
                _dbContext.AddressBook.Update(entity);
                _dbContext.SaveChanges();
                return entity;
            }
            return null;
        }

        public AddressBookEntity DeleteContactByID(int id, int UserId)
        {
            var entity = _dbContext.AddressBook.FirstOrDefault(_a => _a.Id == id);
            if (entity == null)
            {
                return null;
            }
            if(entity.UserId == UserId)
            {
                _dbContext.AddressBook.Remove(entity);
                _dbContext.SaveChanges();
                return entity;
            }
            return null;
        }
    }
}
