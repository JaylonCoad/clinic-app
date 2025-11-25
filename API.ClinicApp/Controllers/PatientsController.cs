using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.ClinicApp.Models;
using API.ClinicApp.Data;

namespace API.ClinicApp.Controllers;

[ApiController]
[Route("[controller]")] // Maps to http://localhost:xxxx/Patients
public class PatientsController : ControllerBase
{
    private readonly ClinicDbContext _context;

    public PatientsController(ClinicDbContext context)
    {
        _context = context;
    }

    // GET: returns all patients from the database
    // Endpoint: /Patients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> Get() // returning an IEnumerable<T> because this is read-only and doesn't lock us into using a List<T> data structure if we wanted to change data types later since they are all technically IEnumerable<T>
    {
        return await _context.Patients.ToListAsync();
    }

    // GET: returns one patient given an id
    // Endpoint: /Patients/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(string id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            return NotFound();
        }
        return patient;
    }

    // POST: adds a patient to the database
    // Endpoint: /Patients
    [HttpPost]
    public async Task<ActionResult<Patient>> Add(Patient patient)
    {
        if (string.IsNullOrEmpty(patient.Id))
        {
            return BadRequest("Patient ID must be provided by the client."); // bad request means there was a problem with the data from the client side
        }
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    // Delete: deletes a patient from the database given an id
    // Endpoint: /Patients/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null)
        {
            return NotFound();
        }
        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Update: updates a patient from the database given an id that already exists
    // Endpoint: /Patients/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Patient patient, string id)
    {
        if (patient.Id != id)
        {
            return BadRequest("Patient ID is not equal to the incoming ID.");
        }
        _context.Entry(patient).State = EntityState.Modified; // this line basically tells the server that we are trying to overwrite the existing object
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if (!_context.Patients.Any(p => p.Id == id)) // couldn't find the existing object in the database
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