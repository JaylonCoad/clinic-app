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
    public string? PatientId { get; set; }
    public string? PhysicianId { get; set; }
    public string? IsAdding { get; set; }
    public string? AppointmentToEditId { get; set; }
    public AddAppointment()
    {
        InitializeComponent();
    }

    private void AddClicked(object sender, EventArgs e)
    {
        if (IsAdding == "true")
        {
            if (BindingContext is Appointment newAppointment)
            {
                newAppointment.PhysicianId = PhysicianId;
                newAppointment.PatientId = PatientId;
            }
        }
        AppointmentServiceProxy.Current.AddOrUpdate(BindingContext as Appointment);
        Shell.Current.GoToAsync("//AppointmentPage");
    }

    private void BackToAppointmentPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AppointmentPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        Appointment appointmentToLoad = new();
        if (!string.IsNullOrEmpty(AppointmentToEditId)) // EDIT MODE
        {
            appointmentToLoad = new Appointment(AppointmentToEditId);
        }
        BindingContext = appointmentToLoad;
    }
}