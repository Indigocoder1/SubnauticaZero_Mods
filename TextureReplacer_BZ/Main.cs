﻿using BepInEx;
using BepInEx.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TextureReplacer
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    [BepInDependency("com.snmodding.nautilus", BepInDependency.DependencyFlags.HardDependency)]
    public class Main : BaseUnityPlugin
    {
        private const string myGUID = "com.Indigocoder.TextureReplacerZero";
        private const string pluginName = "Texture Replacer Zero";
        private const string versionString = "1.0.2";

        public static ManualLogSource logger;
        public static string AssetFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");

        private void Awake()
        {
            logger = Logger;

            CustomTextureReplacer.Initialize();

            Logger.LogInfo($"{pluginName} {versionString} Loaded.");
        }

        public class TexturePatchConfigData
        {
            public string configName;
            public int materialIndex;
            public string fileName;
            public string prefabClassID;
            public string rendererHierarchyPath;
            public string textureName;

            public bool isVariation;
            public float variationChance;
            public List<string> linkedConfigNames;
            [JsonIgnore]
            public bool variationAccepted;

            [JsonConstructor]
            public TexturePatchConfigData(string configName, int materialIndex, string fileName, bool isVariation, float variationChance,
                string prefabClassID, string rendererHierarchyPath, string textureName, List<string> linkedConfigNames)
            {
                this.configName = configName;
                this.materialIndex = materialIndex;
                this.fileName = fileName;
                this.prefabClassID = prefabClassID;
                this.rendererHierarchyPath = rendererHierarchyPath;
                this.textureName = textureName;
                this.isVariation = isVariation;
                this.variationChance = variationChance;
                this.linkedConfigNames = linkedConfigNames;
            }

            public TexturePatchConfigData(ConfigInfo configInfo)
            {
                this.materialIndex = configInfo.materialIndex;
                this.fileName = configInfo.fileName;
                this.prefabClassID = configInfo.prefabClassID;
                this.rendererHierarchyPath = configInfo.rendererHierchyPath;
                this.isVariation = configInfo.isVariation;
                this.variationChance = configInfo.variationChance;
                this.linkedConfigNames = configInfo.linkedConfigNames;
            }
        }

        public struct ConfigInfo
        {
            public int materialIndex;
            public string fileName;
            public string prefabClassID;
            public string rendererHierchyPath;

            public bool isVariation;
            public float variationChance;
            public List<string> linkedConfigNames;

            public ConfigInfo(int materialIndex, string fileName, string prefabClassID, string rendererHierchyPath, bool isVariation, float variationChance, List<string> linkedConfigNames)
            {
                this.materialIndex = materialIndex;
                this.fileName = fileName;
                this.prefabClassID = prefabClassID;
                this.rendererHierchyPath = rendererHierchyPath;
                this.isVariation = isVariation;
                this.variationChance = variationChance;
                this.linkedConfigNames = linkedConfigNames;
            }
        }

        public static Dictionary<string, TextureType> customTextureNames = new Dictionary<string, TextureType>
        {
            { "_EmissionMap", TextureType.Emission},
            { "_EmissionColor", TextureType.LightColor},
            { "_SpecInt", TextureType.Value }
        };

        public static Dictionary<TextureType, float> textureNameValueDefaults = new Dictionary<TextureType, float>
        {
            { TextureType.Emission, 0.6f },
            { TextureType.LightColor, 2.6f },
            { TextureType.Value, 1.4f }
        };

        public enum TextureType
        {
            Emission,
            LightColor,
            Value
        }
    }
}
