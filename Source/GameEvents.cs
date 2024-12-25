using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using System.Net;

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
            Helper.SendAdminsMessage(_bot.botClient, $"Player {WebUtility.HtmlEncode(playerName)} connected to server");

            return HookResult.Continue;
        }
    }
}
