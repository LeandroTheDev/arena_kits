extern alias UnityEngineCoreModule;

using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityCoreModule = UnityEngineCoreModule.UnityEngine;

namespace ArenaKits.Commands
{
    public class ListKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "listkit";

        public string Help => "Show available kits";

        public string Syntax => "/listkit";

        public List<string> Aliases => [];

        public List<string> Permissions => [];

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            List<string> availableKits = [];
            foreach (ArenaKits.Kit kit in ArenaKitsPlugin.instance!.Configuration.Instance.Items)
            {
                if (player.HasPermission($"arenakit.{kit.name}"))
                    availableKits.Add(kit.name);
            }
            string kitsString = string.Join(", ", availableKits);

            ChatManager.serverSendMessage(
                ArenaKitsPlugin.instance!.Translate("kit_available", kitsString),
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