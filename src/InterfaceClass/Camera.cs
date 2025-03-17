using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dungeon.src.InterfaceClass
{
    public class Camera
    {
        private Vector2 position;
        private readonly Viewport viewport;
        private readonly float lerpFactor = 0.05f;

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
            position = Vector2.Zero;
        }

        public void Update(Vector2 playerPosition)
        {
            Vector2 targetPosition = playerPosition - new Vector2(viewport.Width / 2, viewport.Height / 2);
            position = Vector2.Lerp(position, targetPosition, lerpFactor);
        }

        public void Reset()
        {
            position = Vector2.Lerp(position, Vector2.Zero, lerpFactor);
        }

        public Matrix GetViewMatrix()
        {
            return Matrix.CreateTranslation(new Vector3(-position, 0));
        }
    }
}