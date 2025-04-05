extern alias UnityEngineCoreModule;

using Rocket.API;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityCoreModule = UnityEngineCoreModule.UnityEngine;

namespace ArenaKits.Commands
{
    public class RandomKit : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "randomkit";

        public string Help => "Next round will select a random kit";

        public string Syntax => "/randomkit";

        public List<string> Aliases => [];

        public List<string> Permissions => [];

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            ArenaKitsPlugin.Database!.RemoveKit(player.Id);

            ChatManager.serverSendMessage(
                ArenaKitsPlugin.instance!.Translate("kit_random"),
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