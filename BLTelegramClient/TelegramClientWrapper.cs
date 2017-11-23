using TeleSharp.TL.Messages;

namespace BLTelegramClient
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TeleSharp.TL;
    using TLSharp.Core;

    public class TelegramClientWrapper
    {
        private readonly int _apiId;
        private readonly string _apiHash;
        private TelegramClient _client;
        private TLInputPeerUser _participant;

        public TelegramClientWrapper(int apiId, string apiHash)
        {
            _apiId = apiId;
            _apiHash = apiHash;
        }

        public async Task SendMessage(string message)
        {
            await _client.SendMessageAsync(_participant, message);
        }

        public async Task<string> GetLastMessage()
        {
            var history = await _client.GetHistoryAsync(_participant, 0, Int32.MaxValue, 1);
            var messages = (TLMessagesSlice)history;
            var lastMessage = (TLMessage)messages.Messages.FirstOrDefault();
            if (lastMessage != null)
            {
                return lastMessage.Message;
            }
            return String.Empty;
        }

        public async Task PrepareConnection(string participantName, bool firstConnect = false, string phoneNumber = "", string code = "")
        {
            await AuthenticateUser();
            if (firstConnect)
            {
                AuthorizeClient(phoneNumber, code);
            }
            await InitializeParticipant(participantName);
        }

        private async Task AuthenticateUser()
        {
            _client = NewClient();
            await _client.ConnectAsync();
        }

        private async Task InitializeParticipant(string participantName)
        {
            var searchResult = await _client.SearchUserAsync(participantName);
            var participant = (TLUser)searchResult.Users.FirstOrDefault();

            if (participant == null)
            {
                throw new Exception($"Didn't find participant: {participantName}");
            }

            var id = participant.Id;
            var hash = participant.AccessHash.Value;

            _participant = new TeleSharp.TL.TLInputPeerUser() { UserId = id, AccessHash = hash };
        }

        private async void AuthorizeClient(string phoneNumber, string code)
        {
            var hash = await _client.SendCodeRequestAsync(phoneNumber);

            if (String.IsNullOrWhiteSpace(code))
            {
                throw new Exception("CodeToAuthenticate is empty, fill it with the code you just got now by SMS/Telegram");
            }

            await _client.MakeAuthAsync(phoneNumber, hash, code);
        }

        private TelegramClient NewClient()
        {
            try
            {
                return new TelegramClient(_apiId, _apiHash);
            }
            catch (MissingApiConfigurationException ex)
            {
                throw new Exception($"Please provide your API settings. (More info: {MissingApiConfigurationException.InfoUrl})", ex);
            }
        }
    }
}
