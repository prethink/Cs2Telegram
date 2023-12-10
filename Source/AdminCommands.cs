using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using PRTelegramBot.Extensions;

namespace Cs2Telegram
{
    public static class AdminCommands
    {
        public static async Task AccessDenied(ITelegramBotClient botClient, Update update)
        {
            string msg = "Access denied";
            await Helper.SendMessage(botClient, update.GetChatId(), msg);
        }
    }
}
