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

        public override void Draw(GameTime gameTime, Renderer r)
        {
            r.Draw(gameTime, "menu");
        }
        
        public override void Update(GameTime gameTime)
        {
            mState = Mouse.GetState();
            playDist = Vector2.Distance(Layout.MenuPlayButton, new Vector2(mState.X, mState.Y));
            exitDist = Vector2.Distance(Layout.MenuExitButton, new Vector2(mState.X, mState.Y));

            if (playDist < Layout.ButtonRadius
                || exitDist < Layout.ButtonRadius)
            {
                if (!smiPlayed)
                {
                    selectMenuItem.Play();
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
