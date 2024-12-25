using CounterStrikeSharp.API;
using Microsoft.Extensions.Logging;
using PRTelegramBot.Core;
using PRTelegramBot.Extensions;
using PRTelegramBot.Interfaces;
using PRTelegramBot.Models;
using PRTelegramBot.Models.Enums;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models.TCommands;
using PRTelegramBot.Utils;
using Telegram.Bot.Types;

namespace Cs2Telegram.InlineInstance
{
    internal class ChangeLevel : ICallbackQueryCommandHandler
    {
        public async Task<UpdateResult> Handle(PRBotBase bot, Update update, CallbackQuery updateType)
        {
            var levelCommand = InlineCallback<StringTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
            if(levelCommand != null)
            {
                string serverCommand = $"changelevel {levelCommand.Data.StrData}";
                Server.NextFrame(() =>
                {
                    Server.ExecuteCommand(serverCommand);
                });

                Cs2TelegramPlugin.Instance.Logger.LogInformation($"TelegramUser [{update.GetInfoUser()}] executed command on server '{serverCommand}'");
                var inlineMenuItems = Helper.GetServerCommandsItems();
                var options = new OptionMessage();
                if (inlineMenuItems.Count > 0)
                {
                    options.MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(MenuGenerator.InlineButtons(1, inlineMenuItems));
                }
                Helper.EditMessage(bot.botClient, update, $"🗯️ Server try execute command: {serverCommand}\n\n{serverCommand}\nGuid:{Guid.NewGuid()}", options);
                return UpdateResult.Handled;
            }
            return UpdateResult.Continue;
        }
    }
}
