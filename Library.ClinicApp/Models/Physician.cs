using System;
using Library.ClinicApp.Services;

namespace Library.ClinicApp.Models;

public class Physician
{
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    public DateTime Graduation { get; set; }
    public DateOnly GraduationPrint => DateOnly.FromDateTime(Graduation);
    public List<Appointment> Appointments { get; set; } = []; // stores current, open appointments
    public List<Appointment> CompletedAppointments { get; set; } = []; // stores appointments that have been marked as completed
    public string Display
    {
        get
        {
            return ToString();
        }
    }
    public Physician()
    {
        Id = GenerateId();
        Graduation = DateTime.Today;
    }
    public Physician(string id)
    {
        var physicianCopy = PhysicianServiceProxy.Current.PhysicianById(id);
        if (physicianCopy != null)
        {
            Id = physicianCopy.Id;
            Name = physicianCopy.Name;
            LicenseNumber = physicianCopy.LicenseNumber;
            Specialization = physicianCopy.Specialization;
            Graduation = physicianCopy.Graduation;
            Appointments = physicianCopy.Appointments;
            CompletedAppointments = physicianCopy.CompletedAppointments;
        }
    }
    private static string GenerateId() // same comment as patient class
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string([.. Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)])]);
    }
    public override string ToString()
    {
        return $"ID: {Id} || Name: {Name} || License Number: {LicenseNumber} || Specialization: {Specialization} || Graduation Date: {GraduationPrint}";
    }
}
