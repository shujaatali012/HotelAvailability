/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Rate model
/// </summary>

namespace HotelAvailability.Core.Models
{
    #region import namespaces
    
    #endregion
   
    public class Rate
    {
        public string RateType { get; set; }

        public string BoardType { get; set; }

        public double Price { get; set; }
    }
}
