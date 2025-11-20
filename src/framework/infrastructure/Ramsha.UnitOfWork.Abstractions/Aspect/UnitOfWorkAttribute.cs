// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Metalama.Extensions.DependencyInjection;
// using Metalama.Framework.Advising;
// using Metalama.Framework.Aspects;
// using Metalama.Framework.Code;


// namespace Ramsha.UnitOfWork.Abstractions;

// [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
// public class UnitOfWorkAttribute : Attribute, IAspect<IMethod>
// {
//     public bool? IsTransactional { get; set; }

//     public bool IsDisabled { get; set; }

//     [IntroduceDependency]
//     private readonly IUnitOfWorkManager _unitOfWorkManager;


//     public UnitOfWorkAttribute()
//     {

//     }

//     public UnitOfWorkAttribute(bool isTransactional)
//     {
//         IsTransactional = isTransactional;
//     }



//     public virtual void SetOptions(UnitOfWorkOptions options)
//     {
//         if (IsTransactional.HasValue)
//         {
//             options.IsTransactional = IsTransactional.Value;
//         }
//     }





//     void IAspect<IMethod>.BuildAspect(IAspectBuilder<IMethod> builder)
//     {

//         var unboundReturnSpecialType = (builder.Target.ReturnType as INamedType)?.Definition.SpecialType ?? SpecialType.None;

//         var overrideTemplates = new MethodTemplateSelector(
//              unboundReturnSpecialType == SpecialType.Task ? nameof(OverrideMethodAsyncTask) : nameof(OverrideMethodAsyncTaskWithReturnType),
//               unboundReturnSpecialType == SpecialType.Task ? nameof(OverrideMethodAsyncTask) : nameof(OverrideMethodAsyncTaskWithReturnType),
//             useAsyncTemplateForAnyAwaitable: true);

//         var genericValueType =
//      unboundReturnSpecialType is SpecialType.Task_T or SpecialType.ValueTask_T or SpecialType.IAsyncEnumerable_T or SpecialType.IAsyncEnumerator_T
//          ? ((INamedType)builder.Target.ReturnType).TypeArguments[0]
//          : null;

//         builder.Advice.Override(
//             builder.Target,
//             overrideTemplates,
//             new
//             {
//                 TValue = genericValueType
//             });

//     }

//     [Template]
//     public async Task OverrideMethodAsyncTask()
//     {
//         var options = new UnitOfWorkOptions();
//         SetOptions(options);
//         if (_unitOfWorkManager.TryBeginReserved(RamshaUnitOfWorkReservationNames.ActionUnitOfWorkReservationName, options))
//         {
//             await meta.ProceedAsync();

//             if (_unitOfWorkManager.Current != null)
//             {
//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//             }

//             return;
//         }

//         using (var uow = _unitOfWorkManager.Begin(options))
//         {
//             await meta.ProceedAsync();
//             await uow.CompleteAsync();
//         }

//     }

//     [Template]
//     public async Task<TValue?> OverrideMethodAsyncTaskWithReturnType<[CompileTime] TValue>()
//     {
//         var options = new UnitOfWorkOptions();
//         SetOptions(options);
//         if (_unitOfWorkManager.TryBeginReserved(RamshaUnitOfWorkReservationNames.ActionUnitOfWorkReservationName, options))
//         {
//             var result = await meta.ProceedAsync();

//             if (_unitOfWorkManager.Current != null)
//             {
//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//             }

//             return result;
//         }

//         using (var uow = _unitOfWorkManager.Begin(options))
//         {
//             var result = await meta.ProceedAsync();
//             await uow.CompleteAsync();
//             return result;
//         }
//     }



// }
