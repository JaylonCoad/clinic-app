using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.ViewModels;

public class AppointmentsViewModel : INotifyPropertyChanged
{
    public Appointment? SelectedAppointment { get; set; }
    public ObservableCollection<Appointment?> Appointments
    {
        get
        {
            return new ObservableCollection<Appointment?>(AppointmentServiceProxy.Current.Appointments);
        }
    }
    public void Refresh()
    {
        NotifyPropertyChanged(nameof(Appointments));
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
