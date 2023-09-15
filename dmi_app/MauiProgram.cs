using CommunityToolkit.Maui.Maps;

namespace dmi_app
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .UseMauiCommunityToolkitMaps("jAIv8o1sdsJVEXcGA3ab~srprjbL_SmJjax98-TdtEA~AhJYs9zG0ewrjVgQafBznmJL68GUlruyWHh8N7sE047fmZf6jtXpLU3UDhSTmZmF")
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            return builder.Build();
        }
    }
}