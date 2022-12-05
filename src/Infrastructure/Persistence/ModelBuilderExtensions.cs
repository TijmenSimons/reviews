using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace Template.Infrastructure.Persistence;

internal static class ModelBuilderExtensions
{
	/// <summary>
	/// Rename default identity tables.
	/// </summary>
	public static ModelBuilder RenameIdentityTables(this ModelBuilder builder)
	{
		builder.Entity<IdentityRole>().ToTable("Roles");
		builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
		builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
		builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
		builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
		builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

		return builder;
	}

	/// <summary>
	/// Save enum values as string in database so changes to the enum do not conflict with existing data in the database.
	/// </summary>
	public static ModelBuilder AddEnumStringConversions(this ModelBuilder builder)
	{
		// Add converters for Enums
		foreach (var property in builder.Model.GetEntityTypes().SelectMany(type => type.GetProperties().Where(property => (Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType).IsEnum)))
		{
			var type = typeof(EnumToStringConverter<>).MakeGenericType((Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType));
			var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;

			property.SetValueConverter(converter);
		}

		return builder;
	}
}
