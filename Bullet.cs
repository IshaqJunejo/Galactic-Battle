using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    // Class for the bullets
    public class Bullet
    {
        // personal data (don't sneak)
        public Vector2 pos;
        private float angle;
        private float speedOffset;
        public float radius;
        private List<BulletTrail> trails;

        // Constructor
        public Bullet(Vector2 pos, float angle)
        {
            this.radius = 5.0f;
            this.pos = pos;
            this.angle = angle;
            this.speedOffset = 20.0f;
            this.trails = new List<BulletTrail>(0);
        }

        // Method for updating the bullets
        public void update()
        {
            // moving the bullets
            float deltaX = (float) Math.Cos(angle) * Raylib.GetFrameTime() * 60 * speedOffset;
            float deltaY = (float) Math.Sin(angle) * Raylib.GetFrameTime() * 60 * speedOffset;

            this.pos.X += deltaX;
            this.pos.Y += deltaY;

            // updating the bullet trails
            BulletTrail toRemove = null;
            foreach (BulletTrail trail in this.trails)
            {
                if (trail.alpha <= 0)
                {
                    toRemove = trail;
                }else
                {
                    trail.alpha -= 5;
                    trail.radius /= 1.01f;
                }
                trail.tint = new Color(150, 150, 150, trail.alpha);
            }

            if (toRemove != null)
            {
                this.trails.Remove(toRemove);
            }
            var newTrail = new BulletTrail(this.pos, this.radius);
            this.trails.Add(newTrail);
        }

        // Method to draw the bullets and their trails
        public void draw()
        {
            foreach (BulletTrail trail in this.trails)
            {
                trail.draw();
            }
            Raylib.DrawCircleV(this.pos, this.radius, Color.WHITE);
        }
    }
}
