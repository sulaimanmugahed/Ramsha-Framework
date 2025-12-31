

using Microsoft.AspNetCore.Mvc;
using Ramsha;
using Ramsha.AspNetCore.Mvc;
using Ramsha.Common.Application;
using Ramsha.Common.Domain;
using Ramsha.LocalMessaging;
using Ramsha.LocalMessaging.Abstractions;

namespace DemoApp.Controllers;

public class FakeManager : RamshaDomainManager
{
    public RamshaResult<FakeEntity> Get(int id)
    {
        var entity = FakeRepo.Find(id);
        if (entity is null)
        {
            return NotFound(message: "no fake entity was found");
        }

        return entity;
    }
}


public class FakeAppService(FakeManager manager) : RamshaService
{
    public RamshaResult<FakeEntity> Get(int id)
    {
        var result = manager.Get(id);
        if (!result.Succeeded)
        {
            return result.Error;
        }

        return result;
    }
}

public class TestResultController(FakeAppService appService) : RamshaApiController
{
    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var entity = FakeRepo.Find(id);
        if (entity is null)
        {
            throw new RamshaErrorException(RamshaResultStatus.Forbidden, "this is message");
        }

        return RamshaResult(RamshaResults.Success(entity));
    }

    [HttpGet("mediator/{id}")]
    public async Task<ActionResult> GetWithMediator(int id)
      => await Query(new FakeQuery { Id = id });

    [HttpGet("use-service/{id}")]
    public async Task<ActionResult> GetWithService(int id)
    {
        var result = appService.Get(id);

        if (!result.Succeeded)
        {
            return RamshaResult(result.Error);
        }

        return RamshaResult(result);
    }

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

