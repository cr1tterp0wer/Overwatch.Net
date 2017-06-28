﻿using AngleSharp;
using AngleSharp.Dom;
using OverwatchAPI.Internal;
using OverwatchAPI.intrnl;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static OverwatchAPI.OverwatchAPIHelpers;

namespace OverwatchAPI
{
    public class OverwatchPlayer
    {
        /// <summary>
        /// Construct a new Overwatch player.
        /// </summary>
        /// <param name="username">The players Battletag (SomeUser#1234) or Username for PSN/XBL</param>
        /// <param name="platform">The players platform - Defaults to "none" if a battletag is not given (use DetectPlatform() if platform is not known)</param>
        /// <param name="region">The players region (only required for PC) - Defaults to "none" (use DetectRegionPC if region is not known)</param>
        public OverwatchPlayer(string username, Platform platform = Platform.none, Region region = Region.none)
        {
            Username = username;
            Platform = platform;
            browsingContext = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
            if (!IsValidBattletag(username) && platform == Platform.pc)
                throw new InvalidBattletagException();
            else if (IsValidBattletag(username))
            {
                Platform = Platform.pc;
                battletagUrlFriendly = username.Replace("#", "-");
                Region = region;
            }
            if(Region != Region.none && Platform != Platform.none)
            {
                ProfileURL = ProfileURL(Username, Region, Platform);
            }
        }

        /// <summary>
        /// The players Battletag with Discriminator - e.g. "SomeUser#1234"
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The PlayOverwatch profile of the player - This is only available if the user has set a region
        /// </summary>
        public string ProfileURL { get; private set; }

        /// <summary>
        /// The Player Level of the player
        /// </summary>
        public ushort PlayerLevel { get; private set; }

        /// <summary>
        /// The Competitive Rank of the player
        /// </summary>
        public ushort CompetitiveRank { get; private set; }
        
        /// <summary>
        ///   The Competitive Rank Image of the player
        /// </summary>
        public string CompetitiveRankImg { get; private set; }

        /// <summary>
        /// The player's region - EU/US/None
        /// </summary>
        public Region Region { get; private set; } 

        /// <summary>
        /// The player's platform - PC/XBL/PSN
        /// </summary>
        public Platform Platform { get; private set; }

        /// <summary>
        /// The players quick-play stats.
        /// </summary>
        public OverwatchStats CasualStats { get; private set; }

        /// <summary>
        /// The players competitive stats.
        /// </summary>
        public OverwatchStats CompetitiveStats { get; private set; }

        /// <summary>
        /// The players achievements.
        /// </summary>
        public OverwatchAchievements Achievements { get; private set; }

        /// <summary>
        /// The last time the profile was downloaded from PlayOverwatch.
        /// </summary>
        public DateTime ProfileLastDownloaded { get; private set; }

        /// <summary>
        /// A direct link to the users profile portrait.
        /// </summary>
        public string ProfilePortraitURL { get; private set; }

        private string battletagUrlFriendly;
        private IBrowsingContext browsingContext;
        private IDocument userPage;

        private async Task DetectRegion()
        {
            string baseUrl = "http://playoverwatch.com/en-gb/career/";
            string naUrl = $"{baseUrl}pc/us/{battletagUrlFriendly}";
            string euUrl = $"{baseUrl}pc/eu/{battletagUrlFriendly}";
            string krUrl = $"{baseUrl}pc/kr/{battletagUrlFriendly}";
            var naRslt = await browsingContext.OpenAsync(naUrl);
            if (naRslt.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                var euRslt = await browsingContext.OpenAsync(euUrl);
                if (euRslt.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var krRslt = await browsingContext.OpenAsync(krUrl);
                    if (krRslt.StatusCode != System.Net.HttpStatusCode.NotFound)
                    {
                        Region = Region.kr;
                        userPage = krRslt;
                        ProfileURL = krUrl;
                    }
                    else Region = Region.none;
                }
                else
                {
                    Region = Region.eu;
                    userPage = euRslt;
                    ProfileURL = euUrl;
                }
            }
            else
            {
                Region = Region.us;
                userPage = naRslt;
                ProfileURL = naUrl;
            }
        }   
        
