using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Domain;

public interface ISoftDelete
{
    Guid? DeletedBy { get; set; }
    DateTime? DeletionDate { get; set; }
}

public interface IEntityModification
{
    Guid? UpdatedBy { get; set; }
    DateTime? LastUpdateDate { get; set; }
}

public interface IEntityCreation
{
    Guid? CreatedBy { get; set; }
    DateTime CreationDate { get; set; }

}

public interface IAudit : IEntityCreation, IEntityModification
{

}
