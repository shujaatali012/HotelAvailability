/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Hotel availability interface
/// </summary>

namespace HotelAvailability.Core.Repositories.Interfaces
{
    #region import namespaces

    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Models;

    #endregion

    public interface IHotelAvailability
    {
        Task<List<Hotel>> GetHotelsAsync(int destinationId, int numberOfNights);
    }
}
