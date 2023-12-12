using PRTelegramBot.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Telegram.Enums
{
    [InlineCommand]
    public enum HeaderCommand
    {
        SelectMap = 0,
        ExecuteServerCommand = 1,
        PlayerKick,
        PlayerChangeTeam,
        PlayerGiveItem,
        PlayerSuicide,
        PlayerRespawn,
        PlayerInfo,
        PlayerKill,
        PlayerInfoList,
    }
}
