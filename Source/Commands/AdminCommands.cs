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
using CounterStrikeSharp.API.Modules.Memory;

namespace Cs2Telegram.Commands
{
    public static class AdminCommands
    {
        public static async Task AccessDenied(ITelegramBotClient botClient, Update update)
        {
            string msg = "Access denied";
            Helper.SendMessage(botClient, update.GetChatId(), msg);
        }

        [ReplyMenuHandler(Constants.ADMIN_MENU_BUTTON)]
        public static async Task AdminMenu(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await AccessDenied(botClient, update);
                return;
            }

            Server.NextFrame(async () =>
            {
                var options = new OptionMessage();
                options.MenuReplyKeyboardMarkup = botClient.GenerateAdminMenu(update.GetChatId());
                await PRTelegramBot.Helpers.Message.Send(botClient, update, "Admin menu", options);
                update.ClearStepUser();
            });

        }
    }
}
