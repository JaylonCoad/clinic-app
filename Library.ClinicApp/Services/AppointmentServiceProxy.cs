using System;
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
}
