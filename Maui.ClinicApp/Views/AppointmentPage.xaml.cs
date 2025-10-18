using Maui.ClinicApp.ViewModels;

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

    private void AddAppointment(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//PatientPage");
    }

    private void EditAppointment(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as AppointmentsViewModel)?.SelectedAppointment?.Id ?? "";
        if (string.IsNullOrEmpty(selectedId))
        {
            DisplayAlert("No Appointment Selected", "Please select an Appointment", "Ok");
            return;
        }
        Shell.Current.GoToAsync($"//AddAppointment?appointmentId={selectedId}");
    }

    private void DeleteAppointment(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as AppointmentsViewModel)?.SelectedAppointment?.Id ?? "";
        if (string.IsNullOrEmpty(selectedId))
        {
            DisplayAlert("No Appointment Selected", "Please select an Appointment", "Ok");
            return;
        }
        (BindingContext as AppointmentsViewModel)?.Delete();
    }
}