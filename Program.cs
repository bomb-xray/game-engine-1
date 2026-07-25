using System;
using System.IO;
using Engine.Core;

namespace Engine
{
    /// <summary>
    /// Executive Entry Point.
    /// Acts as the command-line game executor (Runner).
    /// Usage: engine [path-to-game-folder]
    /// Default: looks for "./game" folder right next to the executable.
    /// NO Editor GUI is included - pure runtime execution environment.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("      GT-Engine Proprietary C# Game Runner        ");
            Console.WriteLine("--------------------------------------------------");

            // Determine target game directory (arg or default to ./game adjacent to executive)
            string gameFolder = args.Length > 0 ? args[0] : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "game");

            if (!Directory.Exists(gameFolder))
            {
                // If adjacent ./game does not exist yet, create default game folder structure
                Console.WriteLine($"[Main] Target game directory '{gameFolder}' not found. Creating default ./game workspace...");
                Directory.CreateDirectory(gameFolder);
                Directory.CreateDirectory(Path.Combine(gameFolder, "assets"));
                Directory.CreateDirectory(Path.Combine(gameFolder, "assets", "sprites"));
                Directory.CreateDirectory(Path.Combine(gameFolder, "assets", "audio"));
                Directory.CreateDirectory(Path.Combine(gameFolder, "assets", "levels"));
                Directory.CreateDirectory(Path.Combine(gameFolder, "patches"));
                
                // Write default game configuration
                string configPath = Path.Combine(gameFolder, "config.json");
                string defaultConfig = @"{
  ""GameTitle"": ""Pixel Gothic Odyssey"",
  ""InternalWidth"": 320,
  ""InternalHeight"": 180,
  ""ScaleFactor"": 4,
  ""TargetFPS"": 60,
  ""VSync"": true
}";
                File.WriteAllText(configPath, defaultConfig);
            }

            try
            {
                var visualGame = new VisualGame(gameFolder);
                visualGame.Run();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Engine Fatal Error]: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
        }
    }
}
