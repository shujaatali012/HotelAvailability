/// <summary>
/// Author:         Shujaat Ali
/// Creation Date:  Feb 7, 2020
/// Description:    Hotel availability repository
/// </summary>

namespace HotelAvailability.Core.Repositories
{
    #region import namespaces

    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Models;
    using Newtonsoft.Json;
    using Interfaces;
    
    #endregion

    public class HotelAvailability : IHotelAvailability
    {
        #region Proprties

        private string _bargainsApiUrl { get; set; }
        private string _bargainsApiAuthenticationCode { get; set; }

        #endregion

        #region Constructor(s)

        public HotelAvailability(string bargainsApiUrl, string bargainsApiAuthenticationCode)
        {
            _bargainsApiUrl = bargainsApiUrl;
            _bargainsApiAuthenticationCode = bargainsApiAuthenticationCode;
        }

        #endregion

        #region Public Methods

        public async Task<List<Hotel>> GetHotelsAsync(int destinationId, int numberOfNights)
        {
            List<Hotel> hotels = new List<Hotel>();
            List<HotelResponse> hotelResponse = new List<HotelResponse>();

            #region External Api Call

            using (HttpClient client = new HttpClient())
            {
                var apiUrl = $"{_bargainsApiUrl}findBargain?destinationId={destinationId}&nights={numberOfNights}&code={_bargainsApiAuthenticationCode}";

                var result = await client.GetStringAsync(apiUrl);
                hotelResponse = JsonConvert.DeserializeObject<List<HotelResponse>>(result);
            }

            #endregion

            //// TODO: Remove region after integration with actual api
            //#region Static Data

            //List<RateResponse> rateList1 = new List<RateResponse>();
            //List<RateResponse> rateList2 = new List<RateResponse>();

            //rateList1.Add(new RateResponse() { RateType = "PerNight", BoardType = "No Meals", Value = 207.6 });
            //rateList1.Add(new RateResponse() { RateType = "PerNight", BoardType = "Half Board", Value = 242.2 });
            //rateList1.Add(new RateResponse() { RateType = "PerNight", BoardType = "Full Board", Value = 276.8 });

            //rateList2.Add(new RateResponse() { RateType = "Stay", BoardType = "No Meals", Value = 590.4 });
            //rateList2.Add(new RateResponse() { RateType = "Stay", BoardType = "Half Board", Value = 688.8 });
            //rateList2.Add(new RateResponse() { RateType = "Stay", BoardType = "Full Board", Value = 787.2 });

            //hotelResponse.Add(new HotelResponse() { PropertyId = 79732, Name = "JAC Canada (CA$)8314", GeoId = 279, Rating = 3, Rates = rateList1 });
            //hotelResponse.Add(new HotelResponse() { PropertyId = 79821, Name = "JAC Canada (CA$)8555", GeoId = 279, Rating = 3, Rates = rateList2 });

            //#endregion

            #region Process Response
            
            if (hotelResponse.Count > 0)
            {
                foreach (var itemHotel in hotelResponse)
                {
                    Hotel hotel = new Hotel();

                    hotel.PropertyId = itemHotel.PropertyId;
                    hotel.Name = itemHotel.Name;
                    hotel.GeoId = itemHotel.GeoId;
                    hotel.Rating = itemHotel.Rating;
                    hotel.Rates = new List<Rate>();

                    List<RateResponse> rateResponse = itemHotel.Rates.ToList();

                    if (rateResponse.Count > 0)
                    {
                        foreach (var itemRate in itemHotel.Rates)
                        {
                            Rate rate = new Rate();

                            rate.BoardType = itemRate.BoardType;
                            rate.RateType = itemRate.RateType;
                            rate.Price = itemRate.RateType == "PerNight" ? Math.Round((itemRate.Value * numberOfNights), 2) : itemRate.Value;

                            hotel.Rates.Add(rate);
                        }
                    }

                    hotels.Add(hotel);
                }
            }

            #endregion

            return hotels;
        }

        #endregion
    }
}
