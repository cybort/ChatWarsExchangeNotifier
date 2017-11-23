using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTelegramClient;
using System.Configuration;
using BLExchangeParser;

namespace UIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var telegramClient = new TelegramClientWrapper(0, "");
            telegramClient.PrepareConnection("ChatWarsBot").Wait();
            telegramClient.SendMessage("/wtb_113").Wait();
            var message = telegramClient.GetLastMessage().Result;
            Console.WriteLine(message);

            var exchangeParser = new ExchangeParser();
            var productOffers = exchangeParser.ParseWtbMessageResults(message);

            Console.ReadKey();
        }
    }
}
