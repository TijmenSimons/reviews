using AutoMapper;
using Template.Application.Common.Mappings;
using Template.Domain.Entities;

namespace Template.Application.Dtos;

public class VideoDto : IMapFrom<Video>
{
	public Guid Id { get; init; }

	public string Url { get; init; } = default!;

	public UserDto Creator { get; init; } = default!;

	public void Mapping(Profile profile)
	{
		profile.CreateMap<Video, VideoDto>()
			.ForMember(videoDto => videoDto.Url, member => member.MapFrom(video => $"/videos/{video.Id}{video.Extension}"));
	}
}
