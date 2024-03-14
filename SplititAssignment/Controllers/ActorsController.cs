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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorsSummaryResponse))]
    public IActionResult GetAllActors([FromQuery] int? rankStart, [FromQuery] int? rankEnd, [FromQuery] string provider = "IMDb", [FromHeader] int skip = 0, [FromHeader] int take = 10)
    {
        var actors = _actorService.GetActorsSummary(provider, rankStart, rankEnd, skip, take);

        return Ok(actors);
    }

    [HttpGet("{actorId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]

    public IActionResult GetActorDetails(string actorId)
    {
        var actor = _actorService.GetActorDetails(actorId);

        return Ok(actor);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))] 
    public IActionResult AddActor([FromBody] ActorRequest addRequest)
    {
        var result = _actorService.AddActor(addRequest);
        return CreatedAtAction(nameof(GetActorDetails), new { actorId = result.Id }, result);
    }

    [HttpPut("{actorId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
    public IActionResult UpdateActor(string actorId, [FromBody] ActorRequest updateRequest)
    {
        var result = _actorService.UpdateActor(actorId, updateRequest);
        return Ok(result);
    }

    [HttpDelete("{actorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public IActionResult DeleteActor(string actorId)
    {
        _actorService.DeleteActor(actorId);

        return NoContent();
    }
}
