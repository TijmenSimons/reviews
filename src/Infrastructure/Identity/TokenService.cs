using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Template.Application.Common.Exceptions;
using Template.Application.Common.Interfaces;
using Template.Domain.Entities;

namespace Template.Infrastructure.Identity;

public class TokenService : ITokenService
{
	private readonly UserManager<User> _userManager;
	private readonly IApplicationDbContext _context;
	private readonly IConfiguration _configuration;
	private readonly IDateTime _dateTime;

	public TokenService(UserManager<User> userManager, IApplicationDbContext context, IConfiguration configuration, IDateTime dateTime)
	{
		_userManager = userManager;
		_context = context;
		_configuration = configuration;
		_dateTime = dateTime;
	}

	public async Task<string> CreateAccessTokenAsync(string username)
	{
		var user = await _userManager.FindByNameAsync(username);

		if (user is null) throw new ForbiddenAccessException();
		
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new(ClaimTypes.Name, user.Id)
			}),
			Expires = _dateTime.Now.AddMilliseconds(_configuration.GetValue<int>("Authentication:TokenDuration")),
			IssuedAt = _dateTime.Now,
			Issuer = _configuration["Authentication:Issuer"],
			Audience = _configuration["Authentication:Audience"],
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecurityKey"])), SecurityAlgorithms.HmacSha256Signature)
		};

		foreach (var role in await _userManager.GetRolesAsync(user))
		{
			tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
		}

		return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
	}

	public async Task<string> CreateRefreshTokenAsync(string username)
	{
		var user = await _userManager.FindByNameAsync(username);

		if (user is null) throw new ForbiddenAccessException();

		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		var token = Convert.ToBase64String(randomNumber);

		await _context.RefreshTokens.AddAsync(new RefreshToken
		{
			IssuedAt = _dateTime.Now,
			Token = token,
			User = user
		});
		await _context.SaveChangesAsync();

		return token;
	}

	public string CreateInternalNetworkAccessToken()
	{
		var now = _dateTime.Now;
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(new Claim[]
			{
				new(ClaimTypes.Role, "InternalNetwork")
			}),
			Expires = now.AddMinutes(1),
			IssuedAt = now,
			Issuer = _configuration["Authentication:Issuer"],
			Audience = _configuration["Authentication:Audience"],
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecurityKey"])), SecurityAlgorithms.HmacSha256Signature)
		};

		return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
	}
}
