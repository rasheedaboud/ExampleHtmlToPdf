namespace HtmltoPdf.Functions;


public class AppInfo
{
    public string BrowserExecutablePath { get; }
    private AppInfo() { }
    public AppInfo(string browserExecutablePath)
    {
        BrowserExecutablePath = browserExecutablePath;
    }
}