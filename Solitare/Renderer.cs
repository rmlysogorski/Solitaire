using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Solitare.Models;
using Solitare.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare
{
    public class Renderer
    {
        protected GraphicsDevice graphicsDevice;
        protected SpriteBatch spriteBatch;
        protected RenderTarget2D renderTarget;
        private float _renderScale = 1.0f;
        private const int _renderScreenHeight = 1080;
        public float AspectRatio { get => (float)graphicsDevice.PresentationParameters.BackBufferWidth / graphicsDevice.PresentationParameters.BackBufferHeight; }

        public Renderer(GraphicsDevice _graphicsDevice, SpriteBatch _spriteBatch)
        {
            graphicsDevice = _graphicsDevice;
            spriteBatch = _spriteBatch;
            renderTarget = new RenderTarget2D(
                _graphicsDevice,
                (int)GetScaledResolution().X,
                (int)GetScaledResolution().Y,
                false,
                _graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24); ;
        }
        protected Vector2 GetScaledResolution()
        {
            var scaledHeight = (float)_renderScreenHeight / _renderScale;
            return new Vector2(AspectRatio * scaledHeight, scaledHeight);
        }

        public void Draw(GameTime gameTime, string state)
        {
            DrawSceneToTexture(renderTarget, state);

            graphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone
                );

            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, graphicsDevice.PresentationParameters.BackBufferWidth, graphicsDevice.PresentationParameters.BackBufferHeight), Color.White);

            spriteBatch.End();
        }

        protected void DrawSceneToTexture(RenderTarget2D renderTarget, string state)
        {
            //Set the render target
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            //Draw the Scene
            graphicsDevice.Clear(Color.DarkGreen);
            switch (state)
            {
                case "game":
                    DrawGameScene();
                    break;
                case "menu":
                    DrawMenuScene();
                    break;
                default:
                    break;
            }

            //Drop the render target
            graphicsDevice.SetRenderTarget(null);
        }

        protected void DrawGameScene()
        {
            int shadowOffset = 5;

            spriteBatch.Begin();

            spriteBatch.Draw(GameState.background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            //spriteBatch.DrawString(GameState.gameFont, "Play Again", Layout.PlayAgain, Color.Yellow);

            //Draws the cardback
            if (GameState.deckManager.CardsInPlay["Deck"].Length > 1
                || (GameState.deckManager.CardsInPlay["Deck"].Length == 1 && !GameState.deckManager.CardWasAlreadyDrawn)
                || (GameState.mcm.MovingCard.IsMoving && GameState.mcm.MovingCard.PilePosition == Layout.DrawPile && GameState.deckManager.CardsInPlay["Deck"].Length == 1))
            {
                for(int i = 0; 
                    (GameState.deckManager.CardWasAlreadyDrawn? 
                        i < GameState.deckManager.CardsInPlay["Deck"].Length - 1 : 
                        i < GameState.deckManager.CardsInPlay["Deck"].Length);
                    i++)
                {
                    spriteBatch.Draw(
                    GameState.cardShadow,
                    new Vector2(Layout.Deck.X + i * 3 + shadowOffset,
                        Layout.Deck.Y + shadowOffset),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                    spriteBatch.Draw(
                        GameState.cardBack.SpriteTexture,
                        new Vector2(Layout.Deck.X + i * 3,
                        Layout.Deck.Y), 
                        null, Color.LightBlue, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );
                }
                
            }
            else
            {
                spriteBatch.Draw(
                        GameState.cardShadow,
                        Layout.Deck,
                        null, Color.LightBlue, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );
            }

            //Draws the Draw Pile
            if (GameState.deckManager.CardsInPlay["Deck"].Length > 0
                && GameState.deckManager.CardWasAlreadyDrawn
                && GameState.mcm.MovingCard.PilePosition != Layout.DrawPile
                && GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].PilePosition == Layout.DrawPile)
            {
                spriteBatch.Draw(
                    GameState.cardShadow,
                    new Vector2(
                        GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].Position.X + shadowOffset,
                        GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].Position.Y + shadowOffset),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].SpriteTexture,
                    GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].Position,
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }

            //Draws the Waste Pile
            if(GameState.deckManager.CardsInPlay["Waste"].Length > 0)
            {
                spriteBatch.Draw(
                    GameState.cardShadow,
                    new Vector2(Layout.WastePile.X + shadowOffset,
                    Layout.WastePile.Y + shadowOffset), 
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Waste"][GameState.deckManager.CardsInPlay["Waste"].Length - 1].SpriteTexture,
                    Layout.WastePile, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            }

            //Draws the four Foundation Piles
            if(GameState.deckManager.CardsInPlay["Hearts"].Length > 1)
            {
                spriteBatch.Draw(
                    GameState.cardShadow, new Vector2(Layout.HeartsPile.X + shadowOffset, Layout.HeartsPile.Y + shadowOffset), null,
                    Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }
            if (GameState.deckManager.CardsInPlay["Spades"].Length > 1)
            {
                spriteBatch.Draw(
                    GameState.cardShadow, new Vector2(Layout.SpadesPile.X + shadowOffset, Layout.SpadesPile.Y + shadowOffset), null,
                    Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }
            if (GameState.deckManager.CardsInPlay["Diamonds"].Length > 1)
            {
                spriteBatch.Draw(
                    GameState.cardShadow, new Vector2(Layout.DiamondsPile.X + shadowOffset, Layout.DiamondsPile.Y + shadowOffset), null,
                    Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }
            if (GameState.deckManager.CardsInPlay["Clubs"].Length > 1)
            {
                spriteBatch.Draw(
                    GameState.cardShadow, new Vector2(Layout.ClubsPile.X + shadowOffset, Layout.ClubsPile.Y + shadowOffset), null,
                    Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }

            spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Hearts"][GameState.deckManager.CardsInPlay["Hearts"].Length - 1].SpriteTexture, Layout.HeartsPile, null, 
                    (GameState.deckManager.CardsInPlay["Hearts"].Length > 1) ? Color.White : Color.LightSlateGray, 
                    0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Spades"][GameState.deckManager.CardsInPlay["Spades"].Length - 1].SpriteTexture, Layout.SpadesPile, null,
                    (GameState.deckManager.CardsInPlay["Spades"].Length > 1) ? Color.White : Color.LightSlateGray,
                    0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Diamonds"][GameState.deckManager.CardsInPlay["Diamonds"].Length - 1].SpriteTexture, Layout.DiamondsPile, null,
                    (GameState.deckManager.CardsInPlay["Diamonds"].Length > 1) ? Color.White : Color.LightSlateGray,
                    0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Clubs"][GameState.deckManager.CardsInPlay["Clubs"].Length - 1].SpriteTexture, Layout.ClubsPile, null,
                    (GameState.deckManager.CardsInPlay["Clubs"].Length > 1) ? Color.White : Color.LightSlateGray,
                    0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            //Draws the seven Tableaus
            for(int i = 0; i < 7; i++)
            {
                int t = i + 1;
                string key = "Tableau" + t;
                if(GameState.deckManager.CardsInPlay[key].Length <= 0)
                {
                    spriteBatch.Draw(GameState.cardShadow,
                        GetTableauPosition(key), 
                        null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );

                }

                foreach (Card c in GameState.deckManager.CardsInPlay[key])
                {
                    spriteBatch.Draw(GameState.cardShadow, new Vector2(c.Position.X + shadowOffset, c.Position.Y + shadowOffset), null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);

                    if (c.IsFaceDown)
                    {
                        spriteBatch.Draw(GameState.cardBack.SpriteTexture, c.Position, null, Color.LightBlue, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        spriteBatch.Draw(c.SpriteTexture, c.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
                    }
                }
            }

            //Draws the buttons
            spriteBatch.Draw(GameState.playAgainButton, new Vector2(Layout.PlayAgain.X - Layout.ButtonRadius, Layout.PlayAgain.Y - Layout.ButtonRadius), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.playAgainButton, new Vector2(Layout.PlayAgain.X - Layout.ButtonRadius, Layout.PlayAgain.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.exitButton, new Vector2(Layout.Exit.X - Layout.ButtonRadius, Layout.Exit.Y - Layout.ButtonRadius), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.exitButton, new Vector2(Layout.Exit.X - Layout.ButtonRadius, Layout.Exit.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

            //Draws the post-it
            spriteBatch.Draw(GameState.postit, new Vector2(50, 86), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.46f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.postit, new Vector2(50, 80), null, Color.White, 0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(GameState.pencil, "Score", Layout.Score, Color.Black);

            spriteBatch.DrawString(GameState.pencil, GameState.score.ToString(), 
                new Vector2 (Layout.PlayerScore.X - GameState.pencil.MeasureString(GameState.score.ToString()).X, Layout.PlayerScore.Y), Color.Black);

            //Draws the Moving Stack
            if (GameState.msm.StackIsMoving)
            {
                foreach(Card c in GameState.msm.MovingStack)
                {
                    spriteBatch.Draw(GameState.cardShadow,
                        new Vector2(c.Position.X + shadowOffset * 2,
                        c.Position.Y + shadowOffset * 2),
                        null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );

                    spriteBatch.Draw(c.SpriteTexture, c.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
                }
            }

            //Draws the Moving Card
            if (GameState.mcm.MovingCard.IsMoving)
            {
                spriteBatch.Draw(GameState.cardShadow, 
                    new Vector2(GameState.mcm.MovingCard.Position.X + shadowOffset * 2,
                    GameState.mcm.MovingCard.Position.Y + shadowOffset * 2), 
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                spriteBatch.Draw(GameState.mcm.MovingCard.SpriteTexture, GameState.mcm.MovingCard.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
            }

            spriteBatch.End();
        }

        protected Vector2 GetTableauPosition(string key)
        {
            if(key == "Tableau1")
            {
                return Layout.Tableau1;
            }
            else if (key == "Tableau2")
            {
                return Layout.Tableau2;
            }
            else if (key == "Tableau3")
            {
                return Layout.Tableau3;
            }
            else if (key == "Tableau4")
            {
                return Layout.Tableau4;
            }
            else if (key == "Tableau5")
            {
                return Layout.Tableau5;
            }
            else if (key == "Tableau6")
            {
                return Layout.Tableau6;
            }
            else if (key == "Tableau7")
            {
                return Layout.Tableau7;
            }
            return Vector2.Zero;
        }


        protected void DrawMenuScene()
        {
            spriteBatch.Begin();

            spriteBatch.Draw(MenuState.background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            spriteBatch.Draw(MenuState.playButton, new Vector2(Layout.MenuPlayButton.X - Layout.ButtonRadius, Layout.MenuPlayButton.Y - Layout.ButtonRadius), null, Color.Black * 0.35f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(MenuState.playButton, new Vector2(Layout.MenuPlayButton.X - Layout.ButtonRadius, Layout.MenuPlayButton.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

            spriteBatch.Draw(MenuState.exitButton, new Vector2(Layout.MenuExitButton.X - Layout.ButtonRadius, Layout.MenuExitButton.Y - Layout.ButtonRadius), null, Color.Black * 0.35f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(MenuState.exitButton, new Vector2(Layout.MenuExitButton.X - Layout.ButtonRadius, Layout.MenuExitButton.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
    }
}
