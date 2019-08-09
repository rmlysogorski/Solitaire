using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare.States
{
    public abstract class State
    {
        #region Fields

        protected ContentManager content;
        protected GraphicsDevice graphicsDevice;
        protected Game1 game;
        protected RenderTarget2D renderTarget;

        #endregion

        #region Methods

        public State(Game1 _game, GraphicsDevice _graphicsDevice, ContentManager _content)
        {
            content = _content;
            graphicsDevice = _graphicsDevice;
            game = _game;
        }

        public abstract void Draw(GameTime gameTime, Renderer r);
        public abstract void Update(GameTime gameTime);
        public abstract void PostUpdate(GameTime gameTime);

        #endregion
    }
}
