namespace BLTelegramClient
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using TeleSharp.TL;
    using TeleSharp.TL.Messages;
    using TLSharp.Core;

    public class TelegramClientWrapper
    {
        private readonly int apiId;
        private readonly string apiHash;
        private TelegramClient client;
        private TLInputPeerUser participant;

        public TelegramClientWrapper(int apiId, string apiHash)
        {
            this.apiId = apiId;
            this.apiHash = apiHash;
        }

        public async Task<string> SendMessageAndGetResponseAsync(string message)
        {
            int requestAttempts = 0, responseAttempts = 0;
            var response = string.Empty;

            while (requestAttempts != 2)
            {
                await SendMessageAsync(message);
                Console.WriteLine($"Sent command {message}");
                while (responseAttempts != 5)
                {
                    response = await GetLastMessageAsync();
                    Console.WriteLine("Got message");
                    if (!response.Equals(message))
                    {
                        Console.WriteLine("Message is valid");
                        return response;
                    }

                    Console.WriteLine("Message is invalid");
                    responseAttempts++;
                    if (responseAttempts != 3)
                    {
                        Console.WriteLine($"Sleep for {200 * responseAttempts}");
                        Thread.Sleep(300 * responseAttempts);
                    }
                }

                requestAttempts++;
            }
            return response;
        }

        public async Task SendMessageAsync(string message)
        {
            await client.SendMessageAsync(participant, message);
        }

        public async Task<string> GetLastMessageAsync()
        {
            var history = await client.GetHistoryAsync(participant, 0, Int32.MaxValue, 1);
            var messages = (TLMessagesSlice) history;
            var lastMessage = (TLMessage) messages.Messages.FirstOrDefault();
            if (lastMessage != null)
            {
                return lastMessage.Message;
            }

            return string.Empty;
        }

        public async Task SendMessageToChannel(string message)
        {
            var dialogs = (TLDialogs)await client.GetUserDialogsAsync();
            var chat = dialogs.Chats
                .OfType<TLChannel>()
                .FirstOrDefault(c => c.Title == "Рубиновый барыга");

            await client.SendMessageAsync(new TLInputPeerChannel() { ChannelId = chat.Id, AccessHash = chat.AccessHash.Value }, message);
        }

        public async Task PrepareConnectionAsync(string participantName, bool firstConnect = false, string phoneNumber = "", string code = "")
        {
            await AuthenticateUserAsync();
            if (firstConnect)
            {
                AuthorizeClientAsync(phoneNumber, code);
            }

            await InitializeParticipantAsync(participantName);
        }

        private async Task AuthenticateUserAsync()
        {
            client = NewClient();
            await client.ConnectAsync();
        }

        private async Task InitializeParticipantAsync(string participantName)
        {
            var searchResult = await client.SearchUserAsync(participantName);
            var participant = (TLUser)searchResult.Users.FirstOrDefault();

            if (participant == null)
            {
                throw new Exception($"Didn't find participant: {participantName}");
            }

            var id = participant.Id;
            var hash = participant.AccessHash.Value;

            this.participant = new TeleSharp.TL.TLInputPeerUser() { UserId = id, AccessHash = hash };
        }

        private async void AuthorizeClientAsync(string phoneNumber, string code)
        {
            var hash = await client.SendCodeRequestAsync(phoneNumber);

            if (String.IsNullOrWhiteSpace(code))
            {
                throw new Exception("CodeToAuthenticate is empty, fill it with the code you just got now by SMS/Telegram");
            }

            await client.MakeAuthAsync(phoneNumber, hash, code);
        }

        private TelegramClient NewClient()
        {
            try
            {
                return new TelegramClient(apiId, apiHash);
            }
            catch (MissingApiConfigurationException ex)
            {
                throw new Exception($"Please provide your API settings. (More info: {MissingApiConfigurationException.InfoUrl})", ex);
            }
        }
    }
}
