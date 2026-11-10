using HarmonyLib;
using StardewValley;

namespace SpiceItUpMod.Patches
{
    [HarmonyPatch(typeof(StardewValley.Object), "getCategoryName")]
    internal static class ObjectCategoryPatch
    {
        private static void Postfix(
            StardewValley.Object __instance,
            ref string __result)
        {
            string modId = ModEntry.ModId;

            if (__instance.QualifiedItemId.StartsWith(
                $"(O){modId}_",
                StringComparison.OrdinalIgnoreCase))
            {
                __result = "Seasoning";
            }
        }
    }
}