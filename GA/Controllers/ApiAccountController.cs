using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GA.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public ApiAccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpPost("ApiLogin")]
        public async Task<IActionResult> ApiLogin(LoginVM loginVm)
        {

            var user = await _userManager.FindByNameAsync(loginVm.UserName);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVm.Password, false, false);
                if (result.Succeeded)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

                    };
                    var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1s5da4sd213&*^*sadjkhskjshksfd5s4fsd42^&%s7dasdfgsdfsdf4sd8f4sd8f465d%^^%$DS&"));

                    var token = new JwtSecurityToken(
                        issuer: "http://oec.com",
                        audience: "http://oec.com",
                        expires: DateTime.UtcNow.AddMinutes(15),
                        claims: claims,
                        signingCredentials: new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256)

                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }

            }


            return Unauthorized();
        }
    }
}
