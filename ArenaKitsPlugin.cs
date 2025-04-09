extern alias UnityEngineCoreModule;

using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using Rocket.Unturned.Skills;
using SDG.Unturned;
using UnityCoreModule = UnityEngineCoreModule.UnityEngine;

namespace ArenaKits;

public class ArenaKitsPlugin : RocketPlugin<ArenaKitsConfiguration>
{
    static public ArenaKitsPlugin? instance = null;
    static public List<string> PlayersInArea = [];
    static public DatabaseMgr? Database;

    public override void LoadPlugin()
    {
        Database = new(this);

        base.LoadPlugin();
        Logger.Log("ArenaKits by LeandroTheDev");
        Rocket.Unturned.U.Events.OnPlayerConnected += OnPlayerConnected;

        if (Configuration.Instance.KitCommandOnlyInArea)
            Rocket.Unturned.Events.UnturnedPlayerEvents.OnPlayerUpdatePosition += PositionUpdate;

        instance = this;
    }

    private void OnPlayerConnected(UnturnedPlayer player)
    {
        Database!.AddNewPlayer(player.Id);

        // Add the event for player revived
        player.Events.OnRevive += PlayerRevived;

        DefaultKit(player);
    }

    private void PositionUpdate(UnturnedPlayer player, UnityCoreModule.Vector3 position)
    {
        // Check if player is in the area
        foreach (KitAreas area in Configuration.Instance.KitCommandAreas)
        {
            // Verify X position
            if (position.x < area.X1 || position.x > area.X2)
            {
                // Logger.Log($"{position.x} : {area.X1},{area.X2}");
                PlayersInArea.Remove(player.Id);
                return;
            }
            // Verify Y position
            else if (position.y < area.Y1 || position.y > area.Y2)
            {
                // Logger.Log($"{position.y} : {area.Y1},{area.Y2}");
                PlayersInArea.Remove(player.Id);
                return;
            }
            // Verifiy Z position
            else if (position.z < area.Z1 || position.z > area.Z2)
            {
                // Logger.Log($"{position.z} : {area.Z1},{area.Z2}");
                PlayersInArea.Remove(player.Id);
                return;
            }
        }
        if (!PlayersInArea.Contains(player.Id)) PlayersInArea.Add(player.Id);
    }

    private void PlayerRevived(UnturnedPlayer player, UnityCoreModule.Vector3 position, byte angle)
        => DefaultKit(player);

    private void DefaultKit(UnturnedPlayer player)
    {
        string? kitName = Database!.GetKit(player.Id);

        Kit kit;
        if (kitName == null || kitName == "") kit = ArenaKitsUtils.GetRandomKit(player);
        else kit = ArenaKitsUtils.GetKitByName(kitName);

        ArenaKitsUtils.ClearInventory(player);
        ArenaKitsUtils.SetSkill(player, kit.experience);
        ArenaKitsUtils.GiveItems(player, kit);

        ChatManager.serverSendMessage(
            Translate("kit_received", kit.name),
            new UnityCoreModule.Color(0, 255, 0),
            null,
            player.SteamPlayer(),
            EChatMode.SAY,
            Configuration.Instance.ChatIconURL,
            true
        );
    }

    public override TranslationList DefaultTranslations => new()
        {
            {"kit_received", "Kit received: {0}"},
            {"kit_nopermission", "No permission for: {0}"},
            {"kit_available", "Available kits: {0}"},
            {"kit_random", "Random Kit selected"},
            {"kit_unavailable", "Unavailable in combat"}
        };
}

public class ArenaKitsUtils
{
    static public void GiveItems(UnturnedPlayer player, ArenaKits.Kit kit)
    {
        foreach (KitItem item in kit.items)
        {
            Item spawnItem = new(item.Id, true)
            {
                amount = item.Amount
            };

            if (item.Metadata != null && item.Metadata.Length > 0)
                UpdateItemMetada(item, ref spawnItem);

            player.Inventory.tryAddItemAuto(spawnItem, true, false, true, false);
        }
    }

    static private void UpdateItemMetada(KitItem itemKit, ref Item item)
    {
        item.state = itemKit.Metadata;
        item.metadata = itemKit.Metadata;
    }

    static public void ClearInventory(UnturnedPlayer player)
    {
        // Deleting items
        for (byte page = 0; page < PlayerInventory.PAGES; page++)
        {
            try
            {
                while (player.Inventory.getItemCount(page) > 0)
                {
                    player.Inventory.removeItem(page, 0);
                }
            }
            catch (Exception) { }
        }

        // Deleting clothes
        player.Inventory.player.clothing.updateClothes(0, 0, [], 0, 0, [], 0, 0, [], 0, 0, [], 0, 0, [], 0, 0, [], 0, 0, []);
    }

