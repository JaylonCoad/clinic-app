using System;
using Library.ClinicApp.Services;

namespace Library.ClinicApp.Models;

public class Physician
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? LicenseNumber { get; set; }
    public string? Specialization { get; set; }
    public DateTime Graduation { get; set; }
    public DateOnly GraduationPrint => DateOnly.FromDateTime(Graduation);
    public List<Appointment> Appointments { get; set; } = [];
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
        var physicianCopy = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(b => (b?.Id ?? "") == id);
        if (physicianCopy != null)
        {
            Id = physicianCopy.Id;
            Name = physicianCopy.Name;
            LicenseNumber = physicianCopy.LicenseNumber;
            Specialization = physicianCopy.Specialization;
            Graduation = physicianCopy.Graduation;
            Appointments = physicianCopy.Appointments;
        }
    }
    private static string GenerateId() // same comment as patient class
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string([.. Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)])]);
    }
    // public bool IsValidAppointment(DateTime requestedDate) // checks the appointment
    // {
    //     var earliestDate = DateTime.Today.AddDays(7); // earliest appointment day is 1 week in advance
    //     var latestDate = earliestDate.AddMonths(1); // latest appointment date is 4 weeks from the earliest date, giving user a total one month range to book
    //     if (requestedDate.Minute != 0 || requestedDate.Second != 0)
    //     {
    //         Console.WriteLine("Appointments must be booked on the hour (e.g., 09:00, 10:00).");
    //         return false;
    //     }
    //     if (requestedDate.Date < earliestDate || requestedDate.Date > latestDate)
    //     {
    //         Console.WriteLine($"Appointments must be scheduled between {earliestDate.ToShortDateString()} and {latestDate.ToShortDateString()}.");
    //         return false;
    //     }
    //     if (requestedDate.DayOfWeek == DayOfWeek.Saturday || requestedDate.DayOfWeek == DayOfWeek.Sunday) // check if the day is m-f
    //     {
    //         Console.WriteLine("Appointments are only available Monday through Friday.");
    //         return false;
    //     }
    //     if (requestedDate.Hour < 8 || requestedDate.Hour >= 17) // check if the time is within 8am to 5pm
    //     {
    //         Console.WriteLine("Appointments must be between 8 AM and 5 PM.");
    //         return false;
    //     }
    //     foreach (var existingAppointment in Appointments) // check for double-booking
    //     {
    //         if (existingAppointment.AppointmentTime == requestedDate)
    //         {
    //             Console.WriteLine("That time slot is already booked.");
    //             return false;
    //         }
    //     }
    //     return true;
    // }
    // public static void CreateNotes(Patient patient) // adding diagnosis and prescription to the patient
    // {
    //     Console.WriteLine($"Enter {patient}'s diagnosis: ");
    //     var diagnosis = Console.ReadLine();
    //     Console.Clear();
    //     while (string.IsNullOrWhiteSpace(diagnosis))
    //     {
    //         Console.WriteLine($"Invalid input. Enter diagnosis: ");
    //         diagnosis = Console.ReadLine();
    //         Console.Clear();
    //     }
    //     Console.WriteLine($"Enter {patient}'s prescription: ");
    //     var prescription = Console.ReadLine();
    //     Console.Clear();
    //     while (string.IsNullOrWhiteSpace(prescription))
    //     {
    //         Console.WriteLine($"Invalid input. Enter prescription: ");
    //         prescription = Console.ReadLine();
    //         Console.Clear();
    //     }
    //     patient.Diagnoses.Add(diagnosis);
    //     patient.Prescriptions.Add(prescription);
    // }
    public override string ToString()
    {
        return $"ID: {Id} || Name: {Name} || License Number: {LicenseNumber} || Specialization: {Specialization} || Graduation Date: {GraduationPrint}";
    }
}
