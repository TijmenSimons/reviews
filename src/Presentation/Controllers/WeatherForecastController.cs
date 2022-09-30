using Microsoft.AspNetCore.Mvc;
using Template.Application.WeatherForecasts.Queries.GetWeatherForecasts;

namespace Template.Presentation.Controllers;

public class WeatherForecastController : ApiControllerBase
{
	[HttpGet]
	public async Task<IEnumerable<WeatherForecast>> Get()
	{
		return await Mediator.Send(new GetWeatherForecastsQuery());
	}
}
