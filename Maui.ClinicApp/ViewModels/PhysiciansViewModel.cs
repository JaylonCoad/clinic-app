using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.ViewModels;

public class PhysiciansViewModel : INotifyPropertyChanged
{
    public Physician? SelectedPhysician { get; set; }
    public ObservableCollection<Physician?> Physicians
    {
        get
        {
            return new ObservableCollection<Physician?>(PhysicianServiceProxy.Current.Physicians);
        }
    }
    public void Refresh()
    {
        NotifyPropertyChanged(nameof(Physicians));
    }
    public void Delete()
    {
        if (SelectedPhysician == null)
        {
            return;
        }
        PhysicianServiceProxy.Current.Delete(SelectedPhysician.Id);
        SelectedPhysician = null;
        NotifyPropertyChanged(nameof(Physicians));
        NotifyPropertyChanged(nameof(SelectedPhysician));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
