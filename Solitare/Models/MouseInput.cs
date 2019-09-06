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
        private const double doubleClickTimer = 300;
        private static bool wasDoubleClick = false;

        public static bool CheckForSingleClick(MouseState previousState)
        {
            if (wasDoubleClick)
            {
                return false;
            }
            return (previousState.LeftButton == ButtonState.Released) && (Mouse.GetState().LeftButton == ButtonState.Pressed);
        }

        public static bool CheckForDoubleClick(MouseState previousState, GameTime gameTime, double clickTimer)
        {
            if((CheckForSingleClick(previousState) && (gameTime.TotalGameTime.TotalMilliseconds - clickTimer < doubleClickTimer)))
            {
                wasDoubleClick = true;
                return true;
            }
            else
            {
                wasDoubleClick = false;
            }
            return false;
        }
    }
}
