using Microsoft.AspNetCore.Mvc;

namespace AddressBookApp.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressBookController : ControllerBase
{
    // using await operation because while handling database operations there should be no blocking of threads so it is the suggested way to do it properly
    // i created dummy object because right now i haven't created any model or DTO class in my project
    public AddressBookController()
    {
    }

    /// <summary>
    /// Get all contacts
    /// </summary>
    /// <returns>List of contacts</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetAllContacts()
    {
        return Ok("hrioeil"); 
    }

    /// <summary>
    /// Get contact by ID
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>Contact details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetContactById(int id)
    {
        return Ok(); 
    }

    /// <summary>
    /// Add a new contact
    /// </summary>
    /// <param name="contact">Contact object</param>
    /// <returns>Created contact</returns>
    [HttpPost]
    public async Task<ActionResult<object>> AddContact([FromBody] object contact)
    {
        return CreatedAtAction(nameof(GetContactById), new { id = 1 }, contact); // Replace with actual implementation
    }

    /// <summary>
    /// Update an existing contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <param name="contact">Updated contact object</param>
    /// <returns>No content</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] object contact)
    {
        return NoContent(); // Replace with actual implementation
    }

    /// <summary>
    /// Delete a contact
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        return NoContent(); 
    }
}
