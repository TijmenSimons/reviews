using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Application.Dtos;
using Template.Domain.Entities;

namespace Template.Application.DefaultCategories.Commands.UpdateDefaultCategory;

public record UpdateDefaultCategoryCommand : IRequest<DefaultCategoryDto>
{
	public Guid Id { get; set; } = default!; 

	public string? Name { get; set; } = default!;

	public string? Description { get; set; } = default!;

	public Guid? ImageId { get; set; }

	public Guid? PartnerId { get; set; }
}

public class UpdateDefaultCategoryCommandHandler : IRequestHandler<UpdateDefaultCategoryCommand, DefaultCategoryDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public UpdateDefaultCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context; 
		_mapper = mapper;
	}

	public async Task<DefaultCategoryDto> Handle(UpdateDefaultCategoryCommand request, CancellationToken cancellationToken)
	{
		var defaultCategory = await _context.DefaultCategories
			.FirstOrDefaultAsync(category => category.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.Id);

		defaultCategory.Name = request.Name!;
		defaultCategory.Description = request.Description!;

		defaultCategory.Image = request.ImageId is null
			? null
			: await _context.Images
				.FirstOrDefaultAsync(image => image.Id.Equals(request.ImageId), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.ImageId);

		defaultCategory.Partner = request.PartnerId is null
			? null
			: await _context.Partners
				.FirstOrDefaultAsync(partner => partner.Id.Equals(request.PartnerId), cancellationToken) ?? throw new NotFoundException(nameof(DefaultCategory), request.PartnerId);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<DefaultCategoryDto>(defaultCategory);
	}
}
