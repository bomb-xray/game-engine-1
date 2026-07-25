using System;
using System.IO;
using System.Reflection;

namespace Engine.Logic
{
    /// <summary>
    /// Game logic loader. Dynamically discovers and instantiates the game's IGameLogic implementation
    /// from compiled game assemblies or embedded game modules inside `./game`.
    /// </summary>
    public static class ScriptLoader
    {
        public static IGameLogic LoadGameLogic(string gameFolderPath)
        {
            string dllPath = Path.Combine(gameFolderPath, "Game.dll");

            if (File.Exists(dllPath))
            {
                Console.WriteLine($"[ScriptLoader] Found compiled Game assembly at: {dllPath}");
                var assembly = Assembly.LoadFrom(dllPath);
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IGameLogic).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        var instance = Activator.CreateInstance(type) as IGameLogic;
                        if (instance != null)
                        {
                            Console.WriteLine($"[ScriptLoader] Instantiated game entry point: {type.FullName}");
                            return instance;
                        }
                    }
                }
            }

            Console.WriteLine("[ScriptLoader] Loading default engine fallback game mode...");
            return new FallbackGameLogic();
        }
    }

    /// <summary>
    /// Default game logic fallback if no custom Game.dll is compiled yet in `./game`.
    /// Draws a Blasphemous-style 2.5D dark pixel scene demonstration.
    /// </summary>
    public class FallbackGameLogic : IGameLogic
    {
        private float _timeCounter;
        private float _playerX = 160;
        private float _playerY = 120;
        private float _playerVY = 0;
        private bool _isGrounded = true;

        public void Initialize(string gameFolderPath, Graphics.PixelCanvas canvas)
        {
            Console.WriteLine("[Game] Fallback Game logic initialized.");
        }

        public void Update(float deltaTime)
        {
            _timeCounter += deltaTime;

            // Simulate simple physics for 2.5D character
            if (!_isGrounded)
            {
                _playerVY += 400f * deltaTime; // Gravity
                _playerY += _playerVY * deltaTime;

                if (_playerY >= 120)
                {
                    _playerY = 120;
                    _playerVY = 0;
                    _isGrounded = true;
                }
            }

            // Periodic jump demo
            if (_timeCounter % 3.0f < 0.05f && _isGrounded)
            {
                _playerVY = -150f;
                _isGrounded = false;
            }
        }

        public void Render(Graphics.SpriteBatch batch, Graphics.PixelCanvas canvas)
        {
            // Background - Dark Gothic Sky
            batch.DrawRect(0, 0, canvas.Width, canvas.Height, new Graphics.Color(15, 12, 28));

            // Distant Mountains (Parallax Layer 1)
            int bgOffset1 = (int)(MathF.Sin(_timeCounter * 0.5f) * 10);
            batch.DrawRect(bgOffset1, 80, canvas.Width, 60, new Graphics.Color(28, 25, 45));

            // Gothic Ruins Foreground (Parallax Layer 2)
            batch.DrawRect(0, 140, canvas.Width, 40, new Graphics.Color(45, 40, 60)); // Ground floor

            // Decorative Pillars
            batch.DrawRect(30, 40, 20, 100, new Graphics.Color(35, 30, 50));
            batch.DrawRect(270, 40, 20, 100, new Graphics.Color(35, 30, 50));

            // Player Pixel Sprite (2.5D Penitent One style)
            int px = (int)_playerX;
            int py = (int)_playerY;
            batch.DrawRect(px - 6, py - 20, 12, 20, new Graphics.Color(180, 50, 50)); // Body
            batch.DrawRect(px - 2, py - 28, 4, 8, new Graphics.Color(220, 200, 120)); // High Conical Helmet

            // Red Health/Faith Bar HUD (Pixel Perfect)
            batch.DrawRect(10, 10, 80, 8, new Graphics.Color(40, 10, 10));
            batch.DrawRect(11, 11, 60, 6, new Graphics.Color(200, 30, 30));
        }

        public void Shutdown()
        {
            Console.WriteLine("[Game] Fallback Game logic shutdown.");
        }
    }
}
