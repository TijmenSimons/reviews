using AutoMapper;
using Template.Application.Common.Mappings;
using Template.Domain.Entities;

namespace Template.Application.Dtos;

public class UserDto : IMapFrom<User>
{
	public string Id { get; set; } = default!;

	public string Username { get; set; } = default!;
}
