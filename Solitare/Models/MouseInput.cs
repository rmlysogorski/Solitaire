using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitare.Models
{
    public class MouseInput
    {
        private const double doubleClickTimer = 500;

        public static bool CheckForSingleClick(MouseState previousState)
        {
            return (previousState.LeftButton == ButtonState.Released) && (Mouse.GetState().LeftButton == ButtonState.Pressed);
        }

        public static bool CheckForDoubleClick(MouseState previousState, GameTime gameTime, double clickTimer)
        {
            return (CheckForSingleClick(previousState) && (gameTime.TotalGameTime.TotalMilliseconds - clickTimer < doubleClickTimer));
        }
    }
}
