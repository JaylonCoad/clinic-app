using System;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Library.ClinicApp.Models;
using Microsoft.VisualBasic;

namespace Enterprise.ClinicApp;

public class PhysicianEC
{
    private readonly HttpClient _client;
    private const string BaseUrl = "http://localhost:5075/api/Physicians";
    public PhysicianEC()
    {
        _client = new HttpClient();
    }
    // GET: api/physicians
    public async Task<List<Physician>> GetPhysiciansAsync()
    {
        return await _client.GetFromJsonAsync<List<Physician>>(BaseUrl) ?? [];
    }
    // POST: api/physicians
    public async Task<Physician?> AddPhysicianAsync(Physician physician)
    {
        var response = await _client.PostAsJsonAsync(BaseUrl, physician);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to add physician");
        }
        return await response.Content.ReadFromJsonAsync<Physician>();
    }
    // PUT: api/physicians/{id}
    public async Task UpdatePhysicianAsync(Physician physician)
    {
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{physician.Id}", physician);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to update physician");
        }
    }
    // DELETE: api/physicians/{id}
    public async Task DeletePhysicianAsync(string id)
    {
        var response = await _client.DeleteAsync($"{BaseUrl}/{id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to delete physician");
        }
    }
}
