using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;
using Template.Infrastructure.Common;
using Template.Infrastructure.Persistence.Interceptors;

namespace Template.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
{
	private readonly IMediator _mediator;
	private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IMediator mediator,
		AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor) : base(options)
	{
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		_mediator = mediator;
		_auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
	}

	public DbSet<Bookmark> Bookmarks => Set<Bookmark>();

	public DbSet<Category> Categories => Set<Category>();

	public DbSet<Client> Clients => Set<Client>();

	public DbSet<Content> Contents => Set<Content>();

	public DbSet<DefaultCategory> DefaultCategories => Set<DefaultCategory>();

	public DbSet<Image> Images => Set<Image>();

	public DbSet<Partner> Partners => Set<Partner>();

	public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

	public DbSet<Video> Videos => Set<Video>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.AddEnumStringConversions();
		builder.RenameIdentityTables();

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
	}

	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		await _mediator.DispatchDomainEvents(this);

		return await base.SaveChangesAsync(cancellationToken);
	}
}
