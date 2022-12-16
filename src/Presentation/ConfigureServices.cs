using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Template.Application.Common.Interfaces;
using Template.Infrastructure.Persistence;
using Template.Presentation.Filters;
using Template.Presentation.Services;

namespace Template.Presentation;

public static class ConfigureServices
{
	public static IServiceCollection AddPresentationServices(this IServiceCollection services,
		IConfiguration configuration,
		IWebHostEnvironment environment)
	{ 
		if (environment.IsDevelopment())
			services.AddDatabaseDeveloperPageExceptionFilter();

		services.AddSingleton<ICurrentUserService, CurrentUserService>();

		services.AddHttpContextAccessor();

		services.AddHealthChecks()
			.AddDbContextCheck<ApplicationDbContext>();

		services.AddControllersWithViews(options =>
			options.Filters.Add<ApiExceptionFilterAttribute>())
				.AddFluentValidation(validationConfiguration => validationConfiguration.AutomaticValidationEnabled = false)
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
					//options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
				});

		services.AddRazorPages();

		// Customise default API behaviour
		services.Configure<ApiBehaviorOptions>(options =>
			options.SuppressModelStateInvalidFilter = true);

		// Configure swagger API Docs
		services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = environment.EnvironmentName, Description = "Swagger documentation for the Medocan auth api {\r\n    \"GrantType\": \"Password\",\r\n    \"Username\": \"administrator@localhost\",\r\n    \"Password\": \"Administrator1!\",\r\n    \"ClientId\": \"902e3558-ca65-48aa-baec-bddcb0047117\"\r\n}" });
			options.AddSecurityDefinition("Bearer",
				new OpenApiSecurityScheme
				{
					Description = "Example: 'Bearer {your JWT token}'",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
				});
			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
						Scheme = SecuritySchemeType.OAuth2.ToString(),
						Name = "Bearer",
						In = ParameterLocation.Header
					},
					Array.Empty<string>()
				}
			});
			options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
			options.CustomOperationIds(api => $"{api.ActionDescriptor.RouteValues["action"]}");
		});

		// Configure JWT authentication
		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = configuration["Authentication:Issuer"],
					ValidAudience = configuration["Authentication:Audience"],
					ValidateIssuerSigningKey = true,
					IssuerSigningKey =
						new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:SecurityKey"]))
				};
			});

		services.AddAuthorization(options =>
			options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

		return services;
	}

	public static WebApplication InitialiseAndSeedDatabase(this WebApplication app)
	{
		var task = Task.Run(async () =>
		{
			using var scope = app.Services.CreateScope();
			var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
			await initialiser.InitialiseAsync();
			await initialiser.SeedAsync();
		});
		task.Wait();
		return app;
	}
}
