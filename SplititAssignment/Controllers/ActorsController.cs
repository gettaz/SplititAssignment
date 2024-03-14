using Microsoft.AspNetCore.Mvc;
using SplititAssignment.Interfaces;
using SplititAssignment.Models;

/// <summary>
/// Controller for managing actors.
/// </summary>
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

    /// <summary>
    /// Retrieves a summary of all actors.
    /// </summary>
    /// <param name="rankStart">The starting rank.</param>
    /// <param name="rankEnd">The ending rank.</param>
    /// <param name="provider">The provider, "IMDb" and "Pinkvilla" are preloaded.</param>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="take">The number of items to take, defaults with 10</param>
    /// <returns>A list of actors.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorsSummaryResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]

    public IActionResult GetAllActors([FromQuery] int? rankStart, [FromQuery] int? rankEnd, [FromQuery] string provider = "IMDb", [FromHeader] int skip = 0, [FromHeader] int take = 10)
    {
        var actors = _actorService.GetActorsSummary(provider, rankStart, rankEnd, skip, take);

        return Ok(actors);
    }

    /// <summary>
    /// Retrieves detailed information about a specific actor.
    /// </summary>
    /// <param name="actorId">The unique identifier of the actor.</param>
    /// <returns>Detailed information about the actor.</returns>

    [HttpGet("{actorId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]

    public IActionResult GetActorDetails(string actorId)
    {
        var actor = _actorService.GetActorDetails(actorId);

        return Ok(actor);
    }

    /// <summary>
    /// Adds a new actor to the system.
    /// </summary>
    /// <param name="addRequest">The actor data to add.</param>
    /// <returns>The created actor's details.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))] 
    public IActionResult AddActor([FromBody] ActorRequest addRequest)
    {
        var result = _actorService.AddActor(addRequest);
        return CreatedAtAction(nameof(GetActorDetails), new { actorId = result.Id }, result);
    }

    /// <summary>
    /// Updates information about an existing actor.
    /// </summary>
    /// <remarks>
    /// Note: Updating an actor to have the same rank and name as another actor within the same provider is not supported.
    /// If attempted, the operation will fail with a conflict error.
    /// </remarks>
    /// <param name="actorId">The unique identifier of the actor to update.</param>
    /// <param name="updateRequest">The updated actor data. Ensure that the combination of rank, name, and provider remains unique.</param>
    /// <returns>The updated actor's details. If the operation is successful, the updated details of the actor are returned. In case of a conflict due to duplicate rank and name for the same provider, a conflict error is returned.</returns>

    [HttpPut("{actorId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActorResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(Error))]
    public IActionResult UpdateActor(string actorId, [FromBody] ActorRequest updateRequest)
    {
        var result = _actorService.UpdateActor(actorId, updateRequest);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an actor from the system.
    /// </summary>
    /// <param name="actorId">The unique identifier of the actor to delete.</param>
    /// <returns>An acknowledgment of deletion.</returns>
    [HttpDelete("{actorId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public IActionResult DeleteActor(string actorId)
    {
        _actorService.DeleteActor(actorId);

        return NoContent();
    }
}
