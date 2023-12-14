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
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Cs2Telegram;

public partial class Cs2TelegramPlugin : BasePlugin, IPluginConfig<TelegramCfg>
{
    public override string ModuleName => "Cs2Telegram";
    public override string ModuleVersion => "0.2.3";
    public override string ModuleAuthor => "PreThink";

    private PRBot _bot;

    public TelegramCfg Config { get; set; } = new TelegramCfg();
    public static Cs2TelegramPlugin Instance { get; private set; }
    private string _logsPath = "";
    public void OnConfigParsed(TelegramCfg config)
    {
        if (config.Version < Config.Version)
        {
            Logger.LogWarning($"" +
                $"\nThe version of the configuration file differs from the required configuration!" +
                $"\nFile version:{config.Version}" +
                $"\nRequired:{Config.Version}");
        }
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        Instance = this; 

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
        Logger.LogError(ex.ToString());
    }

    private void Telegram_OnLogCommon(string msg, PRBot.TelegramEvents typeEvent, ConsoleColor color)
    {
        Logger.LogInformation(msg);
    }

    void HandlerInit(PRBot tg)
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
