using Library.ClinicApp.Models;
using Library.ClinicApp.Services;

namespace Maui.ClinicApp.Views;
[QueryProperty(nameof(PhysicianId), "physicianId")]
public partial class AddPhysician : ContentPage
{
    public string PhysicianId { get; set; }
	public AddPhysician()
	{
		InitializeComponent();
	}

    private void AddClicked(object sender, EventArgs e)
    {
        PhysicianServiceProxy.Current.AddOrUpdate(BindingContext as Physician);
        Shell.Current.GoToAsync("//PhysicianPage");
    }

    private void BackToPhysicianPage(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//PhysicianPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (string.IsNullOrEmpty(PhysicianId))
        {
            BindingContext = new Physician();
        }
        else
        {
            BindingContext = new Physician(PhysicianId);
        }
    }
}