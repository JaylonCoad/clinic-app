using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.ClinicApp.Models;
using API.ClinicApp.Data;

namespace API.ClinicApp.Controllers;

[ApiController]
[Route("[controller]")] // Maps to http://localhost:xxxx/Physicians
public class PhysiciansController : ControllerBase
{
    private readonly ClinicDbContext _context;

    public PhysiciansController(ClinicDbContext context)
    {
        _context = context;
    }

    // GET: returns all physicians from the database
    // Endpoint: /Physicians
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Physician>>> Get() // returning an IEnumerable<T> because this is read-only and doesn't lock us into using a List<T> data structure if we wanted to change data types later since they are all technically IEnumerable<T>
    {
        return await _context.Physicians.ToListAsync();
    }

    // GET: returns one physician given an id
    // Endpoint: /Physicians/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Physician>> GetById(string id)
    {
        var physician = await _context.Physicians.FindAsync(id);
        if (physician == null)
        {
            return NotFound();
        }
        return physician;
    }

    // POST: adds a physician to the database
    // Endpoint: /Physicians
    [HttpPost]
    public async Task<ActionResult<Physician>> Add(Physician physician)
    {
        if (string.IsNullOrEmpty(physician.Id))
        {
            return BadRequest("Physician ID must be provided by the client."); // bad request means there was a problem with the data from the client side
        }
        _context.Physicians.Add(physician);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = physician.Id }, physician);
    }

    // Delete: deletes a physician from the database given an id
    // Endpoint: /Physicians/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var physician = await _context.Physicians.FindAsync(id);
        if (physician == null)
        {
            return NotFound();
        }
        _context.Physicians.Remove(physician);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Update: updates a physician from the database given an id that already exists
    // Endpoint: /Physicians/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Physician physician, string id)
    {
        if (physician.Id != id)
        {
            return BadRequest("Physician ID is not equal to the incoming ID.");
        }
        _context.Entry(physician).State = EntityState.Modified; // this line basically tells the server that we are trying to overwrite the existing object
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if (!_context.Physicians.Any(p => p.Id == id)) // couldn't find the existing object in the database
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }
}