using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;
namespace Maui.ClinicApp.ViewModels;

public class PatientsViewModel : INotifyPropertyChanged
{
    public Patient? SelectedPatient { get; set; }
    public ObservableCollection<Patient?> Patients
    {
        get
        {
            return new ObservableCollection<Patient?>(PatientServiceProxy.Current.Patients);
        }
    }
    public void Refresh()
    {
        NotifyPropertyChanged(nameof(Patients));
    }
    public void Delete()
    {
        if (SelectedPatient == null)
        {
            return;
        }
        PatientServiceProxy.Current.Delete(SelectedPatient.Id);
        NotifyPropertyChanged(nameof(Patients));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
