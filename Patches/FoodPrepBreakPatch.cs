using HarmonyLib;
using SpiceItUpMod.Models;

namespace SpiceItUpMod.Patches
{
    [HarmonyPatch(
        typeof(StardewValley.Object),
        nameof(StardewValley.Object.performToolAction)
    )]
    internal static class FoodPrepBreakPatch
    {
        private static bool Prefix(
            StardewValley.Object __instance,
            ref bool __result)
        {
            string stationId =
                $"(BC){ModEntry.ModId}_FoodPrepStation";

            if (__instance.QualifiedItemId != stationId)
                return true;

            if (!FoodPrepStorage.HasItems(__instance))
                return true;

            __result = false;
            return false;
        }
    }
}