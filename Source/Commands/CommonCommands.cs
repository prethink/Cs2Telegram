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
using Cs2Telegram.Models;
using PRTelegramBot.Models.Interface;
using PRTelegramBot.Models.InlineButtons;

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
                options.MenuReplyKeyboardMarkup = botClient.GenerateCommonMenu(update.GetChatId());
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
                    options.MenuReplyKeyboardMarkup = botClient.GenerateCommonMenu(update.GetChatId());

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
                    options.MenuReplyKeyboardMarkup = botClient.GenerateCommonMenu(update.GetChatId());
                    if (players.Count == 0)
                    {
                        await PRTelegramBot.Helpers.Message.Send(botClient, update, "No players :(", options);
                    }
                    else
                    {
                        foreach (var player in players)
                        {
                            message += $"\n {(player.IsBot ? "🤖" : "👦")} {player.PlayerName}";
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

        public static async Task CustomMenu(ITelegramBotClient botClient, Update update)
        {
            CustomMenu custom = Cs2TelegramPlugin.Instance.Config.CustomMenu;
            string message = custom.Message;
            var option = new OptionMessage();
            if(custom.WebMenuItems.Count > 0)
            {
                var inlineMenu = new List<IInlineContent>();
                foreach (var item in custom.WebMenuItems)
                {
                    inlineMenu.Add(new InlineURL(item.Name, item.Url));
                }

                var menuItems = MenuGenerator.InlineButtons(custom.Column > 0 ? custom.Column : 1, inlineMenu);
                var menu = MenuGenerator.InlineKeyboard(menuItems);
                option.MenuInlineKeyboardMarkup = menu;
            }
            Helper.SendMessage(botClient, update, message, option);
        }
    }
}
