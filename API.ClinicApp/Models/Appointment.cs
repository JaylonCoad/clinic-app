using System;

namespace API.ClinicApp.Models;

public class Appointment
{
    public string Id { get; set; } = string.Empty;
    public string PhysicianId { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public Physician? Physician { get; set; }
    public Patient? Patient { get; set; }
    public DateTime AppointmentDate { get; set; }
    public bool Completed { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Treatment { get; set; } = string.Empty;
}
