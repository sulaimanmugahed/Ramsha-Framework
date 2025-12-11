using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.Common.Domain;
using Ramsha.SettingsManagement.Domain;

namespace Ramsha.SettingsManagement.Domain;

public interface ISettingRepository : IRepository<Setting, Guid>
{

}
