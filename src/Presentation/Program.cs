using Microsoft.Extensions.FileProviders;
using Template.Application;
using Template.Infrastructure;
using Template.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment);
builder.Services.AddPresentationServices(builder.Configuration, builder.Environment);

builder.WebHost.ConfigureKestrel(options => options.Limits.MaxRequestBodySize = builder.Configuration.GetValue<long>("MaxUploadSize"));

var app = builder.Build();

app.InitialiseAndSeedDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
	app.UseMigrationsEndPoint();
}
else
{
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));

app.UseHealthChecks("/health");
app.UseHealthChecks("/api/health");

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(builder.Configuration["ImagePath"]),
	RequestPath = "/images"
});

app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(builder.Configuration["VideoPath"]),
	RequestPath = "/videos"
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html");

app.Run();

// Make the implicit Program class public so test projects can access it
namespace Template.Presentation
{
	public partial class Program { }
}
