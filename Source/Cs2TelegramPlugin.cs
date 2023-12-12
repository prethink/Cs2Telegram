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
using Cs2Telegram.TelegramEvents;
using System.Text.Json.Serialization;

namespace Cs2Telegram;

public class Cs2TelegramPlugin : BasePlugin, IPluginConfig<TelegramCfg>
{
    public override string ModuleName => "Cs2Telegram";
    public override string ModuleVersion => "0.2.2";
    public override string ModuleAuthor => "PreThink";

    private PRBot _bot;

    public TelegramCfg Config { get; set; } = new TelegramCfg();
    public static TelegramCfg GlobalConfig { get; set; } = new TelegramCfg();
    public void OnConfigParsed(TelegramCfg config)
    {
        Config = config;
        GlobalConfig = config;
    }

    public override void Load(bool hotReload)
    {
        _bot = new PRBot(options =>
        {
            options.Token = Config.Token;
            options.WhiteListUsers = Config.WhiteListUsers;
            options.Admins = Config.Admins;
            options.ClearUpdatesOnStart = Config.ClearUpdatesOnStart;
            options.BotId = Config.BotId;
        });
        // Subscribe to basic logs
        _bot.OnLogCommon += Telegram_OnLogCommon;
        // Subscribe to error logs
        _bot.OnLogError += Telegram_OnLogError;
        // Start the bot
        _bot.Start();
        HandlerInit(_bot);
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

    void HandlerInit(PRTelegramBot.Core.PRBot tg)
    {
        if (tg.Handler != null)
        {
            //Обработка не правильный тип сообщений
            tg.Handler.Router.OnWrongTypeMessage += CommonEvents.WrongMessage;

            //Обработка пропущенной  команды
            tg.Handler.Router.OnMissingCommand += CommonEvents.OnMissingCommand;
        }
    }
}
