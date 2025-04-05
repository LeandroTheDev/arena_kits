extern alias UnityEngineCoreModule;

using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityCoreModule = UnityEngineCoreModule.UnityEngine;

namespace ArenaKits.Commands
{
    public class Metadata : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "metadata";

        public string Help => "Show currently equipped item metadata";

        public string Syntax => "/metadata";

        public List<string> Aliases => [];

        public List<string> Permissions => [];

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            for (byte page = 0; page < PlayerInventory.PAGES; page++)
            {
                try
                {
                    int itemCount = player.Inventory.getItemCount(page);
                    for (byte i = 0; i < itemCount; i++)
                    {
                        ItemJar itemJar = player.Inventory.getItem(page, i);
                        ItemAsset asset = (ItemAsset)Assets.find(EAssetType.ITEM, itemJar.item.id);
                        Logger.Log("--------------- " + page + " ---------------");
                        if (asset != null && (itemJar.item.state?.Length > 0 || itemJar.item.metadata?.Length > 0))
                        {
                            Logger.Log($"Item Name: {asset.itemName}");
                            if (itemJar.item.state?.Length > 0)
                            {
                                Logger.Log($"Item State: {Convert.ToBase64String(itemJar.item.state)}");
                            }
                            if (itemJar.item.metadata?.Length > 0)
                            {
                                Logger.Log($"Item Metadata: {Convert.ToBase64String(itemJar.item.metadata)}");
                            }
                        }
                        Logger.Log("--------------- " + page + " ---------------");
                    }
                }
                catch (Exception) { }
            }
            SendMessage("Metadata send to the server console", player);
        }

        private void SendMessage(string message, UnturnedPlayer player)
        {
            ChatManager.serverSendMessage(
                message,
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