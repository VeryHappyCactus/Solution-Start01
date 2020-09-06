using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebUi.Models;

namespace WebUi.Controllers
{

    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public enum ResponseStatusTypes
    {
        Success,
        Error
    }

    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly ILogger<LoginModel> _logger;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthenticationController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IAuthenticationSchemeProvider schemeProvider)
        {
            _signInManager = signInManager;
            //_logger = logger;
            _schemeProvider = schemeProvider;
            _userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Name) || string.IsNullOrEmpty(user.Password))
            {
                return Ok(new { errorMessage = "Invalid login attempt.", status = ResponseStatusTypes.Error });
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.Name, user.Password, user.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                ApplicationUser currentUser = await _userManager.FindByEmailAsync(user.Name);
                // Get the roles for the user
                IList<string> roles = await _userManager.GetRolesAsync(currentUser);

                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                };

                foreach(string role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                // создаем объект ClaimsIdentity
                ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                return Ok(new { errorMessage = (object)null, status = ResponseStatusTypes.Success});
            }
            else
            {
                return Ok(new { errorMessage = "Invalid login attempt.", status = ResponseStatusTypes.Error });
            }
        }
    }
}
