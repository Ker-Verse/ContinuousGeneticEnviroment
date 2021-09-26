using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContinuousGeneticEnviroment
{
    class Food
    {
        public Vector2f position;
        public CircleShape shape = new CircleShape(5,5);
        public Food(Vector2f pos)
        {
            shape.FillColor = Color.Green;
            position = pos;
            shape.Position = position;
        }
        public void Show(RenderTarget target,RenderStates state)
        {
            target.Draw(shape);
        }
    }
}
