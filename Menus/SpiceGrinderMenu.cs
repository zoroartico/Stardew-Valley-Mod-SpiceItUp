
using StardewValley;
using StardewValley.Menus;

namespace SpiceItUpMod.Menus
{
    internal class SpiceGrinderMenu : CraftingPage
    {
        public SpiceGrinderMenu()
            : base(
                Game1.uiViewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2,
                Game1.uiViewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2,
                800 + IClickableMenu.borderWidth * 2,
                600 + IClickableMenu.borderWidth * 2,
                cooking: false,
                standaloneMenu: true)
        {
        }

        protected override List<string> GetRecipesToDisplay()
        {
            string recipePrefix = $"{SpiceItUpMod.ModEntry.ModId}_Recipe_";

            var recipes = Game1.content.Load<Dictionary<string, string>>(
                "Data/CraftingRecipes"
            );

            return recipes.Keys
                .Where(key => key.StartsWith(
                    recipePrefix,
                    StringComparison.OrdinalIgnoreCase
                ))
                .ToList();
        }
    }
}
