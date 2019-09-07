using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ResolutionBuddy;
using Solitare.Models;

namespace Solitare.States
{
    public class MenuState : State
    {
        //private List<Component> components; //Put buttons, etc. in here

        public static Texture2D background;
        public static Texture2D playButton;
        public static Texture2D exitButton;

        SoundEffect selectMenuItem;
        bool smiPlayed;

        MouseState mState;
        MouseState pmState;

        float playDist;
        float exitDist;


        public MenuState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _content) : base(_game, _graphicsDevice, _content)
        {
            background = content.Load<Texture2D>("Images/Backgrounds/background1");
            playButton = content.Load<Texture2D>("Images/MenuItems/playButton");
            exitButton = content.Load<Texture2D>("Images/MenuItems/exitButton");

            selectMenuItem = content.Load<SoundEffect>("Sounds/SoundEffects/selectMenuItem");
            smiPlayed = false;

            pmState = Mouse.GetState();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            spriteBatch.Draw(playButton, new Vector2(Layout.MenuPlayButton.X - Layout.ButtonRadius, Layout.MenuPlayButton.Y - Layout.ButtonRadius), null, Color.Black * 0.35f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(playButton, new Vector2(Layout.MenuPlayButton.X - Layout.ButtonRadius, Layout.MenuPlayButton.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

            spriteBatch.Draw(exitButton, new Vector2(Layout.MenuExitButton.X - Layout.ButtonRadius, Layout.MenuExitButton.Y - Layout.ButtonRadius), null, Color.Black * 0.35f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(exitButton, new Vector2(Layout.MenuExitButton.X - Layout.ButtonRadius, Layout.MenuExitButton.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

        }

        public override void Update(GameTime gameTime)
        {
            var scaleX = (float)graphicsDevice.PresentationParameters.BackBufferWidth / 1920;
            var scaleY = (float)graphicsDevice.PresentationParameters.BackBufferHeight / 1080;
            var matrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

            mState = Mouse.GetState();
            var mousePosition = new Vector2(mState.X, mState.Y);
            var smPosition = Vector2.Transform(mousePosition, Matrix.Invert(matrix));

            playDist = Vector2.Distance(Layout.MenuPlayButton, new Vector2(smPosition.X, smPosition.Y));
            exitDist = Vector2.Distance(Layout.MenuExitButton, new Vector2(smPosition.X, smPosition.Y));

            if (playDist < Layout.ButtonRadius
                || exitDist < Layout.ButtonRadius)
            {
                if (!smiPlayed)
                {
                    SoundEffectInstance smi = selectMenuItem.CreateInstance();
                    smi.Volume = 0.2f;
                    smi.Pitch = 0.5f;
                    smi.Play();
                    smiPlayed = true;
                }
            }

            if (MouseInput.CheckForSingleClick(pmState))
            {
                if (playDist < Layout.ButtonRadius)
                {
                    game.ChangeToGameState();
                }
                if(exitDist < Layout.ButtonRadius)
                {
                    game.Exit();
                }
            }


        }
        public override void PostUpdate(GameTime gameTime)
        {
            if(smiPlayed
                && playDist > Layout.ButtonRadius
                && exitDist > Layout.ButtonRadius)
            {
                smiPlayed = false;
            }
            
           
            pmState = mState;
        }
    }
}
