# HTML to PDF

Im sure everyone at some point has found the need to render html to pdf. I though it could be useful to show a simple example of how to this using azure functions. Setup for this function app is 100% stolen from Anthony Chu's [blog post](https://anthonychu.ca/post/azure-functions-puppeteer-pdf-razor-template/). I've used this with great success both internally to my company and for external tools as well. The cold start is not an issue for us, so we run it on a consumption plan behind API management gateway. If you want something that boots up faster you can always publish it to a premium plan.

I will try an keep this example up to date with .Net LTS release cycles as a reference to myself and anyone else who finds this useful.

Its also very trivial to write the same login in F# if that your jam! 😉

## Running the App

1. Install latest [Azure Function Tools](https://learn.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=linux%2Cisolated-process%2Cnode-v4%2Cpython-v2%2Chttp-trigger%2Ccontainer-apps&pivots=programming-language-csharp).
2. Install [.Net 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
3. Clone the repo `gh repo clone rasheedaboud/ExampleHtmlToPdf`.
4. `cd` into the project folder and run `dotnet build` and `func host start`.

## Caveats

1. This only work on linux. When you deploy you app make sure you do not deploy wot windows machine.
 