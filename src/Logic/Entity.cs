using Engine.Physics;
using Engine.Graphics;

namespace Engine.Logic
{
    /// <summary>
    /// Base game entity class designed for object pooling to prevent Garbage Collection pauses.
    /// </summary>
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Entity";
        public bool IsActive { get; set; } = true;

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; } // Z order for 2.5D depth sorting
        public float VX { get; set; }
        public float VY { get; set; }

        public int Width { get; set; } = 16;
        public int Height { get; set; } = 16;

        public Color Tint { get; set; } = Color.White;

        public AABB Bounds => new AABB(X, Y, Width, Height);

        public virtual void OnSpawn() { }
        public virtual void OnUpdate(float deltaTime) { }
        public virtual void OnRender(SpriteBatch batch, int camOffsetX, int camOffsetY) { }
        public virtual void OnDespawn() { }
    }
}
