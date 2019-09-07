using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Solitare.Managers;
using Solitare.States;
using System.Collections.Generic;

namespace Solitare
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<DisplayMode> displayModes = new List<DisplayMode>();

        private State currentState;
        private State nextState;

        private int modeIndex = 0;

        public void ChangeToGameState()
        {
            nextState = new GameState(this, graphics.GraphicsDevice, Content);
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.IsFullScreen = true;
            IsMouseVisible = true;
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                displayModes.Add(mode);
                if(mode.Width == 1920 && mode.Height == 1080)
                {
                    modeIndex = displayModes.Count - 1;
                }
            }

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {                        
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //Load Game Assets
            

            //Set this to whatever state should load first, usually the menu
            currentState = new MenuState(this, graphics.GraphicsDevice, Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                modeIndex++;
                if(modeIndex > displayModes.Count - 1)
                {
                    modeIndex = 0;
                }
                graphics.PreferredBackBufferWidth = displayModes[modeIndex].Width;
                graphics.PreferredBackBufferHeight = displayModes[modeIndex].Height;
                graphics.ApplyChanges();
            }
            //This is where we can change the state
            //Inside the state, set something to call the change state method on click
            if (nextState != null)
            {
                currentState = nextState;
                nextState = null;
            }

            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var scaleX = (float)graphics.PreferredBackBufferWidth / 1920;
            var scaleY = (float)graphics.PreferredBackBufferHeight / 1080;
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Immediate,
                      BlendState.AlphaBlend,
                      null, null, null, null,
                      transformMatrix: matrix);

            currentState.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
