using AutoMapper;
using Kubera.App.Infrastructure;
using Kubera.App.Models;
using Kubera.Business.Repository;
using Kubera.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kubera.General.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AssetController : BaseController
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMapper _mapper;

        public AssetController(IAssetRepository assetRepository, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all assets suported by the platform
        /// </summary>
        /// <returns>Collection of assets</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AssetModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AssetModel>>> GetAssets()
        {
            var ct = HttpContext.RequestAborted;
            var query = await _assetRepository.GetAll(cancellationToken: ct)
                .ConfigureAwait(false);

            var asstes = await query
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return Ok(asstes.Select(_mapper.Map<Asset, AssetModel>));
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
        public async Task<ActionResult<AssetModel>> GetAssets(Guid id)
        {
            var asset = await _assetRepository.GetById(id, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (asset == null)
                return NotFound();

            if (asset.OwnerId != User.Identity.Name)
                return Forbid();

            return Ok(_mapper.Map<Asset, AssetModel>(asset));
        }

        /// <summary>
        /// Create a new asset for logged user
        /// </summary>
        /// <param name="model">Input model for the new asset</param>
        /// <returns>The new asset</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(AssetModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<AssetModel>> PostAsset(AssetPostModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ct = HttpContext.RequestAborted;
            var asset = new Asset
            {
                Code = model.Code,
                Name = model.Name,
                GroupId = model.GroupId,
                Order = model.Order,
                Icon = model.Icon,
                Symbol = model.Symbol,
                CreatedAt = DateTime.UtcNow,
                OwnerId = User.Identity.Name
            };

            if (await _assetRepository.Exists(asset, ct).ConfigureAwait(false))
                return Conflict("Code or Name alredy exists");

            asset = await _assetRepository.Add(asset, ct)
                .ConfigureAwait(false);

            var result = _mapper.Map<Asset, AssetModel>(asset);

            return CreatedAtAction(nameof(GetAssets), new { id = result.Id }, result);
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
        public async Task<IActionResult> PutAsset(Guid id, AssetPutModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ct = HttpContext.RequestAborted;
            var asset = await _assetRepository.GetById(id).ConfigureAwait(false);

            if (asset.OwnerId != User.Identity.Name)
                return Forbid();

            if (await _assetRepository.Exists(asset, ct).ConfigureAwait(false))
                return Conflict("Code or Name alredy exists");

            asset.Code = model.Code;
            asset.Name = model.Name;
            asset.Symbol = model.Symbol;
            asset.Icon = model.Icon;
            asset.GroupId = model.GroupId;
            asset.Order = model.Order;

            await _assetRepository.Update(asset, ct)
                    .ConfigureAwait(false);

            return NoContent();
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
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var ct = HttpContext.RequestAborted;
            var asset = await _assetRepository.GetById(id, ct);

            if (asset == null)
                return NotFound();

            if (asset.OwnerId != User.Identity.Name)
                return Forbid();

            await _assetRepository.Delete(asset.Id, ct)
                .ConfigureAwait(false);

            return NoContent();
        }
    }
}
