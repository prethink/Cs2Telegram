using CounterStrikeSharp.API.Core;
using Cs2Telegram.Enums;
using Cs2Telegram.InlineInstance;
using Cs2Telegram.TelegramEvents;
using Microsoft.Extensions.Logging;
using PRTelegramBot.Core;

namespace Cs2Telegram;

public partial class Cs2TelegramPlugin : BasePlugin, IPluginConfig<TelegramCfg>
{
    public override string ModuleName => "Cs2Telegram";
    public override string ModuleVersion => "0.3.2";
    public override string ModuleAuthor => "PreThink";

    private PRBotBase _bot;

    public TelegramCfg Config { get; set; } = new TelegramCfg();
    public static Cs2TelegramPlugin Instance { get; private set; }

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

    public Cs2TelegramPlugin()
    {
        Instance = this;
    }

    public override void Load(bool hotReload)
    {
        _bot = new PRBotBuilder(Config.Token)
            .AddUsersWhiteList(Config.WhiteListUsers)
            .AddAdmins(Config.Admins)
            .SetBotId(Config.BotId)
            .SetClearUpdatesOnStart(Config.ClearUpdatesOnStart)
            .AddInlineClassHandler(HeaderCommand.ChangeLevel, typeof(ChangeLevel))
            .AddInlineClassHandler(HeaderCommand.WorkshopChangeLevel, typeof(WorkshopChangeLevel))
            .Build();

        // Subscribe to basic logs
        _bot.Events.OnCommonLog += Events_OnCommonLog; ;
        // Subscribe to error logs
        _bot.Events.OnErrorLog += Events_OnErrorLog; ;
        // Start the bot
        _bot.Start();
        HandlerInit(_bot);
        Helper.RegisterCustomCommands(_bot);
    }

    private async Task Events_OnErrorLog(PRTelegramBot.Models.EventsArgs.ErrorLogEventArgs e)
    {
        Logger.LogError(e.Exception.ToString());
    }

    private async Task Events_OnCommonLog(PRTelegramBot.Models.EventsArgs.CommonLogEventArgs arg)
    {
        Logger.LogInformation(arg.Message);
    }

    void HandlerInit(PRBotBase bot)
    {
        if (bot.Handler != null)
        {
            //Обработка не правильный тип сообщений
            bot.Events.OnWrongTypeMessage += CommonEvents.WrongMessage;

            //Обработка пропущенной  команды
            bot.Events.OnMissingCommand += CommonEvents.OnMissingCommand;
        }
    }
}
