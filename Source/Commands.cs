using CounterStrikeSharp.API;
using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using CounterStrikeSharp.API.Modules.Cvars;
using PRTelegramBot.Models;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Extensions;

namespace Cs2Telegram
{
    public class Commands
    {
        [ReplyMenuHandler("start")]
        [SlashHandler("/start")]
        public static async Task Start(ITelegramBotClient botClient, Update update)
        {
            await Menu(botClient, update);
        }

        [ReplyMenuHandler("Menu", "Меню")]
        [SlashHandler("/Menu")]
        public static async Task Menu(ITelegramBotClient botClient, Update update)
        {
            var options = new OptionMessage();

            var menu = new List<string>();
            menu.Add("Status");
            menu.Add("Players");

            var generateMenu = MenuGenerator.ReplyKeyboard(1, menu);
            options.MenuReplyKeyboardMarkup = generateMenu;
            await PRTelegramBot.Helpers.Message.Send(botClient, update, "Server menu", options);
        }

        [ReplyMenuHandler("Status", "Статус")]
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

                    if(hostnameCvar != null)
                    {
                        hostname = hostnameCvar.StringValue;
                    }

                    var msg = $"🌐 Server '{hostname}':\n" +
                        $"👨‍👩‍👧‍👦 Players: {allCount}/{maxPlayers}\n" +
                        $"👦 Humans: {userCount}\n" +
                        $"🤖 Bots: {botCount}\n" +
                        $"🗺️ Map: {mapName}\n" +
                        $"🕛 Server Uptime: {TimeSpan.FromSeconds(serverTime).ToReadableString()}  ";

                    await PRTelegramBot.Helpers.Message.Send(botClient, update, msg);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
        }

        [ReplyMenuHandler("Players", "Игроки")]
        public static async Task Players(ITelegramBotClient botClient, Update update)
        {
            Server.NextFrame(async () =>
            {
                try
                {
                    var players = Utilities.GetPlayers();
                    string message = "Players on server:";
                    if(players.Count == 0)
                    {
                        await PRTelegramBot.Helpers.Message.Send(botClient, update, "No players :(");
                    }
                    else
                    {
                        foreach (var player in players)
                        {
                            message += $"\n {(player.IsBot ? "🤖" : "👦")} {player.PlayerName}";
                        }

                        await PRTelegramBot.Helpers.Message.Send(botClient, update, message);
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
