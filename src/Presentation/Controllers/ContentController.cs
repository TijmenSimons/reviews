using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Contents.Commands.CreateContent;
using Template.Application.Contents.Commands.DeleteContent;
using Template.Application.Contents.Commands.UpdateContent;
using Template.Application.Contents.Queries.GetContent;
using Template.Application.Contents.Queries.GetContents;
using Template.Application.Dtos;

namespace Template.Presentation.Controllers;

[AllowAnonymous]
public class ContentController : ApiControllerBase
{
	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<IList<ContentDto>>> Get(
		[FromQuery] bool? isActive,
		[FromQuery] List<Guid>? categories
		)
	{
		return Ok(await Mediator.Send(new GetContentsQuery
		{
			IsActive = isActive,
			Categories = categories
		}));
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> Get(Guid id)
	{
		return Ok(await Mediator.Send(new GetContentQuery { Id = id }));
	}

	[HttpPost]
	public async Task<ActionResult<ContentDto>> Create(CreateContentCommand command)
	{
		return await Mediator.Send(command);
	}

	[HttpPatch]
	public async Task<ActionResult<ContentDto>> Update(UpdateContentCommand command)
	{
		return await Mediator.Send(command);
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		await Mediator.Send(new DeleteContentCommand(id));

		return NoContent();
	}
}
