using HarmonyLib;
using Microsoft.Xna.Framework;

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

    [HarmonyPatch(typeof(StardewValley.Object), "GetCategoryColor")]
    internal static class ObjectCategoryColorPatch
    {
        private static void Postfix(
            int category,
            ref Color __result)
        {
            if (category == -17)
            {
                __result = new Color(94, 46, 3);
            }
        }
    }
}