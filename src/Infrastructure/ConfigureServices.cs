using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;
using Template.Infrastructure.Files;
using Template.Infrastructure.Identity;
using Template.Infrastructure.Persistence;
using Template.Infrastructure.Persistence.Interceptors;
using Template.Infrastructure.Services;

namespace Template.Infrastructure;

public static class ConfigureServices
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
		IConfiguration configuration,
		IWebHostEnvironment environment)
	{
		services.AddScoped<AuditableEntitySaveChangesInterceptor>();

		if (configuration.GetValue<bool>("UseInMemoryDatabase") || environment.IsEnvironment("Testing"))
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseInMemoryDatabase($"{environment.ApplicationName}-{environment.EnvironmentName}" ));
		}
		else
		{
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
					builder =>
					{
						builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
						builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
					})
			);
		}

		services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

		services.AddScoped<ApplicationDbContextInitialiser>();

		services.AddIdentity<User, IdentityRole>()
			.AddUserManager<UserManager<User>>()
			.AddEntityFrameworkStores<ApplicationDbContext>()
			.AddDefaultTokenProviders();

		services.AddTransient<IDateTime, DateTimeService>();
		services.AddTransient<IIdentityService, IdentityService>();
		services.AddTransient<ITokenService, TokenService>();
		services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
		
		return services;
	}
}
