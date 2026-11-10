using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;

namespace SpiceItUpMod.Menus
{
    internal class FoodPrepMenu : IClickableMenu
    {
        private InventoryMenu inventoryMenu;
        private ClickableComponent foodSlot;
        private ClickableComponent seasoningSlot;
        private ClickableComponent outputSlot;
        private Item? heldItem;
        private Item? foodItem;
        private Item? seasoningItem;
        public FoodPrepMenu()
            : base(
                Game1.uiViewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2,
                Game1.uiViewport.Height / 2 - (500 + IClickableMenu.borderWidth * 2) / 2,
                800 + IClickableMenu.borderWidth * 2,
                500 + IClickableMenu.borderWidth * 2)
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

            // Create player inventory.
            this.inventoryMenu = new InventoryMenu(
                xPositionOnScreen + IClickableMenu.borderWidth,
                yPositionOnScreen + 350,
                playerInventory: true
            );
            int slotSize = 64;
            int slotY = yPositionOnScreen + 150;

            this.foodSlot = new ClickableComponent(
                new Rectangle(xPositionOnScreen + 150, slotY, slotSize, slotSize),
                "FoodSlot"
            );

            this.seasoningSlot = new ClickableComponent(
                new Rectangle(xPositionOnScreen + 350, slotY, slotSize, slotSize),
                "SeasoningSlot"
            );

            this.outputSlot = new ClickableComponent(
                new Rectangle(xPositionOnScreen + 550, slotY, slotSize, slotSize),
                "OutputSlot"
            );
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            base.receiveLeftClick(x, y, playSound);

            // Food slot
            if (this.foodSlot.containsPoint(x, y))
            {
                if (this.heldItem == null)
                {
                    this.heldItem = this.foodItem;
                    this.foodItem = null;
                }
                else if (IsValidFood(this.heldItem))
                {
                    Item? previousItem = this.foodItem;
                    this.foodItem = this.heldItem;
                    this.heldItem = previousItem;
                }

                return;
            }

            // Seasoning slot
            if (this.seasoningSlot.containsPoint(x, y))
            {
                if (this.heldItem == null)
                {
                    this.heldItem = this.seasoningItem;
                    this.seasoningItem = null;
                }
                else if (IsValidSeasoning(this.heldItem))
                {
                    Item? previousItem = this.seasoningItem;
                    this.seasoningItem = this.heldItem;
                    this.heldItem = previousItem;
                }

                return;
            }

            // output (non-functioning)
            if (this.outputSlot.containsPoint(x, y))
                return;

       
            this.heldItem = this.inventoryMenu.leftClick(x, y, this.heldItem);
        }

        //what's allowed in food slot
        private static bool IsValidFood(Item item)
        {
            return item is StardewValley.Object obj
                && obj.Category == StardewValley.Object.CookingCategory
                && obj.Edibility > 0;
        }
        //what's allowed in seasoning slot
        private static bool IsValidSeasoning(Item item)
        {
            return item is StardewValley.Object obj
                && obj.Category == -17;
        }

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

            // Draw input/outputs.
            IClickableMenu.drawTextureBox(
                b,
                foodSlot.bounds.X,
                foodSlot.bounds.Y,
                foodSlot.bounds.Width,
                foodSlot.bounds.Height,
                Color.White
            );

            IClickableMenu.drawTextureBox(
                b,
                seasoningSlot.bounds.X,
                seasoningSlot.bounds.Y,
                seasoningSlot.bounds.Width,
                seasoningSlot.bounds.Height,
                Color.White
            );

            IClickableMenu.drawTextureBox(
                b,
                outputSlot.bounds.X,
                outputSlot.bounds.Y,
                outputSlot.bounds.Width,
                outputSlot.bounds.Height,
                Color.White
            );

            this.foodItem?.drawInMenu(
                b,
                new Vector2(foodSlot.bounds.X, foodSlot.bounds.Y),
                1f
            );

            this.seasoningItem?.drawInMenu(
                b,
                new Vector2(seasoningSlot.bounds.X, seasoningSlot.bounds.Y),
                1f
            );

            //draw inventory
            this.inventoryMenu.draw(b);
            if (this.heldItem != null)
            {
                this.heldItem.drawInMenu(
                    b,
                    new Vector2(Game1.getOldMouseX(), Game1.getOldMouseY()),
                    1f
                );
            }
            drawMouse(b);
        }
    }
}