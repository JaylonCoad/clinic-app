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
        var newAppointment = new Appointment
        {
            PatientId = PatientId,
            PhysicianId = PhysicianId,
            AppointmentDate = DatePickerControl.Date,
            AppointmentTime = TimePickerControl.Time
        };
        AppointmentServiceProxy.Current.AddOrUpdate(newAppointment);
        Shell.Current.GoToAsync("//AppointmentPage");
    }

    private void BackToAppointmentPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AppointmentPage");
    }
    // protected override void OnAppearing()
    // {
    //     base.OnAppearing();

    //     BindingContext = new Appointment
    //     {
    //         PatientId = PatientId,
    //         PhysicianId = PhysicianId
    //     };
    // }
    // private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    // {
    //     var newAppointment = new Appointment
    //     {
    //         PatientId = PatientId,
    //         PhysicianId = PhysicianId
    //     };
    //     BindingContext = newAppointment;
    // }
}