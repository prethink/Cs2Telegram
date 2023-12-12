using CounterStrikeSharp.API.Modules.Utils;
using Newtonsoft.Json;
using PRTelegramBot.Models.CallbackCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Telegram.Models
{
    public class ChangeTeamPlayerCommand : TCommandBase
    {
        [JsonProperty("1")]
        public CsTeam Team { get; set; }

        [JsonProperty("2")]
        public int UserId { get; set; }

        public ChangeTeamPlayerCommand(int userId, CsTeam team)
        {
            Team = team;
            UserId = userId;
        }
    }
}
