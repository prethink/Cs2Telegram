using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;

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
            var options = new OptionMessage();
            options.MenuReplyKeyboardMarkup = botClient.GenerateCommonMenu(update.GetChatId());
            Helper.SendMessage(botClient, update.GetChatId(), msg, options);
        }
    }
}
