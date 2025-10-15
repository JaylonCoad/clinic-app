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

    public Patient? AddOrUpdate(Patient? patient)
    {
        if (patient == null)
        {
            return null;
        }
        var existingPatient = Patients.FirstOrDefault(p => p?.Id == patient.Id);
        if (existingPatient != null) // existing patient found, so edit instead of add
        {
            var index = Patients.IndexOf(existingPatient);
            Patients.RemoveAt(index);
            Patients.Insert(index, patient);
        }
        else
        {
            patients.Add(patient); // new patient
        }

        return patient;
    }

    public Patient? Delete(string id)
    {
        var patientToDelete = patients.Where(b => b != null).FirstOrDefault(b => (b?.Id ?? "") == id);
        patients.Remove(patientToDelete);
        return patientToDelete;
    }
}
