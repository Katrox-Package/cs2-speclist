using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;

namespace SpecList
{
    public static class SpecListManager
    {
        public static CCSPlayerController? GetSpectatingPlayer(this CCSPlayerController spec)
        {
            if (spec.Pawn.Value is not { IsValid: true } pawn)
                return null;

            if (spec.ControllingBot)
                return null;

            if (pawn.ObserverServices is not { } obServices)
                return null;

            if (obServices.ObserverTarget?.Value?.As<CCSPlayerPawn>() is not { IsValid: true } obPawn)
                return null;

            if (obPawn.OriginalController.Value is not { IsValid: true } obController)
                return null;

            return obController;
        }

        public static List<CCSPlayerController> GetSpectators(this CCSPlayerController obPlayer)
        {
            var spectators = new List<CCSPlayerController>();

            var players = Utilities.GetPlayers();
            foreach (var player in players)
            {
                if (player.Pawn.Value is not { IsValid: true } pawn)
                    continue;

                if (player.ControllingBot)
                    continue;

                if (pawn.ObserverServices is not { } obServices)
                    continue;

                if (obServices.ObserverTarget?.Value?.As<CCSPlayerPawn>() is not { IsValid: true } obPawn)
                    continue;

                if (obPawn.OriginalController.Value is not { IsValid: true } obController)
                    continue;

                if (obController.Slot == obPlayer.Slot)
                {
                    spectators.Add(player);
                }
            }

            return spectators;
        }
    }
}
