/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Hotel model
/// </summary>

namespace HotelAvailability.Core.Models
{
    #region import namespaces

    using System.Collections.Generic;

    #endregion

    public class Hotel
    {
        public int PropertyId { get; set; }

        public string Name { get; set; }

        public int GeoId { get; set; }

        public int Rating { get; set; }

        public List<Rate> Rates { get; set; }
    }
}
