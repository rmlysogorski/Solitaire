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
            spriteBatch.Begin();

            spriteBatch.Draw(GameState.background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(GameState.gameFont, "Play Again", Layout.PlayAgain, Color.Yellow);

            //Draws the cardback
            if (GameState.deckManager.CardsInPlay["Deck"].Length > 1
                || (GameState.deckManager.CardsInPlay["Deck"].Length == 1 && !GameState.deckManager.CardWasAlreadyDrawn)
                || (GameState.mcm.MovingCard.IsMoving && GameState.mcm.MovingCard.PilePosition == Layout.DrawPile && GameState.deckManager.CardsInPlay["Deck"].Length == 1))
            {
                spriteBatch.Draw(
                    GameState.cardBack.SpriteTexture,
                    Layout.Deck, null, Color.LightBlue, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );
            }

            //Draws the Draw Pile
            if (GameState.deckManager.CardsInPlay["Deck"].Length > 0
                && GameState.deckManager.CardWasAlreadyDrawn
                && GameState.mcm.MovingCard.PilePosition != Layout.DrawPile
                && GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].PilePosition == Layout.DrawPile)
            {
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
                    GameState.deckManager.CardsInPlay["Waste"][GameState.deckManager.CardsInPlay["Waste"].Length - 1].SpriteTexture,
                    Layout.WastePile, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            }

            //Draws the four Foundation Piles
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
                foreach(Card c in GameState.deckManager.CardsInPlay[key])
                {
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

            //Draws the Moving Stack
            if (GameState.msm.StackIsMoving)
            {
                foreach(Card c in GameState.msm.MovingStack)
                {
                    spriteBatch.Draw(c.SpriteTexture, c.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
                }
            }

            //Draws the Moving Card
            if (GameState.mcm.MovingCard.IsMoving)
            {
                spriteBatch.Draw(GameState.mcm.MovingCard.SpriteTexture, GameState.mcm.MovingCard.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
            }

            spriteBatch.End();
        }

        protected void DrawMenuScene()
        {
            spriteBatch.Begin();

            spriteBatch.Draw(MenuState.background, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            spriteBatch.Draw(MenuState.playButton, new Vector2(Layout.MenuPlayButton.X - Layout.ButtonRadius, Layout.MenuPlayButton.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            spriteBatch.Draw(MenuState.exitButton, new Vector2(Layout.MenuExitButton.X - Layout.ButtonRadius, Layout.MenuExitButton.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
    }
}
