using System;

namespace Engine.Graphics
{
    /// <summary>
    /// Virtual Low-Resolution Pixel Framebuffer (e.g. 640x360 or 320x180).
    /// Renders pixel art on a fixed virtual grid with zero-allocation memory buffers,
    /// enabling 60+ FPS performance on low-end hardware like GT 630.
    /// </summary>
    public sealed class PixelCanvas
    {
        public int Width { get; }
        public int Height { get; }
        public int ScaleFactor { get; private set; }

        private readonly Color[] _pixelBuffer;

        public PixelCanvas(int width, int height, int scaleFactor = 3)
        {
            Width = width;
            Height = height;
            ScaleFactor = Math.Max(1, scaleFactor);
            _pixelBuffer = new Color[width * height];
            Clear(Color.Black);
        }

        public void Clear(Color color)
        {
            Array.Fill(_pixelBuffer, color);
        }

        public void SetPixel(int x, int y, Color color)
        {
            if ((uint)x >= (uint)Width || (uint)y >= (uint)Height) return;

            // Simple alpha blending
            if (color.A == 255)
            {
                _pixelBuffer[y * Width + x] = color;
            }
            else if (color.A > 0)
            {
                int index = y * Width + x;
                Color bg = _pixelBuffer[index];
                float alpha = color.A / 255f;
                _pixelBuffer[index] = Color.Lerp(bg, color, alpha);
            }
        }

        public Color GetPixel(int x, int y)
        {
            if ((uint)x >= (uint)Width || (uint)y >= (uint)Height) return Color.Clear;
            return _pixelBuffer[y * Width + x];
        }

        public ReadOnlySpan<Color> GetRawBuffer() => _pixelBuffer.AsSpan();

        public void SetScaleFactor(int scale)
        {
            ScaleFactor = Math.Max(1, scale);
        }
    }
}
