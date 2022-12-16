using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces;

public interface IApplicationDbContext
{
	DbSet<Bookmark> Bookmarks { get; }

	DbSet<Category> Categories { get; }

	DbSet<Client> Clients { get; }

	DbSet<Content> Contents { get; }

	DbSet<DefaultCategory> DefaultCategories { get; }

	DbSet<Image> Images { get; }

	DbSet<Partner> Partners { get; }

	DbSet<RefreshToken> RefreshTokens { get; }

	DbSet<Video> Videos { get; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
