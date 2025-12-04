using Ramsha.Authorization;
using Ramsha.Common.Domain;
using Ramsha.Core;
using Ramsha.Permissions.Shared;

namespace Ramsha.Permissions.Domain;

public class PermissionManager(
    IPermissionRepository assignmentRepository,
    IPermissionDefinitionManager definitionManager)
     : RamshaDomainManager, IPermissionManager
{
    public async Task<List<PermissionInfo>> GetAllAsync(string providerName, string providerKey)
    {
        return await UnitOfWork(async () =>
         {
             var permissions = await definitionManager.GetPermissionsAsync();
             List<PermissionInfo> lines = [];

             foreach (var permission in permissions)
             {
                 lines.Add(await GetPermission(permission.Name, providerName, providerKey));
             }

             return lines;
         });
    }

    public async Task<PermissionInfo> GetAsync(string permissionName, string providerName, string providerKey)
    {
        return await UnitOfWork(async () =>
         {
             var permission = await definitionManager.GetPermissionAsync(permissionName);
             if (permission == null)
             {
                 return new PermissionInfo(permissionName, false);
             }

             return await GetPermission(permissionName, providerName, providerKey);
         });
    }



    private async Task<PermissionInfo> GetPermission(string permissionName, string providerName, string providerKey)
    {

        var assignment = await assignmentRepository
     .FindAsync(x =>
      x.Name == permissionName &&
      x.ProviderName == providerName &&
      x.ProviderKey == providerKey);

        return new PermissionInfo(permissionName, assignment is null ? false : true);
    }


    public async Task<RamshaResult> AssignAsync(string name, string providerName, string providerKey)
    {
        return await UnitOfWork(async () =>
          {
              var permission = await definitionManager.GetPermissionAsync(name);

              if (permission is null)
              {
                  return RamshaError.Create(RamshaErrorsCodes.EntityNotFoundErrorCode, $"no permission exist with this name: {name}");
              }

              var exist = await assignmentRepository.FindAsync(x => x.Name == name && x.ProviderKey == providerKey && x.ProviderName == providerName);


              if (exist is null)
              {
                  var assignment = new Permission
                  {
                      Id = Guid.NewGuid(),
                      ProviderName = providerName,
                      Name = name,
                      ProviderKey = providerKey
                  };
                  await assignmentRepository.AddAsync(assignment);
              }

              return RamshaResult.Ok();
          });
    }

    public async Task<RamshaResult> RevokeAsync(string name, string providerName, string providerKey)
    {
        return await UnitOfWork(async () =>
          {
              var permission = await definitionManager.GetPermissionAsync(name);

              if (permission is null)
              {
                  return RamshaError.Create(RamshaErrorsCodes.EntityNotFoundErrorCode, $"no permission exist with this name: {name}");
              }

              var exist = await assignmentRepository.FindAsync(x => x.Name == name && x.ProviderKey == providerKey && x.ProviderName == providerName);

              if (exist is not null)
              {
                  await assignmentRepository.DeleteAsync(exist);
              }

              return RamshaResult.Ok();
          });
    }
}
