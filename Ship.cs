using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    // Class for Ships (Enemies)
    public class Ship
    {
        // Variable to help with calculations
        private float timer;
        private int index;
        public Texture2D shape;
        public Vector2 pos;
        public Vector2 vel;
        public float angle;
        public float speedOffset;
        public float maxHealth;
        public float health;
        public List<Bullet> bullets;

        // Constructors
        public Ship()
        {
            this.index = Raylib.GetRandomValue(0, 4);
            this.shape = Raylib.LoadTexture("Assets/enemy_" + this.index.ToString() + ".png");
            this.pos = new Vector2(Raylib.GetRandomValue(-5000, 5000), Raylib.GetRandomValue(-5000, 5000));
            this.angle = Raylib.GetRandomValue((int) -Math.PI, (int) Math.PI);
            this.bullets = new List<Bullet>(0);
            this.maxHealth = 100.0f;
            this.health = this.maxHealth;
            this.speedOffset = 4.0f;
            this.timer = 0.0f;
        }

        // Method for updating the ships
        public void update(World space, Sound sfxFire)
        {
            if (Raylib.CheckCollisionCircles(this.pos, 1200.0f, space.playerShip.pos, 200.0f * space.playerShip.size))
            {
                if (Raylib.CheckCollisionCircles(this.pos, 300.0f, space.playerShip.pos, 100.0f * space.playerShip.size))
                {
                    this.angle = (float) Math.Atan2(space.playerShip.pos.Y - this.pos.Y, space.playerShip.pos.X - this.pos.X);
                    this.speedOffset = 0.0f;

                    if (timer <= 0.0f)
                    {
                        var newBullet = new Bullet(this.pos, this.angle);
                        this.bullets.Add(newBullet);
                        Raylib.PlaySound(sfxFire);

                        timer = 0.5f;
                    }else
                    {
                        timer -= Raylib.GetFrameTime();
                    }
                    
                }else
                {
                    this.angle = (float) Math.Atan2(space.playerShip.pos.Y - this.pos.Y, space.playerShip.pos.X - this.pos.X);
                    this.speedOffset = 10.0f;
                }
            }else
            {
                if (this.pos.X <= -6000 || this.pos.X >= 6000)
                {
                    this.vel.X *= -1;
                    this.angle = (float) (Math.Atan2(this.vel.Y, this.vel.X));
                };
                if (this.pos.Y <= -6000 || this.pos.Y >= 6000)
                {
                    this.vel.Y *= -1;
                    this.angle = (float) (Math.Atan2(this.vel.Y, this.vel.X));
                };
            }
            Bullet toRemove = null;
            foreach (Bullet bullet in space.playerShip.bullets)
            {
                if (Raylib.CheckCollisionCircles(bullet.pos, bullet.radius, this.pos, this.shape.width / 2))
                {
                    this.health -= 20.0f * space.playerShip.size;
                    toRemove = bullet;
                }
            }

            if (toRemove != null)
            {
                space.playerShip.bullets.Remove(toRemove);
            };

            this.vel.X = (float) Math.Cos(angle) * Raylib.GetFrameTime() * 60 * speedOffset;
            this.vel.Y = (float) Math.Sin(angle) * Raylib.GetFrameTime() * 60 * speedOffset;

            this.pos += this.vel;

            Bullet bulletToRemove = null;
            foreach (Bullet bullet in this.bullets)
            {
                foreach (Meteor meteor in space.asteroids)
                {
                    if (Raylib.CheckCollisionCircles(bullet.pos, bullet.radius, meteor.pos, meteor.shape.width / 2))
                    {
                        bulletToRemove = bullet;
                    }else if (bullet.pos.X >= 7000 || bullet.pos.X <= -7000 || bullet.pos.Y >= 7000 || bullet.pos.Y <= -7000)
                    {
                        bulletToRemove = bullet;
                    }
                }
                bullet.update();
            }
            if (bulletToRemove != null)
            {
                this.bullets.Remove(bulletToRemove);
            }
        }

        // Method for drawing the ships and their bullets
        public void draw(Color tint)
        {
            foreach (Bullet bullet in bullets)
            {
                bullet.draw();
            }
            var rect01 = new Rectangle(0, 0, this.shape.width, this.shape.height);
            var rect02 = new Rectangle(this.pos.X, this.pos.Y, this.shape.width, this.shape.height);
            var origin = new Vector2(this.shape.width / 2, this.shape.height / 2);

            Raylib.DrawTexturePro(this.shape, rect01, rect02, origin, (float) (angle * 180 / Math.PI) + 90.0f, tint);
        }
    }
}
