using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace BLExchangeParser
{
    using BLEntities;
    using System.Collections.Generic;

    public class ExchangeParser
    {
        public ProductOffers ParseWtbMessageResults(Product product, string message)
        {
            var productOffers = new ProductOffers(product);

            string productOffersPattern = $@"{product.Name}\s\(.+\)\n([\s\S]*)\n\n\nКупить 1шт: {product.Command}_1\n\nКупить 5шт: {product.Command}_5";
            Match productOffersMatch = Regex.Match(message, productOffersPattern);
            if (productOffersMatch.Success)
            {
                var offers = productOffersMatch.Groups[1].Value;
                productOffers.Offers = ParseOffers(offers);
            }

            return productOffers;
        }

        private IList<Offer> ParseOffers(string offersLine)
        {
            var offers = new List<Offer>();
            string productPattern = @"(\d+) шт по (\d+)";
            foreach (var line in offersLine.Split('\n'))
            {
                Match productMatch = Regex.Match(line, productPattern);
                if (productMatch.Success)
                {
                    var offer = new Offer
                    {
                        Count = Convert.ToInt32(productMatch.Groups[1].Value),
                        Price = Convert.ToInt32(productMatch.Groups[2].Value)
                    };

                    offers.Add(offer);
                }
            }

            return offers;
        }
    }
}
