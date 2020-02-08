/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Rate response model
/// </summary>

namespace HotelAvailability.Core.Models
{
    #region import namespaces

    using Newtonsoft.Json;

    #endregion

    public class RateResponse
    {
        [JsonProperty("rateType")]
        public string RateType { get; set; }

        [JsonProperty("boardType")]
        public string BoardType { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }
    }
}
