using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Domain;

namespace Ramsha.Identity.Domain;

public interface IIdentityUserRepository<TUser, TId> : IRepository<TUser, TId>
 where TId : IEquatable<TId>
 where TUser : RamshaIdentityUserBase<TId>
{

}
