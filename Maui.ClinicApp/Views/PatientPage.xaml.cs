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
    }

    private void EditPatient(object sender, EventArgs e)
    {
    }

    private void DeletePatient(object sender, EventArgs e)
    {
    }
}