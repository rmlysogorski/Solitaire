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
        public static Texture2D solitareTitle;
        public static Texture2D solitareMenu;
        public static Texture2D menuHand;
        public static Vector2 menuHandPos;

        SoundEffect selectMenuItem;
        bool smiPlayed;

        MouseState mState;
        MouseState pmState;

        double handTimer;

        public MenuState(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _content) : base(_game, _graphicsDevice, _content)
        {
            background = content.Load<Texture2D>("Images/Backgrounds/background1");
            solitareTitle = content.Load<Texture2D>("Images/MenuItems/solitareTitle");
            solitareMenu = content.Load<Texture2D>("Images/MenuItems/solitareMenu");
            menuHand = content.Load<Texture2D>("Images/MenuItems/menuHand");

            selectMenuItem = content.Load<SoundEffect>("Sounds/SoundEffects/selectMenuItem");
            smiPlayed = false;

            menuHandPos = Layout.MenuHandPlay[0];
            pmState = Mouse.GetState();
            handTimer = 0;
        }

        public override void Draw(GameTime gameTime, Renderer r)
        {
            r.Draw(gameTime, "menu");
        }
        
        public override void Update(GameTime gameTime)
        {
            mState = Mouse.GetState();

            handTimer += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Layout.MenuPlayBox.Contains(mState.Position))
            {
                if (!smiPlayed)
                {
                    selectMenuItem.Play();
                    smiPlayed = true;
                }

                if(menuHandPos == Layout.MenuHandPlay[1])
                {
                    menuHandPos = Layout.MenuHandPlay[1];
                }
                else
                {
                    menuHandPos = Layout.MenuHandPlay[0];
                }

                if (MouseInput.CheckForSingleClick(pmState))
                {
                    game.ChangeToGameState();
                }
            }
            else if (Layout.MenuExitBox.Contains(mState.Position))
            {
                if (!smiPlayed)
                {
                    selectMenuItem.Play();
                    smiPlayed = true;
                }

                if (menuHandPos == Layout.MenuHandExit[1])
                {
                    menuHandPos = Layout.MenuHandExit[1];
                }
                else
                {
                    menuHandPos = Layout.MenuHandExit[0];
                }

                if (MouseInput.CheckForSingleClick(pmState))
                {
                    game.Exit();
                }
            }
        }
        public override void PostUpdate(GameTime gameTime)
        {
            if(!Layout.MenuPlayBox.Contains(mState.Position)
                && !Layout.MenuExitBox.Contains(mState.Position))
            {
                smiPlayed = false;
            }

            if(handTimer > 1000)
            {
                if(menuHandPos == Layout.MenuHandPlay[0])
                {
                    menuHandPos = Layout.MenuHandPlay[1];
                }
                else if(menuHandPos == Layout.MenuHandPlay[1])
                {
                    menuHandPos = Layout.MenuHandPlay[0];
                }
                else if(menuHandPos == Layout.MenuHandExit[0])
                {
                    menuHandPos = Layout.MenuHandExit[1];
                }
                else if (menuHandPos == Layout.MenuHandExit[1])
                {
                    menuHandPos = Layout.MenuHandExit[0];
                }

                handTimer = 0;
            }
            pmState = mState;
        }
    }
}
