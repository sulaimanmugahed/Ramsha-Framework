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

public class AccountController(
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager,
    ICurrentUser currentUser)
: RamshaApiController
{
    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register(string username, string email, string password, string profile)
    {
        var user = new AppIdentityUser(Guid.NewGuid(), username, profile) { Email = email };

        if (UnitOfWorkManager.TryBeginReserved(Ramsha.UnitOfWork.UnitOfWork.UnitOfWorkReservationName, new Ramsha.UnitOfWork.Abstractions.UnitOfWorkOptions { IsTransactional = true }))
        {
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return base.BadRequest(result.Errors);
            }
        }

        return Ok();
    }

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
