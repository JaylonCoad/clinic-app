using Maui.ClinicApp.ViewModels;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.Views;
[QueryProperty(nameof(PatientId), "patientId")]
[QueryProperty(nameof(PhysicianId), "physicianId")]
public partial class AddAppointment : ContentPage
{
    public string PatientId { get; set; }
    public string PhysicianId { get; set; }
	public AddAppointment()
	{
		InitializeComponent();
	}

    private void AddClicked(object sender, EventArgs e)
    {
        AppointmentServiceProxy.Current.AddOrUpdate(BindingContext as Appointment);
        Shell.Current.GoToAsync("//AppointmentPage");
    }

    private void BackToAppointmentPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AppointmentPage");
    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        var newAppointment = new Appointment();
        newAppointment.PatientId = this.PatientId;
        newAppointment.PhysicianId = this.PhysicianId;
        BindingContext = newAppointment;
    }
}