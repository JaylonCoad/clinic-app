using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.ClinicApp.Models;
using API.ClinicApp.Data;

namespace API.ClinicApp.Controllers;

[ApiController]
[Route("api/[controller]")] // Maps to http://localhost:xxxx/Appointments
public class AppointmentsController : ControllerBase
{
    private readonly ClinicDbContext _context;

    public AppointmentsController(ClinicDbContext context)
    {
        _context = context;
    }

    // GET: returns all appointments from the database
    // Endpoint: /Appointments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Appointment>>> Get() // returning an IEnumerable<T> because this is read-only and doesn't lock us into using a List<T> data structure if we wanted to change data types later since they are all technically IEnumerable<T>
    {
        return await _context.Appointments.ToListAsync();
    }

    // GET: returns one appointment given an id
    // Endpoint: /Appointments/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Appointment>> GetById(string id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }
        return appointment;
    }

    // POST: adds an appointment to the database
    // Endpoint: /Appointments
    [HttpPost]
    public async Task<ActionResult<Appointment>> Add(Appointment appointment)
    {
        if (string.IsNullOrEmpty(appointment.Id))
        {
            return BadRequest("Appointment ID must be provided by the client."); // bad request means there was a problem with the data from the client side
        }
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, appointment);
    }

    // Delete: deletes an appointment from the database given an id
    // Endpoint: /Appointments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }
        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Update: updates an appointment from the database given an id that already exists
    // Endpoint: /Appointments/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Appointment appointment, string id)
    {
        if (appointment.Id != id)
        {
            return BadRequest("Appointment ID is not equal to the incoming ID.");
        }
        _context.Entry(appointment).State = EntityState.Modified; // this line basically tells the server that we are trying to overwrite the existing object
        try
        {
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            if (!_context.Appointments.Any(p => p.Id == id)) // couldn't find the existing object in the database
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