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
using Kubera.App.Infrastructure.Extensions;
using Kubera.Data.Extensions;
using Kubera.General.Models;
using Kubera.General.Services;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        private readonly IUserIdAccesor _userIdAccesor;

        public TransactionController(ITransactionRepository transactionRepository, 
                                     IAssetRepository assetRepository,
                                     IGroupRepository groupRepository,
                                     ICurrencyRepository currencyRepository,
                                     IMapper mapper,
                                     IUserIdAccesor userIdAccesor)
        {
            _transactionRepository = transactionRepository;
            _assetRepository = assetRepository;
            _groupRepository = groupRepository;
            _currencyRepository = currencyRepository;
            _mapper = mapper;
            _userIdAccesor = userIdAccesor;
        }

        /// <summary>
        /// Get all groups for the logged user
        /// </summary>
        /// <returns>Collection of logged user groups</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TransactionModel>>> GetTransactions([FromQuery] Paging paging, [FromQuery] DateFilter filter, [FromQuery] Order? order)
        {
            paging ??= new Paging();
            order ??= Order.Descending;

            var ct = HttpContext.RequestAborted;

             var query = await _transactionRepository.GetAll(paging, filter, ct)
                .ConfigureAwait(false);

            query = order.Value == Order.Descending
                        ? query.OrderByDescending(t => t.CreatedAt)
                        : query.OrderBy(t => t.CreatedAt);

            var transactions = await query.ToListAsync(ct)
                .ConfigureAwait(false);
            var currencies = await _currencyRepository.GetAll(cancellationToken: ct)
                .ToListAsync(ct)
                .ConfigureAwait(false);
            var assets = await _assetRepository.GetAll(cancellationToken: ct)
                .ToListAsync(ct)
                .ConfigureAwait(false);
            var groups = await _groupRepository.GetAll(cancellationToken: ct)
                .ToListAsync(ct)
                .ConfigureAwait(false);


            HttpContext.AddPaging(paging.Result);

            var models = transactions.Select(_mapper.Map<Transaction, TransactionModel>).ToList();

            foreach (var model in models)
            {
                if (currencies.Found(model.CurrencyId, out var currency))
                    model.Currency = _mapper.Map<Currency, CurrencyModel>(currency);

                if (assets.Found(model.AssetId, out var asset))
                {
                    model.Asset = _mapper.Map<Asset, AssetModel>(asset);


                    if (groups.Found(asset.GroupId, out var group))
                        model.Asset.Group = _mapper.Map<Group, GroupModel>(group);
                }

                if (model.FeeCurrencyId.HasValue && currencies.Found(model.FeeCurrencyId.Value, out var feeCurrency))
                    model.FeeCurrency = _mapper.Map<Currency, CurrencyModel>(feeCurrency);
            }


            return Ok(models);
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
                CreatedAt = model.CreatedAt,
                Amount = model.Amount,
                CurrencyId = model.CurrencyId,
                Rate = model.Rate,
                Fee = model.Fee,
                FeeCurrencyId = model.FeeCurrencyId,
                OwnerId = _userIdAccesor.Id
            };

            transaction = await _transactionRepository.Add(transaction, ct)
                .ConfigureAwait(false);
            var currencies = await _currencyRepository.GetAll(cancellationToken: ct)
                .ToListAsync(ct)
                .ConfigureAwait(false);
            var assets = await _assetRepository.GetAll(cancellationToken: ct)
                .ToListAsync(ct)
                .ConfigureAwait(false);
            var groups = await _groupRepository.GetAll(cancellationToken: ct)
                .ToListAsync(ct)
                .ConfigureAwait(false);

            var result = _mapper.Map<Transaction, TransactionModel>(transaction);

            if (currencies.Found(model.CurrencyId, out var currency))
                result.Currency = _mapper.Map<Currency, CurrencyModel>(currency);

            if (assets.Found(model.AssetId, out var asset))
            {
                result.Asset = _mapper.Map<Asset, AssetModel>(asset);


                if (groups.Found(asset.GroupId, out var group))
                    result.Asset.Group = _mapper.Map<Group, GroupModel>(group);
            }

            if (result.FeeCurrencyId.HasValue && currencies.Found(model.FeeCurrencyId.Value, out var feeCurrency))
                result.FeeCurrency = _mapper.Map<Currency, CurrencyModel>(feeCurrency);

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
