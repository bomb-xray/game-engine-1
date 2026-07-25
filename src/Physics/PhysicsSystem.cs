using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Engine.Physics
{
    /// <summary>
    /// Grid-based physics engine supporting static collision boundaries and dynamic bodies.
    /// Ideal for Blasphemous-style precise pixel platforming.
    /// </summary>
    public sealed class PhysicsSystem
    {
        private readonly List<AABB> _staticColliders = new List<AABB>();

        public void AddStaticCollider(float x, float y, float width, float height)
        {
            _staticColliders.Add(new AABB(x, y, width, height));
        }

        public void ClearStaticColliders()
        {
            _staticColliders.Clear();
        }

        public bool CheckCollision(in AABB box)
        {
            for (int i = 0; i < _staticColliders.Count; i++)
            {
                if (box.Intersects(_staticColliders[i]))
                    return true;
            }
            return false;
        }

        public ReadOnlyCollection<AABB> GetStaticColliders() => _staticColliders.AsReadOnly();
    }
}
