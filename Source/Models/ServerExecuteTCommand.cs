using Newtonsoft.Json;
using PRTelegramBot.Models.CallbackCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Telegram.Models
{
    internal class ServerExecuteTCommand : TCommandBase
    {
        [JsonProperty("1")]
        public string Command { get; set; }

        public ServerExecuteTCommand(string command)
        {
            Command = command;
        }
    }
}
