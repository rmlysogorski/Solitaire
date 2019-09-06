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
        private int shadowOffset = 5;
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

            DrawButtons();

            DrawPostit();

            DrawCardBack();

            //Always draws moving cards on top (last)
            if(CheckMovingToDrawPile())
            {
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles();
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
            }
            else if(CheckMovingToWastePile())
            {
                DrawFoundationPiles();
                DrawTableauPiles();
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
            }
            else if (CheckMovingToHeartsPile())
            {
                DrawTableauPiles();
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
            }
            else if (CheckMovingToSpadesPile())
            {
                DrawTableauPiles();
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
            }
            else if (CheckMovingToDiamondsPile())
            {
                DrawTableauPiles();
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
            }
            else if (CheckMovingToClubsPile())
            {
                DrawTableauPiles();
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
            }
            else if (CheckMovingToTableau1())
            {
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles();
            }
            else if (CheckMovingToTableau2())
            {
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles(2);
            }
            else if (CheckMovingToTableau3())
            {
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles(3);
            }
            else if (CheckMovingToTableau4())
            {
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles(4);
            }
            else if (CheckMovingToTableau5())
            {
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles(5);
            }
            else if (CheckMovingToTableau6())
            {
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles(6);
            }
            else if (CheckMovingToTableau7())
            {
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles(7);
            }
            else if(GameState.mcm.MovingCard.IsMoving || GameState.msm.StackIsMoving)
            {
                DrawWinButton();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles();
                DrawMovingStack();
                DrawMovingCard();
            }
            else
            {
                DrawWinButton();
                DrawMovingStack();
                DrawMovingCard();
                DrawDrawPile();
                DrawWastePile();
                DrawFoundationPiles();
                DrawTableauPiles();
            }

            spriteBatch.End();
        }

        protected bool CheckMovingToDrawPile()
        {
            if(GameState.deckManager.CardsInPlay["Deck"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].Position != GameState.deckManager.CardsInPlay["Deck"][GameState.deckManager.CardsInPlay["Deck"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToWastePile()
        {
            if (GameState.deckManager.CardsInPlay["Waste"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Waste"][GameState.deckManager.CardsInPlay["Waste"].Length - 1].Position != GameState.deckManager.CardsInPlay["Waste"][GameState.deckManager.CardsInPlay["Waste"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToHeartsPile()
        {
            if (GameState.deckManager.CardsInPlay["Hearts"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Hearts"][GameState.deckManager.CardsInPlay["Hearts"].Length - 1].Position != GameState.deckManager.CardsInPlay["Hearts"][GameState.deckManager.CardsInPlay["Hearts"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToSpadesPile()
        {
            if (GameState.deckManager.CardsInPlay["Spades"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Spades"][GameState.deckManager.CardsInPlay["Spades"].Length - 1].Position != GameState.deckManager.CardsInPlay["Spades"][GameState.deckManager.CardsInPlay["Spades"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToDiamondsPile()
        {
            if (GameState.deckManager.CardsInPlay["Diamonds"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Diamonds"][GameState.deckManager.CardsInPlay["Diamonds"].Length - 1].Position != GameState.deckManager.CardsInPlay["Diamonds"][GameState.deckManager.CardsInPlay["Diamonds"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToClubsPile()
        {
            if (GameState.deckManager.CardsInPlay["Clubs"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Clubs"][GameState.deckManager.CardsInPlay["Clubs"].Length - 1].Position != GameState.deckManager.CardsInPlay["Clubs"][GameState.deckManager.CardsInPlay["Clubs"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau1()
        {
            if (GameState.deckManager.CardsInPlay["Tableau1"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau1"][GameState.deckManager.CardsInPlay["Tableau1"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau1"][GameState.deckManager.CardsInPlay["Tableau1"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau2()
        {
            if (GameState.deckManager.CardsInPlay["Tableau2"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau2"][GameState.deckManager.CardsInPlay["Tableau2"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau2"][GameState.deckManager.CardsInPlay["Tableau2"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau3()
        {
            if (GameState.deckManager.CardsInPlay["Tableau3"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau3"][GameState.deckManager.CardsInPlay["Tableau3"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau3"][GameState.deckManager.CardsInPlay["Tableau3"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau4()
        {
            if (GameState.deckManager.CardsInPlay["Tableau4"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau4"][GameState.deckManager.CardsInPlay["Tableau4"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau4"][GameState.deckManager.CardsInPlay["Tableau4"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau5()
        {
            if (GameState.deckManager.CardsInPlay["Tableau5"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau5"][GameState.deckManager.CardsInPlay["Tableau5"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau5"][GameState.deckManager.CardsInPlay["Tableau5"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau6()
        {
            if (GameState.deckManager.CardsInPlay["Tableau6"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau6"][GameState.deckManager.CardsInPlay["Tableau6"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau6"][GameState.deckManager.CardsInPlay["Tableau6"].Length - 1].PilePosition;
            }
            return false;
        }

        protected bool CheckMovingToTableau7()
        {
            if (GameState.deckManager.CardsInPlay["Tableau7"].Length > 0)
            {
                return GameState.deckManager.CardsInPlay["Tableau7"][GameState.deckManager.CardsInPlay["Tableau7"].Length - 1].Position != GameState.deckManager.CardsInPlay["Tableau7"][GameState.deckManager.CardsInPlay["Tableau7"].Length - 1].PilePosition;
            }
            return false;
        }

        protected void DrawCardBack()
        {
            if (GameState.deckManager.CardsInPlay["Deck"].Length > 1
                || (GameState.deckManager.CardsInPlay["Deck"].Length == 1 && !GameState.deckManager.CardWasAlreadyDrawn)
                || (GameState.mcm.MovingCard.IsMoving && GameState.mcm.MovingCard.PilePosition == Layout.DrawPile && GameState.deckManager.CardsInPlay["Deck"].Length == 1))
            {
                for (int i = 0;
                    (GameState.deckManager.CardWasAlreadyDrawn ?
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
        }

        protected void DrawDrawPile()
        {
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
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
            }
        }

        protected void DrawWastePile()
        {
            int wasteLen = GameState.deckManager.CardsInPlay["Waste"].Length;
            if (wasteLen > 0)
            {
                if(wasteLen > 1 && GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].Position != GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].PilePosition)
                {
                    spriteBatch.Draw(
                        GameState.cardShadow,
                        new Vector2(GameState.deckManager.CardsInPlay["Waste"][wasteLen - 2].Position.X + shadowOffset,
                        GameState.deckManager.CardsInPlay["Waste"][wasteLen - 2].Position.Y + shadowOffset),
                        null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );

                    spriteBatch.Draw(
                        GameState.deckManager.CardsInPlay["Waste"][wasteLen - 2].SpriteTexture,
                        GameState.deckManager.CardsInPlay["Waste"][wasteLen - 2].Position,
                        null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );
                }
                spriteBatch.Draw(
                    GameState.cardShadow,
                    new Vector2(GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].Position.X + shadowOffset,
                    GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].Position.Y + shadowOffset),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                spriteBatch.Draw(
                    GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].SpriteTexture,
                    GameState.deckManager.CardsInPlay["Waste"][wasteLen - 1].Position,
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            }
        }

        protected void DrawFoundationPiles()
        {
            if (GameState.deckManager.CardsInPlay["Hearts"].Length > 1)
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

        }

        protected void DrawTableauPiles(int drawLast = 1)
        {
            for (int i = 0; i < 7; i++)
            {
                if(i + 1 != drawLast)
                {
                    int t = i + 1;
                    string key = "Tableau" + t;

                    DrawTableauPileShadow(key);
                    DrawTableauPileCards(key);
                }
            }

            DrawTableauPileShadow("Tableau" + drawLast);
            DrawTableauPileCards("Tableau" + drawLast);
        }

        protected void DrawTableauPileShadow(string key)
        {
            if (GameState.deckManager.CardsInPlay[key].Length <= 0
                || (GameState.deckManager.CardsInPlay[key].Length > 0 
                    && GameState.deckManager.CardsInPlay[key][0].Position
                        != GameState.deckManager.CardsInPlay[key][0].PilePosition))
            {
                spriteBatch.Draw(GameState.cardShadow,
                    GetTableauPosition(key),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

            }
        }

        protected void DrawTableauPileCards(string key)
        {
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

        protected void DrawButtons()
        {
            spriteBatch.Draw(GameState.playAgainButton, new Vector2(Layout.PlayAgain.X - Layout.ButtonRadius, Layout.PlayAgain.Y - Layout.ButtonRadius), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.playAgainButton, new Vector2(Layout.PlayAgain.X - Layout.ButtonRadius, Layout.PlayAgain.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.exitButton, new Vector2(Layout.Exit.X - Layout.ButtonRadius, Layout.Exit.Y - Layout.ButtonRadius), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.exitButton, new Vector2(Layout.Exit.X - Layout.ButtonRadius, Layout.Exit.Y - Layout.ButtonRadius), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f);

        }

        protected void DrawPostit()
        {
            spriteBatch.Draw(GameState.postit, new Vector2(50, 86), null, Color.Black * 0.6f, 0f, Vector2.Zero, 0.46f, SpriteEffects.None, 0f);

            spriteBatch.Draw(GameState.postit, new Vector2(50, 80), null, Color.White, 0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0f);

            spriteBatch.DrawString(GameState.pencil, "Score", Layout.Score, Color.Black);

            spriteBatch.DrawString(GameState.pencil, GameState.score.ToString(),
                new Vector2(Layout.PlayerScore.X - GameState.pencil.MeasureString(GameState.score.ToString()).X, Layout.PlayerScore.Y), Color.Black);

        }

        protected void DrawMovingStack()
        {
            if (GameState.msm.StackIsMoving)
            {
                foreach (Card c in GameState.msm.MovingStack)
                {
                    spriteBatch.Draw(GameState.cardShadow,
                        new Vector2(c.Position.X + shadowOffset * 2,
                        c.Position.Y + shadowOffset * 2),
                        null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                        );

                    spriteBatch.Draw(c.SpriteTexture, c.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
                }
            }
        }

        protected void DrawMovingCard()
        {
            if (GameState.mcm.MovingCard.IsMoving)
            {
                spriteBatch.Draw(GameState.cardShadow,
                    new Vector2(GameState.mcm.MovingCard.Position.X + shadowOffset * 2,
                    GameState.mcm.MovingCard.Position.Y + shadowOffset * 2),
                    null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f
                    );

                spriteBatch.Draw(GameState.mcm.MovingCard.SpriteTexture, GameState.mcm.MovingCard.Position, null, Color.White, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
            }
        }

        protected void DrawWinButton()
        {
            if (GameState.canWin)
            {
                spriteBatch.DrawString(GameState.pencil, "You won!", new Vector2(800, 800), Color.Yellow);
            }
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
