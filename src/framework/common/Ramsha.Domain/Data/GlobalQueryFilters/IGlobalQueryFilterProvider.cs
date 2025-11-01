using System.Linq.Expressions;

namespace Ramsha.Domain;

public interface IGlobalQueryFilterProvider
{
     Type FilterType { get; }
}

public interface IGlobalQueryFilterProvider<TDbContext>:IGlobalQueryFilterProvider
     where TDbContext : IRamshaDbContext

{
    LambdaExpression? GetFilter(TDbContext dbContext);
}


