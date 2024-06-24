using ChatAppNET8.Client;
using ChatAppNET8.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

//register signalr
builder.Services.AddScoped<SignalRService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });


await builder.Build().RunAsync();
