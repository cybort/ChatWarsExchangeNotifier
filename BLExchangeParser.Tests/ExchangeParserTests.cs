namespace BLExchangeParser.Tests
{
    using BLEntities;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class ExchangeParserTests
    {
        private readonly IList<string> testMessages;

        private readonly IList<ProductOffers> expectedResults;

        public ExchangeParserTests()
        {
            testMessages = new List<string>
            {
                @"Философский камень (y тебя 24шт и 489💰)\n1 шт по 873💰\n1 шт по 900💰\n3 шт по 950💰\n1 шт по 970💰\n23 шт по 982💰\n\n\nКупить 1шт: /wtb_113_1\n\nКупить 5шт: /wtb_113_5"
            };
            expectedResults = new List<ProductOffers>
            {
                new ProductOffers
                {
                    Product = new Product { Id = 123, Name = "Философский камень" },
                    Offers = new List<Offer>
                    {
                        new Offer { Count = 1, Price = 873 },
                        new Offer { Count = 1, Price = 900 },
                        new Offer { Count = 3, Price = 950 },
                        new Offer { Count = 1, Price = 970 },
                        new Offer { Count = 23, Price = 982 }
                    }
                }
            };
        }

        [Test]
        public void Test()
        {
            var exchangeParser = new ExchangeParser();
            for (int i = 0; i < testMessages.Count; i++)
            {
                var actualResult = exchangeParser.ParseWtbMessageResults(testMessages[i]);
                Assert.AreEqual(actualResult, expectedResults[i]);
            }
        }
    }
}
