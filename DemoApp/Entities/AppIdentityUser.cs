using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Identity.Domain;

namespace DemoApp.Entities;

public class AppIdentityUser : RamshaIdentityUser
{
    public AppIdentityUser()
    {

    }
    public AppIdentityUser(Guid id, string username, string profile) : base(id, username)
    {
        Profile = profile;
    }

    public string Profile { get; set; }
}
