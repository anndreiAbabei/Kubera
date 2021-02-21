using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Kubera.App.Infrastructure;
using Kubera.Application.Common.Models;
using Kubera.Application.Features.Queries.GetAllGroups.V1;
using Kubera.App.Infrastructure.Extensions;
using Kubera.Application.Features.Queries.GetGroup.V1;
using Kubera.Application.Features.Commands.CreateGroup;
using Kubera.Application.Features.Commands.UpdateGroup.V1;
using MediatR;
using Kubera.Application.Features.Commands.DeleteGroup.V1;
using Kubera.Application.Features.Queries.GetGroupTotals.V1;
using System.Linq;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class GroupController : BaseController
    {
        public GroupController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Get all groups for the logged user
        /// </summary>
        /// <returns>Collection of logged user groups</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GroupModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GroupModel>>> GetGroups()
        {
            var query = new GetAllGroupsQuery();

            return await ExecuteRequest(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a group for the current user
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Requested group</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(GroupModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<GroupModel>> GetGroup(Guid id)
        {
            var query = new GetGroupQuery
            {
                Id = id
            };

            return await ExecuteRequest(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all assets with their total of a user
        /// </summary>
        /// <returns>Collection of assets with their respective assets</returns>
        [HttpGet("totals")]
        [ProducesResponseType(typeof(IEnumerable<GroupTotalModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GroupTotalModel>>> GetGroupTotals([FromQuery] Guid currencyId)
        {
            var query = new GetGroupTotalQuery
            {
                CurrencyId = currencyId
            };

            return await ExecuteRequest(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new group for logged user
        /// </summary>
        /// <param name="model">Input model for the new group</param>
        /// <returns>The new group</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(GroupModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<GroupModel>> PostGroup(GroupInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateGroupCommand
            {
                Input = model
            };
            var result = await Mediator.Send(command, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (result.IsFailure)
                return result.AsActionResult();

            return CreatedAtAction(nameof(GetGroup), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Update a group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="model">Group update model</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutGroup(Guid id, GroupUpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new UpdateGroupCommand
            {
                Id = id,
                Input = model
            };

            return await ExecuteRequest(command).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete a group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var command = new DeleteGroupCommand
            {
                Id = id
            };

            return await ExecuteRequest(command).ConfigureAwait(false);
        }
    }
}
