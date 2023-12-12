﻿using CounterStrikeSharp.API;
using Cs2Telegram.Enums;
using Cs2Telegram.Models;
using Cs2Telegram.TelegramEvents;
using PRTelegramBot.Attributes;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models.Interface;
using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models.CallbackCommands;
using CounterStrikeSharp.API.Modules.Utils;

namespace Cs2Telegram.Commands
{
    public static class PlayerCommands
    {
        const string PLAYER_NOT_VALID = "Player not found or player is no valid";

        [ReplyMenuHandler(Constants.SERVER_PLAYERS_INFO_BUTTON)]
        [InlineCallbackHandler<HeaderCommand>(HeaderCommand.PlayerInfoList)]
        public static async Task PlayersInfo(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await CommonEvents.AccessDenied(botClient, update);
                return;
            }

            var inlineMenuItems = new List<IInlineContent>();
            Server.NextFrame(() =>
            {
                var players = Utilities.GetPlayers();
                foreach (var player in players)
                {
                    var commandItem = new InlineCallback<EntityTCommand<int>>($"{player.PlayerName}{(player.IsBot ? " [Bot]" : "")}", HeaderCommand.PlayerInfo, new EntityTCommand<int>(player.UserId != null ? player.UserId.Value : -1));
                    inlineMenuItems.Add(commandItem);
                }

                var options = new OptionMessage();
                string msg = "Players not found";
                if (inlineMenuItems.Count > 0)
                {
                    msg = "Players on server:";
                    options.MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(MenuGenerator.InlineButtons(1, inlineMenuItems));
                }
                else
                {
                    options.MenuReplyKeyboardMarkup = botClient.GenerateOnlyMenu(update.GetChatId());
                }
                var command = InlineCallback<EntityTCommand<int>>.GetCommandByCallbackOrNull(update.CallbackQuery?.Data ?? "");
                if(command != null)
                {
                    Helper.EditMessage(botClient, update, msg, options);
                }
                else
                {
                    Helper.SendMessage(botClient, update, msg, options);
                }

            });
        }

        [InlineCallbackHandler<HeaderCommand>(HeaderCommand.PlayerInfo)]
        public static async Task PlayerInfoHandler(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await CommonEvents.AccessDenied(botClient, update);
                return;
            }

