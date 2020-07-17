﻿using System.Collections.Generic;
using System.Threading.Tasks;

using TwitchBotDb.Models;

using TwitchBotUtil.Libraries;

namespace TwitchBotCore.Repositories
{
    public class QuoteRepository
    {
        private readonly string _twitchBotApiLink;

        public QuoteRepository(string twitchBotApiLink)
        {
            _twitchBotApiLink = twitchBotApiLink;
        }

        public async Task<List<Quote>> GetQuotes(int broadcasterId)
        {
            return await ApiBotRequest.GetExecuteAsync<List<Quote>>(_twitchBotApiLink + $"quotes/get/{broadcasterId}");
        }

        public async Task AddQuote(string quote, string username, int broadcasterId)
        {
            Quote freshQuote = new Quote
            {
                Username = username,
                UserQuote = quote,
                BroadcasterId = broadcasterId
            };

            await ApiBotRequest.PostExecuteAsync(_twitchBotApiLink + $"quotes/create", freshQuote);
        }
    }
}
