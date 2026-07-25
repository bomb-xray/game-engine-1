using System;

namespace Engine.Graphics
{
    /// <summary>
    /// 2.5D Camera system with smooth target tracking, parallax layer calculation, and screen shake.
    /// </summary>
    public sealed class Camera2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Zoom { get; set; } = 1.0f;

        private float _shakeTime;
        private float _shakeIntensity;
        private readonly Random _random = new Random();

        public Camera2D(float x = 0, float y = 0)
        {
            X = x;
            Y = y;
        }

        public void Follow(float targetX, float targetY, float lerpSpeed, float deltaTime)
        {
            X += (targetX - X) * Math.Clamp(lerpSpeed * deltaTime, 0f, 1f);
            Y += (targetY - Y) * Math.Clamp(lerpSpeed * deltaTime, 0f, 1f);
        }

        public void Shake(float intensity, float duration)
        {
            _shakeIntensity = intensity;
            _shakeTime = duration;
        }

        public void Update(float deltaTime)
        {
            if (_shakeTime > 0)
            {
                _shakeTime -= deltaTime;
                if (_shakeTime <= 0) _shakeIntensity = 0;
            }
        }

        public (int OffX, int OffY) GetRenderOffset()
        {
            int rx = (int)MathF.Round(X);
            int ry = (int)MathF.Round(Y);

            if (_shakeTime > 0)
            {
                rx += _random.Next(-(int)_shakeIntensity, (int)_shakeIntensity + 1);
                ry += _random.Next(-(int)_shakeIntensity, (int)_shakeIntensity + 1);
            }

            return (rx, ry);
        }
    }
}
