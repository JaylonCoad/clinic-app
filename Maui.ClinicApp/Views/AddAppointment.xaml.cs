using Maui.ClinicApp.ViewModels;
using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.Views;
[QueryProperty(nameof(PatientId), "patientId")]
[QueryProperty(nameof(PhysicianId), "physicianId")]
[QueryProperty(nameof(IsAdding), "isAdding")]
[QueryProperty(nameof(AppointmentToEditId), "appointmentId")]
public partial class AddAppointment : ContentPage
{
    public string PatientId { get; set; }
    public string PhysicianId { get; set; }
    public string? IsAdding { get; set; }
    public string AppointmentToEditId { get; set; }
	public AddAppointment()
	{
		InitializeComponent();
	}

    private void AddClicked(object sender, EventArgs e)
    {
        if (IsAdding == "true")
        {
            var newAppointment = new Appointment
            {
                PatientId = PatientId,
                PhysicianId = PhysicianId,
                AppointmentDate = DatePickerControl.Date,
                AppointmentTime = TimePickerControl.Time
            };
            AppointmentServiceProxy.Current.AddOrUpdate(newAppointment);
        }
        else
        {
            var existingAppointment = new Appointment(AppointmentToEditId);
            AppointmentServiceProxy.Current.AddOrUpdate(existingAppointment);
        }
        Shell.Current.GoToAsync("//AppointmentPage");
    }

    private void BackToAppointmentPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AppointmentPage");
    }

    // private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    // {
    //     Appointment appointmentToLoad;
    //     if (string.IsNullOrEmpty(AppointmentId)) // ADD MODE
    //     {
    //         appointmentToLoad = new Appointment();
    //     }
    //     else // EDIT MODE
    //     {
    //         appointmentToLoad = new Appointment(AppointmentId);
    //     }
    //     appointmentToLoad.PatientId = PatientId;
    //     appointmentToLoad.PhysicianId = PhysicianId;

    //     // 4. Set the BindingContext to the object, ensuring it's never null
    //     BindingContext = appointmentToLoad;
    // }
    
}