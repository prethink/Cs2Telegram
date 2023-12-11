using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using PRTelegramBot.Extensions;

namespace Cs2Telegram.TelegramEvents
{
    public static class CommonEvents
    {
        public static async Task WrongMessage(ITelegramBotClient botClient, Update update)
        {
            string msg = "Wrong Message";
            Helper.SendMessage(botClient, update.GetChatId(), msg);
        }

        public static async Task OnMissingCommand(ITelegramBotClient botClient, Update update)
        {
            string msg = "Missing Command";
            Helper.SendMessage(botClient, update.GetChatId(), msg);
        }
    }
}
