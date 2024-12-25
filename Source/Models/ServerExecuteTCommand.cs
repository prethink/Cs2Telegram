using PRTelegramBot.Models.CallbackCommands;
using System.Text.Json.Serialization;

namespace Cs2Telegram.Models
{
    internal class ServerExecuteTCommand : TCommandBase
    {
        [JsonPropertyName("1")]
        public string Command { get; set; }

        public ServerExecuteTCommand(string command)
        {
            Command = command;
        }
    }
}
