using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer;
using ModelLayer.DTO;
using ModelLayer.Response;
using RepositoryLayer.Entity;
using System.Security.Claims;

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
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAllContacts()
        {
            // now we will only show the contacts of the logged in user
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "Token is missing UserId claim." });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid UserId in token." });
            }

            List<AddressBookEntity> entities = await _addressBookBL.GetAllContactsBL(userId);
            Response<List<AddressBookEntity>> fetchResponse = new Response<List<AddressBookEntity>>();
            fetchResponse.Success = true;
            fetchResponse.Message = "Contacts Fetched SuccessFully";
            fetchResponse.Data = entities;
            return Ok(fetchResponse);
        }

        /// <summary>
        /// Get contact by ID.
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetContactById(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "Token is missing UserId claim." });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid UserId in token." });
            }

            AddressBookDTO contact = await _addressBookBL.GetContactByIDBL(id, userId);
            Response<AddressBookDTO> getContactResponse = new Response<AddressBookDTO>();
            getContactResponse.Success = true;
            getContactResponse.Message = "Contact Fetched SuccessFully";
            getContactResponse.Data = contact;
            return Ok(getContactResponse);
        }

        /// <summary>
        /// Add a new contact.
        /// </summary>
        [Authorize]
        [HttpPost]
        // Method to add contact to the database
        public async Task<ActionResult> AddContact([FromBody] AddressBookDTO addContact)
        {
            // Ensure User is Authenticated and Extract UserId Safely
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "Token is missing UserId claim." });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid UserId in token." });
            }

            Console.WriteLine($"Extracted UserId: {userId}");


            // Validate request body
            if (addContact == null)
            {
                return BadRequest(new { message = "Invalid request data." });
            }

            // Call Business Logic Layer to Add Contact
            CreateContactDTO createdContact = await _addressBookBL.AddContactBL(addContact, userId);

            // Create Response Object
            var addResponse = new Response<CreateContactDTO>
            {
                Success = true,
                Message = "Contact Added Successfully",
                Data = createdContact
            };

            return Ok(addResponse);
        }


        /// <summary>
        /// Update an existing contact.
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        //Method to Update Contact in AddressBook By ID
        public async Task<ActionResult> UpdateContactByID(int id, [FromBody] AddressBookDTO updateContact)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "Token is missing UserId claim." });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid UserId in token." });
            }

            AddressBookDTO UpdatedContact = await _addressBookBL.UpdateContactByIDBL(id, updateContact, userId);
            Response<AddressBookDTO> updateResponse = new Response<AddressBookDTO>();
            updateResponse.Success = true;
            updateResponse.Message = "Contact Updated SuccessFully";
            updateResponse.Data = updateContact;

            return Ok(updateResponse);

        }

        /// <summary>
        /// Delete a contact by ID.
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContactByID(int id)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(new { message = "Token is missing UserId claim." });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "Invalid UserId in token." });
            }
            AddressBookDTO deletedContact = await _addressBookBL.DeleteContactByIDBL(id, userId);
            Response<AddressBookDTO> deleteResponse = new Response<AddressBookDTO>();
            deleteResponse.Success = true;
            deleteResponse.Message = "Contact Deleted SuccessFully";
            deleteResponse.Data = deletedContact;
            return Ok(deleteResponse);
        }
    }
}