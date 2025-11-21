using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Identity.Domain;
using Ramsha.Security.Claims;
using Ramsha.Security.Users;
using Ramsha.UnitOfWork;

namespace DemoApp.Controllers;

public class AuthController(
    SignInManager<AppIdentityUser> signInManager,
    ICurrentUser currentUser)
: RamshaApiController
{


    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (UnitOfWorkManager.TryBeginReserved(Ramsha.UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new Ramsha.UnitOfWork.Abstractions.UnitOfWorkOptions { IsTransactional = false }))
        {
            var result = await signInManager.PasswordSignInAsync(username, password, false, false);
            if (!result.Succeeded)
            {
                return base.BadRequest(result);
            }
        }
        return Ok();
    }

    [HttpPost(nameof(Logout))]
    public async Task<IActionResult> Logout()
    {
        await UnitOfWork(signInManager.SignOutAsync);
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
