using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Ramsha.Domain;

namespace Ramsha.EntityFrameworkCore;

public interface IEFGlobalQueryFilterProvider<TDbContext> : IGlobalQueryFilterProvider<TDbContext>
     where TDbContext : RamshaEFDbContext<TDbContext>
{

}

// public class AbpEntityQueryProvider : EntityQueryProvider
// {
//     protected RamshaEfCoreCurrentDbContext RamshaEfCoreCurrentDbContext { get; }
//     protected ICurrentDbContext CurrentDbContext { get; }

//     public AbpEntityQueryProvider(
//         IQueryCompiler queryCompiler,
//         RamshaEfCoreCurrentDbContext abpEfCoreCurrentDbContext,
//         ICurrentDbContext currentDbContext)
//         : base(queryCompiler)
//     {
//         RamshaEfCoreCurrentDbContext = abpEfCoreCurrentDbContext;
//         CurrentDbContext = currentDbContext;
//     }

//     public override object Execute(Expression expression)
//     {
//         using (RamshaEfCoreCurrentDbContext.Use(CurrentDbContext.Context as IRamshaEFDbContext))
//         {
//             return base.Execute(expression);
//         }
//     }

//     public override TResult Execute<TResult>(Expression expression)
//     {
//         using (RamshaEfCoreCurrentDbContext.Use(CurrentDbContext.Context as IRamshaEFDbContext))
//         {
//             return base.Execute<TResult>(expression);
//         }
//     }

//     public override TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new CancellationToken())
//     {
//         using (RamshaEfCoreCurrentDbContext.Use(CurrentDbContext.Context as IRamshaEFDbContext))
//         {
//             return base.ExecuteAsync<TResult>(expression, cancellationToken);
//         }
//     }
// }

// public class RamshaEfCoreCurrentDbContext
// {
//     private readonly AsyncLocal<IRamshaEFDbContext?> _current = new AsyncLocal<IRamshaEFDbContext?>();

//     public IRamshaEFDbContext? Context => _current.Value;

//     public IDisposable Use(IRamshaEFDbContext? context)
//     {
//         var previousValue = Context;
//         _current.Value = context;
//         return new OnDispose(() =>
//         {
//             _current.Value = previousValue;
//         });
//     }
// }

// public class RamshaCompiledQueryCacheKeyGenerator<TDbContext> : ICompiledQueryCacheKeyGenerator
// where TDbContext : RamshaEFDbContext<TDbContext>
// {
//     private readonly ICompiledQueryCacheKeyGenerator _baseKeyGenerator;
//     private readonly ICurrentDbContext _currentContext;


//     public RamshaCompiledQueryCacheKeyGenerator(
//         ICompiledQueryCacheKeyGenerator baseKeyGenerator,
//         ICurrentDbContext currentDbContext)
//     {
//         _baseKeyGenerator = baseKeyGenerator;
//         _currentContext = currentDbContext;
//     }

//     public object GenerateCacheKey(Expression query, bool async)
//     {
//         var baseKey = _baseKeyGenerator.GenerateCacheKey(query, async);



//         return baseKey;
//     }
// }







