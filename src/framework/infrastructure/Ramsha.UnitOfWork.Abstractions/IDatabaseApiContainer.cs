namespace Ramsha.UnitOfWork.Abstractions;

public interface IDatabaseApiContainer
{
    IDatabaseApi? FindDatabaseApi(string key);

    void AddDatabaseApi(string key, IDatabaseApi api);

    IDatabaseApi GetOrAddDatabaseApi(string key, Func<IDatabaseApi> factory);

    IServiceProvider ServiceProvider { get; }
}
