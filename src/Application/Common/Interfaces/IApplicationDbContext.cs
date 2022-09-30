using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Category> Categories { get; }

	DbSet<Client> Clients { get; }

	DbSet<Partner> Partners { get; }

	DbSet<RefreshToken> RefreshTokens { get; }

	DbSet<TodoList> TodoLists { get; }

	DbSet<TodoItem> TodoItems { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
