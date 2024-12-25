using CounterStrikeSharp.API.Modules.Utils;
using PRTelegramBot.Models.CallbackCommands;
using System.Text.Json.Serialization;

namespace Cs2Telegram.Models
{
    public class ChangeTeamPlayerCommand : TCommandBase
    {
        [JsonPropertyName("1")]
        public CsTeam Team { get; set; }

        [JsonPropertyName("2")]
        public int UserId { get; set; }

        public ChangeTeamPlayerCommand(int userId, CsTeam team)
        {
            Team = team;
            UserId = userId;
        }
    }
}
