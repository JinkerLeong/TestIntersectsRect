using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CustomHelper;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System;
using System.Threading;

namespace TestIntersectsRect
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        MyRectangle rect1;
        MyRectangle rect2;
        SpriteFont font;
        KeyboardState lastKeyboardState;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            PublicHelper.GraphicsDevice = this.GraphicsDevice;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            rect1 = new MyRectangle(50, 50, 100, 100) { Origin = new Vector2(50) };
            rect2 = new MyRectangle(200, 200, 100, 100, Color.Green);

            font = Content.Load<SpriteFont>("sss");
            // TODO: use this.Content to load your game content here
        }

        float speed = 2f;
        float rotationSpeed = 0.2f;
        Vector2 mtv;
        string result = "";
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState nowKeyboardState = Keyboard.GetState();

            if (nowKeyboardState.IsKeyDown(Keys.W))
                rect1.Y -= speed;
            if (nowKeyboardState.IsKeyDown(Keys.S))
                rect1.Y += speed;
            if (nowKeyboardState.IsKeyDown(Keys.A))
                rect1.X -= speed;
            if (nowKeyboardState.IsKeyDown(Keys.D))
                rect1.X += speed;
            if (nowKeyboardState.IsKeyDown(Keys.R))
                rect1.Rotation += rotationSpeed;
            result = "Real Origin: " + rect1.RealOrigin.ToString() +
                "\nPosition: " + rect1.Position +
                "\nCenter: " + rect1.Center;
            if (rect1.IsCollision(rect2, out mtv))
            {
                rect1.RectColor = Color.LightPink;

                rect1.X += mtv.X;
                rect1.Y += mtv.Y;
            }
            else
            {
                rect1.RectColor = Color.Red;
            }


            if (nowKeyboardState.IsKeyDown(Keys.Up))
                rect2.Y -= speed;
            if (nowKeyboardState.IsKeyDown(Keys.Down))
                rect2.Y += speed;
            if (nowKeyboardState.IsKeyDown(Keys.Left))
                rect2.X -= speed;
            if (nowKeyboardState.IsKeyDown(Keys.Right))
                rect2.X += speed;
            if (nowKeyboardState.IsKeyDown(Keys.T))
                rect2.Rotation += rotationSpeed;

            if (rect2.IsCollision(rect1, out mtv))
            {
                rect2.RectColor = Color.LightGreen;

                rect2.X += mtv.X;
                rect2.Y += mtv.Y;
            }
            else
            {
                rect2.RectColor = Color.Green;
            }
            

            lastKeyboardState = nowKeyboardState;
            base.Update(gameTime);
        }

        private bool SameDirection(float a, float b)
        {
            return ((a > 0 && b > 0) || (a < 0 && b < 0));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            rect1.Draw(_spriteBatch);
            rect2.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, result, Vector2.Zero, Color.Yellow);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
