using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Oauth2.Commands;
using Template.Application.Oauth2.Models;

namespace Template.Presentation.Controllers;

[Authorize]
public class Oauth2Controller : ApiControllerBase
{
	[AllowAnonymous]
	[HttpPost]
	public async Task<ActionResult<TokenVm>> Create(CreateTokenCommand command, CancellationToken cancellationToken)
	{
		return Ok(await Mediator.Send(command, cancellationToken));
	}
}
