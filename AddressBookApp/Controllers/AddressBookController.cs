using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.DTO;
using ModelLayer.Response;
using RepositoryLayer.Entity;

namespace AddressBookApp.Controllers
{
    [ApiController]
    [Route("api/AddressBook")]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressBookBL _addressBookBL;
        public AddressBookController(IAddressBookBL addressBookBL) 
        {
            _addressBookBL = addressBookBL;
        }
        /// <summary>
        /// Fetch all contacts.
        /// </summary>
        [HttpGet]
        public ActionResult GetAllContacts()
        {
            List<AddressBookEntity> entities = _addressBookBL.GetAllContactsBL();
            Response<List<AddressBookEntity>> fetchResponse = new Response<List<AddressBookEntity>>();
            fetchResponse.Success = true;
            fetchResponse.Message = "Contacts Fetched SuccessFully";
            fetchResponse.Data = entities;
            return Ok(fetchResponse);
        }

        /// <summary>
        /// Get contact by ID.
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult GetContactById(int id)
        {
            AddressBookDTO contact = _addressBookBL.GetContactByIDBL(id);
            Response<AddressBookDTO> getContactResponse = new Response<AddressBookDTO>();
            getContactResponse.Success = true;
            getContactResponse.Message = "Contact Fetched SuccessFully";
            getContactResponse.Data = contact;
            return Ok(getContactResponse);
        }

        /// <summary>
        /// Add a new contact.
        /// </summary>
        [HttpPost]
        //Method to add contact to the database
        public ActionResult AddContact([FromBody] AddressBookDTO addContact)
        {
            CreateContactDTO createdContact = _addressBookBL.AddContactBL(addContact);
            Response<CreateContactDTO> addResponse = new Response<CreateContactDTO>();
            addResponse.Success = true;
            addResponse.Message = "Contact Added SuccessFully";
            addResponse.Data = createdContact;
            return Ok(addResponse);
        }

        /// <summary>
        /// Update an existing contact.
        /// </summary>
        [HttpPut("{id}")]
        //Method to Update Contact in AddressBook By ID
        public ActionResult UpdateContactByID(int id, [FromBody] AddressBookDTO updateContact)
        {
            AddressBookDTO UpdatedContact = _addressBookBL.UpdateContactByIDBL(id, updateContact);
            Response<AddressBookDTO> updateResponse = new Response<AddressBookDTO>();
            updateResponse.Success = true;
            updateResponse.Message = "Contact Updated SuccessFully";
            updateResponse.Data = updateContact;

            return Ok(updateResponse);

        }

        /// <summary>
        /// Delete a contact by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteContactByID(int id)
        {
            AddressBookDTO deletedContact = _addressBookBL.DeleteContactByIDBL(id);
            Response<AddressBookDTO> deleteResponse = new Response<AddressBookDTO>();
            deleteResponse.Success = true;
            deleteResponse.Message = "Contact Deleted SuccessFully";
            deleteResponse.Data = deletedContact;
            return Ok(deleteResponse);
        }


    }
}