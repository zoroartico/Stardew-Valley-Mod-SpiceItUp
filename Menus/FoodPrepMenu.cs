using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiceItUpMod.Content;
using SpiceItUpMod.Models;
using StardewValley;
using StardewValley.Menus;
using System.Text.Json;

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
        private Item? outputItem;
        private Item? hoveredItem;
        private const string SeasoningDataKey =
            "SpiceItUpMod/SeasoningEffects";

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
                this.UpdateOutputPreview();
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
                this.UpdateOutputPreview();
                return;
            }

            // Output slot
            if (this.outputSlot.containsPoint(x, y))
            {
                if (this.heldItem == null && this.outputItem != null)
                {
                    this.heldItem = this.outputItem;
                    this.outputItem = null;

                    ConsumeOne(ref this.foodItem);
                    ConsumeOne(ref this.seasoningItem);

                    this.UpdateOutputPreview();

                    Game1.playSound("coin");
                }

                return;
            }

            // Player inventory.
            this.heldItem = this.inventoryMenu.leftClick(x, y, this.heldItem);
        }

        public override void performHoverAction(int x, int y)
        {
            base.performHoverAction(x, y);

            this.hoveredItem = null;

            if (foodSlot.containsPoint(x, y))
                this.hoveredItem = foodItem;
            else if (seasoningSlot.containsPoint(x, y))
                this.hoveredItem = seasoningItem;
            else if (outputSlot.containsPoint(x, y))
                this.hoveredItem = outputItem;
            else
                this.hoveredItem = inventoryMenu.hover(x, y, null);
        }
        private void UpdateOutputPreview()
        {
            this.outputItem = null;

            if (this.foodItem == null || this.seasoningItem == null)
                return;

            var spice = SpiceCatalog.GetSpices().FirstOrDefault(
                s => $"(O){ModEntry.ModId}_{s.Id}" ==
                     this.seasoningItem.QualifiedItemId
            );

            if (spice == null)
                return;

            // Create a copy of the food.
            Item seasonedFood = this.foodItem.getOne();
            seasonedFood.Stack = 1;

            // Retrieve existing seasoning effects.
            List<SpiceEffect> effects = new();

            if (seasonedFood.modData.TryGetValue(
                SeasoningDataKey,
                out string existingData))
            {
                effects = JsonSerializer.Deserialize<List<SpiceEffect>>(
                    existingData
                ) ?? new List<SpiceEffect>();
            }

            // Append the new effect.
            effects.AddRange(spice.Effects);

            // Save the updated effects.
            seasonedFood.modData[SeasoningDataKey] =
                JsonSerializer.Serialize(effects);

            // Display the preview.
            this.outputItem = seasonedFood;
        }

        private static void ConsumeOne(ref Item? item)
        {
            if (item == null)
                return;

            item.Stack--;

            if (item.Stack <= 0)
                item = null;
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

            this.outputItem?.drawInMenu(
                b,
                new Vector2(outputSlot.bounds.X, outputSlot.bounds.Y),
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

            if (this.hoveredItem != null)
            {
                IClickableMenu.drawToolTip(
                    b,
                    this.hoveredItem.getDescription(),
                    this.hoveredItem.DisplayName,
                    this.hoveredItem
                );
            }

            drawMouse(b);
        }
    }
}