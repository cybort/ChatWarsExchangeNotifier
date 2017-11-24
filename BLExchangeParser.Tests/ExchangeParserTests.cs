namespace BLExchangeParser.Tests
{
    using BLEntities;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class ExchangeParserTests
    {
        [Test]
        public void TestParseWtbMessageResults_NormalOffers()
        {
            var testMessage =
                "Философский камень (y тебя 24шт и 489💰)\n1 шт по 873💰\n1 шт по 900💰\n3 шт по 950💰\n1 шт по 970💰\n23 шт по 982💰\n\n\nКупить 1шт: /wtb_113_1\n\nКупить 5шт: /wtb_113_5";
            var testProduct = new Product { Id = 113, Name = "Философский камень" };
            var expectedResult = new ProductOffers
            {
                Product = testProduct,
                Offers = new List<Offer>
                {
                    new Offer { Count = 1, Price = 873 },
                    new Offer { Count = 1, Price = 900 },
                    new Offer { Count = 3, Price = 950 },
                    new Offer { Count = 1, Price = 970 },
                    new Offer { Count = 23, Price = 982 }
                }
            };

            var exchangeParser = new ExchangeParser();
            var actualResult = exchangeParser.ParseWtbMessageResults(testProduct, testMessage);

            Assert.AreEqual(actualResult, expectedResult);
        }

        [Test]
        public void TestParseWtbMessageResults_SingleOffer()
        {
            var testMessage = "Заготовка кузнеца (y тебя 0шт и 194💰)\n1 шт по 1000💰\n\n\nКупить 1шт: /wtb_133_1\n\nКупить 5шт: /wtb_133_5";
            var testProduct = new Product { Id = 133, Name = "Заготовка кузнеца" };
            var expectedResult = new ProductOffers
            {
                Product = testProduct,
                Offers = new List<Offer>
                {
                    new Offer { Count = 1, Price = 1000 }
                }
            };

            var exchangeParser = new ExchangeParser();
            var actualResult = exchangeParser.ParseWtbMessageResults(testProduct, testMessage);

            Assert.AreEqual(actualResult, expectedResult);
        }

        [Test]
        public void TestParseWtbMessageResults_ProductIsNotForExchange()
        {
            var testMessage = "Предложений не нашлось";
            var testProduct = new Product { Id = 222, Name = "Шоколадка" };
            var expectedResult = new ProductOffers(testProduct);

            var exchangeParser = new ExchangeParser();
            var actualResult = exchangeParser.ParseWtbMessageResults(testProduct, testMessage);

            Assert.AreEqual(actualResult, expectedResult);
        }

        [Test]
        public void TestParseWtbMessageResults_NoOffers()
        {
            var testMessage = "Этот тип товара пока не разрешен к продаже на бирже.";
            var testProduct = new Product { Id = 1406, Name = "Цилиндр" };
            var expectedResult = new ProductOffers(testProduct);

            var exchangeParser = new ExchangeParser();
            var actualResult = exchangeParser.ParseWtbMessageResults(testProduct, testMessage);

            Assert.AreEqual(actualResult, expectedResult);
        }
    }
}
