using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;
namespace Maui.ClinicApp.ViewModels;

public class PatientsViewModel : INotifyPropertyChanged
{
    public PatientViewModel? SelectedPatient { get; set; }
    public bool IsPatientSelected => SelectedPatient != null;
    public ObservableCollection<PatientViewModel?> Patients
    {
        get
        {
            return new ObservableCollection<PatientViewModel?>(PatientServiceProxy.Current.Patients.Select(b => new PatientViewModel (b)));
        }
    }
    public void Refresh()
    {
        NotifyPropertyChanged(nameof(Patients));
    }
    public void Delete()
    {
        if (string.IsNullOrEmpty(SelectedPatient?.Model?.Id)) // changed this from SelectedPatient
        {
            return;
        }
        PatientServiceProxy.Current.Delete(SelectedPatient.Model.Id); // this should never be null
        SelectedPatient = null;
        NotifyPropertyChanged(nameof(Patients));
        NotifyPropertyChanged(nameof(SelectedPatient));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
