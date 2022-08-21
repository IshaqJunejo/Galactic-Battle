using System.Numerics;
using Raylib_cs;

namespace GalacticBattle
{
    // Class for space stations (Far Objects)
    public class Station
    {
        // private variables
        private int index;
        private Texture2D shape;
        private Vector2 pos;
        private Color fade;
        
        // Constructor
        public Station()
        {
            this.index = Raylib.GetRandomValue(0, 6);
            this.shape = Raylib.LoadTexture("Assets/station_" + this.index.ToString() + ".png");
            this.pos = new Vector2(Raylib.GetRandomValue(-4000, 4000), Raylib.GetRandomValue(-4000, 4000));

            int colorValues = Raylib.GetRandomValue(60, 90);
            this.fade = new Color(colorValues, colorValues, colorValues, 255);
        }

        // Method for Drawing the stations
        public void draw()
        {
            Raylib.DrawTextureV(this.shape, this.pos, this.fade);
        }
    }
}
