using Microsoft.AspNetCore.Mvc;
using Template.Application.Images.Commands.UploadImage;
using Template.Application.Images.Commands.DeleteImages;
using Template.Application.Images.Queries.GetImage;
using Template.Application.Images.Queries.GetImages;
using Template.Application.Dtos;

namespace Template.Presentation.Controllers;

public class ImagesController : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IList<ImageDto>>> Get()
	{
		return Ok(await Mediator.Send(new GetImagesQuery()));
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> Get(Guid id)
	{
		return Ok(await Mediator.Send(new GetImageQuery { Id = id }));
	}

	[HttpPost]
	public async Task<IActionResult> OnPostUploadAsync([FromForm] UploadImageCommand command)
	{
		return Ok(await Mediator.Send(command));
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		await Mediator.Send(new DeleteImageCommand(id));

		return NoContent();
	}
}
