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
            
            if (@event.Userid == null || !@event.Userid.IsValid || @event.Userid.IsBot)
                return HookResult.Continue;

            Server.NextFrame(() =>
            {
                string playerName = @event.Userid.PlayerName;
                Helper.SendAdminsMessage(_bot.botClient, $"Player {playerName} connected to server");
            });

            return HookResult.Continue;
        }
    }
}
