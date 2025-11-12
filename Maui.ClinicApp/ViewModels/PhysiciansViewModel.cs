using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.ViewModels;

public class PhysiciansViewModel : INotifyPropertyChanged
{
    public PhysicianViewModel? SelectedPhysician { get; set; }
    public bool HasPhysicians => Physicians.Any(); // boolean to tell me if there are physicians available, tells us if we can bring a patientid to this page to create an appointment
    public ObservableCollection<PhysicianViewModel?> Physicians
    {
        get
        {
            return new ObservableCollection<PhysicianViewModel?>(PhysicianServiceProxy.Current.Physicians.Select(b => new PhysicianViewModel (b)));
        }
    }
    public void Refresh()
    {
        NotifyPropertyChanged(nameof(Physicians));
    }
    public void Delete()
    {
        if (string.IsNullOrEmpty(SelectedPhysician?.Model?.Id)) // changed this from SelectedPhysician
        {
            return;
        }
        PhysicianServiceProxy.Current.Delete(SelectedPhysician.Model.Id);
        SelectedPhysician = null;
        NotifyPropertyChanged(nameof(Physicians));
        NotifyPropertyChanged(nameof(SelectedPhysician));
    }

    public void SortByGraduationAscending()
    {
        PhysicianServiceProxy.Current.SortPhysiciansAscending();
    }

    public void SortByGraduationDescending()
    {
        PhysicianServiceProxy.Current.SortPhysiciansDescending();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}