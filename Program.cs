using System.Numerics;
using Raylib_cs;

// namespace for the Main Project
namespace GalacticBattle
{
    // Main Class for the execution of the game
    static class Program
    {
        // Main function
        public static void Main()
        {
            // Initializing background color
            var Background = new Color(50, 50, 50, 255);
            var transparent = new Color(100, 100, 100, 150);

            // Initializing The Window
            const int Width = 1750;
            const int Height = 1000;
            Raylib.InitWindow(Width, Height, "Galactic Battle");
            // Initializing the sound device
            Raylib.InitAudioDevice();
            Raylib.SetMasterVolume(0.2f);

            // Loading the Sound effects
            Sound sfxCollide = Raylib.LoadSound("Assets/Collide.ogg");
            Sound sfxFire = Raylib.LoadSound("Assets/Fire.ogg");

            Font kenneyFont = Raylib.LoadFont("Assets/Kenney Future Narrow.ttf");

            // Main Camera for the close objects
            Camera2D cam = new Camera2D();
            cam.offset = new Vector2(Width / 2, Height / 2);
            cam.zoom = 1.0f;

            // Second Camera for the Far Objects making parallex effect
            Camera2D parallex = new Camera2D();
            parallex.offset = new Vector2(Width / 2, Height / 2);
            parallex.zoom = 0.75f;

            Camera2D sizeCam = new Camera2D();
            sizeCam.offset = new Vector2(Width / 2, Height / 2);
            sizeCam.zoom = 1.0f;

            // Initializing the Main Game World
            var Space = new World(75, 30, 100);

            // Main Loop with setting target Frame Rate
            Raylib.SetTargetFPS(120);
            while (!Raylib.WindowShouldClose())
            {
                // keeping camera(s) in action
                cam.target = Space.playerShip.pos;
                parallex.target += Space.playerShip.vel * 0.5f;
                sizeCam.target = Space.playerShip.pos;
                if (Space.playerShip.health > 0)
                {                
                    sizeCam.zoom = 1.0f;
                    parallex.zoom = 0.75f * (1 / Space.playerShip.size);
                    cam.zoom = 1 / Space.playerShip.size;

                    // Updating the game world
                    Space.updateWorld(Width, Height, sfxFire, sfxCollide); 
                }else
                {
                    Space.playerShip.vel = Vector2.Zero;
                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
                    {
                        Space.reset(30, 75);
                    }
                }
                
                // Drawing Session
                Raylib.BeginDrawing();
                    // Background color
                    Raylib.ClearBackground(Background);
                                        
                    // Parallex effect camera
                    Raylib.BeginMode2D(parallex);
                        Space.worldParallexEffect();
                    Raylib.EndMode2D();

                    // Main Camera
                    Raylib.BeginMode2D(cam);
                        Space.drawWorld();
                    Raylib.EndMode2D();

                    // Size Camera of Player
                    Raylib.BeginMode2D(sizeCam);
                        Space.drawPlayer();
                    Raylib.EndMode2D();

                    Space.playerShip.DrawHealth(Width, kenneyFont);
                    if (Space.playerShip.health <= 0)
                    {
                        //Raylib.ClearBackground(transparent);
                        Raylib.DrawRectangle(0, 0, Width, Height, transparent);

                        // Restart Menu
                        Vector2 posOfRestart = new Vector2((Width / 2) - (Raylib.MeasureTextEx(kenneyFont, "Press Space to Restart", 48, 5).X / 2), 200);
                        Vector2 posOfScore = new Vector2((Width / 2) - (Raylib.MeasureTextEx(kenneyFont, "Score", 64, 5).X / 2), 400);
                        Vector2 posOfScore2 = new Vector2((Width / 2) - (Raylib.MeasureTextEx(kenneyFont, Space.playerShip.score.ToString(), 64, 5).X / 2), 500);
                        Raylib.DrawTextEx(kenneyFont, "Press Space to Restart", posOfRestart, 48, 5, Color.WHITE);
                        Raylib.DrawTextEx(kenneyFont, "Score", posOfScore, 64, 5, Color.WHITE);
                        Raylib.DrawTextEx(kenneyFont, Space.playerShip.score.ToString(), posOfScore2, 64, 5, Color.WHITE);
                    }
                    // Drawing session ended
                Raylib.EndDrawing();
            }

            // Ending the Game when needed
            Raylib.CloseWindow();
        }
    }
}
