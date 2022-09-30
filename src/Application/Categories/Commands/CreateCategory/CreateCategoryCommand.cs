using MediatR;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<Guid>
{
	public string? Name { get; set; } = default!;

	public string? Description { get; set; } = default!;

	public string? ImagePath { get; set; } = default!;

	public Partner? Partner { get; set; }

	public bool IsActive { get; set; } = true;

	public bool IsMainCategory { get; set; } = true;
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
	private readonly IApplicationDbContext _context;

	public CreateCategoryCommandHandler(IApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
	{
		var Category = new Category
		{
			Name = request.Name,
			Description = request.Description,
			ImagePath = request.ImagePath,
			Partner = request.Partner,
		};

		_context.Categories.Add(Category);

		await _context.SaveChangesAsync(cancellationToken);

		return Category.Id;
	}
}
