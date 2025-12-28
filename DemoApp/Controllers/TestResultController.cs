
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ramsha;
using Ramsha.AspNetCore.Mvc;
using Ramsha.LocalMessaging;
using Ramsha.LocalMessaging.Abstractions;

namespace DemoApp.Controllers;

public class TestResultController : RamshaApiController
{
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var entity = FakeRepo.Find(id);
        if (entity is null)
        {
            throw new RamshaErrorException(ResultStatus.Forbidden, "this is message");
        }

        return RamshaResult(RamshaResults.Success(entity));
    }

    [HttpGet("mediator/{id}")]
    public async Task<ActionResult> GetWithMediator(int id)
      => await Query(new FakeQuery { Id = id });

}


public class FakeQuery : IRamshaQuery
{
    public int Id { get; set; }
}

public class FakeQueryHandler : RamshaQueryHandler<FakeQuery>
{
    public override async Task<IRamshaResult> HandleAsync(FakeQuery message, CancellationToken cancellationToken = default)
    {
        var entity = FakeRepo.Find(message.Id);
        if (entity is null)
        {
            return NotFound("no entity was found");
        }
        return Success(entity);
    }
}



public static class FakeRepo
{
    private static List<FakeEntity> rows = new()
    {
        new FakeEntity(1, "Entity 1"),
        new FakeEntity(2, "Entity 2"),
        new FakeEntity(3, "Entity 3")
    };
    public static FakeEntity? Find(int id)
    {
        return rows.FirstOrDefault(e => e.Id == id);
    }
}

public record FakeEntity(int Id, string Name);

