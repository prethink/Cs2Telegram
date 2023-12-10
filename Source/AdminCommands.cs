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

namespace Cs2Telegram
{
    public static class AdminCommands
    {
        public static async Task AccessDenied(ITelegramBotClient botClient, Update update)
        {
            string msg = "Access denied";
            Helper.SendMessage(botClient, update.GetChatId(), msg);
        }
    }
}
