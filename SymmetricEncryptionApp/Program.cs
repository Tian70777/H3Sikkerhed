using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SymmetricEncryptionApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Register CryptoServiceFactory and all crypto services
// builder.Services.AddSingleton<CryptoServiceFactory>();

// Register individual crypto services if necessary
//builder.Services.AddScoped<Aes128CryptoService>();
//builder.Services.AddScoped<Aes256CryptoService>();
//builder.Services.AddScoped<DesCryptoService>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Set abse address to API endpoint
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7145/") });

await builder.Build().RunAsync();
