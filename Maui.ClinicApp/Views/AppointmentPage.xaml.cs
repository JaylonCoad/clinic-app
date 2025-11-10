using Maui.ClinicApp.ViewModels;

namespace Maui.ClinicApp.Views;

public partial class AppointmentPage : ContentPage
{
	public AppointmentPage()
	{
        InitializeComponent();
        BindingContext = new AppointmentsViewModel();
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
        var selectedId = (BindingContext as AppointmentsViewModel)?.SelectedAppointment?.Model?.Id;
        if (string.IsNullOrEmpty(selectedId))
        {
            DisplayAlert("No Appointment Selected", "Please select an Appointment", "Ok");
            return;
        }
        Shell.Current.GoToAsync($"//AddAppointment?appointmentId={selectedId}&isAdding=false");
    }

    private void DeleteAppointment(object sender, EventArgs e)
    {
        var selectedId = (BindingContext as AppointmentsViewModel)?.SelectedAppointment?.Model?.Id;
        if (string.IsNullOrEmpty(selectedId))
        {
            DisplayAlert("No Appointment Selected", "Please select an Appointment", "Ok");
            return;
        }
        (BindingContext as AppointmentsViewModel)?.Delete();
    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as AppointmentsViewModel)?.Refresh();
    }

    private void RefreshClicked(object sender, EventArgs e)
    {
        (BindingContext as AppointmentsViewModel)?.Refresh();
    }
}