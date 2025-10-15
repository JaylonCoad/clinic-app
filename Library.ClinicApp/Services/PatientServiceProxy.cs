using System;
using Library.ClinicApp.Models;

namespace Library.ClinicApp.Services;

public class PatientServiceProxy
{
    private List<Patient?> patients;
    private PatientServiceProxy()
    {
        patients = new List<Patient?>();
    }
    private static PatientServiceProxy? instance;
    private static object instanceLock = new object();
    public static PatientServiceProxy Current
    {
        get
        {
            lock(instanceLock)
            { 
                if (instance == null)
                {
                    instance = new PatientServiceProxy();
                }
            }

            return instance;
        }
    }

    public List<Patient?> Patients
    {
        get
        {
            return patients;
        }
    }

    // public Patient? AddOrUpdate(Patient? patient)
    // {
        
    // }

    // public Patient? Delete(string id)
    // {
    //     return patients;
    // }
}
