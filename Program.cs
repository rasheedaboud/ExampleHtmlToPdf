using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PuppeteerSharp;
using System.Runtime.InteropServices;
using HtmltoPdf.Functions;

var browserFetcherOptions = new BrowserFetcherOptions();

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    browserFetcherOptions.Path = Path.GetTempPath();
}

var bf = new BrowserFetcher(browserFetcherOptions);
bf.DownloadAsync().GetAwaiter().GetResult();
var browser = bf.GetInstalledBrowsers().First();
var info = new AppInfo(bf.GetExecutablePath(browser.BuildId));

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<AppInfo>(info);
    })
    .Build();

host.Run();
