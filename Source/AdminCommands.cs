using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using PRTelegramBot.Extensions;
using PRTelegramBot.Attributes;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Interface;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Helpers;
using Cs2Telegram.Models;

namespace Cs2Telegram
{
    public static class AdminCommands
    {
        public static async Task AccessDenied(ITelegramBotClient botClient, Update update)
        {
            string msg = "Access denied";
            Helper.SendMessage(botClient, update.GetChatId(), msg);
        }

        [ReplyMenuHandler("MapList")]
        public static async Task MapList(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await AccessDenied(botClient, update);
                return;
            }


            Server.NextFrame(() =>
            {
                try
                {
                    Console.WriteLine(NativeAPI.GetGameDirectory());
                    var maps = Server.GetMapList();
                    GenerateMapList(botClient, update, maps);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Helper.SendMessage(botClient, update.GetChatId(), $"Error when getting a list of maps: {ex}");
                }
            });
        }

        public static async Task GenerateMapList(ITelegramBotClient botClient, Update update, string[] maps)
        {
            string message = "";
            var options = new OptionMessage();

            if (maps.Length > 0)
            {
                message += "Server maps:\n\n";
                List<IInlineContent> buttons = new();
                foreach (var map in maps)
                {
                    var menuItem = new InlineCallback<MapTCommand>(map, HeadersCommads.SelectMap, new MapTCommand(map));
                    buttons.Add(menuItem);

                }
                var menu = MenuGenerator.InlineKeyboard(1, buttons);
                options.MenuInlineKeyboardMarkup = menu;
            }
            else
            {
                message = "No maps :(";
            }

            Helper.SendMessage(botClient, update.GetChatId(), message, options);
        }
    }
}
