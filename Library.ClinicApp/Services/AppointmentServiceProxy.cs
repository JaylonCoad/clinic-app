using Library.ClinicApp.Models;

namespace Library.ClinicApp.Services;

public class AppointmentServiceProxy
{
    private List<Appointment> appointments;
    private static List<string> rooms = ["Room 1", "Room 2", "Room 3", "Room 4", "Room 5", "Room 6", "Room 7", "Room 8", "Room 9", "Room 10"];
    private static Dictionary<string, List<DateTime>> roomSchedule = [];
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
    public List<string> Rooms
    {
        get
        {
            return rooms;
        }
    }
    public Dictionary<string, List<DateTime>> RoomSchedule
    {
        get
        {
            return roomSchedule;
        }
    }
    public Appointment? AddOrUpdate(Appointment? appointment)
    {
        if (appointment == null)
        {
            return null;
        }
        var requestedPhysician = PhysicianServiceProxy.Current.PhysicianById(appointment.PhysicianId!);
        var requestedPatient = PatientServiceProxy.Current.PatientById(appointment.PatientId!);
        if (requestedPatient == null || requestedPhysician == null)
        {
            return null;
        }
        var existingAppointment = AppointmentById(appointment.Id);
        var checkConditions = IsTimeValid(appointment.AppointmentTimePrint) && IsDateValid(appointment.AppointmentDatePrint) && IsPhysicianAvailable(requestedPhysician.Id, appointment.AppointmentDatePrint, appointment.AppointmentTimePrint) && IsPatientAvailable(requestedPatient.Id, appointment.AppointmentDatePrint, appointment.AppointmentTimePrint);
        bool found = false;
        if (checkConditions)
        {
            appointment.AppointmentDate = appointment.AppointmentDatePrint.ToDateTime(appointment.AppointmentTimePrint);
            found = CheckForRoom(appointment);
        }
        if (!found)
        {
            return null;
        }
        if (existingAppointment != null) // existing appointment found, so edit instead of add
        {
            if (checkConditions && found)
            {
                RoomSchedule[existingAppointment.RoomNumber].Remove(existingAppointment.AppointmentDate);
                requestedPatient?.Appointments.RemoveAll(p => p.Id == existingAppointment.Id);
                requestedPhysician?.Appointments.RemoveAll(p => p.Id == existingAppointment.Id);
                var index = Appointments.IndexOf(existingAppointment);
                Appointments.RemoveAt(index);
                Appointments.Insert(index, appointment);
                requestedPatient!.Appointments.Add(appointment);
                requestedPhysician!.Appointments.Add(appointment);
                RoomSchedule[appointment.RoomNumber].Add(appointment.AppointmentDate);
            }
        }
        else
        {
            if (checkConditions && found)
            {
                if (!RoomSchedule.TryGetValue(appointment.RoomNumber, out List<DateTime>? value))
                {
                    value = [];
                    RoomSchedule[appointment.RoomNumber] = value;
                }
                appointment.Completed = false;
                value.Add(appointment.AppointmentDate);
                appointments.Add(appointment); // new appointment
                requestedPatient.Appointments.Add(appointment);
                requestedPhysician.Appointments.Add(appointment);
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
        var physician = PhysicianServiceProxy.Current.PhysicianById(physicianId);
        if (physician?.Appointments == null || physician.Appointments.Count == 0)
        {
            return true;
        }
        bool conflictExists = physician.Appointments.Any(existingAppointment => existingAppointment.AppointmentDatePrint == newAppointmentDate && existingAppointment.AppointmentTimePrint == newAppointmentTime);
        return !conflictExists;
    }
    public bool IsPatientAvailable(string patientId, DateOnly newAppointmentDate, TimeOnly newAppointmentTime)
    {
        var patient = PatientServiceProxy.Current.PatientById(patientId);
        if (patient?.Appointments == null || patient.Appointments.Count == 0)
        {
            return true;
        }
        bool conflictExists = patient.Appointments.Any(existingAppointment => existingAppointment.AppointmentDatePrint == newAppointmentDate && existingAppointment.AppointmentTimePrint == newAppointmentTime);
        return !conflictExists;
    }
    public Appointment? Delete(string id)
    {
        var appointmentToDelete = AppointmentById(id);
        if (appointmentToDelete != null)
        {
            var patient = PatientServiceProxy.Current.PatientById(appointmentToDelete.PatientId!);
            var physician = PhysicianServiceProxy.Current.PhysicianById(appointmentToDelete.PhysicianId!);
            patient?.Appointments.RemoveAll(p => p.Id == id);
            physician?.Appointments.RemoveAll(p => p.Id == id);
            // remove the time from the list at the room number of the appointment
            RoomSchedule[appointmentToDelete.RoomNumber].Remove(appointmentToDelete.AppointmentDate);
            appointments.RemoveAll(p => p.Id == appointmentToDelete.Id);
        }
        return appointmentToDelete;
    }
    public bool CheckForRoom(Appointment appointment)
    {
        // go through each room, check if it's in the dictionary, if it is then check if there's a time conflict, if not then you can assign that room
        bool foundRoom = false;
        foreach (var room in Rooms)
        {
            if (RoomSchedule.ContainsKey(room))
            {
                // iterate through the list at the room key and if it contains the appointment time then we cannot schedule the room at that time
                if (!RoomSchedule[room].Contains(appointment.AppointmentDate))
                {
                    appointment.RoomNumber = room;
                    foundRoom = true;
                    break;
                }
            }
            else
            {
                appointment.RoomNumber = room;
                foundRoom = true;
                break;
            }
        }
        return foundRoom;
    }
    public Appointment CompleteAppointment(Appointment appointment)
    {
        appointment.Completed = true;
        var existingAppointment = AppointmentById(appointment.Id);
        if (existingAppointment != null)
        {
            var index = Appointments.IndexOf(existingAppointment);
            Appointments.RemoveAt(index);
            Appointments.Insert(index, appointment);
            RoomSchedule[appointment.RoomNumber].Remove(appointment.AppointmentDate); // room becomes available when an appointment is marked as completed
            var patient = PatientServiceProxy.Current.PatientById(appointment.PatientId!);
            var physician = PhysicianServiceProxy.Current.PhysicianById(appointment.PhysicianId!);
            patient!.CompletedAppointments.Add(appointment);
            physician!.CompletedAppointments.Add(appointment);
            patient.Appointments.RemoveAll(p => p.Id == appointment.Id);
            physician.Appointments.RemoveAll(p => p.Id == appointment.Id);
        }
        return appointment;
    }

    public Appointment? AppointmentById(string appointmentId)
    {
        if (string.IsNullOrEmpty(appointmentId))
        {
            return null;
        }
        return appointments.FirstOrDefault(p => p?.Id == appointmentId);
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
