using System;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Library.ClinicApp.Models;
using Microsoft.VisualBasic;

namespace Enterprise.ClinicApp;

public class AppointmentEC
{
    private readonly HttpClient _client;
    private const string BaseUrl = "http://localhost:5075/api/Appointments";
    public AppointmentEC()
    {
        _client = new HttpClient();
    }
    // GET: api/appointments
    public async Task<List<Appointment>> GetAppointmentsAsync()
    {
        return await _client.GetFromJsonAsync<List<Appointment>>(BaseUrl) ?? [];
    }
    // POST: api/appointments
    public async Task<Appointment?> AddAppointmentAsync(Appointment appointment)
    {
        var response = await _client.PostAsJsonAsync(BaseUrl, appointment);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to add appointment");
        }
        return await response.Content.ReadFromJsonAsync<Appointment>();
    }
    // PUT: api/appointments/{id}
    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{appointment.Id}", appointment);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to update appointment");
        }
    }
    // DELETE: api/appointments/{id}
    public async Task DeleteAppointmentAsync(string id)
    {
        var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to delete appointment");
        }
    }
}
