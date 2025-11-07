using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Ramsha.Domain;
using Ramsha.EntityFrameworkCore;

namespace DemoApp.Identity;

public interface IIdentityUserRepository : IIdentityUserRepository<Guid>
{

}

public interface IIdentityUserRepository<TId> : IIdentityUserRepository<RamshaIdentityUser<TId>, TId, RamshaIdentityUserRole<TId>, RamshaIdentityRoleClaim<TId>, RamshaIdentityUserClaim<TId>, RamshaIdentityUserLogin<TId>, RamshaIdentityUserToken<TId>, RamshaIdentityRole<TId>>
where TId : IEquatable<TId>
{

}

public interface IIdentityUserRepository<TUser, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken, TRole> : IRepository<TUser, TId>
 where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
 where TUserClaim : RamshaIdentityUserClaim<TId>
 where TUserRole : RamshaIdentityUserRole<TId>
 where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>

{

}

