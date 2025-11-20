using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Ramsha.Domain;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.EntityFrameworkCore;

public class UoWDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
    where TDbContext : IEFDbContext
{
    [Injectable]
    public ILogger<UoWDbContextProvider<TDbContext>> Logger { get; set; }
    protected readonly IUnitOfWorkManager UnitOfWorkManager;
    protected readonly IConnectionStringResolver ConnectionStringResolver;
    protected readonly IEfDbContextTypeProvider DbContextTypeProvider;

    public UoWDbContextProvider(IUnitOfWorkManager uowManager, IConnectionStringResolver connectionStringResolver, IEfDbContextTypeProvider dbContextTypeProvider)
    {
        Logger = NullLogger<UoWDbContextProvider<TDbContext>>.Instance;
        UnitOfWorkManager = uowManager;
        ConnectionStringResolver = connectionStringResolver;
        DbContextTypeProvider = dbContextTypeProvider;
    }
    public async Task<TDbContext> GetDbContextAsync()
    {
        var unitOfWork = UnitOfWorkManager.Current;
        if (unitOfWork == null)
        {
            throw new Exception("A DbContext can only be created inside a unit of work!");
        }

        var dbContextType = DbContextTypeProvider.GetDbContextType(typeof(TDbContext));
        var connectionStringName = ConnectionStringAttribute.GetNameOrNull(dbContextType);
        var connectionString = await ResolveConnectionStringAsync(connectionStringName);
        if (connectionString is null)
        {
            throw new Exception($"no connection string for name: {connectionStringName}");
        }

        var apiKey = $"{dbContextType.FullName}_{connectionString}";

        var databaseApi = unitOfWork.FindDatabaseApi(apiKey);

        if (databaseApi is null)
        {
            databaseApi = new EfCoreDatabaseApi(
                await CreateDbContextAsync(unitOfWork, connectionStringName, connectionString)
            );

            unitOfWork.AddDatabaseApi(apiKey, databaseApi);
        }

        return (TDbContext)((EfCoreDatabaseApi)databaseApi).DbContext;
    }

    protected virtual async Task<string?> ResolveConnectionStringAsync(string? connectionStringName = null)
    {
        return await ConnectionStringResolver.ResolveAsync(connectionStringName);

    }

    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork, string connectionStringName, string connectionString)
    {
        var creationContext = new DbContextCreationContext(connectionStringName, connectionString);
        using (DbContextCreationContext.Use(creationContext))
        {
            var dbContext = await CreateDbContextAsync(unitOfWork);

            if (dbContext is IRamshaEFDbContext ramshaEfDbContext)
            {
                ramshaEfDbContext.Init(
                    new EfDbContextInitContext(
                        unitOfWork
                    )
                );
            }
            return dbContext;
        }
    }

    protected virtual async Task<TDbContext> CreateDbContextAsync(IUnitOfWork unitOfWork)
    {
        return unitOfWork.Options.IsTransactional
            ? await CreateDbContextWithTransactionAsync(unitOfWork)
            : unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();
    }

    protected virtual async Task<TDbContext> CreateDbContextWithTransactionAsync(IUnitOfWork unitOfWork)
    {
        var transactionApiKey = $"EntityFrameworkCore_{DbContextCreationContext.Current.ConnectionString}";
        var activeTransaction = unitOfWork.FindTransactionApi(transactionApiKey) as EfCoreTransactionApi;

        if (activeTransaction == null)
        {
            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            try
            {
                var isolationLevel = unitOfWork.Options.IsolationLevel;
                var dbTransaction = isolationLevel.HasValue ? await dbContext.Database.BeginTransactionAsync(isolationLevel.Value)
                                    : await dbContext.Database.BeginTransactionAsync();

                unitOfWork.AddTransactionApi(
                    transactionApiKey,
                    new EfCoreTransactionApi(
                        dbTransaction
                    )
                );
            }
            catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
            {
                Logger.LogWarning("Current database does not support transactions");
                return dbContext;
            }

            return dbContext;
        }
        else
        {
            DbContextCreationContext.Current.ExistingConnection = activeTransaction.DbContextTransaction.GetDbTransaction().Connection;

            var dbContext = unitOfWork.ServiceProvider.GetRequiredService<TDbContext>();

            if (dbContext.HasRelationalTransactionManager())
            {
                if (dbContext.Database.GetDbConnection() == DbContextCreationContext.Current.ExistingConnection)
                {
                    await dbContext.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    try
                    {
                        await dbContext.Database.BeginTransactionAsync();
                    }
                    catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                    {
                        Logger.LogWarning("Current database does not support transactions");

                        return dbContext;
                    }
                }
            }
            else
            {
                try
                {
                    await dbContext.Database.BeginTransactionAsync();
                }
                catch (Exception e) when (e is InvalidOperationException || e is NotSupportedException)
                {
                    Logger.LogWarning("Current database does not support transactions");
                    return dbContext;
                }
            }

            activeTransaction.AttendedDbContexts.Add(dbContext);

            return dbContext;
        }
    }

}