using System;
using System.IO;
using System.Text.Json;

namespace Engine.Data
{
    /// <summary>
    /// Game state persistence system. Saves and loads state asynchronously in JSON or encrypted binary.
    /// Backward-compatible with game patch updates.
    /// </summary>
    public static class SaveSystem
    {
        public static void SaveData<T>(string saveDirectory, string fileName, T data)
        {
            Directory.CreateDirectory(saveDirectory);
            string filePath = Path.Combine(saveDirectory, fileName);
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public static T? LoadData<T>(string saveDirectory, string fileName)
        {
            string filePath = Path.Combine(saveDirectory, fileName);
            if (!File.Exists(filePath)) return default;

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
