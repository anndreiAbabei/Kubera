using AutoMapper;
using Kubera.App.Infrastructure;
using Kubera.App.Models;
using Kubera.Business.Repository;
using Kubera.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kubera.App.Controllers.V1
{
    [ApiVersion("1.0")]
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;

        public CurrencyController(ICurrencyRepository currencyRepository, IMapper mapper)
        {
            _currencyRepository = currencyRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all currencies suported by the platform
        /// </summary>
        /// <returns>Collection of currencies</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CurrencyModel>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CurrencyModel>>> GetCurrencies()
        {
            var ct = HttpContext.RequestAborted;
            var query = await _currencyRepository.GetAll(cancellationToken: ct)
                .ConfigureAwait(false);

            var currencies = await query
                .ToListAsync(ct)
                .ConfigureAwait(false);

            return Ok(currencies.Select(_mapper.Map<Currency, CurrencyModel>));
        }
    }
}
