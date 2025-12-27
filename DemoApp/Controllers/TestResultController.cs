
using Microsoft.AspNetCore.Mvc;
using Ramsha;
using Ramsha.AspNetCore.Mvc;

namespace DemoApp.Controllers;

public class TestResultController : RamshaApiController
{

    [HttpGet("record")]
    public ActionResult Record()
    {
        return Ok(TestRecord.Value.Code);
    }

    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var entity = FakeRepo.Find(id);
        if (entity is null)
        {
            return RamshaResult(RamshaResults.Invalid());
        }

        return RamshaResult(RamshaResults.Success(entity));
    }

}


public record struct TestRecord(string Code = "default code")
{
    private static TestRecord _Self = new("default code");
    public static ref TestRecord Value => ref _Self;
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

