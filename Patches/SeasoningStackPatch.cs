using HarmonyLib;
using StardewValley;

namespace SpiceItUpMod.Patches
{
    [HarmonyPatch(typeof(Item), nameof(Item.canStackWith))]
    internal static class SeasoningStackPatch
    {
        private static void Postfix(
            Item __instance,
            ISalable other,
            ref bool __result)
        {
            if (!__result)
                return;

            if (__instance is not StardewValley.Object firstObject
                || other is not StardewValley.Object secondObject)
                return;

            var firstEffects =
                SeasoningRestorationHelper.GetEffects(firstObject);

            var secondEffects =
                SeasoningRestorationHelper.GetEffects(secondObject);

            if (firstEffects.Count != secondEffects.Count)
            {
                __result = false;
                return;
            }

            var firstSorted = firstEffects
                .OrderBy(effect => effect.Type)
                .ThenBy(effect => effect.Value)
                .ThenBy(effect => effect.Duration)
                .ToList();

            var secondSorted = secondEffects
                .OrderBy(effect => effect.Type)
                .ThenBy(effect => effect.Value)
                .ThenBy(effect => effect.Duration)
                .ToList();

            for (int i = 0; i < firstSorted.Count; i++)
            {
                if (firstSorted[i].Type != secondSorted[i].Type
                    || firstSorted[i].Value != secondSorted[i].Value
                    || firstSorted[i].Duration != secondSorted[i].Duration)
                {
                    __result = false;
                    return;
                }
            }
        }
    }
}
