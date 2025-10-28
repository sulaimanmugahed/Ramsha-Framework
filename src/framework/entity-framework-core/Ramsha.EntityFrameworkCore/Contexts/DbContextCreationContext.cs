using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.EntityFrameworkCore;

public class DbContextCreationContext
{
    public static DbContextCreationContext Current => _current.Value!;
    private static readonly AsyncLocal<DbContextCreationContext> _current = new();

    public string ConnectionStringName { get; }

    public string ConnectionString { get; }

    public DbConnection? ExistingConnection { get; internal set; }

    public DbContextCreationContext(string connectionStringName, string connectionString)
    {
        ConnectionStringName = connectionStringName;
        ConnectionString = connectionString;
    }

    public static IDisposable Use(DbContextCreationContext context)
    {
        var previousValue = Current;
        _current.Value = context;
        return new OnDispose(() => _current.Value = previousValue);
    }
}
