﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Modules.Cvars;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Cs2Telegram.Commands
{
    public static class CommonCommands
    {
        [ReplyMenuHandler("start")]
        [SlashHandler("/start")]
        public static async Task Start(ITelegramBotClient botClient, Update update)
        {
            await Menu(botClient, update);
        }

        [ReplyMenuHandler(Constants.MAIN_MENU_BUTTON)]
        [SlashHandler(Constants.SLASH_MENU_BUTTON)]
        public static async Task Menu(ITelegramBotClient botClient, Update update)
        {

            Server.NextFrame(async () =>
            {
                var options = new OptionMessage();
                options.MenuReplyKeyboardMarkup = await botClient.GenerateCommonMenu(update.GetChatId());
                await PRTelegramBot.Helpers.Message.Send(botClient, update, "Main menu", options);
            });
        }

        [ReplyMenuHandler(Constants.STATUS_BUTTON)]
        public static async Task Status(ITelegramBotClient botClient, Update update)
        {
            Server.NextFrame(async () =>
            {
                try
                {
                    var botCount = Utilities.GetPlayers().Count(x => x.IsBot);
                    var userCount = Utilities.GetPlayers().Count(x => !x.IsBot);
                    var allCount = botCount + userCount;
                    var maxPlayers = Server.MaxPlayers;
                    string mapName = Server.MapName;

                    var serverTime = Server.EngineTime;

                    string hostname = "";

                    var hostnameCvar = ConVar.Find("hostname");
                    var gameTypeCvar = ConVar.Find("game_type");
                    var gameModeCvar = ConVar.Find("game_mode");
                    string gametype = Helper.GetGameModeWithGameType(gameTypeCvar.GetPrimitiveValue<int>(), gameModeCvar.GetPrimitiveValue<int>());
                    if (hostnameCvar != null)
                    {
                        hostname = hostnameCvar.StringValue;
                    }

                    var msg = $"🌐 Server '{hostname}':\n" +
                        $"🕹️ Game mode: {gametype}\n" +
                        $"👨‍👩‍👧‍👦 Players: {allCount}/{maxPlayers}\n" +
                        $"👦 Humans: {userCount}\n" +
                        $"🤖 Bots: {botCount}\n" +
                        $"🗺️ Map: {mapName}\n" +
                        $"🕛 Server Uptime: {TimeSpan.FromSeconds(serverTime).ToReadableString()}  ";

                    var options = new OptionMessage();
                    options.MenuReplyKeyboardMarkup = await botClient.GenerateCommonMenu(update.GetChatId());

                    await PRTelegramBot.Helpers.Message.Send(botClient, update, msg, options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

        [ReplyMenuHandler(Constants.PLAYERS_BUTTON)]
        public static async Task Players(ITelegramBotClient botClient, Update update)
        {
            Server.NextFrame(async () =>
            {
                try
                {
                    var players = Utilities.GetPlayers();
                    string message = "Players on server:";
                    var options = new OptionMessage();
                    options.MenuReplyKeyboardMarkup = await botClient.GenerateCommonMenu(update.GetChatId());
                    if (players.Count == 0)
                    {
                        await PRTelegramBot.Helpers.Message.Send(botClient, update, "No players :(", options);
                    }
                    else
                    {
                        foreach (var player in players)
                        {
                            message += $"\n {(player.IsBot ? "🤖" : "👦")} {WebUtility.HtmlEncode(player.PlayerName)}";
                        }

                        await PRTelegramBot.Helpers.Message.Send(botClient, update, message, options);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }
    }
}
