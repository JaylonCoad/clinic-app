using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.ViewModels;

public class AppointmentsViewModel : INotifyPropertyChanged
{
    public AppointmentViewModel? SelectedAppointment { get; set; }
    public ObservableCollection<AppointmentViewModel?> Appointments
    {
        get
        {
            return new ObservableCollection<AppointmentViewModel?>(AppointmentServiceProxy.Current.Appointments.Select(b => new AppointmentViewModel (b)));
        }
    }
    public void Refresh()
    {
        NotifyPropertyChanged(nameof(Appointments));
    }
    public void Delete()
    {
        if (string.IsNullOrEmpty(SelectedAppointment?.Model?.Id)) // changed this from SelectedAppointment
        {
            return;
        }
        AppointmentServiceProxy.Current.Delete(SelectedAppointment.Model.Id);
        SelectedAppointment = null;
        NotifyPropertyChanged(nameof(Appointments));
        NotifyPropertyChanged(nameof(SelectedAppointment));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