    static public void SetSkill(UnturnedPlayer player, KitExperience experience)
    {
        player.Experience = 0;
        player.SetSkillLevel(UnturnedSkill.Agriculture, experience.Agriculture);
        player.SetSkillLevel(UnturnedSkill.Cardio, experience.Cardio);
        player.SetSkillLevel(UnturnedSkill.Cooking, experience.Cooking);
        player.SetSkillLevel(UnturnedSkill.Crafting, experience.Crafting);
        player.SetSkillLevel(UnturnedSkill.Dexerity, experience.Dexerity);
        player.SetSkillLevel(UnturnedSkill.Diving, experience.Diving);
        player.SetSkillLevel(UnturnedSkill.Engineer, experience.Engineer);
        player.SetSkillLevel(UnturnedSkill.Exercise, experience.Exercise);
        player.SetSkillLevel(UnturnedSkill.Fishing, experience.Fishing);
        player.SetSkillLevel(UnturnedSkill.Healing, experience.Healing);
        player.SetSkillLevel(UnturnedSkill.Immunity, experience.Immunity);
        player.SetSkillLevel(UnturnedSkill.Mechanic, experience.Mechanic);
        player.SetSkillLevel(UnturnedSkill.Outdoors, experience.Outdoors);
        player.SetSkillLevel(UnturnedSkill.Overkill, experience.Overkill);
        player.SetSkillLevel(UnturnedSkill.Parkour, experience.Parkour);
        player.SetSkillLevel(UnturnedSkill.Sharpshooter, experience.Sharpshooter);
        player.SetSkillLevel(UnturnedSkill.Sneakybeaky, experience.Sneakybeaky);
        player.SetSkillLevel(UnturnedSkill.Strength, experience.Strength);
        player.SetSkillLevel(UnturnedSkill.Survival, experience.Survival);
        player.SetSkillLevel(UnturnedSkill.Toughness, experience.Toughness);
        player.SetSkillLevel(UnturnedSkill.Vitality, experience.Vitality);
        player.SetSkillLevel(UnturnedSkill.Warmblooded, experience.Warmblooded);
    }

    static public UnturnedSkill? ConvertStringToSkill(string skillString)
    {
        return skillString switch
        {
            "agriculture" => UnturnedSkill.Agriculture,
            "cardio" => UnturnedSkill.Cardio,
            "cooking" => UnturnedSkill.Cooking,
            "crafting" => UnturnedSkill.Crafting,
            "dexerity" => UnturnedSkill.Dexerity,
            "diving" => UnturnedSkill.Diving,
            "engineer" => UnturnedSkill.Engineer,
            "exercise" => UnturnedSkill.Exercise,
            "fishing" => UnturnedSkill.Fishing,
            "healing" => UnturnedSkill.Healing,
            "immunity" => UnturnedSkill.Immunity,
            "mechanic" => UnturnedSkill.Mechanic,
            "outdoors" => UnturnedSkill.Outdoors,
            "overkill" => UnturnedSkill.Overkill,
            "parkour" => UnturnedSkill.Parkour,
            "sharpshooter" => UnturnedSkill.Sharpshooter,
            "sneakybeaky" => UnturnedSkill.Sneakybeaky,
            "strength" => UnturnedSkill.Strength,
            "survival" => UnturnedSkill.Survival,
            "toughness" => UnturnedSkill.Toughness,
            "vitality" => UnturnedSkill.Vitality,
            "warmblooded" => UnturnedSkill.Warmblooded,
            _ => null,
        };
    }

    static public Kit GetRandomKit(UnturnedPlayer player)
    {
        Random random = new();
        var kits = ArenaKitsPlugin.instance!.Configuration.Instance.Items;

        if (kits.Count == 0) return new Kit();

        for (int i = 0; i < 10; i++)
        {
            int randomIndex = random.Next(kits.Count);
            if (player.HasPermission($"arenakit.{kits[randomIndex].name}"))
            {
                return kits[randomIndex];
            }
        }
        return kits[0];
    }

    static public Kit GetKitByName(string name)
    {
        foreach (Kit kit in ArenaKitsPlugin.instance!.Configuration.Instance.Items)
        {
            if (kit.name.ToLower() == name.ToLower())
            {
                return kit;
            }
        }
        return new();
    }
}
