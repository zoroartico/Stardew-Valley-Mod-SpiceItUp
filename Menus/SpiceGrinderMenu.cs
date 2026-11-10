
using StardewValley;
using StardewValley.Menus;

namespace SpiceItUpMod.Menus
{
    internal class SpiceGrinderMenu : CraftingPage
    {
        private readonly string recipePrefix;
        public SpiceGrinderMenu(string modId)
            : base(
                Game1.uiViewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2,
                Game1.uiViewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2,
                800 + IClickableMenu.borderWidth * 2,
                600 + IClickableMenu.borderWidth * 2,
                cooking: false,
                standaloneMenu: true)
        {
            this.recipePrefix = $"{modId}_Recipe_";
        }
        protected override List<string> GetRecipesToDisplay()
        {
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
