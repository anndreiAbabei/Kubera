using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Kubera.App.Models;
using Kubera.App.Infrastructure;
using Kubera.Business.Repository;
using Kubera.Data.Entities;
using Kubera.General.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class GroupsController : BaseController
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupsController(IGroupRepository groupRepository, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all groups for the logged user
        /// </summary>
        /// <returns>Collection of logged user groups</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GroupModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GroupModel>>> GetGroups()
        {
            var groups = await _groupRepository.GetAll()
                .ToListAsync(HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(groups.Select(_mapper.Map<Group, GroupModel>));
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
            var group = await _groupRepository.GetById(id, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (group == null)
                return NotFound();

            if (group.OwnerId != User.Identity.Name)
                return Forbid();

            return Ok(_mapper.Map<Group, GroupModel>(group));
        }

        /// <summary>
        /// Create a new group for logged user
        /// </summary>
        /// <param name="model">Input model for the new group</param>
        /// <returns>The new group</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(GroupModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<GroupModel>> PostGroup(GroupPostModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ct = HttpContext.RequestAborted;
            var group = new Group
            {
                Code = model.Code,
                Name = model.Name,
                CreatedAt = DateTime.UtcNow,
                OwnerId = User.Identity.Name
            };

            if (await _groupRepository.Exists(group, ct).ConfigureAwait(false))
                return Conflict("Code or Name alredy exists");

            group = await _groupRepository.Add(group, ct)
                .ConfigureAwait(false);

            var result = _mapper.Map<Group, GroupModel>(group);

            return CreatedAtAction(nameof(GetGroups), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update a group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="group">Group update model</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutGroup(Guid id, GroupPutModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ct = HttpContext.RequestAborted;
            var group = await _groupRepository.GetById(id).ConfigureAwait(false);

            if (group == null)
                return NotFound();

            if (group.OwnerId != User.Identity.Name)
                return Forbid();

            if (await _groupRepository.Exists(group, ct).ConfigureAwait(false))
                return Conflict("Code or Name alredy exists");

            group.Code = model.Code;
            group.Name = model.Name;

            await _groupRepository.Update(group, ct)
                    .ConfigureAwait(false);

            return NoContent();
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
            var ct = HttpContext.RequestAborted;
            var group = await _groupRepository.GetById(id, ct);

            if (group == null)
                return NotFound();

            if (group.OwnerId != User.Identity.Name)
                return Forbid();

            await _groupRepository.Delete(group.Id, ct)
                .ConfigureAwait(false);

            return NoContent();
        }
    }
}
