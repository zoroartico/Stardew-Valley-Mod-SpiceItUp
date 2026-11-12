using HarmonyLib;
using StardewValley;

namespace SpiceItUpMod.Patches
{
    [HarmonyPatch(typeof(Farmer), nameof(Farmer.doneEating))]
    internal static class FoodBuffReplacementPatch
    {
        private static void Prefix(
            Farmer __instance,
            Item ___itemToEat)
        {
            if (___itemToEat is not StardewValley.Object food)
                return;

            // Drinks must not replace food buffs.
            if (Game1.objectData.TryGetValue(
                food.ItemId,
                out var data
            ) && data.IsDrink)
            {
                return;
            }

            // Only replace existing buffs when the new food has buffs.
            if (!food.GetFoodOrDrinkBuffs().Any())
                return;

            List<string> buffIds = __instance.buffs.AppliedBuffs.Keys
                .Where(id =>
                    id == "food"
                    || id.StartsWith(
                        $"{ModEntry.ModId}_SeasonedFood_"
                    )
                )
                .ToList();

            foreach (string id in buffIds)
                __instance.buffs.Remove(id);
        }
    }
}