using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OAuthWithGoogle.DTOs;
using OAuthWithGoogle.Models;
using OAuthWithGoogle.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWithGoogle.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private readonly string _jwtKey;

        public AuthController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
            _jwtKey = config["AppSetting:JwtKey"];
        }

        [AllowAnonymous]
        [HttpPost("google")]
        public async Task<IActionResult> GoogleAuthenticate([FromBody] AuthenticateRequest request)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(request.TokenId, new GoogleJsonWebSignature.ValidationSettings()).Result;
                var user = await _authService.Authenticate(payload);

                var token = GenerateToken(user);
                return Ok(token);
            }
            catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim("UserId", Convert.ToString(user.Id)),
                        new Claim("Name", user.Name),
                        new Claim("Email", user.Email)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
