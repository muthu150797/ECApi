using EcAPI.Interfaces;
using EcAPI.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EcAPI.Entity;

namespace EcAPI.Services
{
	public class TokenService : ITokenService
	{
		private const double EXPIRY_DURATION_MINUTES = 30;
		private readonly IConfiguration _config;
		private readonly SymmetricSecurityKey _key;

		public TokenService(IConfiguration config)
		{
			_config = config;
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
		}
		public string BuildToken(string key, string issuer, dynamic userDetail)
		{
			//UserDTO userDTO = new UserDTO();
			//userDTO.UserName = userDetail.Name;
			//userDTO.Role = userDetail.Role;
			//userDTO.Email = userDetail.Email;

			var claims = new[] {
			new Claim(ClaimTypes.Name, userDetail.UserName),
			new Claim(ClaimTypes.Email,userDetail.Email),
            //new Claim(ClaimTypes.Role, userDetail.Role),
            new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
		    };

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			var tokenDescriptor = new JwtSecurityToken(issuer, issuer, claims,
				expires: DateTime.Now.AddDays(30), signingCredentials: credentials);
			return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
		}
		public bool IsTokenValid(string key, string issuer, string audience, string token)
		{
			var mySecret = Encoding.UTF8.GetBytes(key);
			var mySecurityKey = new SymmetricSecurityKey(mySecret);
			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token,
				new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = issuer,
					ValidAudience = issuer,
					IssuerSigningKey = mySecurityKey,
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			return true;
		}
		public string CreateToken(AppUser user)
		{
			var claims = new[] {
			new Claim(ClaimTypes.GivenName, user.DisplayName),
			new Claim(ClaimTypes.Email,user.Email),
            //new Claim(ClaimTypes.Role, userDetail.Role),
            new Claim(ClaimTypes.NameIdentifier,Guid.NewGuid().ToString())
			};

			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			var tokenDescriptor = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims,
				expires: DateTime.Now.AddDays(30), signingCredentials: credentials);
			return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
		}
	}

}
