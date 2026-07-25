namespace Engine.Physics
{
    /// <summary>
    /// Fast 2.5D Axis-Aligned Bounding Box (AABB) struct.
    /// Uses zero-allocation value types for 60 FPS performance on weak CPUs.
    /// </summary>
    public struct AABB
    {
        public float X;
        public float Y;
        public float Width;
        public float Height;

        public AABB(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public readonly bool Intersects(in AABB other)
        {
            return X < other.X + other.Width &&
                   X + Width > other.X &&
                   Y < other.Y + other.Height &&
                   Y + Height > other.Y;
        }

        public readonly bool Contains(float px, float py)
        {
            return px >= X && px <= X + Width && py >= Y && py <= Y + Height;
        }
    }
}
