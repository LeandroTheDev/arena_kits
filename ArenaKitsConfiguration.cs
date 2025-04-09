using Rocket.API;

namespace ArenaKits
{
    public class ArenaKitsConfiguration : IRocketPluginConfiguration
    {
        public string DatabaseAddress = "127.0.0.1";
        public string DatabaseName = "unturned";
        public string DatabaseUsername = "admin";
        public string DatabasePassword = "root";
        public int DatabasePort = 3306;
        public string ArenaKitsTableName = "arenakits";
        public string ChatIconURL = "https://add-image-url.com";
        public bool KitCommandOnlyInArea = false;
        public bool KitCommandArenaPlayingCheck = true;
        public List<KitAreas> KitCommandAreas = [];
        public List<Kit> Items = [];

        public void LoadDefaults()
        {
            KitCommandAreas = [
                new()
                {
                    X1 = 431.41,
                    X2 = 440.92,
                    Y1 = 51.00,
                    Y2 = 54.00,
                    Z1 = 437.25,
                    Z2 = 447.05
                }
            ];

            Items = [
                new()
                {
                    name = "Hunter",
                    permissionId = null,
                    items = [
                        new() {
                            Id = 1182,
                            Amount = 1,
                            Metadata = [148, 0, 0, 0, 0, 0, 0, 0, 103, 0, 5, 1, 1, 100, 100, 100, 100, 100]
                        },
                        new() {
                            Id = 101,
                            Amount = 1
                        },
                        new() {
                            Id = 103,
                            Amount = 5
                        },
                        new() {
                            Id = 254,
                            Amount = 3
                        },
                        new() {
                            Id = 1385,
                            Amount = 1
                        },
                        new() {
                            Id = 1386,
                            Amount = 1
                        },
                        new() {
                            Id = 1387,
                            Amount = 1
                        },
                        new() {
                            Id = 1480,
                            Amount = 1
                        },
                        new() {
                            Id = 113,
                            Amount = 1
                        }
                    ],
                    experience = new() {
                        Cardio = 5,
                        Dexerity = 5,
                        Sneakybeaky = 2,
                        Sharpshooter = 3
                    }
                },
                new()
                {
                    name = "Smoker",
                    permissionId = null,
                    items = [
                        new() {
                            Id = 1270,
                            Amount = 1
                        },
                        new() {
                            Id = 194,
                            Amount = 1
                        },
                        new() {
                            Id = 247,
                            Amount = 1
                        },
                        new() {
                            Id = 162,
                            Amount = 1
                        },
                        new() {
                            Id = 209,
                            Amount = 1
                        },
                        new() {
                            Id = 112,
                            Amount = 1
                        },
                        new() {
                            Id = 113,
                            Amount = 2
                        },
                        new() {
                            Id = 99,
                            Amount = 1
                        },
                        new() {
                            Id = 100,
                            Amount = 6
                        },
                        new() {
                            Id = 263,
                            Amount = 4
                        }
                    ],
                    experience = new() {
                        Vitality = 2,
                        Healing = 3,
                        Immunity = 5,
                        Overkill = 5,
                        Sneakybeaky = 2
                    }
                },
            ];
        }
    }
    public class Kit
    {
        public string name = "";
        public string? permissionId = null;
        public List<KitItem> items = [];
        public KitExperience experience = new() { };
    }

    public class KitItem
    {
        public ushort Id = 1;
        public byte Amount = 1;
        public byte[]? Metadata = [];
    }

    public class KitExperience
    {
        public byte Agriculture = 0;
        public byte Cardio = 0;
        public byte Cooking = 0;
        public byte Crafting = 0;
        public byte Dexerity = 0;
        public byte Diving = 0;
        public byte Engineer = 0;
        public byte Exercise = 0;
        public byte Fishing = 0;
        public byte Healing = 0;
        public byte Immunity = 0;
        public byte Mechanic = 0;
        public byte Outdoors = 0;
        public byte Overkill = 0;
        public byte Parkour = 0;
        public byte Sharpshooter = 0;
        public byte Sneakybeaky = 0;
        public byte Strength = 0;
        public byte Survival = 0;
        public byte Toughness = 0;
        public byte Vitality = 0;
        public byte Warmblooded = 0;
    }

    public class KitAreas
    {
        public double X1;
        public double Y1;
        public double Z1;
        public double X2;
        public double Y2;
        public double Z2;
    }
}
