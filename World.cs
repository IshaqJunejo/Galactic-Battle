using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    // Class for the game world
    public class World
    {
        // object variables (Lists of different game objects)
        private Color enemyTint;
        public List<Ship> enemies;
        public List<Meteor> asteroids;
        public List<Station> stations;
        public Player playerShip;
        public List<Heal> healers;
        public List<Star> stars;

        // Constructor 
        public World(int numOfAsteroids, int numOfEnemies, int numOfStations)
        {
            enemyTint = new Color(Raylib.GetRandomValue(30, 255), Raylib.GetRandomValue(80, 255), Raylib.GetRandomValue(0, 99), 255);
            this.enemies = new List<Ship>(0);
            for (int i = 0; i < numOfEnemies; i++)
            {
                var newEnemy = new Ship();
                this.enemies.Add(newEnemy);
            };

            this.asteroids = new List<Meteor>(0);
            for (int i = 0; i < numOfAsteroids; i++)
            {
                var newAsteroid = new Meteor();
                this.asteroids.Add(newAsteroid);
            };

            this.stations = new List<Station>(0);
            for (int i = 0; i < numOfStations; i++)
            {
                var newStation = new Station();
                this.stations.Add(newStation);
            };

            this.stars = new List<Star>(0);
            for (int i = 0; i < 150; i++)
            {
                var newStar = new Star();
                this.stars.Add(newStar);
            };

            this.healers = new List<Heal>(0);
            this.playerShip = new Player();
        }

        // Method for Updating the Game World
        public void updateWorld(int w, int h, Sound sfxFire, Sound sfxCollide)
        {
            foreach (Ship enemy in this.enemies)
            {
                if (enemy.health <= 0)
                {
                    var newHeal = new Heal(enemy.pos);
                    this.healers.Add(newHeal);
                    enemy.health = enemy.maxHealth;
                    enemy.pos = new Vector2(Raylib.GetRandomValue(-5000, 5000), Raylib.GetRandomValue(-5000, 5000));
                }else
                {
                    enemy.update(this, sfxFire);
                }
            }

            Meteor meteorToRemove = null;
            foreach (Ship enemy in this.enemies)
            {
                foreach (Meteor meteor in this.asteroids)
                {
                    if (Raylib.CheckCollisionCircles(enemy.pos, enemy.shape.width / 2, meteor.pos, meteor.shape.width / 2))
                    {
                        meteorToRemove = meteor;
                        enemy.health = 0.0f;
                        if ((this.playerShip.pos.X - enemy.pos.X) * (this.playerShip.pos.X - enemy.pos.X) + (this.playerShip.pos.Y - enemy.pos.Y) * (this.playerShip.pos.Y - enemy.pos.Y) <= 1300 * 1300)
                        {
                            Raylib.PlaySound(sfxCollide);
                        }
                    }
                }
            }
            if (meteorToRemove != null)
            {
                this.asteroids.Remove(meteorToRemove);
            };
            playerShip.updatePlayer(w, h, this, sfxFire, sfxCollide);
        }

        // Method to draw the object for main camera
        public void drawWorld()
        {
            foreach (Ship enemy in this.enemies)
            {
                enemy.draw(enemyTint);
            }
            foreach (Meteor asteroid in this.asteroids)
            {
                asteroid.draw();
            }
            foreach (Heal heal in this.healers)
            {
                heal.draw(enemyTint);
            }

            foreach (Bullet bull in this.playerShip.bullets)
            {
                bull.draw();
            }
        }

        // Method to draw objects for parallex camera
        public void worldParallexEffect()
        {
            foreach (Station station in this.stations)
            {
                station.draw();
            }
            foreach (Star star in this.stars)
            {
                star.draw();
            }
        }

        public void drawPlayer()
        {
            this.playerShip.drawPlayer();
        }

        public void reset(int numOfEnemies, int numOfAsteroids)
        {
            this.asteroids.Clear();
            this.healers.Clear();
            this.enemies.Clear();
            for (int i = 0; i < numOfEnemies; i++)
            {
                var enem = new Ship();
                this.enemies.Add(enem);
            };
            for (int i = 0; i < numOfAsteroids; i++)
            {
                var meteor = new Meteor();
                this.asteroids.Add(meteor);
            };

            this.playerShip.health = this.playerShip.maxHealth;
            this.playerShip.pos = new Vector2(0, 0);
            this.playerShip.size = 0.8f;
            this.playerShip.score = 0;
            this.enemyTint = new Color(Raylib.GetRandomValue(30, 255), Raylib.GetRandomValue(80, 255), Raylib.GetRandomValue(0, 99), 255);
        }
    }
}
