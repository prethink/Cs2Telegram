using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using Cs2Telegram.Enums;
using Cs2Telegram.Models;
using Microsoft.Extensions.Logging;
using PRTelegramBot.Extensions;
using PRTelegramBot.Helpers.TG;
using PRTelegramBot.Models;
using PRTelegramBot.Models.InlineButtons;
using PRTelegramBot.Models.Interface;
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
        public static void SendAdminsMessage(ITelegramBotClient botClient, string message, OptionMessage options = null)
        {
            var admins = Cs2TelegramPlugin.Instance.Config.Admins;
            Task task = Task.Run(async () =>
            {
                try
                {
                    foreach (var item in admins)
                    {
                        await PRTelegramBot.Helpers.Message.Send(botClient, item, message, options);
                    }
                }
                catch (Exception ex)
                {
                    Cs2TelegramPlugin.Instance.Logger.LogError(ex.ToString());
                }
            });
        }

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
                    Cs2TelegramPlugin.Instance.Logger.LogError(ex.ToString());
                }
            });
        }

        public static void EditMessage(ITelegramBotClient botClient, Update update, string msg, OptionMessage options = null)
        {
            Task task = Task.Run(async () =>
            {
                try
                {
                    var editMessage = await PRTelegramBot.Helpers.Message.Edit(botClient, update, msg, options);
                }
                catch (Exception ex)
                {
                    Cs2TelegramPlugin.Instance.Logger.LogError(ex.ToString());
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
            if(Cs2TelegramPlugin.Instance.Config.CustomMenu.IsValid() && Cs2TelegramPlugin.Instance.Config.ShowCustomMenu)
            {
                menu.Add(Cs2TelegramPlugin.Instance.Config.CustomMenu.ButtonName);
            }

            return MenuGenerator.ReplyKeyboard(1, menu, true, Constants.MAIN_MENU_BUTTON);
        }

        public static List<IInlineContent> GetServerCommandsItems()
        {
            var inlineMenuItems = new List<IInlineContent>();
            var config = Cs2TelegramPlugin.Instance.Config;
            if (config.ServerCommandsMenuItems.Count > 0)
            {
                foreach (var command in config.ServerCommandsMenuItems.Where(command => !string.IsNullOrWhiteSpace(command)))
                {
                    var commandItem = new InlineCallback<ServerExecuteTCommand>(command, HeaderCommand.ExecuteServerCommand, new ServerExecuteTCommand(command));
                    inlineMenuItems.Add(commandItem);
                }
            }
            return inlineMenuItems;
        }

        public static ReplyKeyboardMarkup GenerateAdminMenu(this ITelegramBotClient botClient, long userId)
        {
            var menu = new List<string>();

            if (botClient.IsAdmin(userId))
            {
                menu.Add(Constants.SERVER_COMMAND_BUTTON);
                menu.Add(Constants.SERVER_SEND_MESSAGE_BUTTON);
                menu.Add(Constants.SERVER_PLAYERS_INFO_BUTTON);
            }
            else
            {
                menu.Add(Constants.ACCESS_DENIED_BUTTON);
            }

            return MenuGenerator.ReplyKeyboard(1, menu, true, Constants.MAIN_MENU_BUTTON);
        }
        public static string GetGameModeWithGameType(int gametype, int gamemode)
        {
            //https://developer.valvesoftware.com/wiki/Counter-Strike:_Global_Offensive/Game_Modes

            string[][] games = new string[7][];

            games[0] = new string[] { "Casual", "Competitive", "Wingman", "Weapons Expert", "Training Day" };
            games[1] = new string[] { "Arms Race", "Demolition", "Deathmatch", };
            games[2] = new string[] { "Training", };
            games[3] = new string[] { "Custom", };
            games[4] = new string[] { "Guardian", "Co-op Strike" };
            games[5] = new string[] { "War Games" };
            games[6] = new string[] { "Danger Zone" };

            try
            {
                return games[gametype][gamemode];
            }
            catch(Exception ex)
            {
                return "Unknown game type with game mode";
            }
        }


        public static string GetFullMessage(this CommandInfo info)
        {
            return string.Join(" ", Enumerable.Range(1, info.ArgCount)
            .Select(i => info.GetArg(i))
            .Where(arg => !string.IsNullOrWhiteSpace(arg))) ?? "No reason given.";
        }

    }
}
