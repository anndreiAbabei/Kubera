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
    public class TransactionController : BaseController
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all groups for the logged user
        /// </summary>
        /// <returns>Collection of logged user groups</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetTransactions()
        {
            var transactions = await _transactionRepository.GetAll()
                .ToListAsync(HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return Ok(transactions.Select(_mapper.Map<Transaction, TransactionModel>));
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
            var transaction = await _transactionRepository.GetById(id, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            if (transaction == null)
                return NotFound();

            if (transaction.OwnerId != User.Identity.Name)
                return Forbid();

            return Ok(_mapper.Map<Transaction, TransactionModel>(transaction));
        }

        /// <summary>
        /// Create a new transaction for logged user
        /// </summary>
        /// <param name="model">Input model for the new transaction</param>
        /// <returns>The new transaction</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TransactionModel), StatusCodes.Status201Created)]
        public async Task<ActionResult<TransactionModel>> PostTransaction(TransactionPostModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ct = HttpContext.RequestAborted;
            var transaction = new Transaction
            {
                AssetId = model.AssetId,
                Wallet = model.Wallet,
                CreatedAt = DateTime.UtcNow,
                Amount = model.Amount,
                CurrencyId = model.CurrencyId,
                Rate = model.Rate,
                Fee = model.Fee,
                FeeCurrencyId = model.FeeCurrencyId,
                OwnerId = User.Identity.Name
            };

            transaction = await _transactionRepository.Add(transaction, ct)
                .ConfigureAwait(false);

            var result = _mapper.Map<Transaction, TransactionModel>(transaction);

            return CreatedAtAction(nameof(GetTransaction), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update a transaction
        /// </summary>
        /// <param name="id">Id of the transaction</param>
        /// <param name="group">Transaction update model</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutTransaction(Guid id, TransactionPutModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ct = HttpContext.RequestAborted;
            var transaction = await _transactionRepository.GetById(id).ConfigureAwait(false);

            if (transaction == null)
                return NotFound();

            if (transaction.OwnerId != User.Identity.Name)
                return Forbid();

            transaction.AssetId = model.AssetId;
            transaction.Wallet = model.Wallet;
            transaction.Amount = model.Amount;
            transaction.CurrencyId = model.CurrencyId;
            transaction.Rate = model.Rate;
            transaction.Fee = model.Fee;
            transaction.FeeCurrencyId = model.FeeCurrencyId;

            await _transactionRepository.Update(transaction, ct)
                    .ConfigureAwait(false);

            return NoContent();
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
            var ct = HttpContext.RequestAborted;
            var group = await _transactionRepository.GetById(id, ct);

            if (group == null)
                return NotFound();

            if (group.OwnerId != User.Identity.Name)
                return Forbid();

            await _transactionRepository.Delete(group.Id, ct)
                .ConfigureAwait(false);

            return NoContent();
        }
    }
}
