using System;
using Library.ClinicApp.Models;
namespace Maui.ClinicApp.ViewModels;

public class PatientsViewModel
{
    public List<Patient?> Patients
    {
        get
        {
            return new List<Patient>
            {
                new Patient { Name = "idk", Race = "green"}
            };
        }
    }
    public Patient? SelectedPatient { get; set; }

}
