using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Entities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace SpecList
{
    public class SpecListConfig : BasePluginConfig
    {
        public string Prefix { get; set; } = "{lightred}[Katrox]";
        public string[] ShowSpecsCommands { get; set; } =
        {
            "showspecs", "showspeclist"
        };
    }

    public partial class SpecList : BasePlugin, IPluginConfig<SpecListConfig>
    {
        public override string ModuleName => "cs2-speclist";
        public override string ModuleAuthor => "Roxy & Katarina";
        public override string ModuleVersion => "0.0.1";

        public override void Load(bool hotReload)
        {
            _Logger = Logger;
            _Localizer = Localizer;

            RegisterListener<Listeners.OnTick>(OnTick);

            foreach (var x in Config.ShowSpecsCommands) AddCommand(x, "", ShowSpecsCommand);
        }

        public override void Unload(bool hotReload)
        {
            RemoveListener<Listeners.OnTick>(OnTick);
        }

        public void OnConfigParsed(SpecListConfig config)
        {
            config.Prefix = config.Prefix.ReplaceColorTags();
            Config = config;
            _Config = config;
        }

        private void OnTick()
        {
            Utils.GetPlayers().ForEach(x =>
            {
                if (x == null || !x.IsValid)
                    return;

                if (AllowSpecList.Contains(x.SteamID))
                {
                    if (x.GetSpectators() is { } spectators)
                    {
                        if (spectators.Count <= 0) return;
                        var spectatorNames = string.Join("\n", spectators.Select(x => x.PlayerName));
                        var localizedText = Localizer["SpectatorListHeader"] + "\n" + spectatorNames;
                        foreach (var spec in spectators)
                        {
                            spec.PrintToCenterHtml(localizedText);
                        }

                        x.PrintToCenterHtml(localizedText);
                    }
                }
            });
        }

        public static ILogger? _Logger { get; set; }
        public static IStringLocalizer? _Localizer { get; set; }
        public SpecListConfig Config { get; set; } = new();
        public static SpecListConfig _Config { get; set; } = new();
    }
}
