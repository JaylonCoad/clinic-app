using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.Views;
[QueryProperty(nameof(PatientId), "patientId")]
public partial class AddPatient : ContentPage
{
    public string PatientId { get; set; }
	public AddPatient()
	{
        InitializeComponent();
	}

    private void AddClicked(object sender, EventArgs e)
    {
        PatientServiceProxy.Current.AddOrUpdate(BindingContext as Patient);
        Shell.Current.GoToAsync("//PatientPage");
    }

    private void BackToPatientPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//PatientPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (string.IsNullOrEmpty(PatientId))
        {
            BindingContext = new Patient();
        }
        else
        {
            BindingContext = new Patient(PatientId);
        }
    }
}