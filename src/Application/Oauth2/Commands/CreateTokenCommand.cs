using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using OAuth;
using Presentation.Common.OAuth2;
using Template.Application.Common.Extensions;
using Template.Application.Common.Interfaces;
using Template.Application.Oauth2.Models;

namespace Template.Application.Oauth2.Commands;

public class CreateTokenCommand : IRequest<TokenVm>
{
	public GrantType? GrantType { get; set; }
	public List<ScopeType>? Scope { get; set; }
	public string? RedirectUri { get; set; }
	public string? ClientId { get; set; }
	public string? ClientSecret { get; set; }
	public string? Code { get; set; }
	public string? Username { get; set; }
	public string? Password { get; set; }
	public string? RefreshToken { get; set; }
}


public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
{
	public CreateTokenCommandValidator(IApplicationDbContext context, IIdentityService identityService)
	{
		RuleFor(command => command.GrantType)
			.NotEmpty()
			.Must(grantType => grantType is GrantType.Password or GrantType.RefreshToken)
			.WithMessage("UnsupportedGrantType");

		RuleFor(command => command.RedirectUri)
			.Url();

		//RuleFor(command => command.ClientId)
		//	.NotEmpty()
		//	.Must(command => context.Clients.FirstOrDefault(client => client.Id.Equals(command)) is not null)
		//	.WithMessage("UnknownClient");

		RuleFor(command => command.Username)
			.NotEmpty()
			.EmailAddress();

		When(command => command.GrantType is GrantType.Password && command.Username is not null, () =>
			RuleFor(command => command.Password)
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.MustAsync((command, _, _) => identityService.CheckPasswordAsync(command.Username!, command.Password!))
				.WithMessage("UsernameOrPasswordIncorrect"));

		When(command => command.GrantType is GrantType.RefreshToken && command.Username is not null, () =>
			RuleFor(command => command.RefreshToken)
				.Cascade(CascadeMode.Stop)
				.NotEmpty()
				.Length(44)
				.WithMessage("InvalidFormat")
				.MustAsync((command, _, _) => identityService.CheckRefreshTokenAsync(command.Username!, command.RefreshToken!))
				.WithMessage("UsernameOrRefreshTokenIncorrect"));
	}
}

public class CreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, TokenVm>
{
	private readonly ITokenService _tokenService;
	private readonly IConfiguration _configuration;
	private readonly IIdentityService _identityService;

	public CreateTokenCommandHandler(ITokenService tokenService, IIdentityService identityService, IConfiguration configuration)
	{
		_tokenService = tokenService;
		_identityService = identityService;
		_configuration = configuration;
	}

	public async Task<TokenVm> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
	{
		return new TokenVm
		{
			AccessToken = await _tokenService.CreateAccessTokenAsync(request.Username!),
			TokenType = TokenType.Bearer,
			ExpiresIn = int.Parse(_configuration["Authentication:TokenDuration"]),
			RefreshToken = await _tokenService.CreateRefreshTokenAsync(request.Username!),
			Roles = await _identityService.GetRolesAsync(request.Username!) 
		};
	}
}
