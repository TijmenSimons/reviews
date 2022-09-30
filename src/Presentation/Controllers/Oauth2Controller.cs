using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Oauth2.Commands;
using Template.Application.Oauth2.Models;

namespace Template.Presentation.Controllers;

[Authorize]
public class Oauth2Controller : ApiControllerBase
{
	[HttpPost][AllowAnonymous]
	public async Task<ActionResult<TokenVm>> Create(CreateTokenCommand command)
	{
		return Ok(await Mediator.Send(command));
	}
}
