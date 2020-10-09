using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.NetworkInformation;
using System.Security.AccessControl;

namespace MonogameInterestingMovement
{
    public class Game1 : Game
    {
        //IMAGE SOURCES:
        //icy lake : https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/i/515f04c5-5954-4826-ad10-4ce082a20fee/d87thcu-e88a7692-a8aa-44c0-8f9f-eb5dd03430f3.png/v1/fill/w_1219,h_656,q_70,strp/icy_lake_on_the_mountaintop_in_whistler__bc_by_thoun_d87thcu-pre.jpg        
        //ice skater: https://www.clipartmax.com/png/middle/228-2282565_skating-clipart-transparent-background-ice-skater-clip-art.png

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D iceSkater;
        Texture2D icyLake;

        Vector2 skaterPosition;
        Vector2 lakePosition;
        Vector2 skaterDirection;
        Vector2 skaterVelocity;
        Vector2 skaterOrigin;

        SpriteEffects spriteEffects;

        Rectangle rectangle;

        float acceleration;
        float friction;
        float skaterSpeed;
        float skaterRotation;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            TargetElapsedTime = TimeSpan.FromTicks(333333);

            graphics.PreferredBackBufferWidth = 1219;
            graphics.PreferredBackBufferHeight = 656;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Graphics rendering
            icyLake = Content.Load<Texture2D>("IcyLake");
            iceSkater = Content.Load<Texture2D>("IceSkater");
            rectangle = new Rectangle(320, 670, 130, 140);

            //Game settings
            skaterOrigin = new Vector2(150, 215);
            skaterPosition = new Vector2(320, 670);
            skaterVelocity = new Vector2(0, 0);
            lakePosition = new Vector2(0, 0);
            skaterRotation = 0f;
            skaterDirection = new Vector2(1, 0);
            skaterSpeed = 250;
            friction = .23f;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {

            float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Right
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                skaterVelocity.X = 0;
                //Reset accel
                acceleration = .01f;
                spriteEffects = SpriteEffects.None;
                //Deincrement accel
                acceleration += .5f;
                //Calculate velocity and move
                skaterVelocity.X += acceleration - friction * skaterVelocity.X * (time / 1000);
                skaterPosition += (skaterDirection * skaterSpeed) * (time / 1000);
            }
            //Left
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                skaterVelocity.X = 0;
                //Reset accel
                acceleration = -.01f;
                spriteEffects = SpriteEffects.FlipHorizontally;
                //Deincrement accel
                acceleration -= .5f;
                //Calculate velocity and move
                skaterVelocity.X -= acceleration - friction * skaterVelocity.X * (time / 1000);
                skaterPosition -= (skaterDirection * skaterSpeed) * (time / 1000);
            }
            //Friction when keys are released
            if (Keyboard.GetState().IsKeyUp(Keys.D) || (Keyboard.GetState().IsKeyUp(Keys.A)))
            {
                skaterVelocity.X += acceleration - friction * skaterVelocity.X;
                skaterPosition += skaterVelocity;
                if (acceleration > 0)
                {
                    acceleration -= .005f;
                }
                else if (acceleration < 10)
                {
                    acceleration += .005f;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            spriteBatch.Draw(icyLake, lakePosition, Color.White);
            spriteBatch.Draw(iceSkater, skaterPosition, null, Color.White, skaterRotation, skaterOrigin, new Vector2(1, 1), spriteEffects, 0f);
            spriteBatch.End();

            //Boundries
            if ((skaterPosition.X > graphics.GraphicsDevice.Viewport.Width - iceSkater.Width)
                || (skaterPosition.X < 0)
               )
            {
                skaterPosition = new Vector2(320, 670);
            }

            base.Draw(gameTime);
        }
    }
}
