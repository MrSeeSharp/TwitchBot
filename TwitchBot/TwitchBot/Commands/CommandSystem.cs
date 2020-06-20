﻿using System;
using System.Threading.Tasks;

using TwitchBot.Commands.Features;
using TwitchBot.Configuration;
using TwitchBot.Libraries;
using TwitchBot.Models;
using TwitchBot.Services;
using TwitchBot.Threads;

namespace TwitchBot.Commands
{
    /// <summary>
    /// The "Facade" class for the command system
    /// </summary>
    public class CommandSystem
    {
        private readonly BankFeature _bank;
        private readonly TwitterFeature _twitter;
        private readonly SongRequestFeature _songRequestFeature;
        private readonly LibVLCSharpPlayerFeature _libVLCSharpPlayerFeature;
        private readonly TwitchChannelFeature _twitchChannelFeature;
        private readonly FollowerFeature _followerFeature;
        private readonly GeneralFeature _generalFeature;
        private readonly InGameNameFeature _inGameNameFeature;
        private readonly ReminderFeature _reminderFeature;
        private readonly SpotifyFeature _spotifyFeature;
        private readonly ErrorHandler _errHndlrInstance = ErrorHandler.Instance;

        public CommandSystem(IrcClient irc, TwitchBotConfigurationSection botConfig, bool hasTwitterInfo, System.Configuration.Configuration appConfig, 
            BankService bank, SongRequestBlacklistService songRequestBlacklist, LibVLCSharpPlayer libVLCSharpPlayer, SongRequestSettingService songRequestSetting,
            SpotifyWebClient spotify, TwitchInfoService twitchInfo, FollowerService follower, GameDirectoryService gameDirectory, InGameUsernameService ign)
        {
            _bank = new BankFeature(irc, botConfig, bank);
            _twitter = new TwitterFeature(irc, botConfig, appConfig, hasTwitterInfo);
            _songRequestFeature = new SongRequestFeature(irc, botConfig, appConfig, songRequestBlacklist, libVLCSharpPlayer, songRequestSetting);
            _libVLCSharpPlayerFeature = new LibVLCSharpPlayerFeature(irc, botConfig, appConfig, libVLCSharpPlayer);
            _twitchChannelFeature = new TwitchChannelFeature(irc, botConfig);
            _followerFeature = new FollowerFeature(irc, botConfig, twitchInfo, follower, appConfig);
            _generalFeature = new GeneralFeature(irc, botConfig, twitchInfo);
            _inGameNameFeature = new InGameNameFeature(irc, botConfig, twitchInfo, gameDirectory, ign);
            _reminderFeature = new ReminderFeature(irc, botConfig, twitchInfo, gameDirectory);
            _spotifyFeature = new SpotifyFeature(irc, botConfig, spotify);
        }

        public async Task ExecRequest(TwitchChatter chatter)
        {
            try
            {
                if (_bank.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_twitter.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_songRequestFeature.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_libVLCSharpPlayerFeature.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_twitchChannelFeature.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_followerFeature.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_generalFeature.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_inGameNameFeature.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_reminderFeature.IsRequestExecuted(chatter))
                {
                    return;
                }
                else if (_spotifyFeature.IsRequestExecuted(chatter))
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                await _errHndlrInstance.LogError(ex, "CommandSystem", "ExecRequest(TwitchChatter)", false, "N/A", chatter.Message);
            }
        }
    }
}
