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
        // check to make sure the time is a valid time, available for the physician
    }

    private void BackToAppointmentPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//AppointmentPage");
    }
}