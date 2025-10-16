namespace Maui.ClinicApp.Views;

public partial class AppointmentPage : ContentPage
{
	public AppointmentPage()
	{
		InitializeComponent();
	}

    private void BackToHome(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }
}