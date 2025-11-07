using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.EntityFrameworkCore;

namespace DemoApp.Identity;


public class IdentityRoleRepository<TRole, TId, TUserRole, TRoleClaim>
(IDbContextProvider<AppDbContext> dbContextProvider)
: EFCoreRepository<AppDbContext, TRole, TId>(dbContextProvider),
IIdentityRoleRepository<TRole, TId, TUserRole, TRoleClaim>
where TId : IEquatable<TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
where TUserRole : RamshaIdentityUserRole<TId>
where TRoleClaim : RamshaIdentityRoleClaim<TId>
{

}