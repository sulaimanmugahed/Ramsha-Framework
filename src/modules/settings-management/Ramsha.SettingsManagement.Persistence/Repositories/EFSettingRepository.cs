using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.EntityFrameworkCore;
using Ramsha.SettingsManagement.Domain;

namespace Ramsha.SettingsManagement.Persistence;

public class EFSettingRepository : EFCoreRepository<SettingsManagementDbContext, Setting, Guid>, ISettingRepository
{

}
