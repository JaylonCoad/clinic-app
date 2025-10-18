using System;
using System.Runtime.Serialization;
using Library.ClinicApp.Models;

namespace Library.ClinicApp.Services;

public class AppointmentServiceProxy
{
    private List<Appointment?> appointments;
    private AppointmentServiceProxy()
    {
        appointments = new List<Appointment?>();
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
    public List<Appointment?> Appointments
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
        var existingAppointment = Appointments.FirstOrDefault(p => p?.Id == appointment.Id);
        if (existingAppointment != null) // existing appointment found, so edit instead of add
        {
            var index = Appointments.IndexOf(existingAppointment);
            Appointments.RemoveAt(index);
            Appointments.Insert(index, appointment);
        }
        else
        {
            if (IsTimeValid(appointment.AppointmentTime) && IsDateValid(appointment.AppointmentDatePrint) && IsPhysicianAvailable(appointment.PhysicianId, appointment.AppointmentDatePrint, appointment.AppointmentTime))
            {
                appointments.Add(appointment); // new appointment
            }
        }
        return appointment;
    }
    public bool IsTimeValid(TimeOnly time)
    {
        var earliestTime = new TimeOnly(8, 0, 0);
        var latestTime = new TimeOnly(16, 0, 0);
        bool withinBounds = time >= earliestTime && time <= latestTime;
        bool onHour = time.Minute == 0 && time.Second == 0;
        return withinBounds && onHour;
    }
    public bool IsDateValid(DateOnly date)
    {
        DateOnly earliestDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7));
        DateOnly latestDate = DateOnly.FromDateTime(DateTime.Today.AddDays(30));
        bool IsWeekday = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        if (date <= latestDate && date >= earliestDate && IsWeekday)
        {
            return true;
        }
        return false;
    }
    public bool IsPhysicianAvailable(string physicianId, DateOnly newAppointmentDate, TimeOnly newAppointmentTime)
    {
        var physician = PhysicianServiceProxy.Current.Physicians.FirstOrDefault(p => p?.Id == physicianId);
        if (physician?.Appointments == null || physician.Appointments.Count == 0)
        {
            return true;
        }
        bool conflictExists = physician.Appointments.Any(existingAppointment => existingAppointment.AppointmentDatePrint == newAppointmentDate && existingAppointment.AppointmentTime == newAppointmentTime);
        return !conflictExists;
    }
    public Appointment? Delete(string id)
    {
        var appointmentToDelete = appointments.Where(b => b != null).FirstOrDefault(b => (b?.Id ?? "") == id);
        appointments.Remove(appointmentToDelete);
        return appointmentToDelete;
    }
}
