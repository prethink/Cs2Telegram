using PRTelegramBot.Models.CallbackCommands;
using System.Text.Json.Serialization;

namespace Cs2Telegram.Models
{
    public class MapTCommand : TCommandBase
    {
        [JsonPropertyName("1")]
        public string MapName { get; set; }

        public MapTCommand(string mapName)
        {
            MapName = mapName;
        }
    }
}
