using Maui.ClinicApp.ViewModels;

namespace Maui.ClinicApp.Views;

public partial class PatientPage : ContentPage
{
	public PatientPage()
	{
        InitializeComponent();
        BindingContext = new PatientsViewModel();
	}

    private void AddPatient(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AddPatient?patientId=");
    }

    private void EditPatient(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as PatientsViewModel)?.SelectedPatient?.Id ?? "";
        if (string.IsNullOrEmpty(selectedId))
        {
            return;
        }
        Shell.Current.GoToAsync($"//AddPatient?patientId={selectedId}");
    }

    private void DeletePatient(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as PatientsViewModel)?.SelectedPatient?.Id ?? "";
        if (string.IsNullOrEmpty(selectedId))
        {
            return;
        }
        (BindingContext as PatientsViewModel)?.Delete();
    }

    private void BackToHome(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as PatientsViewModel)?.Refresh();
    }

    private void SendPatientToPhysicianPage(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as PatientsViewModel)?.SelectedPatient?.Id ?? "";
        // we want this selected ID to be non null
        // we also want to assure that there is at least 1 physician in the physicianpage because we need one physician
        if (string.IsNullOrEmpty(selectedId))
        {
            return;
        }
        Shell.Current.GoToAsync($"//PhysicianPage?patientId={selectedId}");
    }
}