using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nidhinanban.Models;

namespace Nidhinanban.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginApiController : ControllerBase
{

    [HttpPost("putlogin")]
    public IActionResult login([FromBody]LoginDataModel loginData)
    {

        if (loginData.UserName == "admin" && loginData.Password == "admin")
        {
            var Token = GenerateJWTToken(loginData.UserName);
            return Ok(Token);
        }
        else
        {
            return Unauthorized("Invalid User");
        }

    }

    private string GenerateJWTToken(string name)
    {
        var Claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,name)

        };
        var token = new JwtSecurityToken(
            issuer: "https://localhost:7065",
            audience: "https://localhost:7065",
            claims: Claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Ayds1jdAxlbJvSovJcXtckE9wXvNfh+oNDIdX+7ezOU=")), SecurityAlgorithms.HmacSha256)
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    [Authorize] 
    [HttpGet("GETUSER")]
    public string getusername()
    {
        string? user = User.Identity?.Name;
        return user!;
    }
}
