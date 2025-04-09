extern alias UnityEngineCoreModule;

using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityCoreModule = UnityEngineCoreModule.UnityEngine;

namespace ArenaKits.Commands
{
    public class Kit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "kit";

        public string Help => "Select a kit";

        public string Syntax => "/kit";

        public List<string> Aliases => [];

        public List<string> Permissions => [];

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            if (ArenaKitsPlugin.instance!.Configuration.Instance.KitCommandArenaPlayingCheck && LevelManager.arenaState == EArenaState.PLAY)
            {
                ChatManager.serverSendMessage(
                    ArenaKitsPlugin.instance!.Translate("kit_unavailable"),
                    new UnityCoreModule.Color(0, 255, 0),
                    null,
                    player.SteamPlayer(),
                    EChatMode.SAY,
                    ArenaKitsPlugin.instance!.Configuration.Instance.ChatIconURL,
                    true
                );
                return;
            }

            if (ArenaKitsPlugin.instance!.Configuration.Instance.KitCommandOnlyInArea && ArenaKitsPlugin.PlayersInArea.Contains(player.Id))
            {
                ChatManager.serverSendMessage(
                    ArenaKitsPlugin.instance!.Translate("kit_unavailable"),
                    new UnityCoreModule.Color(0, 255, 0),
                    null,
                    player.SteamPlayer(),
                    EChatMode.SAY,
                    ArenaKitsPlugin.instance!.Configuration.Instance.ChatIconURL,
                    true
                );
                return;
            }

            string kitName = string.Join(" ", command);
            ArenaKits.Kit kit = ArenaKitsUtils.GetKitByName(kitName);

            if (player.HasPermission($"arenakit.{kit.name}"))
            {
                ArenaKitsUtils.ClearInventory(player);
                ArenaKitsUtils.SetSkill(player, kit.experience);
                ArenaKitsUtils.GiveItems(player, kit);
                ArenaKitsPlugin.Database!.SetKit(player.Id, kit.name);

                ChatManager.serverSendMessage(
                    ArenaKitsPlugin.instance!.Translate("kit_received", kit.name),
                    new UnityCoreModule.Color(0, 255, 0),
                    null,
                    player.SteamPlayer(),
                    EChatMode.SAY,
                    ArenaKitsPlugin.instance!.Configuration.Instance.ChatIconURL,
                    true
                );
            }
            else
            {
                ChatManager.serverSendMessage(
                    ArenaKitsPlugin.instance!.Translate("kit_nopermission", kit.name),
                    new UnityCoreModule.Color(0, 255, 0),
                    null,
                    player.SteamPlayer(),
                    EChatMode.SAY,
                    ArenaKitsPlugin.instance!.Configuration.Instance.ChatIconURL,
                    true
                );
            }
        }
    }
}