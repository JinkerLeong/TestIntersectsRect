using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper
{
    static class VectorHelper
    {
        public static Vector2 Rotate(Vector2 tar, Vector2 origin, float rotation)
        {
            float cos = (float)Math.Cos(rotation);
            float sin = (float)Math.Sin(rotation);
            float tx = tar.X - origin.X;
            float ty = tar.Y - origin.Y;

            return new Vector2
                (
                   tx * cos - ty * sin + origin.X,
                   tx * sin + ty * cos + origin.Y
                );
        }
    }
}
