using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Domain;

namespace Ramsha.Identity.Domain;

public interface IIdentityRoleRepository<TRole, TId> : IRepository<TRole, TId>
 where TId : IEquatable<TId>
 where TRole : RamshaIdentityRoleBase<TId>
{

}

