using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.ViewModels;

public class AppointmentViewModel : INotifyPropertyChanged
{
    public AppointmentViewModel()
    {
        Model = new Appointment();
        SetupCommands();
    }
    public AppointmentViewModel(Appointment model)
    {
        Model = model;
        SetupCommands();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void Refresh()
    {
        NotifyPropertyChanged(nameof(Model));
        NotifyPropertyChanged(nameof(AppointmentColor));
    }

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SetupCommands()
    {
        DeleteCommand = new Command(DoDelete);
        EditCommand = new Command((p) => DoEdit(p as AppointmentViewModel));
    }
    private void DoDelete()
    {
        if (!string.IsNullOrEmpty(Model?.Id))
        {
            AppointmentServiceProxy.Current.Delete(Model.Id);
        }
    }
    private void DoEdit(AppointmentViewModel? pvm)
    {
        if (pvm == null)
        {
            return;
        }
        var selectedAppointmentId = pvm?.Model?.Id;
        Shell.Current.GoToAsync($"//AddAppointment?appointmentId={selectedAppointmentId}&isAdding=false");
    }
    public Appointment Model { get; set; }
    public ICommand? DeleteCommand { get; set; }
    public ICommand? EditCommand { get; set; }
    public bool Completed => Model.Completed;
    public bool NotCompleted => !Completed;
    // public Color AppointmentColor => Completed ? Colors.DarkGray : Colors.Transparent;
    public Color AppointmentColor => Completed ? Color.FromArgb("#404040") : Colors.Transparent;
}
