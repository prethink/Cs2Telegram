using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API;
using PRTelegramBot.Attributes;
using PRTelegramBot.Core;
using Telegram.Bot.Types;
using Telegram.Bot;
using PRTelegramBot.Models;
using PRTelegramBot.Configs;
using System.Text.Json;
using Newtonsoft.Json;
using File = System.IO.File;
using CounterStrikeSharp.API.Modules.Memory;
using PRTelegramBot.Helpers;

namespace Cs2Telegram;

public class TelegramCore : BasePlugin
{
    public override string ModuleName => "Cs2Telegram";
    public override string ModuleVersion => "0.2.0";
    public override string ModuleAuthor => "PreThink";

    const string FILE_CONFIG = "telegramconfig.json";

    private PRBot _bot;



    public override void Load(bool hotReload)
    {
        var config = GetOrCreateConfig();
        _bot = new PRBot(config);
        // Subscribe to basic logs
        _bot.OnLogCommon += Telegram_OnLogCommon;
        // Subscribe to error logs
        _bot.OnLogError += Telegram_OnLogError;
        // Start the bot
        _bot.Start();
    }

    private TelegramConfig GetOrCreateConfig()
    {
        string path = Path.Combine(ModuleDirectory, FILE_CONFIG);

        if(!File.Exists(path))
        {
            TelegramConfig telegramConfig = new TelegramConfig
            {
                Token = "",
                Admins = new List<long> { },
                WhiteListUsers = new List<long> { },
                ClearUpdatesOnStart = true,
                BotId = 0
            };

            string jsonData = JsonConvert.SerializeObject(telegramConfig, Formatting.Indented);
            File.WriteAllText(path,jsonData);
            return telegramConfig;
        }
        try
        {
            var config = JsonConvert.DeserializeObject<TelegramConfig>(File.ReadAllText(path));
            return config;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error while reading the file {FILE_CONFIG}. Error: {ex}");
            return new TelegramConfig();
        }

    }

    private void Telegram_OnLogError(Exception ex, long? id)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        string errorMsg = $"{DateTime.Now}:{ex}";
        Console.WriteLine(errorMsg);
        Console.ResetColor();
    }

    private void Telegram_OnLogCommon(string msg, PRBot.TelegramEvents typeEvent, ConsoleColor color)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        string message = $"{DateTime.Now}:{msg}";
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
