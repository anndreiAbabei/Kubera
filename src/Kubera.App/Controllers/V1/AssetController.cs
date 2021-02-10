using Kubera.App.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kubera.Application.Common.Models;
using MediatR;
using Kubera.Application.Features.Queries.GetAllAssets.V1;
using Kubera.App.Infrastructure.Extensions;
using Kubera.Application.Features.Queries.GetAsset.V1;
using Kubera.Application.Features.Commands.CreateAsset.V1;
using Kubera.Application.Features.Commands.UpdateAsset.V1;
using Kubera.Application.Features.Commands.DeleteAsset.V1;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AssetController : BaseController
    {

        public AssetController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Get all assets suported by the platform
        /// </summary>
        /// <returns>Collection of assets</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AssetModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AssetModel>>> GetAssets()
        {
            var query = new GetAllAssetsQuery();
            var result = await Mediator.Send(query, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return result.AsActionResult();
        }

        /// <summary>
        /// Get an asset for the current user
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Requested group</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(AssetModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<AssetModel>> GetAsset(Guid id)
        {
            var query = new GetAssetQuery
            {
                Id = id
            };
            var result = await Mediator.Send(query, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return result.AsActionResult();
        }

        /// <summary>
        /// Create a new asset for logged user
        /// </summary>
        /// <param name="model">Input model for the new asset</param>
        /// <returns>The new asset</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(AssetModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<AssetModel>> PostAsset(AssetInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateAssetCommand
            {
                Input = model
            };
            var result = await Mediator.Send(command, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if(result.IsFailure)
                return result.AsActionResult();

            return CreatedAtAction(nameof(GetAsset), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Update an asset
        /// </summary>
        /// <param name="id">Id of the asset</param>
        /// <param name="group">Asset update model</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutAsset(Guid id, AssetUpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new UpdateAssetCommand
            {
                Id = id,
                Input = model
            };
            var result = await Mediator.Send(command, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return result.AsActionResult();
        }

        /// <summary>
        /// Delete an asset
        /// </summary>
        /// <param name="id">Id of the asset</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsset(Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new DeleteAssetCommand
            {
                Id = id
            };
            var result = await Mediator.Send(command, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return result.AsActionResult();
        }
    }
}
