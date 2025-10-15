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
        if (string.IsNullOrEmpty(patient?.Id))
        {
            patients.Add(patient);
        }
        else
        {
            var patientToEdit = Patients.FirstOrDefault(b => (b?.Id ?? "") == patient.Id);
            if (patientToEdit != null)
            {
                var index = Patients.IndexOf(patientToEdit);
                Patients.RemoveAt(index);
                patients.Insert(index, patient);
            }
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
