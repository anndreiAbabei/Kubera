using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Kubera.App.Models;
using Kubera.App.Infrastructure;
using Kubera.App.Infrastructure.Extensions;
using Kubera.General.Models;
using Kubera.Application.Common.Models;
using MediatR;
using Kubera.Application.Features.Queries.GetTransactions.V1;
using Kubera.Application.Features.Queries.GetTransaction.V1;
using Kubera.Application.Features.Commands.CreateTransaction.V1;
using Kubera.Application.Features.Commands.UpdateTransaction.V1;
using Kubera.Application.Features.Commands.DeleteTransaction.V1;
using Kubera.Application.Features.Queries.GetAssetGroupedTransactions.V1;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class TransactionController : BaseController
    {
        public TransactionController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Get all groups for the logged user
        /// </summary>
        /// <returns>Collection of logged user groups</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetTransactions([FromQuery] Paging paging, [FromQuery] DateFilter filter, [FromQuery] Order? order)
        {
            var query = new GetTransactionsQuery
            {
                Paging = paging,
                Date = filter,
                Order = order
            };

            HttpContext.AddCachePrefernces(query);

            var result = await Mediator.Send(query, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (result.IsFailure)
                return result.Error.AsErrorActionResult();

            HttpContext.AddPaging(result.Value.Paging);
            HttpContext.AddFromCacheHeader(query);

            return Ok(result.Value.Transactions);
        }
        
        /// <summary>
        /// Get all groups for the logged user
        /// </summary>
        /// <returns>Collection of logged user groups</returns>
        [HttpGet("grouped")]
        [ProducesResponseType(typeof(IEnumerable<GroupedTransactionsModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GroupedTransactionsModel>>> GetGroupedTransactions([FromQuery] Order? order)
        {
            var query = new GetAssetGroupedTransactionsQuery
            {
                Order = order
            };

            return await ExecuteRequest(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a transaction for the current user
        /// </summary>
        /// <param name="id">Group id</param>
        /// <returns>Requested group</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(TransactionModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<TransactionModel>> GetTransaction(Guid id)
        {
            var query = new GetTransactionQuery
            {
                Id = id
            };

            return await ExecuteRequest(query).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new transaction for logged user
        /// </summary>
        /// <param name="model">Input model for the new transaction</param>
        /// <returns>The new transaction</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TransactionModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<TransactionModel>> PostTransaction(TransactionInputModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new CreateTransactionCommand
            {
                Input = model
            };
            var result = await Mediator.Send(command, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (result.IsFailure)
                return result.AsActionResult();

            return CreatedAtAction(nameof(GetTransaction), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Update a transaction
        /// </summary>
        /// <param name="id">Id of the transaction</param>
        /// <param name="model">Transaction update model</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutTransaction(Guid id, TransactionUpdateModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new UpdateTransactionCommand
            {
                Id = id,
                Input = model
            };

            return await ExecuteRequest(command).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete a transaction
        /// </summary>
        /// <param name="id">Id of the transaction</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var command = new DeleteTransactionCommand
            {
                Id = id
            };

            return await ExecuteRequest(command).ConfigureAwait(false);
        }
    }
}
