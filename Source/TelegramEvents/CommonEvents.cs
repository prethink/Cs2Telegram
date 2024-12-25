using CounterStrikeSharp.API;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Cs2Telegram.TelegramEvents
{
    public static class CommonEvents
    {
        public static async Task AccessDenied(ITelegramBotClient botClient, Update update)
        {
            string msg = "Access denied";
            Helper.SendMessage(botClient, update.GetChatId(), msg);
        }

        public static async Task WrongMessage(PRTelegramBot.Models.EventsArgs.BotEventArgs arg)
        {
            string msg = "Wrong Message";
            Helper.SendMessage(arg.BotClient, arg.Update.GetChatId(), msg);
        }

        public static async Task OnMissingCommand(PRTelegramBot.Models.EventsArgs.BotEventArgs arg)
        {
            Server.NextFrame(async () =>
            {
                string msg = "Missing Command";
                var options = new OptionMessage();
                options.MenuReplyKeyboardMarkup = await arg.BotClient.GenerateCommonMenu(arg.Update.GetChatId());
                Helper.SendMessage(arg.BotClient, arg.Update.GetChatId(), msg, options);
            });
        }
    }
}
