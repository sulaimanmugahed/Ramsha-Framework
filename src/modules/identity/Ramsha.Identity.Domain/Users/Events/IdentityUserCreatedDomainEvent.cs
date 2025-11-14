using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Identity.Domain;

public record IdentityUserCreatedDomainEvent<TUser>(TUser User);

