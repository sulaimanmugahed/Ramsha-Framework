using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Domain;

namespace Ramsha.Identity.Domain;

public interface IIdentityUserRepository<TUser,TRole, TId, TUserRole, TRoleClaim, TUserClaim, TUserLogin, TUserToken> : IRepository<TUser, TId>
 where TId : IEquatable<TId>
where TUser : RamshaIdentityUser<TId, TUserClaim, TUserRole, TUserLogin, TUserToken>
 where TUserClaim : RamshaIdentityUserClaim<TId>
 where TUserRole : RamshaIdentityUserRole<TId>
 where TUserLogin : RamshaIdentityUserLogin<TId>
where TUserToken : RamshaIdentityUserToken<TId>

{

}
