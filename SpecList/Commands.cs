using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;

namespace SpecList
{
    public partial class SpecList
    {
        List<ulong> AllowSpecList = new();
        public void ShowSpecsCommand(CCSPlayerController? player, CommandInfo info)
        {
            if (!player.IsRealValid())
                return;

            if (!AllowSpecList.Contains(player.SteamID))
            {
                AllowSpecList.Add(player.SteamID);
            }
            else
            {
                AllowSpecList.Remove(player.SteamID);
            }
            var text = AllowSpecList.Contains(player.SteamID) ? "SpecList_Enabled" : "SpecList_Disabled"; 
            player.Print(text);
        }
    }
}
