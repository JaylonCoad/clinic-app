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

    // GET: /Patients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> Get()
    {
        return await _context.Patients.ToListAsync();
    }

    // GET: /Patients/{id}
    // Example: /Patients/X99Z12
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

    // POST: /Patients
    [HttpPost]
    public async Task<ActionResult<Patient>> Add([FromBody] Patient patient)
    {
        // We rely on the MAUI app (Client) to generate the String ID
        if (string.IsNullOrEmpty(patient.Id))
        {
            return BadRequest("Patient ID must be provided by the client.");
        }
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }
}