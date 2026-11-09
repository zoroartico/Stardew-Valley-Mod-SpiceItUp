
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiceItUpMod.Services;
using StardewValley;
using StardewValley.Menus;

namespace SpiceItUpMod.Menus
{
    internal class SpiceGrinderMenu : IClickableMenu
    {
        private readonly SpiceManager spiceManager;

        private string statusMessage = "Select a recipe to craft.";

        private readonly Rectangle craftButton;
        private readonly Rectangle closeButton;

        public SpiceGrinderMenu(SpiceManager spiceManager)
            : base(
                Game1.uiViewport.Width / 2 - 300,
                Game1.uiViewport.Height / 2 - 200,
                600,
                400)
        {
            this.spiceManager = spiceManager;

            craftButton = new Rectangle(
                xPositionOnScreen + 170,
                yPositionOnScreen + 285,
                260,
                55
            );

            closeButton = new Rectangle(
                xPositionOnScreen + 170,
                yPositionOnScreen + 345,
                260,
                40
            );
        }

        public override void receiveLeftClick(
            int x,
            int y,
            bool playSound = true)
        {
            if (craftButton.Contains(x, y))
            {
                var recipe = spiceManager.GetRecipes().FirstOrDefault();

                if (recipe == null)
                {
                    statusMessage = "No recipes available.";
                    return;
                }

                if (spiceManager.Craft(Game1.player, recipe.Id))
                {
                    statusMessage = $"Crafted {recipe.SpiceId}!";
                    Game1.playSound("coin");
                }
                else
                {
                    statusMessage = "Not enough ingredients or inventory space.";
                    Game1.playSound("cancel");
                }

                return;
            }

            if (closeButton.Contains(x, y))
            {
                Game1.exitActiveMenu();
            }
        }

        public override void draw(
            SpriteBatch b,
            int red = -1,
            int green = -1,
            int blue = -1)
        {
            // Darken the background.
            b.Draw(
                Game1.fadeToBlackRect,
                Game1.graphics.GraphicsDevice.Viewport.Bounds,
                Color.Black * 0.5f
            );

            // Draw the menu background.
            Game1.drawDialogueBox(
                xPositionOnScreen,
                yPositionOnScreen,
                width,
                height,
                speaker: false,
                drawOnlyBox: true
            );

            // Title.
            b.DrawString(
                Game1.dialogueFont,
                "Spice Grinder",
                new Vector2(xPositionOnScreen + 190, yPositionOnScreen + 45),
                Color.Black
            );

            // Recipe information.
            b.DrawString(
                Game1.smallFont,
                "Cinnamon",
                new Vector2(xPositionOnScreen + 90, yPositionOnScreen + 115),
                Color.Black
            );

            b.DrawString(
                Game1.smallFont,
                "Ingredients:",
                new Vector2(xPositionOnScreen + 90, yPositionOnScreen + 155),
                Color.Black
            );

            b.DrawString(
                Game1.smallFont,
                "1x Spice Berry",
                new Vector2(xPositionOnScreen + 120, yPositionOnScreen + 185),
                Color.Black
            );

            b.DrawString(
                Game1.smallFont,
                "1x Wild Horseradish",
                new Vector2(xPositionOnScreen + 120, yPositionOnScreen + 215),
                Color.Black
            );

            // Craft button.
            b.Draw(
                Game1.fadeToBlackRect,
                craftButton,
                Color.ForestGreen
            );

            b.DrawString(
                Game1.smallFont,
                "Craft Cinnamon",
                new Vector2(craftButton.X + 65, craftButton.Y + 18),
                Color.White
            );

            // Close button.
            b.Draw(
                Game1.fadeToBlackRect,
                closeButton,
                Color.DarkRed
            );

            b.DrawString(
                Game1.smallFont,
                "Close",
                new Vector2(closeButton.X + 105, closeButton.Y + 10),
                Color.White
            );

            // Status message.
            b.DrawString(
                Game1.smallFont,
                statusMessage,
                new Vector2(xPositionOnScreen + 90, yPositionOnScreen + 260),
                Color.Black
            );

            drawMouse(b);
        }
    }
}
