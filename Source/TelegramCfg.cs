using CounterStrikeSharp.API.Core;
using Cs2Telegram.Models;
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
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("Token")] public string Token { get; set; } = "";

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("Admins")] public List<long> Admins { get; set; } = new List<long>();

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("WhiteListUsers")] public List<long> WhiteListUsers { get; set; } = new List<long>();
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("ClearUpdatesOnStart")] public bool ClearUpdatesOnStart { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("BotId")] public int BotId { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("ServerCommandsMenuItems")] public List<string> ServerCommandsMenuItems { get; set; } = new List<string>() { "bot_add", "bot_kick" };
        
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("NotifyAdminOnConnectUser")] public bool NotifyAdminOnConnectUser { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("ColumnMainMenu")] public int ColumnMainMenu { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("CustomCommandsEnabled")] public bool CustomCommandsEnabled { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("CustomCommands")] public CustomCommands CustomCommands { get; set; } = new CustomCommands();

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("ConfigVersion")] public override int Version { get; set; } = 4;
    }
}
