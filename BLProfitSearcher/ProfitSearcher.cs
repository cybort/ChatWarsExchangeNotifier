using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BLEntities;

namespace BLProfitSearcher
{
    public class ProfitSearcher
    {
        private readonly Config config;
        private readonly Exchange exchange;

        public ProfitSearcher(Exchange exchange, Config config)
        {
            this.exchange = exchange;
            this.config = config;
        }

        public IEnumerable<Product> FindProfitableOffers()
        {
            var profitableProducts = new List<Product>();
            foreach (var productOffers in exchange.ProductOffers)
            {
                if (productOffers.Offers.Count > 1)
                {
                    double averagePrice = CountAveragePrice(productOffers.Offers);
                    if (IsPriceProfitable(productOffers.Offers[0].Price, productOffers.Offers[1].Price, averagePrice))
                    {
                        profitableProducts.Add(productOffers.Product);
                    }
                }
            }

            return profitableProducts;
        }

        private double CountAveragePrice(IEnumerable<Offer> offers)
        {
            int sum = 0, count = 0;
            foreach (var offer in offers)
            {
                sum += offer.Count * offer.Price;
                count += offer.Count;
            }

            double averagePrice = count == 0 ? 0 : sum / count;
            return averagePrice;
        }

        private bool IsPriceProfitable(int price, int nextPrice, double averagePrice)
        {
            return price <= averagePrice * (100 - config.ProfitPercentage) / 100 &&
                   nextPrice - price >= config.ProfitAmount;
        }
    }
}
