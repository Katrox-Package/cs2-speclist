using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Utils;

namespace SpecList
{
    public static class Utils
    {
        public static bool IsRealValid([NotNullWhen(true)] this CCSPlayerController? player, bool includeBot = false)
        {
            return player != null
                && player.IsValid
                && player.PlayerPawn.IsValid
                && player.PlayerPawn.Value?.IsValid == true
                && player.Connected == PlayerConnectedState.PlayerConnected
                && !player.IsHLTV
                && (includeBot ? true : !player.IsBot);
        }

        public static List<CCSPlayerController> GetPlayers(CsTeam? team = null, bool? alive = null)
        {
            var players = Utilities.GetPlayers();
            return players.FindAll(x =>
                x.IsRealValid() &&
                (team == null || x.Team == team) &&
                (alive == null || x.IsAlive() == alive)
            );
        }
        public static bool IsAlive(this CCSPlayerController player)
        {
            return player.IsRealValid() && player.PlayerPawn.Value?.LifeState == (byte)LifeState_t.LIFE_ALIVE;
        }

        public static void Print(this CCSPlayerController player, string msg, params object[] args)
        {
            player.PrintToChat($" {SpecList._Config.Prefix} {ChatColors.White}{SpecList._Localizer?.ForPlayer(player, msg, args).TrimStart(' ')}");
        }
    }
}
