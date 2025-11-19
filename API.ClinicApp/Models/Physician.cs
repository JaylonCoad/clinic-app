using System;

namespace API.ClinicApp.Models;

public class Physician
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public DateTime Graduation { get; set; }
}
