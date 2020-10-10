using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CustomHelper;
using System.Runtime.InteropServices;
using System.Net.Http.Headers;
using System;

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
            rect1 = new MyRectangle(50, 50, 100, 100);
            rect2 = new MyRectangle(200, 200, 100, 100, Color.Green);
            font = Content.Load<SpriteFont>("sss");
            // TODO: use this.Content to load your game content here
        }

        float speed = 2f;
        float rotationSpeed = 0.2f;
        MyRectangle.MTV  mtv;
        Vector2 Velocity;
        string result = "";
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState nowKeyboardState = Keyboard.GetState();
            Velocity = Vector2.Zero;

            if (nowKeyboardState.IsKeyDown(Keys.W))
                Velocity.Y -= speed;
            if (nowKeyboardState.IsKeyDown(Keys.S))
                Velocity.Y += speed;
            if (nowKeyboardState.IsKeyDown(Keys.A))
                Velocity.X -= speed;
            if (nowKeyboardState.IsKeyDown(Keys.D))
                Velocity.X += speed;
            if (nowKeyboardState.IsKeyDown(Keys.R))
                rect1.Rotation += rotationSpeed;
            rect1.X += Velocity.X;
            rect1.Y += Velocity.Y;

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
            if (rect1.IsCollision(rect2, out mtv))
            {
                rect1.RectColor = Color.Red;
                result = mtv.overlap.ToString();

                Vector2 translation = mtv.overlap * mtv.Axis;
                float dot = Vector2.Dot(translation, Velocity);
                if (dot > 0)
                    translation *= -1;
                rect1.X += translation.X;
                rect1.Y += translation.Y;
            }
            else
            {
                rect1.RectColor = Color.White;
            }

            lastKeyboardState = nowKeyboardState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            rect1.Draw(_spriteBatch);
            rect2.Draw(_spriteBatch);
            _spriteBatch.DrawString(font, result, Vector2.Zero, Color.Red);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
