using Engine.Graphics;

namespace Engine.Logic
{
    /// <summary>
    /// Contract implemented by custom game scripts in the `./game` folder.
    /// The engine calls these lifecycle hooks during the main game loop.
    /// </summary>
    public interface IGameLogic
    {
        void Initialize(string gameFolderPath, PixelCanvas canvas);
        void Update(float deltaTime);
        void Render(SpriteBatch batch, PixelCanvas canvas);
        void Shutdown();
    }
}
