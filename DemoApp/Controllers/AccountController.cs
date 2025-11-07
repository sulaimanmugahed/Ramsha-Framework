using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Security.Claims;
using Ramsha.Security.Users;

namespace DemoApp.Controllers;

public class AccountController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    ICurrentUser currentUser)
: RamshaApiController
{
    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(string username, string email, string password)
    {
        var user = new IdentityUser { UserName = username, Email = email };
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        return Ok();
    }

    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(string username, string password)
    {
        var result = await signInManager.PasswordSignInAsync(username, password, false, false);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }
        return Ok();
    }


    [HttpGet(nameof(Me))]
    public async Task<IActionResult> Me()
    {
        return Ok(new
        {
            currentUser.Id,
            currentUser.Username,
            currentUser.Email,
            roles = currentUser.GetRoles(),
            IsAuthenticated = currentUser.IsAuthenticated(),
            demo = currentUser.FindClaimValue("demo")
        });
    }


}
