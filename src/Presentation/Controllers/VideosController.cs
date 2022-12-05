using Microsoft.AspNetCore.Mvc;
using Template.Application.Videos.Commands.UploadVideo;
using Template.Application.Videos.Commands.DeleteVideos;
using Template.Application.Videos.Queries.GetVideo;
using Template.Application.Videos.Queries.GetVideos;
using Template.Application.Dtos;

namespace Template.Presentation.Controllers;

public class VideosController : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult<IList<VideoDto>>> Get()
	{
		return Ok(await Mediator.Send(new GetVideosQuery()));
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> Get(Guid id)
	{
		return Ok(await Mediator.Send(new GetVideoQuery { Id = id }));
	}

	[HttpPost]
	public async Task<IActionResult> OnPostUploadAsync([FromForm] UploadVideoCommand command)
	{
		return Ok(await Mediator.Send(command));
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult> Delete(Guid id)
	{
		await Mediator.Send(new DeleteVideoCommand(id));

		return NoContent();
	}
}
