using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare.Models
{
    public class CardBack : Component

    {
        private Texture2D spriteTexture;
        private Vector2 position;
        private Vector2 pilePosition;
        private Rectangle cardBox;

        public Texture2D SpriteTexture { get => spriteTexture; set => spriteTexture = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 PilePosition { get => pilePosition; set => pilePosition = value; }
        public Rectangle CardBox { get => cardBox; set => cardBox = value; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, position, null, Color.LightBlue, 0f, Vector2.Zero, Layout.CardScale, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}
