﻿using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace TextureReplacer
{
    internal static class SaveManager<T>
    {
        public static void SaveToJson(List<T> saveData, string filePath, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var textureConfigJson = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(filePath, textureConfigJson);
            Console.WriteLine($"Data saved to JSON at {filePath}");
        }

        public static List<T> LoadJsons(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            List<T> configDatas = new List<T>();
            foreach (string file in Directory.EnumerateFiles(folderPath, "*.json"))
            {
                List<T> tempData = new List<T>();
                try
                {
                    tempData = LoadJson(file);
                }
                catch (Exception e)
                {
                    Main.logger.LogError($"Error loading JSON at {file} \nMessage is: {e.Message}");
                }

                configDatas.AddRange(tempData);
            }

            return configDatas;
        }

        public static List<T> LoadJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            string data = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(data);
        }
    }
}
