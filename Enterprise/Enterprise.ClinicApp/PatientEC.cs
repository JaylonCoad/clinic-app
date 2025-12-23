using System;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Library.ClinicApp.Models;
using Microsoft.VisualBasic;

namespace Enterprise.ClinicApp;

public class PatientEC
{
    private readonly HttpClient _client;
    private const string BaseUrl = "http://localhost:5075/api/Patients";
    public PatientEC()
    {
        _client = new HttpClient();
    }
    // GET: api/patients
    public async Task<List<Patient>> GetPatientsAsync()
    {
        return await _client.GetFromJsonAsync<List<Patient>>(BaseUrl) ?? [];
    }
    // POST: api/patients
    public async Task<Patient?> AddPatientAsync(Patient patient)
    {
        var response = await _client.PostAsJsonAsync(BaseUrl, patient);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to add patient");
        }
        return await response.Content.ReadFromJsonAsync<Patient>();
    }
    // PUT: api/patients/{id}
    public async Task UpdatePatientAsync(Patient patient)
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{patient.Id}", patient);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to update patient");
        }
    }
    // DELETE: api/patients/{id}
    public async Task DeletePatientAsync(string id)
    {
        var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to delete patient");
        }
    }
}
