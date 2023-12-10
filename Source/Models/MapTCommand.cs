using Newtonsoft.Json;
using PRTelegramBot.Models.CallbackCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Telegram.Models
{
    public class MapTCommand : TCommandBase
    {
        [JsonProperty("1")]
        public string MapName { get; set; }

        public MapTCommand(string mapName)
        {
            MapName = mapName;
        }
    }
}
