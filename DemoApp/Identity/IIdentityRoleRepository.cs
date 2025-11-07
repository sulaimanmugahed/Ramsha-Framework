using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Domain;

namespace DemoApp.Identity;

public interface IIdentityRoleRepository : IIdentityRoleRepository<Guid>
{

}

public interface IIdentityRoleRepository<TId> : IIdentityRoleRepository<RamshaIdentityRole<TId>, TId, RamshaIdentityUserRole<TId>, RamshaIdentityRoleClaim<TId>>
where TId : IEquatable<TId>
{

}

public interface IIdentityRoleRepository<TRole, TId, TUserRole, TRoleClaim> : IRepository<TRole, TId>
where TId : IEquatable<TId>
where TRole : RamshaIdentityRole<TId, TUserRole, TRoleClaim>
where TUserRole : RamshaIdentityUserRole<TId>
where TRoleClaim : RamshaIdentityRoleClaim<TId>
{

}
