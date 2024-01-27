using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;


namespace HtmltoPdf.Functions
{
    public class GeneratePdf
    {

        public class PdfRequest
        {
            public string? content { get; set; }
        }
        private readonly ILogger<GeneratePdf> _logger;
        private readonly AppInfo _appInfo;

        public GeneratePdf(ILogger<GeneratePdf> logger, AppInfo appInfo)
        {
            _logger = logger;
            _appInfo = appInfo;
        }

        [Function(nameof(GeneratePdf))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req, [FromBody] PdfRequest request)
        {
            try
            {
                if (request is null || string.IsNullOrEmpty(request.content)) throw new ArgumentException("pdf request cannot be null or contain empty content.");

                var browser = await Puppeteer.LaunchAsync(new LaunchOptions() { Headless = true, ExecutablePath = _appInfo.BrowserExecutablePath });

                await using var page = await browser.NewPageAsync();
                await page.SetContentAsync(request.content);

                // Example of using options to add page numbers in a footer
                // Should be valid HTML markup with following CSS classes used to inject printing values into them:
                // - date formatted print date
                // - title document title
                // - url document location
                // - pageNumber current page number
                // - totalPages total pages in the document
                var footerTemplate =
                    @"<div style=""display: flex; justify-content: space-between; font-size: 8px; margin: 0px 45px 0px 45px; width: 100%"">                                        
                        <div class=""date""></div>
                        <div>Page 
                            <span class=""pageNumber""></span>
                            <span> of </span><span class=""totalPages""></span>
                        </div>
                    </div>";

                // Example of how to setup page options, modify to suit your needs, or better yet pass this into the function as a parameter!
                var options = new PdfOptions()
                {
                    Landscape = false,
                    DisplayHeaderFooter = true,
                    Format = PaperFormat.A4,
                    Scale = 1.0m,
                    MarginOptions = new() { Bottom = "10px", Top = "10px", Left = "10px", Right = "10px" },
                    FooterTemplate = footerTemplate,

                };
                var pdfStream = await page.PdfStreamAsync(options);

                return new FileStreamResult(pdfStream, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
