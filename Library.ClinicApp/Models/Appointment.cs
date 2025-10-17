using System;
using Library.ClinicApp.Services;

namespace Library.ClinicApp.Models;

public class Appointment
{
    public string? Id { get; }
    public Physician? Physician { get; set; }
    public Patient? Patient { get; set; }
    public DateTime? AppointmentTime { get; set; }
    public string? TimeOfDay { get; set; } // AM or PM
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
    public Appointment(string id)
    {
        var appointmentCopy = AppointmentServiceProxy.Current.Appointments.FirstOrDefault(b => (b?.Id ?? "") == id);
        if (appointmentCopy != null)
        {
            Id = appointmentCopy.Id;
            Physician = appointmentCopy.Physician;
            Patient = appointmentCopy.Patient;
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
        return $"Appointment with Dr. {Physician?.Name} for {Patient?.Name} at {AppointmentTime?.ToShortDateString()} {AppointmentTime?.ToShortTimeString()}";
    }
}
