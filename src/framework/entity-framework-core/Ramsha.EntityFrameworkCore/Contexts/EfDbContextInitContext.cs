using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ramsha.UnitOfWork.Abstractions;

namespace Ramsha.EntityFrameworkCore;

public class EfDbContextInitContext
{
    public IUnitOfWork UnitOfWork { get; }

    public EfDbContextInitContext(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}