        /// <summary>
        /// Detect the platform of the player [SLOW]
        /// </summary>
        /// <returns></returns>
        private async Task DetectPlatform()
        {
            if (IsValidBattletag(Username))
            {
                Platform = Platform.pc;
                return;
            }
            string baseUrl = "http://playoverwatch.com/en-gb/career/";
            string psnAppend = $"psn/{Username}";
            string xblAppend = $"xbl/{Username}";
            using (HttpClient _client = new HttpClient())
            {
                _client.BaseAddress = new Uri(baseUrl);
                using (var responsePsn = await _client.GetAsync(psnAppend))
                {
                    if (responsePsn.IsSuccessStatusCode)
                    {
                        Platform = Platform.psn;
                        ProfileURL = baseUrl + psnAppend;
                        return;
                    }
                    else
                    {
                        using (var responseXbl = await _client.GetAsync(xblAppend))
                        {
                            if (responseXbl.IsSuccessStatusCode)
                            {
                                Platform = Platform.xbl;
                                ProfileURL = baseUrl + xblAppend;
                                return;
                            }
                        }
                    }
                }
            }
            Platform = Platform.none;
        }

        /// <summary>
        /// Downloads and parses the players profile
        /// </summary>
        /// <param name="useAutoDetect">Attempt to auto-detect Region/Platform</param>
        /// <returns></returns>
        public async Task UpdateStats(bool useAutoDetect = true)
        {
            if(!useAutoDetect)
            {
                if (Region == Region.none && Platform == Platform.pc)
                    throw new UserRegionNotDefinedException();
                if (Platform == Platform.none)
                    throw new UserPlatformNotDefinedException();
            }
            else
            {
                if(IsValidBattletag(Username))
                {
                    Platform = Platform.pc;
                    await DetectRegion();
                    ParseUserPage();
                }
            }       
        }

        private void ParseUserPage()
        {
            GetUserRanks();
            GetProfilePortrait();
            CasualStats = new OverwatchStats();
            CompetitiveStats = new OverwatchStats();
            Achievements = new OverwatchAchievements();
            Achievements.UpdateAchievementsFromPage(userPage);
            CasualStats.UpdateStatsFromPage(userPage, Mode.Casual);
            CompetitiveStats.UpdateStatsFromPage(userPage, Mode.Competitive);
            if (CompetitiveStats.Count == 0) CompetitiveStats = null;
            ProfileLastDownloaded = DateTime.UtcNow;
        }

        internal void GetUserRanks()
        {
            ushort parsedPlayerLevel = 0;
            PlayerLevel = 0;
            ushort parsedCompetitiveRank = 0;
            CompetitiveRank = 0;
            if (ushort.TryParse(userPage.QuerySelector("div.player-level div")?.TextContent, out parsedPlayerLevel))
                PlayerLevel = parsedPlayerLevel;
            string playerLevelImageId = StaticVars.playerRankImageRegex.Match(userPage.QuerySelector("div.player-level")?.GetAttribute("style")).Value;
            PlayerLevel += StaticVars.prestigeDefinitions[playerLevelImageId];
            if (ushort.TryParse(userPage.QuerySelector("div.competitive-rank div")?.TextContent, out parsedCompetitiveRank))
                CompetitiveRank = parsedCompetitiveRank;
            var compImg = userPage.QuerySelector("div.competitive-rank img")?.OuterHtml;
            if (!string.IsNullOrEmpty(compImg))
                CompetitiveRankImg = compImg.Replace("<img src=\"", "").Replace("\">", "");
        }

        private void GetProfilePortrait()
        {
            ProfilePortraitURL = userPage.QuerySelector(".player-portrait").GetAttribute("src");
        }
    }
}
