using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Common.Contracts;

public interface IContractsOptionsBaseBuilder
{
    IContractsOptionsBaseBuilder ReplaceDto<TBaseDto, TTargetDto>();
    IContractsOptionsBaseBuilder ReplaceDto(Type baseDtoType, Type targetDtoType);
}
