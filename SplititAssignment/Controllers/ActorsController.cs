using Microsoft.AspNetCore.Mvc;
using SplititAssignment.Interfaces;
using SplititAssignment.Models;

[ApiController]
[Route("api/v1/actors")]
[ServiceFilter(typeof(CustomResponseFilterAttribute))]

public class ActorsController : ControllerBase
{
    private readonly IActorService _actorService;
    public ActorsController(IActorService actorService)
    {
        _actorService = actorService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ActorSummary>))]
    public IActionResult GetAllActors([FromQuery] int? rankStart, [FromQuery] int? rankEnd, [FromQuery] string provider = "IMDb", [FromHeader] int skip = 0, [FromHeader] int take = 10)
    {
        var actors = _actorService.GetActorsSummary(provider, rankStart, rankEnd, skip, take);

        return Ok(actors);
    }

    [HttpGet("{actorId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorResponse))]
    public IActionResult GetActorDetails(string actorId)
    {
        var actor = _actorService.GetActorDetails(actorId);

        return Ok(actor);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActorResponse))]
    public IActionResult AddActor([FromBody] ActorRequest addRequest)
    {
        var result = _actorService.AddActor(addRequest);
        return Ok(result);
    }

    [HttpPut("{actorId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorResponse))]

    public IActionResult UpdateActor(string actorId, [FromBody] ActorRequest updateRequest)
    {
        var result = _actorService.UpdateActor(actorId, updateRequest);
        return Ok(result);
    }

    [HttpDelete("{actorId}")]
    public IActionResult DeleteActor(string actorId)
    {
        var result = _actorService.DeleteActor(actorId);

        return Ok(result);
    }
}
