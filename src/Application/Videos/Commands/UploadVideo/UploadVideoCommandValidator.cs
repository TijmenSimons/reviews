using FluentValidation;
using Template.Application.Common.Constants;
using Template.Application.Common.Validators;

namespace Template.Application.Videos.Commands.UploadVideo;

public class UploadVideoCommandValidator : AbstractValidator<UploadVideoCommand>
{

	public UploadVideoCommandValidator()
	{
		RuleFor(command => command.Video)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.MaxFileSize(FileConstants.MaxVideoSize)
			.IsSupportedVideo();

		RuleFor(command => command.CreatorId)
			.NotEmpty();
	}
}
