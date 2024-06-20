using ChatAppNET8.Client;
using ChatAppNET8.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

//register signalr
builder.Services.AddScoped<SignalRService>();

await builder.Build().RunAsync();
