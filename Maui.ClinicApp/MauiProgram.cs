using Maui.ClinicApp.ViewModels;
using Maui.ClinicApp.Views;
using Microsoft.Extensions.Logging;

namespace Maui.ClinicApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddSingleton<PhysiciansViewModel>(); // ViewModel we want to access
        builder.Services.AddSingleton<PatientPage>(); // page we want to access the ViewModel ^

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
