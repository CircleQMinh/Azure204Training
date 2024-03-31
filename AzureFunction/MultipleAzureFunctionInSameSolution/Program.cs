using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MultipleAzureFunctionInSameSolution.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services => {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.AddScoped<IMyService, MyService>();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
