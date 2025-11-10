using System;
using System.Windows.Input;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.ViewModels;

public class PatientViewModel
{
    public PatientViewModel()
    {
        Model = new Patient();
        SetupCommands();
    }
    public PatientViewModel(Patient? model)
    {
        Model = model;
        SetupCommands();
    }
    private void SetupCommands()
    {
        DeleteCommand = new Command(DoDelete);
        EditCommand = new Command((p) => DoEdit(p as PatientViewModel));
    }
    private void DoDelete()
    {
        if (!string.IsNullOrEmpty(Model?.Id))
        {
            PatientServiceProxy.Current.Delete(Model.Id);
        }
    }
    private void DoEdit(PatientViewModel? pvm)
    {
        if (pvm == null)
        {
            return;
        }
        var selectedPatientId = pvm?.Model?.Id;
        Shell.Current.GoToAsync($"//AddPatient?patientId={selectedPatientId}");
    }
    public Patient? Model { get; set; }
    public ICommand? DeleteCommand { get; set; }
    public ICommand? EditCommand { get; set; }
}