            var command = InlineCallback<EntityTCommand<int>>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
            if (command != null && command.Data.EntityId != -1)
            {
                Server.NextFrame(() =>
                {
                    int userId = command.Data.EntityId;
                    string msg = PLAYER_NOT_VALID;
                    var options = new OptionMessage();
                    var player = Utilities.GetPlayerFromUserid(userId);
                    if (player != null && player.IsValid)
                    {
                        msg = 
                        $"Player name:{player.PlayerName}\n" +
                        $"Clan name: {player.ClanName}\n" +
                        $"Score: {player.Score}\n" +
                        $"IP: {player.IpAddress ?? ""}\n" +
                        //$"Time: {TimeSpan.FromSeconds(player.).ToReadableString()}\n" +
                        $"Ping: {player.Ping}\n" +
                        $"SteamId: {player.AuthorizedSteamID?.ToString()}\n";

                        //var changeTeamTButton = new InlineCallback<ChangeTeamPlayerCommand>($"Team T", HeaderCommand.PlayerChangeTeam, new ChangeTeamPlayerCommand(player.UserId != null ? player.UserId.Value : -1, CsTeam.Terrorist));
                        //var changeTeamCTButton = new InlineCallback<ChangeTeamPlayerCommand>($"Team CT", HeaderCommand.PlayerChangeTeam, new ChangeTeamPlayerCommand(player.UserId != null ? player.UserId.Value : -1, CsTeam.CounterTerrorist));
                        //var changeTeamSpectatorButton = new InlineCallback<ChangeTeamPlayerCommand>($"Team Spectator", HeaderCommand.PlayerChangeTeam, new ChangeTeamPlayerCommand(player.UserId != null ? player.UserId.Value : -1, CsTeam.Spectator));
                        var kickCommandItem = new InlineCallback<EntityTCommand<int>>($"Kick", HeaderCommand.PlayerKick, new EntityTCommand<int>(player.UserId != null ? player.UserId.Value : -1));
                        var BackCommandItem = new InlineCallback<EntityTCommand<int>>($"Back", HeaderCommand.PlayerInfoList, new EntityTCommand<int>(-1));
                        //var killCommandItem = new InlineCallback<EntityTCommand<int>>($"Kill", HeaderCommand.PlayerKill, new EntityTCommand<int>(player.UserId != null ? player.UserId.Value : -1));
                        var menuTeamButtons = MenuGenerator.InlineButtons(2,new List<IInlineContent> { kickCommandItem, BackCommandItem });
                        var menu = MenuGenerator.InlineKeyboard(menuTeamButtons);
                        options.MenuInlineKeyboardMarkup = menu;

                    }
 
                    Helper.EditMessage(botClient, update, msg, options);
                });
            }
        }

        [InlineCallbackHandler<HeaderCommand>(HeaderCommand.PlayerKick)]
        public static async Task PlayerKickHandler(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await CommonEvents.AccessDenied(botClient, update);
                return;
            }

            var command = InlineCallback<EntityTCommand<int>>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
            if (command != null && command.Data.EntityId != -1)
            {
                Server.NextFrame(() =>
                {
                    int userId = command.Data.EntityId;
                    string msg = PLAYER_NOT_VALID;
                    var options = new OptionMessage();
                    var player = Utilities.GetPlayerFromUserid(userId);
                    if (player != null && player.IsValid)
                    {
                        msg = $"Server kicked {player.PlayerName}";
                        Server.ExecuteCommand($"kickid {player.UserId}");
                    }

                    Helper.EditMessage(botClient, update, msg, options);
                });
            }
        }

        //[InlineCallbackHandler<HeaderCommand>(HeaderCommand.PlayerKill)]
        //public static async Task PlayerKillHandler(ITelegramBotClient botClient, Update update)
        //{
        //    if (!botClient.IsAdmin(update.GetChatId()))
        //    {
        //        await CommonEvents.AccessDenied(botClient, update);
        //        return;
        //    }

        //    var command = InlineCallback<EntityTCommand<int>>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
        //    if (command != null && command.Data.EntityId != -1)
        //    {
        //        Server.NextFrame(() =>
        //        {
        //            int userId = command.Data.EntityId;
        //            string msg = PLAYER_NOT_VALID;
        //            var options = new OptionMessage();
        //            var player = Utilities.GetPlayerFromUserid(userId);
        //            if (player != null && player.IsValid)
        //            {
        //                if(player.PawnIsAlive)
        //                {
        //                    msg = $"Player {player.PlayerName} kill";
        //                    player.CommitSuicide(false, true);
        //                }
        //                else
        //                {
        //                    msg = $"Player {player.PlayerName} already dead";
        //                }
        //            }

        //            Helper.EditMessage(botClient, update, msg, options);
        //        });
        //    }
        //}


        //[InlineCallbackHandler<HeaderCommand>(HeaderCommand.PlayerChangeTeam)]
        //public static async Task PlayerChangeTeamHandler(ITelegramBotClient botClient, Update update)
        //{
        //    if (!botClient.IsAdmin(update.GetChatId()))
        //    {
        //        await CommonEvents.AccessDenied(botClient, update);
        //        return;
        //    }

        //    var command = InlineCallback<ChangeTeamPlayerCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
        //    if (command != null && command.Data.UserId != -1)
        //    {
        //        Server.NextFrame(() =>
        //        {
        //            int userId = command.Data.UserId;
        //            string msg = PLAYER_NOT_VALID;
        //            var options = new OptionMessage();
        //            var player = Utilities.GetPlayerFromUserid(userId);
        //            if (player != null && player.IsValid)
        //            {
        //                msg = "ChangeTeam";
        //                player.ChangeTeam(CsTeam.CounterTerrorist);
        //            }

        //            Helper.SendMessage(botClient, update, msg, options);
        //        });
        //    }
        //}
    }
}
