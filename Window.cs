using System;
using System.Collections.Generic;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace ContinuousGeneticEnviroment
{


    class Window
    {
        //Initialization values
        private const int WIDTH = 1280;
        private const int HEIGHT = 720;
        private const string TITLE = "Ecosystem";
        private const int FPS = 60;
        private RenderWindow window;
        

        

        

        //FPS Counter values
        List<float> fps = new List<float>(60);
        Clock clock = new Clock();
        Time previousTime;
        Time currentTime;


        //Entities And Entity Variables
        List<Blob> Blobs = new List<Blob>();
        public int foodCount = 500;
        public int InitialCount = 200;
        public float ReproductionRate = 41.2f;
        public float FoodSpawnRate = 20f;
        public float mutationRate = 20f;
        public List<Food> Foods = new List<Food>();



        //System variables
        float fpsAvg = 60;
        public Window()
        {
            for (int i = 0; i < InitialCount; i++)
            {
                Blobs.Add(new Blob());
            }
            
            //Window Initialization
            this.window = new RenderWindow(new VideoMode(WIDTH,HEIGHT), TITLE);
            for (int i = 0; i < foodCount; i++)
            {
                Vector2f rand = new Vector2f(new Random().Next(0, (int)window.Size.X), new Random().Next(0, (int)window.Size.X));
                Foods.Add(new Food(rand));
            }
            window.SetFramerateLimit(FPS);
            window.Closed += (sender, args) => { this.window.Close(); };
            run();
        }

        public void run()
        {
            while (this.window.IsOpen)
            {
                this.handleEvents();
                this.update();
                this.draw();
                this.CountFPS();
            }
        }
        private void update()
        {
            for (int s = 0; s < 1; s++)
            {
                if (new Random().NextDouble() < FoodSpawnRate / 100)
                {
                    Vector2f rand = new Vector2f(new Random().Next(0, (int)window.Size.X), new Random().Next(0, (int)window.Size.Y));
                    Foods.Add(new Food(rand));
                }
                for (int i = 0; i < Blobs.Count; i++)
                {
                    if (Blobs[i].Health > 0)
                    {
                        Blobs[i].Update(window, RenderStates.Default);
                        for (int f = 0; f < Foods.Count; f++)
                        {
                            if (Blobs[i].shape.GetGlobalBounds().Intersects(Foods[f].shape.GetGlobalBounds()))
                            {
                                Foods.Remove(Foods[f]);
                                Blobs[i].Health += 50;
                                if (new Random().NextDouble() < ReproductionRate / 100)
                                {
                                    Blob newBorn = new Blob(Blobs[i].Size, Blobs[i].shape.FillColor,Blobs[i].wanderStrength);
                                    if (new Random().NextDouble() < mutationRate / 100)
                                    {
                                        newBorn = new Blob();
                                    }
                                    newBorn.shape.Position = Blobs[i].shape.Position;
                                    newBorn.Speed = 1 / newBorn.Size * 50;
                                    Blobs.Add(newBorn);
                                }
                            }
                        }
                    }
                    else
                    {
                        var DeathFood = new Food(Blobs[i].shape.Position);
                        DeathFood.shape.FillColor = Blobs[i].shape.FillColor;
                        Foods.Add(DeathFood);
                        Blobs.RemoveAt(i);
                    }
                }
            }
        }

        private void draw()
        {
            this.window.Clear(Color.Black);
            foreach (var blob in Blobs)
            {
                blob.Draw(window, RenderStates.Default);
            }
            for (int i = 0; i < Foods.Count; i++)
            {
                Foods[i].Show(window,RenderStates.Default);
            }
            this.window.Display();
        }
        private void handleEvents()
        {
            this.window.DispatchEvents();
        }
        private void CountFPS()
        {
            currentTime = clock.ElapsedTime;
            fps.Add(1.0f / (currentTime.AsSeconds() - previousTime.AsSeconds())); // the asSeconds returns a float
            float sum = 0;
            foreach (var item in fps)
            {
                sum += item;
            }
            if (fps.Count > 500)
            {
                fps.RemoveRange(0, 350);
            }
            fpsAvg = sum;
            window.SetTitle(Math.Floor(sum / fps.Count) + "FPS ,Population :" + Blobs.Count + "Food Amount :" + Foods.Count);
            previousTime = currentTime;
        }    
    }
}
