using BLEntities;
using System.Collections.Generic;

namespace BLExchangeParser
{
    public class ExchangeParser
    {
        public ProductOffers ParseWtbMessageResults(string message)
        {
            return new ProductOffers
            {
                Product = new Product {Id = 123, Name = "Философский камень"},
                Offers = new List<Offer>
                {
                    new Offer {Count = 1, Price = 873},
                    new Offer {Count = 1, Price = 900},
                    new Offer {Count = 3, Price = 950},
                    new Offer {Count = 1, Price = 970},
                    new Offer {Count = 23, Price = 982}
                }
            };
        }
    }
}
