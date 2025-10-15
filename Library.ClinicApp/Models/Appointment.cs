using System;

namespace Library.ClinicApp.Models;

public class Appointment(Physician physician, Patient patient, DateTime appointmentTime)
{
    public Physician? Physician { get; set; } = physician;
    public Patient? Patient { get; set; } = patient;
    public DateTime AppointmentTime { get; set; } = appointmentTime;
    public override string ToString()
    {
        return $"Appointment with Dr. {Physician?.Name} for {Patient?.Name} at {AppointmentTime.ToShortDateString()} {AppointmentTime.ToShortTimeString()}";
    }
}
