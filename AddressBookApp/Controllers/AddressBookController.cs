using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTO;

namespace AddressBookApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressBookController : ControllerBase
    {
        /// <summary>
        /// Fetch all contacts.
        /// </summary>
        /// returns all contacts
        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok("Our project is successfully executing");
        }

        /// <summary>
        /// Get contact by ID.
        /// </summary>
        /// returns contact by ID
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok();
        }

        /// <summary>
        /// Add a new contact.
        /// </summary>
        /// returns contact added
        [HttpPost]
        public ActionResult AddContact([FromBody] AddressBookDTO addContact)
        {

            return Ok();
        }

        /// <summary>
        /// Update an existing contact.
        /// </summary>
        /// returns updated contact
        [HttpPut("{id}")]
        public ActionResult UpdateContactByID(int id, [FromBody] AddressBookDTO updateConntact)
        {

            return Ok();

        }

        /// <summary>
        /// Delete a contact by ID.
        /// </summary>
        /// returns contact deleted status
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }


    }
}