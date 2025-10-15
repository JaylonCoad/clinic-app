namespace Maui.ClinicApp;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

    private void GoToPatientView(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//PatientPage");
    }

    private void GoToPhysicianView(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//PhysicianPage");
    }

    private void GoToAppointmentView(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AppointmentPage");
    }

}

