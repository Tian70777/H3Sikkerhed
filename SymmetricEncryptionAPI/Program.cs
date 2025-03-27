using SymmetricEncryptionAPI.Services.CryptoServices;
using SymmetricEncryptionApp.Services;

var builder = WebApplication.CreateBuilder(args);


// Define a CORS policy that allows your WASM frontend
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("https://localhost:7104")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// 1) Add services for Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Regester all crypto service using DI
builder.Services.AddSingleton<ICryptoService, Aes128CryptoService>();
builder.Services.AddSingleton<ICryptoService, Aes256CryptoService>();
builder.Services.AddSingleton<ICryptoService, DesCryptoService>();
builder.Services.AddSingleton<ICryptoService, RijndaelManaged128CryptoService>();
builder.Services.AddSingleton<ICryptoService, RijndaelManaged256CryptoService>();

// register the crypto service factory
builder.Services.AddSingleton<CryptoServiceFactory>();

var app = builder.Build();

// Use CORS before other middleware
app.UseCors(MyAllowSpecificOrigins);


// Configure the HTTP request pipeline.
// 2) Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        // c.RoutePrefix = string.Empty; // If you want Swagger at the root
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
