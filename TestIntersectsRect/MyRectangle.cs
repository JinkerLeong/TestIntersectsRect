using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomHelper
{
    class MyRectangle
    {
        private Texture2D texture2D;

        private Vector2 position;
        private int width, height;
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public float X { get => position.X; set => position.X = value; }
        public float Y { get => position.Y; set => position.Y = value; }
        private Vector2 origin;
        public Vector2 Origin { get => origin; set => origin = value; }
        private float rotation;
        public float Rotation { get => rotation; set => rotation = value; }

        private Color rectcolor;
        public Color RectColor { get => rectcolor; set { rectcolor = value; InitialTexture(); } }

        private void InitialRectangle(float x, float y, int w, int h, Color color)
        {
            rectcolor = color;
            position = new Vector2(x, y);
            width = w;
            height = h;
            origin = Vector2.Zero;
            InitialTexture();
        }
        public MyRectangle(float x, float y, int w, int h)
        {
            InitialRectangle(x, y, w, h, Color.White);
        }
        public MyRectangle(float x, float y, int w, int h, Color tarColor)
        {
            InitialRectangle(x, y, w, h, tarColor);
        }

        private void InitialTexture()
        {
            texture2D = new Texture2D(PublicHelper.GraphicsDevice, width, height);
            Color[] colors = new Color[width*height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = rectcolor;
            }
            texture2D.SetData(colors);
        }

        private Vector2[] DefaultVertices()
        {
            return new Vector2[]
            {
                new Vector2(X, Y),
                new Vector2(X + width, Y),
                new Vector2(X + width, Y + height),
                new Vector2(X, Y + height)
            };
        }

        public Vector2[] GetVertices()
        {
            Vector2[] dv = DefaultVertices();
            Vector2 realOrigin = new Vector2();
            for (int i = 0; i < dv.Length; i++)
            {
                dv[i].X -= origin.X;
                dv[i].Y -= origin.Y;
                realOrigin = position;
                dv[i] = VectorHelper.Rotate(dv[i], realOrigin, rotation);
            }
            return dv;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture2D, position, null, Color.White, rotation, origin, Vector2.One, SpriteEffects.None, 0f);
        }

        public Vector2[] GetAxes()
        {
            Vector2[] dv = GetVertices();
            Vector2[] axes = new Vector2[dv.Length];
            Vector2 temp;
            for (int i = 0; i < dv.Length; i++)
            {
                temp = i != dv.Length - 1 ? Vector2.Subtract(dv[i + 1], dv[i]) : Vector2.Subtract(dv[0], dv[i]);
                axes[i] = Vector2.Normalize(new Vector2(temp.Y, -temp.X));
            }

            return axes;
        }

        public class MTV
        {
            public Vector2 Axis;
            public float overlap;
            public Vector2 r1r2;
            public MTV(float _overlap, Vector2 axis)
            {
                Axis = axis;
                overlap = _overlap;
            }
            public MTV()
            {
                Axis = Vector2.Zero;
                overlap = 0f;
            }
        }

        public bool IsCollision(MyRectangle otherRect, out MTV mtv)
        {
            List<Vector2> axes = new List<Vector2>(8);
            axes.AddRange(this.GetAxes());
            axes.AddRange(otherRect.GetAxes());

            Projection a, b;

            float temp;
            mtv = new MTV(float.MaxValue, Vector2.Zero);

            foreach (Vector2 axis in axes)
            {
                a = GetProjection(this, axis);
                b = GetProjection(otherRect, axis);
                temp = a.GetOverlaysSize(b);

                if (!a.IsOverlays(b))
                {
                    return false;
                }

                if (temp < mtv.overlap)
                {
                    mtv.overlap = temp;
                    mtv.Axis = axis;
                }
            }
            mtv.r1r2 = this.position - otherRect.position;
            return true;
        }

        public bool IsCollision(MyRectangle otherRect)
        {
            List<Vector2> axes = new List<Vector2>(8);
            axes.AddRange(this.GetAxes());
            axes.AddRange(otherRect.GetAxes());

            Projection a, b;
            foreach (Vector2 axis in axes)
            {
                a = GetProjection(this, axis);
                b = GetProjection(otherRect, axis);

                if (!a.IsOverlays(b))
                    return false;
            }

            return true;
        }

        private Projection GetProjection(MyRectangle rect, Vector2 axis)
        {
            Vector2[] vertices = rect.GetVertices();

            float max = float.MinValue;
            float min = float.MaxValue;
            float dot;

            foreach (Vector2 vertice in vertices)
            {
                dot = Vector2.Dot(vertice, axis);
                if (dot > max) max = dot;
                if (dot < min) min = dot;
            }

            return new Projection(min, max);
        }

        struct Projection
        {
            public float Min, Max;
            public Projection(float m1, float m2)
            {
                Min = m1;
                Max = m2;
            }
            public float GetOverlaysSize(Projection tar)
            {
                return Math.Min(this.Max, tar.Max) - Math.Max(this.Min, tar.Min);
            }
            public bool IsOverlays(Projection tar)
            {
                return this.Max > tar.Min && tar.Max > this.Min;
            }
        }
    }
}
