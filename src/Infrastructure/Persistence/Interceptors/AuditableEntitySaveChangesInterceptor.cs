using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Template.Application.Common.Interfaces;
using Template.Domain.Common;

namespace Template.Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IDateTime _dateTime;

	public AuditableEntitySaveChangesInterceptor(
		ICurrentUserService currentUserService,
		IDateTime dateTime)
	{
		_currentUserService = currentUserService;
		_dateTime = dateTime;
	}

	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		UpdateEntities(eventData.Context);

		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
	{
		UpdateEntities(eventData.Context);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	public void UpdateEntities(DbContext? context)
	{
		if (context == null) return;

		foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CreatedBy = _currentUserService.UserId;
				entry.Entity.Created = _dateTime.Now;
			}

			if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
			{
				entry.Entity.LastModifiedBy = _currentUserService.UserId;
				entry.Entity.LastModified = _dateTime.Now;
			}
		}
	}
}

public static class Extensions
{
	public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
		entry.References.Any(referenceEntry =>
			referenceEntry.TargetEntry != null &&
			referenceEntry.TargetEntry.Metadata.IsOwned() &&
			referenceEntry.TargetEntry.State is EntityState.Added or EntityState.Modified);
}
