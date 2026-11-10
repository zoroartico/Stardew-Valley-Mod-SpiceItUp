using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace SpiceItUpMod.Menus
{
    internal class FoodPrepMenu : IClickableMenu
    {
        public FoodPrepMenu()
            : base(
                Game1.uiViewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2,
                Game1.uiViewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2,
                800 + IClickableMenu.borderWidth * 2,
                600 + IClickableMenu.borderWidth * 2)
        {
        }

        //test
        public override void draw(SpriteBatch b)
        {
            
            Game1.drawDialogueBox(
                xPositionOnScreen,
                yPositionOnScreen,
                width,
                height,
                false,
                true
            );

            drawMouse(b);
        }
    }
}