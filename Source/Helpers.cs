using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using PRTelegramBot.Extensions;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Cs2Telegram
{
    public static class Helper
    {
        public static void SendMessage(ITelegramBotClient botClient, Update update, string msg, OptionMessage options = null)
        {
            SendMessage(botClient, update.GetChatId(),msg, options);
        }

        public static void SendMessage(ITelegramBotClient botClient,long userId, string msg, OptionMessage options = null)
        {
            Task task = Task.Run(async () =>
            {
                try
                {
                    var sendMessage = await PRTelegramBot.Helpers.Message.Send(botClient, userId, msg, options);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });
        }

        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? string.Empty : "s") : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

            if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

            if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

            return formatted;
        }

        public static ReplyKeyboardMarkup GenerateOnlyMenu(this ITelegramBotClient botClient, long userId)
        {
            var menu = new List<string>();
            if (botClient.IsAdmin(userId))
            {
                menu.Add(Constants.ADMIN_MENU_BUTTON);
            }
            return MenuGenerator.ReplyKeyboard(1, menu, true, Constants.MAIN_MENU_BUTTON);
        }


        public static ReplyKeyboardMarkup GenerateCommonMenu(this ITelegramBotClient botClient, long userId)
        {
            int playersCount = Utilities.GetPlayers().Count;
            var menu = new List<string>();
            menu.Add(Constants.STATUS_BUTTON);
            menu.Add($"{Constants.PLAYERS_BUTTON} ({playersCount})");
            if (botClient.IsAdmin(userId))
            {
                menu.Add(Constants.ADMIN_MENU_BUTTON);
            }

            return MenuGenerator.ReplyKeyboard(1, menu, true, Constants.MAIN_MENU_BUTTON);
        }

        public static ReplyKeyboardMarkup GenerateAdminMenu(this ITelegramBotClient botClient, long userId)
        {
            var menu = new List<string>();

            if (botClient.IsAdmin(userId))
            {
                menu.Add(Constants.SERVER_COMMAND_BUTTON);
                menu.Add(Constants.SERVER_SEND_MESSAGE_BUTTON);
            }
            else
            {
                menu.Add(Constants.ACCESS_DENIED_BUTTON);
            }

            return MenuGenerator.ReplyKeyboard(1, menu, true, Constants.MAIN_MENU_BUTTON);
        }
    }
}
