using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ramsha.Common.Domain;

public interface ISoftDelete
{
    string? DeletedBy { get; set; }
    DateTime? DeletionDate { get; set; }
}

public interface IEntityModification
{
    string? UpdatedBy { get; set; }
    DateTime? LastUpdateDate { get; set; }
}

public interface IEntityCreation
{
    string? CreatedBy { get; set; }
    DateTime CreationDate { get; set; }

}

public interface IAudit : IEntityCreation, IEntityModification
{

}
