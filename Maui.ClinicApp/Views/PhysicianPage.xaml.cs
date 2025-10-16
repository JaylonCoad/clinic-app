using Maui.ClinicApp.ViewModels;

namespace Maui.ClinicApp.Views;
[QueryProperty(nameof(PatientId), "patientId")]
public partial class PhysicianPage : ContentPage
{
    public string PatientId { get; set; }
	public PhysicianPage()
	{
        InitializeComponent();
        BindingContext = new PhysiciansViewModel();
	}

    private void AddPhysician(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AddPhysician?physicianId=");
    }

    private void EditPhysician(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as PhysiciansViewModel)?.SelectedPhysician?.Id ?? "";
        if (string.IsNullOrEmpty(selectedId))
        {
            return;
        }
        Shell.Current.GoToAsync($"//AddPhysician?physicianId={selectedId}");
    }

    private void DeletePhysician(object sender, EventArgs e)
    {
        (BindingContext as PhysiciansViewModel)?.Delete();
    }

    private void BackToHome(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as PhysiciansViewModel)?.Refresh();
    }
    private void GoToCreateAppointment(object sender, EventArgs e)
    {
        
    }
}