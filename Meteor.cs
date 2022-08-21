using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    // Class for Meteors
    public class Meteor
    {
        // Private variables
        private int index;
        public Texture2D shape;
        public Vector2 pos;

        // Constructor
        public Meteor()
        {
            this.index = Raylib.GetRandomValue(0, 3);
            this.shape = Raylib.LoadTexture("Assets/meteor_"+ this.index.ToString() + ".png");
            this.pos = new Vector2(Raylib.GetRandomValue(-2500, 2500), Raylib.GetRandomValue(-2500, 2500));
        }
        // Drawing method for meteors
        public void draw()
        {
            Raylib.DrawTexture(this.shape, (int) (this.pos.X - (this.shape.width / 2)), (int) (this.pos.Y - (this.shape.height / 2)), Color.WHITE);
        }
    }
}
