using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha;


public interface IRamshaSuccessResult : IRamshaResult;

public interface IRamshaSuccessResult<T> : IRamshaSuccessResult, IRamshaResult<T>
    where T : IRamshaSuccessResult<T>;




