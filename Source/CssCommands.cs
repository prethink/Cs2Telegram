using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Telegram
{
    public partial class Cs2TelegramPlugin : BasePlugin
    {
        [ConsoleCommand("css_tgreport", "Sends a message to the administrators of the server on Telegram")]
        [CommandHelper(minArgs: 1, usage: "[message]", whoCanExecute: CommandUsage.CLIENT_ONLY)]
        public void OnReportPlayer(CCSPlayerController? player, CommandInfo commandInfo)
        {
            var message = commandInfo.GetFullMessage();
            Server.NextFrame(() =>
            {
                message = $"Player: {player.PlayerName}\n" +
                $"IP:{player?.IpAddress}\n" +
                $"Message: {message}";
                Helper.SendAdminsMessage(_bot.botClient, message);
            });
        }
    }
}
