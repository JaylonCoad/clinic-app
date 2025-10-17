using Maui.ClinicApp.ViewModels;

namespace Maui.ClinicApp.Views;

public partial class PatientPage : ContentPage
{
    private readonly PhysiciansViewModel _viewModel; // using dependency injection to inject the PhysiciansViewModel into this current page to check if there are patients available. this will prevent the user from trying to navigate to create an appointment without creating a physician
	public PatientPage(PhysiciansViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = new PatientsViewModel();
        _viewModel = viewModel;
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
            DisplayAlert("No Patient Selected", "Please select a Patient", "Ok");
            return;
        }
        Shell.Current.GoToAsync($"//AddPatient?patientId={selectedId}");
    }

    private void DeletePatient(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as PatientsViewModel)?.SelectedPatient?.Id ?? "";
        if (string.IsNullOrEmpty(selectedId))
        {
            DisplayAlert("No Patient Selected", "Please select a Patient", "Ok");
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
        var physiciansAvailable = _viewModel.HasPhysicians;
        if (string.IsNullOrEmpty(selectedId) && physiciansAvailable == false)
        {
            DisplayAlert("No Patient Selected & No Physicians Created", "Please select a Patient and Create a Physician", "Ok");
            return;
        }
        else if (string.IsNullOrEmpty(selectedId))
        {
            DisplayAlert("No Patient Selected", "Please select a Patient", "Ok");
            return;
        }
        else if (physiciansAvailable == false)
        {
            DisplayAlert("No Physicians Created", "Please create a Physician", "Ok");
            return;
        }
        Shell.Current.GoToAsync($"//PhysicianPage?patientId={selectedId}");
    }
}