using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContinuousGeneticEnviroment
{
    class Blob
    {
        public CircleShape shape = new CircleShape();
        public Vector2f InitialPosition = new Vector2f(400f, 400f);
        public Vector2f velocity = new Vector2f(0f, 0);
        public Vector2f acceleration;

        public float Size;
        public float Speed;
        public float Health = 250;
        public float maxHealth;
        public bool Alive = true;


        public float wanderStrength;
        Font font;
        
        public Blob(Font fon)
        {
            font = fon;
            var R = (byte)new Random().Next(100,255);
            var G = (byte)new Random().Next(100,255);
            var B = (byte)new Random().Next(100,255);
            shape.FillColor = new Color(R,G,B,100);
            wanderStrength = (float)new Random().NextDouble();
            double random = new Random().NextDouble() * 6.28319f; //generates random angle in radians
            Vector2f randomVector = new Vector2f((float)Math.Cos(random), (float)Math.Sin(random));
            acceleration = randomVector;
            Size = new Random().Next(20,60);
            Health = new Random().Next(100, 500);
            Speed = 1 / Size * 10;
            maxHealth = Size / 2 * Size/(float)Math.Log10(Size);
            shape.Radius = Size;
            shape.Position = InitialPosition;
            shape.Origin = new Vector2f(shape.Radius / 2, shape.Radius / 2);
        }
        public Blob(float size,Color color,float wanderstrength,Font fon)
        {
            font = fon;
            wanderStrength = wanderstrength;
            shape.FillColor = color;
            double random = new Random().NextDouble() * 6.28319f; //generates random angle in radians
            Vector2f randomVector = new Vector2f((float)Math.Cos(random), (float)Math.Sin(random));
            acceleration =  randomVector;
            Size = size;
            Health = new Random().Next(100, 500);
            Speed = 1 / Size * 10;
            shape.Radius = Size;
            maxHealth = Size / 2 * Size;
            shape.Position = InitialPosition;
            shape.Origin = new Vector2f(shape.Radius / 2, shape.Radius / 2);
        }
        void CheckBoundCollision(RenderTarget target, RenderStates states)
        {
            if (shape.Position.X + shape.Radius * 2 > target.Size.X || shape.Position.X < 0)
            {
                velocity = new Vector2f(-velocity.X,velocity.Y);
            }
            if (shape.Position.Y + shape.Radius * 2 > target.Size.Y || shape.Position.Y < 0)
            {
                velocity = new Vector2f(velocity.X, -velocity.Y);
            }
        }
        public void Draw(RenderWindow win, RenderTarget target, RenderStates states)
        {
            var point = target.MapPixelToCoords(Mouse.GetPosition(win));
            if (shape.GetGlobalBounds().Contains(point.X,point.Y))
            {
                string statusText = "Health :" + Health + "\nMaxHealth :" + maxHealth + "\nSize :" + Size;
                Text Status = new Text(statusText, font, 10);
                Status.Position = shape.Position;
                Status.Draw(target, states);
            }
            target.Draw(shape, states);
        }
        public void Update(RenderTarget target, RenderStates states)
        {
            if (new Random().NextDouble() < wanderStrength)
            {
                double random = new Random().NextDouble() * 6.28319f; //generates random angle in radians
                Vector2f randomVector = new Vector2f((float)Math.Cos(random), (float)Math.Sin(random));
                acceleration = randomVector;
            }
            CheckBoundCollision(target, states);
            velocity += acceleration;
            velocity *= 1.5f;
            velocity = new Vector2f(Math.Clamp(velocity.X, -Speed, Speed), Math.Clamp(velocity.Y, -Speed, Speed));
            shape.Position += velocity;
            acceleration *= 0;
            if (Health > maxHealth)
            {
                Health = maxHealth;
                maxHealth+=maxHealth;
            }
            Health--;
        }
        public void ApplyForce(Vector2f force)
        {
            acceleration += force;
        }
    }
}
