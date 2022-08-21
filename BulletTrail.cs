using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    // Class for bullets trails
    public class BulletTrail
    {
        // object variables
        private Vector2 pos;
        public float radius;
        public int alpha;
        public Color tint;

        // Constructor
        public BulletTrail(Vector2 pos, float radius)
        {
            this.pos = pos;
            this.radius = radius;
            this.alpha = 255;
            this.tint = new Color(150, 150, 150, alpha);
        }

        // Method to draw bullet trails
        public void draw()
        {
            Raylib.DrawCircleV(this.pos, this.radius, this.tint);
        }
    }
}
