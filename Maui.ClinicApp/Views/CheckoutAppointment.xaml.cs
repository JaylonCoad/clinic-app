using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.Views;
[QueryProperty(nameof(AppointmentToEditId), "appointmentToEditId")]
public partial class CheckoutAppointment : ContentPage
{
    public string AppointmentToEditId { get; set; }
	public CheckoutAppointment()
	{
		InitializeComponent();
	}

    private void CompleteAppointment(object sender, EventArgs e)
    {
        AppointmentServiceProxy.Current.CompleteAppointment(BindingContext as Appointment);
        Shell.Current.GoToAsync("//AppointmentPage");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AppointmentPage");
    }

    private async void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        await Task.Delay(50);
        Appointment appointmentToLoad = new(AppointmentToEditId);
        BindingContext = appointmentToLoad;
    }
}