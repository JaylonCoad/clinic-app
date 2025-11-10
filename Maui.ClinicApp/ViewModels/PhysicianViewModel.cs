using System.Windows.Input;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.ViewModels;

public class PhysicianViewModel
{
    public PhysicianViewModel()
    {
        Model = new Physician();
        SetupCommands();
    }
    public PhysicianViewModel(Physician? model)
    {
        Model = model;
        SetupCommands();
    }
    private void SetupCommands()
    {
        DeleteCommand = new Command(DoDelete);
        EditCommand = new Command((p) => DoEdit(p as PhysicianViewModel));
    }
    private void DoDelete()
    {
        if (!string.IsNullOrEmpty(Model?.Id))
        {
            PhysicianServiceProxy.Current.Delete(Model.Id);
        }
    }
    private void DoEdit(PhysicianViewModel? pvm)
    {
        if (pvm == null)
        {
            return;
        }
        var selectedPhysicianId = pvm?.Model?.Id;
        Shell.Current.GoToAsync($"//AddPhysician?physicianId={selectedPhysicianId}");
    }
    public Physician? Model { get; set; }
    public ICommand? DeleteCommand { get; set; }
    public ICommand? EditCommand { get; set; }
}
