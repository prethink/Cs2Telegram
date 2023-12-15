using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Telegram
{
    public partial class Cs2TelegramPlugin : BasePlugin
    {
        [GameEventHandler]
        public HookResult OnConnectUser(EventPlayerConnect @event, GameEventInfo info)
        {
            if(!Config.NotifyAdminOnConnectUser)
                return HookResult.Continue;

            if (@event.Bot)
                return HookResult.Continue;

            string playerName = @event.Name;
            Helper.SendAdminsMessage(_bot.botClient, $"Player {playerName} connected to server");

            return HookResult.Continue;
        }
    }
}
