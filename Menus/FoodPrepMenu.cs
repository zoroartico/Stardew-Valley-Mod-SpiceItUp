using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace SpiceItUpMod.Menus
{
    internal class FoodPrepMenu : IClickableMenu
    {
        public FoodPrepMenu()
            : base(
                Game1.uiViewport.Width / 2 - (400 + IClickableMenu.borderWidth * 2) / 2,
                Game1.uiViewport.Height / 2 - (300 + IClickableMenu.borderWidth * 2) / 2,
                400 + IClickableMenu.borderWidth * 2,
                300 + IClickableMenu.borderWidth * 2)
        {
            this.upperRightCloseButton = new ClickableTextureComponent(
                new Rectangle(
                    xPositionOnScreen + width - 36,
                    yPositionOnScreen - 16,
                    48,
                    48
                ),
                Game1.mouseCursors,
                new Rectangle(337, 494, 12, 12),
                4f
            );
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
            this.upperRightCloseButton?.draw(b);

            drawMouse(b);
        }
    }
}