using Microsoft.Extensions.DependencyInjection;

namespace LoggerService;

public static class LoggerDiService
{
    public static void ConfigureLoggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, LoggerManager>();
    }
}
