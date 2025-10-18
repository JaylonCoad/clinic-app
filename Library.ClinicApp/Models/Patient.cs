using System;
using Library.ClinicApp.Services;

namespace Library.ClinicApp.Models;

public class Patient
{
    public string? Id { get; }
    public string? Name { get; set; }
    public string? Race { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }
    public List<string> Diagnoses { get; set; } = [];
    public List<string> Prescriptions { get; set; } = [];
    public List<Appointment> Appointments { get; set; } = [];
    public DateTime Birthday { get; set; }
    public DateOnly BirthdayPrint => DateOnly.FromDateTime(Birthday);
    public string Display
    {
        get
        {
            return ToString();
        }
    }
    public Patient()
    {
        Id = GenerateId();
        Birthday = DateTime.Today;
    }
    public Patient(string id)
    {
        var patientCopy = PatientServiceProxy.Current.Patients.FirstOrDefault(b => (b?.Id ?? "") == id);
        if (patientCopy != null)
        {
            Id = patientCopy.Id;
            Name = patientCopy.Name;
            Race = patientCopy.Race;
            Gender = patientCopy.Gender;
            Address = patientCopy.Address;
            Diagnoses = patientCopy.Diagnoses;
            Prescriptions = patientCopy.Prescriptions;
            Appointments = patientCopy.Appointments;
            Birthday = patientCopy.Birthday;
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
        return $"ID: {Id} || Name: {Name} || Gender: {Gender} || Race: {Race} || Address: {Address} || Birthday: {BirthdayPrint}";
    }
}
