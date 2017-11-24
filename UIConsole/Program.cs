using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTelegramClient;
using System.Configuration;
using BLExchangeParser;
using DALConfigWorker;
using System.Reflection;
using System.IO;
using System.Threading;
using BLEntities;
using BLProfitSearcher;

namespace UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var configWorker = new ConfigWorker(rootPath);
            var products = configWorker.GetAllProducts();
            var config = configWorker.GetConfig();

            var telegramClient = new TelegramClientWrapper(0, "");
            telegramClient.PrepareConnectionAsync("ChatWarsBot").Wait();

            var exchange = new Exchange();
            foreach (var product in products)
            {
                var message = telegramClient.SendMessageAndGetResponseAsync(product.Command).Result;
                Thread.Sleep(1000);

                var exchangeParser = new ExchangeParser();
                var productOffers = exchangeParser.ParseWtbMessageResults(product, message);
                exchange.ProductOffers.Add(productOffers);
            }

            var profitSearcher = new ProfitSearcher(exchange, config);
            var profitableProducts = profitSearcher.FindProfitableOffers();

            if (profitableProducts.Count() != 0)
            {
                foreach (var profitableProduct in profitableProducts)
                {
                    telegramClient.SendMessageToChannel(profitableProduct.Name).Wait();
                }
            }

            Console.ReadKey();
        }
    }
}
