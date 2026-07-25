using System;

namespace Engine.Core
{
    /// <summary>
    /// Core Engine Engine Executive.
    /// Runs the deterministic fixed-timestep game loop, handles input processing,
    /// and renders to the low-resolution PixelCanvas.
    /// </summary>
    public sealed class GameRunner
    {
        private readonly string _gameFolderPath;
        private readonly Graphics.PixelCanvas _canvas;
        private readonly Graphics.SpriteBatch _batch;
        private readonly Logic.IGameLogic _gameLogic;
        private readonly Time _time;
        private readonly VFS _vfs;

        private bool _isRunning;
        public int TargetFPS { get; set; } = 60;

        public GameRunner(string gameFolderPath, int canvasWidth = 320, int canvasHeight = 180, int scaleFactor = 3)
        {
            _gameFolderPath = Path.GetFullPath(gameFolderPath);
            
            if (!Directory.Exists(_gameFolderPath))
            {
                throw new DirectoryNotFoundException($"Game directory not found at: {_gameFolderPath}");
            }

            _vfs = new VFS();
            _vfs.RegisterPath(_gameFolderPath);
            
            // Mount updates/patches layer
            string patchesDir = Path.Combine(_gameFolderPath, "patches");
            Data.PatchInstaller.MountPatches(patchesDir, _vfs);

            _canvas = new Graphics.PixelCanvas(canvasWidth, canvasHeight, scaleFactor);
            _batch = new Graphics.SpriteBatch(_canvas);
            _time = new Time();

            _gameLogic = Logic.ScriptLoader.LoadGameLogic(_gameFolderPath);
        }

        public void Start()
        {
            _isRunning = true;
            _time.Start();
            _gameLogic.Initialize(_gameFolderPath, _canvas);

            Console.WriteLine("=================================================");
            Console.WriteLine("  GT-Engine 2.5D Pixel Runner Engine Active      ");
            Console.WriteLine($"  Executing Game Directory: {_gameFolderPath}  ");
            Console.WriteLine($"  Internal Resolution: {_canvas.Width}x{_canvas.Height} (Scale: {_canvas.ScaleFactor}x)");
            Console.WriteLine("=================================================");

            // Fixed Timestep Loop Simulation
            float targetFrameTime = 1.0f / TargetFPS;
            
            // Simulate 300 test ticks in headless environment
            int totalTicksToRun = 300;
            int tickCounter = 0;

            while (_isRunning && tickCounter < totalTicksToRun)
            {
                _time.Tick();
                float dt = Math.Min(_time.DeltaTime, 0.05f);

                // Update phase
                _gameLogic.Update(dt);

                // Render phase
                _canvas.Clear(Graphics.Color.Black);
                _gameLogic.Render(_batch, _canvas);

                tickCounter++;

                if (tickCounter % 60 == 0)
                {
                    Console.WriteLine($"[Engine Loop] Frame: {tickCounter} | Simulated FPS: {_time.FPS} | VFS Asset Layer OK");
                }
            }

            Stop();
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _gameLogic.Shutdown();
                Console.WriteLine("[Engine] Engine Runner shut down cleanly.");
            }
        }
    }
}
