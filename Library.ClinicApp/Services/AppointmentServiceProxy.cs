using System;
using System.Runtime.Serialization;
using Library.ClinicApp.Models;

namespace Library.ClinicApp.Services;

public class AppointmentServiceProxy
{
    private List<Appointment> appointments;
    private AppointmentServiceProxy()
    {
        appointments = new List<Appointment>();
    }
    private static AppointmentServiceProxy? instance;
    private static object instanceLock = new object();
    public static AppointmentServiceProxy Current
    {
        get
        {
            lock(instanceLock)
            { 
                if (instance == null)
                {
                    instance = new AppointmentServiceProxy();
                }
            }
            return instance;
        }
    }
    public List<Appointment> Appointments
    {
        get
        {
            return appointments;
        }
    }
    public Appointment? AddOrUpdate(Appointment? appointment)
    {
        if (appointment == null)
        {
            return null;
        }
        var requestedPhysician = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(p => p?.Id == appointment.PhysicianId);
        var requestedPatient = PatientServiceProxy.Current.Patients.FirstOrDefault(p => p?.Id == appointment.PatientId);
        if (requestedPatient == null || requestedPhysician == null)
        {
            return null;
        }
        var existingAppointment = Appointments.FirstOrDefault(p => p?.Id == appointment.Id);
        var checkConditions = IsTimeValid(appointment.AppointmentTimePrint) && IsDateValid(appointment.AppointmentDatePrint) && IsPhysicianAvailable(requestedPhysician.Id, appointment.AppointmentDatePrint, appointment.AppointmentTimePrint);
        if (existingAppointment != null) // existing appointment found, so edit instead of add
        {
            if (checkConditions)
            {
                var index = Appointments.IndexOf(existingAppointment);
                Appointments.RemoveAt(index);
                Appointments.Insert(index, appointment);
                requestedPatient?.Appointments.Remove(existingAppointment);
                requestedPhysician?.Appointments.Remove(existingAppointment);
                requestedPatient?.Appointments.Add(appointment);
                requestedPhysician?.Appointments.Add(appointment);
            }
        }
        else
        {
            if (checkConditions)
            {
                appointment.AppointmentDate = appointment.AppointmentDatePrint.ToDateTime(appointment.AppointmentTimePrint);
                appointments.Add(appointment); // new appointment
                requestedPatient?.Appointments.Add(appointment);
                requestedPhysician?.Appointments.Add(appointment);
            }
        }
        return appointment;
    }
    public bool IsTimeValid(TimeOnly time)
    {
        TimeOnly earliestTime = new(8, 0, 0);
        TimeOnly latestTime = new(16, 0, 0);
        bool withinBounds = time >= earliestTime && time <= latestTime;
        bool onHour = time.Minute == 0 && time.Second == 0;
        return withinBounds && onHour;
    }
    public bool IsDateValid(DateOnly date)
    {
        DateOnly earliestDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7));
        DateOnly latestDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));
        bool IsWeekday = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        return date <= latestDate && date >= earliestDate && IsWeekday;
    }
    public bool IsPhysicianAvailable(string physicianId, DateOnly newAppointmentDate, TimeOnly newAppointmentTime)
    {
        var physician = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(p => p?.Id == physicianId);
        if (physician?.Appointments == null || physician.Appointments.Count == 0)
        {
            return true;
        }
        bool conflictExists = physician.Appointments.Any(existingAppointment => existingAppointment.AppointmentDatePrint == newAppointmentDate && existingAppointment.AppointmentTimePrint == newAppointmentTime);
        return !conflictExists;
    }
    public Appointment? Delete(string id)
    {
        var appointmentToDelete = appointments.FirstOrDefault(b => b.Id == id);
        if (appointmentToDelete != null)
        {
            appointments.Remove(appointmentToDelete);
        }
        return appointmentToDelete;
    }

    public void SortAppointmentsAscending()
    {
        appointments = appointments.OrderBy(p => p?.AppointmentDate).ToList();
    }

    public void SortAppointmentsDescending()
    {
        appointments = appointments.OrderByDescending(p => p?.AppointmentDate).ToList();
    }
}
