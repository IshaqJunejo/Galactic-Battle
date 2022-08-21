using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    public class Star
    {
        private Texture2D image;
        private Vector2 pos;

        public Star()
        {
            image = Raylib.LoadTexture("Assets/star_small.png");
            pos = new Vector2(Raylib.GetRandomValue(-10000, 10000), Raylib.GetRandomValue(-10000, 10000));
        }

        public void draw()
        {
            Raylib.DrawTextureV(this.image, this.pos, Color.RAYWHITE);
        }
    }
}