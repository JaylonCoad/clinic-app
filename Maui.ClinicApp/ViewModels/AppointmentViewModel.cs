using System;
using System.Windows.Input;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.ViewModels;

public class AppointmentViewModel
{
    public AppointmentViewModel()
    {
        Model = new Appointment();
        SetupCommands();
    }
    public AppointmentViewModel(Appointment? model)
    {
        Model = model;
        SetupCommands();
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
    public Appointment? Model { get; set; }
    public ICommand? DeleteCommand { get; set; }
    public ICommand? EditCommand { get; set; }
}
