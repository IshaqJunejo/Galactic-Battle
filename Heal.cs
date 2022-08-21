using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    public class Heal
    {
        public Vector2 pos;
        private Texture2D image;
        public float radius;

        public Heal(Vector2 position)
        {
            this.image = Raylib.LoadTexture("Assets/icon_plusLarge.png");
            this.radius = image.width / 2;
            this.pos = new Vector2(position.X - this.radius, position.Y - this.radius);
        }

        public void draw(Color tint)
        {
            Raylib.DrawTextureV(this.image, pos, tint);
        }
    }
}
