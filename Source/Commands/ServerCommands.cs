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
using static System.Runtime.InteropServices.JavaScript.JSType;
using CounterStrikeSharp.API;

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
                await AdminCommands.AccessDenied(botClient, update);
                return;
            }

            string msg = SERVER_COMMAND_MSG;
            update.RegisterNextStep(new StepTelegram(ExecuteCommandInServerHandler, null));
            var options = new OptionMessage();
            options.MenuReplyKeyboardMarkup = botClient.GenerateOnlyMenu(update.GetChatId());
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
                Helper.SendMessage(botClient, update, $"🗯️ Server try execute command: {msg}\n\n{SERVER_COMMAND_MSG}");
            }
            else
            {
                Helper.SendMessage(botClient, update, $"Error empty message, try again");
            }

        }

        [ReplyMenuHandler(Constants.SERVER_SEND_MESSAGE_BUTTON)]
        public static async Task SendMessageToServer(ITelegramBotClient botClient, Update update)
        {
            if (!botClient.IsAdmin(update.GetChatId()))
            {
                await AdminCommands.AccessDenied(botClient, update);
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
                Helper.SendMessage(botClient, update, $"💬 Server send message: {msg}\n\n{SERVER_SEND_MSG}");
            }
            else
            {
                Helper.SendMessage(botClient, update, $"Error empty message, try again");
            }
        }
    }
}
