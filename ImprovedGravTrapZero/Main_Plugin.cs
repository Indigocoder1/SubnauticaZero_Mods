using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace EnhancedGravTrapZero
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    [BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
    public class Main_Plugin : BaseUnityPlugin
    {
        private const string myGUID = "Indigocoder.EnhancedGravTrapZero";
        private const string pluginName = "Enhanced Grav Trap Zero";
        private const string versionString = "1.0.0";

        private static readonly string ConfigFilePath = Path.Combine(Path.GetDirectoryName(Paths.BepInExConfigPath), "EnhancedGravTrap.json");
        public static string AssetsFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");

        public static ConfigEntry<bool> UseScrollWheel;
        public static ConfigEntry<KeyCode> AdvanceKey;
        public static ConfigEntry<KeyCode> OpenStorageKey;
        public static ConfigEntry<int> EnhancedRange;
        public static ConfigEntry<float> EnhancedMaxForce;
        public static ConfigEntry<float> EnhancedMaxMassStable;
        public static ConfigEntry<int> EnhancedMaxObjects;
        public static ConfigEntry<int> GravTrapStorageWidth;
        public static ConfigEntry<int> GravTrapStorageHeight;
        public static ConfigEntry<float> GravStoragePickupDistance;
        public static ConfigEntry<float> GravStorageOpenDistance;

        public static List<TechTypeList> AllowedTypes;

        public static ManualLogSource logger;

        private static readonly Harmony harmony = new Harmony(myGUID);

        private void Awake()
        {
            logger = Logger;

            SetUpConfigs();
            InitializeAllowedTypes();

            new GravTrap_ModOptions();
            EnhancedTrap_Craftable.Patch();

            harmony.PatchAll();

            Logger.LogInfo($"{pluginName} {versionString} Loaded.");
        }

        private void SetUpConfigs()
        {
            UseScrollWheel = Config.Bind("Enhanced Grav Trap", "Use scroll wheel to advance types", false);

            AdvanceKey = Config.Bind("Enhanced Grav Trap", "Key used to advance the type", KeyCode.Mouse2);

            OpenStorageKey = Config.Bind("Enhanced Grav Trap", "Key used to open the grav trap storage", KeyCode.LeftAlt);

            EnhancedRange = Config.Bind("Enhanced Grav Trap", "Enhanced grav trap range", 30,
                new ConfigDescription("The range of the enhanced grav trap",
                acceptableValues: new AcceptableValueRange<int>(17, 40)));

            EnhancedMaxForce = Config.Bind("Enhanced Grav Trap", "Attraction force", 20f,
                new ConfigDescription("The force of the enhanced grav trap",
                acceptableValues: new AcceptableValueRange<float>(15f, 30f)));

            EnhancedMaxMassStable = Config.Bind("Enhanced Grav Trap", "Max mass stable", 150f,
                new ConfigDescription("The max stable mass of the enhanced grav trap",
                acceptableValues: new AcceptableValueRange<float>(15f, 200f)));

            EnhancedMaxObjects = Config.Bind("Enhanced Grav Trap", "Enhanced grav trap max objects", 20,
                new ConfigDescription("The max attracted objects of the enhanced grav trap",
                acceptableValues: new AcceptableValueRange<int>(12, 30)));

            GravTrapStorageWidth = Config.Bind("Enhanced Grav Trap", "Enhanced grav trap storage width", 4,
                new ConfigDescription("How many units wide the storage is (Requires restart)", 
                acceptableValues: new AcceptableValueRange<int>(2, 7)));

            GravTrapStorageHeight = Config.Bind("Enhanced Grav Trap", "Enhanced grav trap storage height", 4,
                new ConfigDescription("How many units tall the storage is (Requires restart)", 
                acceptableValues: new AcceptableValueRange<int>(2, 7)));

            GravStorageOpenDistance = Config.Bind("Enhanced Grav Trap", "Storage open distance", 4f, 
                new ConfigDescription("How far you need to be before you can open the storage",
                acceptableValues: new AcceptableValueRange<float>(2f, 10f)));

            GravStoragePickupDistance = Config.Bind("Enhanced Grav Trap", "Enhanced grav trap pickup distance", 5f, 
                new ConfigDescription("How far an item needs to be from the grav trap before it's picked up",
                acceptableValues: new AcceptableValueRange<float>(2f, 10f)));
        }

        private void InitializeAllowedTypes()
        {
            AllowedTypes = SaveManager.LoadFromJson(ConfigFilePath);
            if (AllowedTypes == null)
            {
                SaveInitialData();
                AllowedTypes = SaveManager.LoadFromJson(ConfigFilePath);
            }
        }

        private void SaveInitialData()
        {
            List<TechTypeList> list = new List<TechTypeList>
            {
                new TechTypeList("All", techTypes:
                new TechType[]
                {
                    TechType.SnowStalker,
                    TechType.SnowStalkerBaby,
                    TechType.Penguin,
                    TechType.PenguinBaby,
                    TechType.ArcticPeeper,
                    TechType.ArcticRay,
                    TechType.ShadowLeviathan,
                    TechType.Bladderfish,
                    TechType.Boomerang,
                    TechType.Crash,
                    TechType.Hoopfish,
                    TechType.PrecursorDroid,
                    TechType.Skyray,
                    TechType.Quartz,
                    TechType.ScrapMetal,
                    TechType.FiberMesh,
                    TechType.LimestoneChunk,
                    TechType.Copper,
                    TechType.Lead,
                    TechType.Salt,
                    TechType.CalciumChunk,
                    TechType.Glass,
                    TechType.Titanium,
                    TechType.Silicone,
                    TechType.Gold,
                    TechType.Sulphur,
                    TechType.Lodestone,
                    TechType.Silver,
                    TechType.BatteryAcidOld,
                    TechType.TitaniumIngot,
                    TechType.CrashPowder,
                    TechType.Diamond,
                    TechType.ObsidianChunk,
                    TechType.Lithium,
                    TechType.PlasteelIngot,
                    TechType.EnameledGlass,
                    TechType.PowerCell,
                    TechType.ComputerChip,
                    TechType.Fiber,
                    TechType.Enamel,
                    TechType.AcidOld,
                    TechType.VesselOld,
                    TechType.CombustibleOld,
                    TechType.OpalGem,
                    TechType.AluminumOxide,
                    TechType.HydrochloricAcid,
                    TechType.Magnetite,
                    TechType.AminoAcids,
                    TechType.Polyaniline,
                    TechType.AramidFibers,
                    TechType.Graphene,
                    TechType.Aerogel,
                    TechType.Nanowires,
                    TechType.Benzene,
                    TechType.Lubricant,
                    TechType.UraniniteCrystal,
                    TechType.ReactorRod,
                    TechType.DepletedReactorRod,
                    TechType.PrecursorIonCrystal,
                    TechType.Kyanite,
                    TechType.Nickel,
                    TechType.Pinnacarid,
                    TechType.SquidShark,
                    TechType.TitanHolefish,
                    TechType.TrivalveBlue,
                    TechType.TrivalveYellow,
                    TechType.ArcticRayEgg,
                    TechType.ArcticRayEggUndiscovered,
                    TechType.BrinewingEgg,
                    TechType.BrinewingEggUndiscovered,
                    TechType.BruteSharkEgg,
                    TechType.BruteSharkEggUndiscovered,
                    TechType.CryptosuchusEgg,
                    TechType.CryptosuchusEggUndiscovered,
                    TechType.GenericEgg,
                    TechType.GlowWhaleEgg,
                    TechType.GlowWhaleEggUndiscovered,
                    TechType.JellyfishEgg,
                    TechType.JellyfishEggUndiscovered,
                    TechType.LavaZoneEgg,
                    TechType.LilyPaddlerEgg,
                    TechType.LilyPaddlerEggUndiscovered,
                    TechType.PenguinEgg,
                    TechType.PenguinEggUndiscovered,
                    TechType.PinnacaridEgg,
                    TechType.PinnacaridEggUndiscovered,
                    TechType.PrecursorLostRiverLabEgg,
                    TechType.RockPuncherEgg,
                    TechType.RockPuncherEggUndiscovered,
                    TechType.SeaMonkeyEgg,
                    TechType.ShockerEgg,
                    TechType.ShockerEggUndiscovered,
                    TechType.SquidSharkEgg,
                    TechType.SquidSharkEggUndiscovered,
                    TechType.TitanHolefishEgg,
                    TechType.TitanHolefishEggUndiscovered,
                    TechType.TrivalveBlueEgg,
                    TechType.TrivalveBlueEggUndiscovered,
                    TechType.TrivalveYellowEgg,
                    TechType.TrivalveYellowEggUndiscovered,
                }),
                new TechTypeList("Creatures", techTypes:
                new TechType[]
                {
                    TechType.SnowStalker,
                    TechType.SnowStalkerBaby,
                    TechType.Penguin,
                    TechType.PenguinBaby,
                    TechType.ArcticPeeper,
                    TechType.ArcticRay,
                    TechType.ShadowLeviathan,
                    TechType.Bladderfish,
                    TechType.Boomerang,
                    TechType.Crash,
                    TechType.Hoopfish,
                    TechType.PrecursorDroid,
                    TechType.Skyray,
                    TechType.Pinnacarid,
                    TechType.SquidShark,
                    TechType.TitanHolefish,
                    TechType.TrivalveBlue,
                    TechType.TrivalveYellow
                }),
                new TechTypeList("Resources", techTypes:
                new TechType[]
                {
                    TechType.CalciumChunk,
                    TechType.ObsidianChunk,
                    TechType.TwistyBridgesMushroomChunk,
                    TechType.JeweledDiskPiece,
                    TechType.Aerogel,
                    TechType.AluminumOxide,
                    TechType.AramidFibers,
                    TechType.Benzene,
                    TechType.Copper,
                    TechType.DepletedReactorRod,
                    TechType.Diamond,
                    TechType.EnameledGlass,
                    TechType.FiberMesh,
                    TechType.Glass,
                    TechType.Gold,
                    TechType.HydrochloricAcid,
                    TechType.Kyanite,
                    TechType.Lead,
                    TechType.LimestoneChunk,
                    TechType.Lithium,
                    TechType.Lubricant,
                    TechType.Magnetite,
                    TechType.Nickel,
                    TechType.PlasteelIngot,
                    TechType.Polyaniline,
                    TechType.PrecursorIonCrystal,
                    TechType.Quartz,
                    TechType.ReactorRod,
                    TechType.Salt,
                    TechType.ScrapMetal,
                    TechType.Silicone,
                    TechType.Silver,
                    TechType.Sulphur,
                    TechType.Titanium,
                    TechType.TitaniumIngot,
                    TechType.UraniniteCrystal,
                }),
                new TechTypeList("Eggs", techTypes:
                new TechType[]
                {
                    TechType.ArcticRayEgg,
                    TechType.ArcticRayEggUndiscovered,
                    TechType.BrinewingEgg,
                    TechType.BrinewingEggUndiscovered,
                    TechType.BruteSharkEgg,
                    TechType.BruteSharkEggUndiscovered,
                    TechType.CryptosuchusEgg,
                    TechType.CryptosuchusEggUndiscovered,
                    TechType.GenericEgg,
                    TechType.GlowWhaleEgg,
                    TechType.GlowWhaleEggUndiscovered,
                    TechType.JellyfishEgg,
                    TechType.JellyfishEggUndiscovered,
                    TechType.LavaZoneEgg,
                    TechType.LilyPaddlerEgg,
                    TechType.LilyPaddlerEggUndiscovered,
                    TechType.PenguinEgg,
                    TechType.PenguinEggUndiscovered,
                    TechType.PinnacaridEgg,
                    TechType.PinnacaridEggUndiscovered,
                    TechType.PrecursorLostRiverLabEgg,
                    TechType.RockPuncherEgg,
                    TechType.RockPuncherEggUndiscovered,
                    TechType.SeaMonkeyEgg,
                    TechType.ShockerEgg,
                    TechType.ShockerEggUndiscovered,
                    TechType.SquidSharkEgg,
                    TechType.SquidSharkEggUndiscovered,
                    TechType.TitanHolefishEgg,
                    TechType.TitanHolefishEggUndiscovered,
                    TechType.TrivalveBlueEgg,
                    TechType.TrivalveBlueEggUndiscovered,
                    TechType.TrivalveYellowEgg,
                    TechType.TrivalveYellowEggUndiscovered,
                })
            };

            SaveManager.SaveToJson(list, ConfigFilePath);
        }
    }
}
