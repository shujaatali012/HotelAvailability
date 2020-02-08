/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Hotel availability controller
/// </summary>

namespace HotelAvailability.Api.Controllers
{
    #region import namespaces

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using HotelAvailability.Core.Repositories.Interfaces;
    using HotelAvailability.Core.Models;
    using Microsoft.Extensions.Options;
    using HotelAvailability.Api.Helpers;
    using Newtonsoft.Json;

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class HotelAvailabilityController : ControllerBase
    {
        #region Proprties
        
        private ILogger<HotelAvailabilityController> _logger { get; set; }
        private readonly IOptions<CacheConfig> _cacheConfig;
        private readonly IMemoryCache _memoryCache;
        private IHotelAvailability _hotelAvailability { get; set; }

        #endregion

        #region Constructor(s)

        public HotelAvailabilityController(ILogger<HotelAvailabilityController> logger,
            IOptions<CacheConfig> cacheConfig,
            IMemoryCache memoryCache,
            IHotelAvailability hotelAvailability)
        {
            _logger = logger;
            _cacheConfig = cacheConfig;
            _memoryCache = memoryCache;
            _hotelAvailability = hotelAvailability;
        }

        #endregion

        #region Controller Methods

        /// <summary>
        /// GET api/HotelAvailability/getHotels/279/1
        /// </summary>
        [HttpPost("getHotels/{destinationId}/{numberOfNights}")]
        public async Task<ActionResult> GetHotels(int destinationId, int numberOfNights)
        {
            try
            {
                if (destinationId <= 0)
                {
                    _logger.LogError($"destinationId must be greater than 0.");
                    return BadRequest("destinationId must be greater than 0.");
                }

                if (numberOfNights <= 0)
                {
                    _logger.LogError("numberOfNights must be greater than 0.");
                    return BadRequest("numberOfNights must be greater than 0.");
                }

                var cacheConfiguration = _cacheConfig.Value;
                
                var cacheKey = $"{cacheConfiguration.Key}-{destinationId}-{numberOfNights}";

                List<Hotel> hotels = new List<Hotel>();

                bool isExist = _memoryCache.TryGetValue(cacheKey, out hotels);

                if (!isExist)
                {
                    hotels = await _hotelAvailability.GetHotelsAsync(destinationId, numberOfNights);

                    if (hotels.Count <= 0)
                        return NotFound();

                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(cacheConfiguration.Timeout));

                    _memoryCache.Set(cacheKey, hotels, cacheEntryOptions);
                }
                
                return Ok(hotels);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetHotels action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion
    }
}
