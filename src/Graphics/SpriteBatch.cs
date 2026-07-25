using System;

namespace Engine.Graphics
{
    public struct Sprite
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Color Tint;
        public bool FlipX;
        public bool FlipY;
    }

    /// <summary>
    /// Zero-Allocation Sprite Batch Renderer.
    /// Draws sprites, tilemaps, and parallax background layers onto the PixelCanvas.
    /// </summary>
    public sealed class SpriteBatch
    {
        private readonly PixelCanvas _canvas;

        public SpriteBatch(PixelCanvas canvas)
        {
            _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        }

        public void DrawPixel(int x, int y, Color color)
        {
            _canvas.SetPixel(x, y, color);
        }

        public void DrawRect(int x, int y, int width, int height, Color color, bool filled = true)
        {
            if (filled)
            {
                for (int py = y; py < y + height; py++)
                {
                    for (int px = x; px < x + width; px++)
                    {
                        _canvas.SetPixel(px, py, color);
                    }
                }
            }
            else
            {
                for (int px = x; px < x + width; px++)
                {
                    _canvas.SetPixel(px, y, color);
                    _canvas.SetPixel(px, y + height - 1, color);
                }
                for (int py = y; py < y + height; py++)
                {
                    _canvas.SetPixel(x, py, color);
                    _canvas.SetPixel(x + width - 1, py, color);
                }
            }
        }

        public void DrawParallaxLayer(int offsetX, int offsetY, float parallaxFactorX, float parallaxFactorY, Action<SpriteBatch, int, int> renderLayer)
        {
            int px = (int)(offsetX * parallaxFactorX);
            int py = (int)(offsetY * parallaxFactorY);
            renderLayer(this, px, py);
        }
    }
}
