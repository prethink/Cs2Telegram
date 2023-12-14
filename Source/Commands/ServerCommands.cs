using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using PRTelegramBot.Attributes;
using PRTelegramBot.Models;
using PRTelegramBot.Extensions;
using File = System.IO.File;
using CounterStrikeSharp.API;
using Cs2Telegram.Models;
using CounterStrikeSharp.API.Core;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models.CallbackCommands;
using PRTelegramBot.Models.Interface;
using PRTelegramBot.Helpers.TG;
using Cs2Telegram.Enums;
using Cs2Telegram.TelegramEvents;
using Microsoft.Extensions.Logging;

namespace Cs2Telegram.Commands
{
    public static class ServerCommands
    {
        const string SERVER_BACK_TO_MENU        = $"\n\nIf you want to access the menu, use the command {Constants.SLASH_MENU_BUTTON}";
        const string SERVER_SEND_MSG            = $"ℹ️ Write the message you want to send to the server. The message will be displayed to all players in the in-game chat.{SERVER_BACK_TO_MENU}";
        const string SERVER_COMMAND_MSG         = $"ℹ️ Write the command you want to execute on the server side.{SERVER_BACK_TO_MENU}";

        [ReplyMenuHandler(Constants.SERVER_COMMAND_BUTTON)]
        public static async Task ExecuteCommandInServer(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await CommonEvents.AccessDenied(botClient, update);
                return;
            }

            var inlineMenuItems = Helper.GetServerCommandsItems();
            var options = new OptionMessage();
            if(inlineMenuItems.Count > 0)
            {
                options.MenuInlineKeyboardMarkup = MenuGenerator.InlineKeyboard(MenuGenerator.InlineButtons(1, inlineMenuItems));
            }
            else
            {
                options.MenuReplyKeyboardMarkup = botClient.GenerateOnlyMenu(update.GetChatId());
            }


            string msg = SERVER_COMMAND_MSG;
            update.RegisterNextStep(new StepTelegram(ExecuteCommandInServerHandler, null));
            Helper.SendMessage(botClient, update, msg, options);
        }

        [RequiredTypeChat(Telegram.Bot.Types.Enums.ChatType.Private)]
        [RequireTypeMessage(Telegram.Bot.Types.Enums.MessageType.Text)]
        public static async Task ExecuteCommandInServerHandler(ITelegramBotClient botClient, Update update, CustomParameters args)
        {
            string msg = update?.Message?.Text;
            if(!string.IsNullOrWhiteSpace(msg))
            {
                Server.NextFrame(() =>
                {
                    Server.ExecuteCommand(msg);
                });
                Cs2TelegramPlugin.Instance.Logger.LogInformation($"TelegramUser [{update.GetInfoUser()}] executed command on server '{msg}'");
                Helper.SendMessage(botClient, update, $"🗯️ Server try execute command: {msg}\n\n{SERVER_BACK_TO_MENU}");
            }
            else
            {
                Helper.SendMessage(botClient, update, $"Error empty message, try again");
            }
        }

        [InlineCallbackHandler<HeaderCommand>(HeaderCommand.ExecuteServerCommand)]
        public static async Task ExecuteCommandInServerHandler(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await CommonEvents.AccessDenied(botClient, update);
                return;
            }

            var command = InlineCallback<ServerExecuteTCommand>.GetCommandByCallbackOrNull(update.CallbackQuery.Data);
            if (command != null)
            {
                if (!string.IsNullOrWhiteSpace(command.Data.Command))
                {
                    string serverCommand = command.Data.Command;
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
                    Helper.EditMessage(botClient, update, $"🗯️ Server try execute command: {serverCommand}\n\n{SERVER_COMMAND_MSG}\nGuid:{Guid.NewGuid()}", options);
                }
                else
                {
                    Helper.EditMessage(botClient, update, $"Error empty message, try again");
                }
            }
        }

        [ReplyMenuHandler(Constants.SERVER_SEND_MESSAGE_BUTTON)]
        public static async Task SendMessageToServer(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await CommonEvents.AccessDenied(botClient, update);
                return;
            }

            string msg = SERVER_SEND_MSG;
            update.RegisterNextStep(new StepTelegram(SendMessageToServerHandler, null));
            var options = new OptionMessage();
            options.MenuReplyKeyboardMarkup = botClient.GenerateOnlyMenu(update.GetChatId());
            Helper.SendMessage(botClient, update, msg, options);
        }

        [RequiredTypeChat(Telegram.Bot.Types.Enums.ChatType.Private)]
        [RequireTypeMessage(Telegram.Bot.Types.Enums.MessageType.Text)]
        public static async Task SendMessageToServerHandler(ITelegramBotClient botClient, Update update, CustomParameters args)
        {
            string msg = update?.Message?.Text;
            if (!string.IsNullOrWhiteSpace(msg))
            {
                Server.NextFrame(() =>
                {
                    Server.PrintToChatAll(msg);
                });
                Cs2TelegramPlugin.Instance.Logger.LogInformation($"TelegramUser [{update.GetInfoUser()}] sended message on server '{msg}'");
                Helper.SendMessage(botClient, update, $"💬 Server send message: {msg}\n\n{SERVER_SEND_MSG}");
            }
            else
            {
                Helper.SendMessage(botClient, update, $"Error empty message, try again");
            }
        }
    }
}
