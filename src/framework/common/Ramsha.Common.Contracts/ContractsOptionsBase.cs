using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Common.Contracts;

public class ContractsOptionsBase : IContractsOptionsBaseBuilder
{
    public Dictionary<Type, Type?> ReplacedDtosTypes { get; } = [];

    public IContractsOptionsBaseBuilder ReplaceDto<TBaseDto, TTargetDto>()
    {
        ReplaceDto(typeof(TBaseDto), typeof(TTargetDto));
        return this;
    }

    public IContractsOptionsBaseBuilder ReplaceDto(Type baseDtoType, Type targetDtoType)
    {
        ReplacedDtosTypes[baseDtoType] = targetDtoType;
        return this;
    }

    public Type GetReplacedDtoOrBase<TBaseDto>()
    {
        return GetReplacedDtoOrBase(typeof(TBaseDto));
    }

    public Type GetReplacedDtoOrBase(Type baseDto)
    {
        return ReplacedDtosTypes.ContainsKey(baseDto) ? ReplacedDtosTypes[baseDto] ?? baseDto : baseDto;
    }
}
