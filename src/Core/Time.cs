using System.Diagnostics;

namespace Engine.Core
{
    /// <summary>
    /// High-precision frame timing system with FPS counter and delta smoothing.
    /// </summary>
    public sealed class Time
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private long _lastTicks;

        public float DeltaTime { get; private set; }
        public float TotalTime { get; private set; }
        public int FPS { get; private set; }

        private int _frameCount;
        private float _fpsTimer;

        public void Start()
        {
            _stopwatch.Start();
            _lastTicks = _stopwatch.ElapsedTicks;
        }

        public void Tick()
        {
            long currentTicks = _stopwatch.ElapsedTicks;
            long elapsedTicks = currentTicks - _lastTicks;
            _lastTicks = currentTicks;

            DeltaTime = (float)elapsedTicks / Stopwatch.Frequency;
            // Cap delta time to prevent physics clipping on lag spikes
            if (DeltaTime > 0.1f) DeltaTime = 0.1f;

            TotalTime += DeltaTime;

            _frameCount++;
            _fpsTimer += DeltaTime;
            if (_fpsTimer >= 1.0f)
            {
                FPS = _frameCount;
                _frameCount = 0;
                _fpsTimer -= 1.0f;
            }
        }
    }
}
