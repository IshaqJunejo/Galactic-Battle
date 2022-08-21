using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    // Class for the Player (inherited from the Enemy's Class)
    public class Player : Ship
    {
        // Color being selected randomly for the Player's ship
        private Color tint = new Color(Raylib.GetRandomValue(30, 255), Raylib.GetRandomValue(80, 255), Raylib.GetRandomValue(0, 99), 255);
        public float size = 0.8f;
        public int score = 0;

        // Constructor
        public Player()
        {
            this.shape = Raylib.LoadTexture("Assets/ship_sidesB.png");
            this.speedOffset = 12.0f;
            this.pos = new Vector2(0, 0);
        }

        // Method for updating the Player
        public void updatePlayer(int w, int h, World space, Sound sfxFire, Sound sfxCollide)
        {
            // setting angle of the Player
            this.angle = ((float) Math.Atan2((h / 2) - Raylib.GetMouseY(), (w / 2) - Raylib.GetMouseX()) * 180.0f / (float) Math.PI) - 90.0f;
            // Reducing the velocity of the Player if the key is not pressed to increase it
            this.vel *= 0.99f;

            // If Space_Key is pressed, move the player
            if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE))
            {
                this.vel.X = (float) Math.Cos((this.angle - 90) * Math.PI / 180.0) * Raylib.GetFrameTime() * 60 * this.speedOffset;
                this.vel.Y = (float) Math.Sin((this.angle - 90) * Math.PI / 180.0) * Raylib.GetFrameTime() * 60 * this.speedOffset;
            };

            // Shoot bullets
            if (Raylib.IsMouseButtonPressed(0))
            {
                Raylib.PlaySound(sfxFire);
                var ammo = new Bullet(this.pos, (this.angle - 90) * (float) Math.PI / 180.0f);
                this.bullets.Add(ammo);
            };

            // Updating the bullets
            foreach (Bullet bullet in bullets)
            {
                bullet.update();
            }

            Meteor meteorToRemove = null;
            foreach (Meteor meteor in space.asteroids)
            {
                Bullet bulletToRemove = null;
                foreach (Bullet bullet in bullets)
                {
                    if (Raylib.CheckCollisionCircles(bullet.pos, bullet.radius, meteor.pos, meteor.shape.width / 2.0f))
                    {
                        bulletToRemove = bullet;
                    }else if (bullet.pos.X >= 7000 || bullet.pos.X <= -7000 || bullet.pos.Y >= 7000 || bullet.pos.Y <= -7000)
                    {
                        bulletToRemove = bullet;
                    }
                }
                if (bulletToRemove != null)
                {
                    bullets.Remove(bulletToRemove);
                }
                var selfRect = new Rectangle(this.pos.X, this.pos.Y, this.shape.width, this.shape.height);
                if (Raylib.CheckCollisionCircles(meteor.pos, meteor.shape.width / 2.0f, this.pos, this.shape.width * this.size / 2.0f))
                {
                    this.health -= 25.0f / this.size;
                    meteorToRemove = meteor;
                    Raylib.PlaySound(sfxCollide);
                }
            }
            if (meteorToRemove != null)
            {
                space.asteroids.Remove(meteorToRemove);
            }

            foreach (Ship enemy in space.enemies)
            {
                Bullet bulletToRemove = null;
                foreach (Bullet bullet in enemy.bullets)
                {
                    if (Raylib.CheckCollisionCircles(bullet.pos, bullet.radius, this.pos, this.shape.width * this.size / 2))
                    {
                        bulletToRemove = bullet;
                        this.health -= 20.0f / this.size;
                    }
                }
                if (bulletToRemove != null)
                {
                    enemy.bullets.Remove(bulletToRemove);
                }
            }
            Heal toRemove = null;
            foreach (Heal heal in space.healers)
            {            
                if (Raylib.CheckCollisionCircles(this.pos, this.shape.width * this.size / 2, heal.pos, heal.radius))
                {
                    toRemove = heal;
                    this.size += 0.05f;
                    this.health += 10.0f;
                    this.score++;
                    if (this.health >= this.maxHealth)
                    {
                        this.health = this.maxHealth;
                    };
                }
            }
            if (toRemove != null)
            {
                space.healers.Remove(toRemove);
            };
            // updating the position according to its velocity
            this.pos += this.vel;
        }

        // Method for drawing the Player
        public void drawPlayer()
        {
            var rect01 = new Rectangle(0, 0, this.shape.width, this.shape.height);
            var rect02 = new Rectangle(this.pos.X, this.pos.Y, this.shape.width, this.shape.height);
            var origin = new Vector2(this.shape.width / 2, this.shape.height / 2);

            Raylib.DrawTexturePro(this.shape, rect01, rect02, origin, angle, tint);
        }

        public void DrawHealth(int w, Font font)
        {
            var rect = new Rectangle((w / 2) - 200, 20, 400, 50);
            var rectHeal = new Rectangle((w / 2) - 200, 20, this.health * 4, 50);
            Vector2 scorePos = new Vector2(30, 20);
            Raylib.DrawRectangleRoundedLines(rect, 25.0f, 8, 5.0f, Color.WHITE);
            Raylib.DrawRectangleRounded(rectHeal, 25.0f, 8, this.tint);
            Raylib.DrawTextEx(font, this.score.ToString(), scorePos, 64, 5, this.tint);
        }
    }
}
