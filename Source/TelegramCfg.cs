using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cs2Telegram
{
    public class TelegramCfg : BasePluginConfig
    {
        [JsonPropertyName("Token")] public string Token { get; set; } = "";
        [JsonPropertyName("Admins")] public List<long> Admins { get; set; } = new List<long>();
        [JsonPropertyName("WhiteListUsers")] public List<long> WhiteListUsers { get; set; } = new List<long>();
        [JsonPropertyName("ClearUpdatesOnStart")] public bool ClearUpdatesOnStart { get; set; } = true;
        [JsonPropertyName("BotId")] public int BotId { get; set; } = 0;
        [JsonPropertyName("ServerCommandsMenuItems")] public List<string> ServerCommandsMenuItems { get; set; } = new List<string>();
        [JsonPropertyName("NotifyAdminOnConnectUser")] public bool NotifyAdminOnConnectUser { get; set; } = true;
        [JsonPropertyName("ConfigVersion")] public override int Version { get; set; } = 2;
    }
}
