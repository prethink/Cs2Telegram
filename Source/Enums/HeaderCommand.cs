using PRTelegramBot.Attributes;

namespace Cs2Telegram.Enums
{
    [InlineCommand]
    public enum HeaderCommand
    {
        SelectMap = 100,
        ExecuteServerCommand,
        PlayerKick,
        PlayerChangeTeam,
        PlayerGiveItem,
        PlayerSuicide,
        PlayerRespawn,
        PlayerInfo,
        PlayerKill,
        PlayerInfoList,
        ChangeLevel,
        WorkshopChangeLevel
    }
}
