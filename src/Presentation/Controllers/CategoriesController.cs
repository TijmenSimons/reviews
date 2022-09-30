using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.Categories.Commands.CreateCategory;
using Template.Application.Categories.Queries.GetCategories;
using Template.Domain.Entities;

namespace Template.Presentation.Controllers;
public class CategoriesController : ApiControllerBase
{
	[HttpGet]
	public async Task<ActionResult<CategoriesVm>> Get()
	{
		return await Mediator.Send(new GetCategoriesQuery());
	}

	[HttpPost]
	public async Task<ActionResult<Guid>> Create(CreateCategoryCommand command)
	{
		return await Mediator.Send(command);
	}
}
