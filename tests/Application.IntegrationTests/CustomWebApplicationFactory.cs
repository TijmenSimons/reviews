using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Template.Application.Common.Interfaces;
using Template.Infrastructure.Persistence;
using Template.Presentation;

namespace Template.Application.IntegrationTests;

using static Testing;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureAppConfiguration(configurationBuilder =>
		{
			var integrationConfig = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables()
				.Build();

			configurationBuilder.AddConfiguration(integrationConfig);
		});

		builder.ConfigureServices((context, services) =>
		{
			services
				.Remove<ICurrentUserService>()
				.AddTransient(provider => Mock.Of<ICurrentUserService>(s =>
					s.UserId == GetCurrentUserId()));

			services
				.Remove<DbContextOptions<ApplicationDbContext>>()
				.AddDbContext<ApplicationDbContext>((sp, dbOptions) =>
					dbOptions.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection"),
						options =>
						{
							options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
							options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
						}));
		});
	}
}
