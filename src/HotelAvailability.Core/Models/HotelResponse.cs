/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Hotel response model
/// </summary>

namespace HotelAvailability.Core.Models
{
    #region import namespaces
    
    using Newtonsoft.Json;
    using System.Collections.Generic;

    #endregion

    public class HotelResponse
    {
        [JsonProperty("propertyID")]
        public int PropertyId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("geoId")]
        public int GeoId { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("rates")]
        public List<RateResponse> Rates { get; set; }
    }
}