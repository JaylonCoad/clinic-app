using System;
using Library.ClinicApp.Services;

namespace Library.ClinicApp.Models;

public class Appointment
{
    public string? Id { get; }
    public string? PhysicianId { get; set; }
    public string? PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public DateOnly AppointmentDatePrint => DateOnly.FromDateTime(AppointmentDate);
    public TimeOnly AppointmentTime { get; set; }

    public string Display
    {
        get
        {
            return ToString();
        }
    }
    public Appointment()
    {
        Id = GenerateId();
    }
    public string DisplayPhysicianName
    {
        get
        {
            var physician = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(p => p?.Id == PhysicianId);
            return physician?.Name ?? "Unknown Physician";
        }
    }
    public string DisplayPatientName
    {
        get
        {
            var patient = PatientServiceProxy.Current.Patients.FirstOrDefault(p => p?.Id == PatientId);
            return patient?.Name ?? "Unknown Patient";
        }
    }
    public Appointment(string id)
    {
        var appointmentCopy = AppointmentServiceProxy.Current.Appointments.FirstOrDefault(b => (b?.Id ?? "") == id);
        if (appointmentCopy != null)
        {
            Id = appointmentCopy.Id;
            PhysicianId = appointmentCopy.PhysicianId;
            PatientId = appointmentCopy.PatientId;
            AppointmentDate = appointmentCopy.AppointmentDate;
            AppointmentTime = appointmentCopy.AppointmentTime;
        }
    }
    private static string GenerateId() // generates a random 8 character alphanumeric string ID for each object created, if this were a bigger application used by hundreds or thousands of users i would check for the same ID amongst all other patients and physicians but in the context of this assignment not really necessary
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string([.. Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)])]);
    }
    public override string ToString()
    {
        return $"Appointment with Dr. {DisplayPhysicianName} for {DisplayPatientName} at {AppointmentTime} on {AppointmentDatePrint}";
    }
}
