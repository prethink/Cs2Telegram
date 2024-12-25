using CounterStrikeSharp.API;
using Cs2Telegram.TelegramEvents;
using PRTelegramBot.Attributes;
using PRTelegramBot.Extensions;
using PRTelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Cs2Telegram.Commands
{
    public static class AdminCommands
    {
        [ReplyMenuHandler(Constants.ADMIN_MENU_BUTTON)]
        public static async Task AdminMenu(ITelegramBotClient botClient, Update update)
        {
            if (!(await botClient.IsAdmin(update.GetChatId())))
            {
                await CommonEvents.AccessDenied(botClient, update);
                return;
            }

            Server.NextFrame(async () =>
            {
                var options = new OptionMessage();
                options.MenuReplyKeyboardMarkup = await botClient.GenerateAdminMenu(update.GetChatId());
                await PRTelegramBot.Helpers.Message.Send(botClient, update, "Admin menu", options);
            });

        }
    }
}
