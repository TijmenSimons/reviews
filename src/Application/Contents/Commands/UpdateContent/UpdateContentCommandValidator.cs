using FluentValidation;
using Template.Application.Common.Interfaces;

namespace Template.Application.Contents.Commands.UpdateContent;

public class UpdateContentCommandValidator : AbstractValidator<UpdateContentCommand>
{
	private readonly IApplicationDbContext _context;

	public UpdateContentCommandValidator(IApplicationDbContext context)
	{
		_context = context;

		RuleFor(c => c.Title)
			.NotEmpty().WithMessage("Title is required.");

		RuleFor(c => c.VideoId)
			.NotEmpty().WithMessage("Provide atleast 1 type of content");
	}
}
