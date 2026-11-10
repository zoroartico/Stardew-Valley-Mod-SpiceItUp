
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HarmonyLib;
using StardewValley;
using SpiceItUpMod.Models;

namespace SpiceItUpMod.Patches
{
    internal static class SeasoningRestorationHelper
    {
        private const string SeasoningDataKey =
            "SpiceItUpMod/SeasoningEffects";

        [ThreadStatic]
        internal static bool IsCalculatingHealth;

        internal static List<SpiceEffect> GetEffects(
            StardewValley.Object item)
        {
            if (!item.modData.TryGetValue(
                SeasoningDataKey,
                out string data))
            {
                return new List<SpiceEffect>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<SpiceEffect>>(data)
                    ?? new List<SpiceEffect>();
            }
            catch (JsonException)
            {
                return new List<SpiceEffect>();
            }
        }
    }

    [HarmonyPatch(
        typeof(StardewValley.Object),
        nameof(StardewValley.Object.staminaRecoveredOnConsumption)
    )]
    internal static class SeasoningEnergyPatch
    {
        private static void Postfix(
            StardewValley.Object __instance,
            ref int __result)
        {
            if (SeasoningRestorationHelper.IsCalculatingHealth)
            {
                return;
            }

            __result += SeasoningRestorationHelper
                .GetEffects(__instance)
                .Where(effect => effect.Type == SpiceEffectType.Energy)
                .Sum(effect => effect.Value);
        }
    }

    [HarmonyPatch(
        typeof(StardewValley.Object),
        nameof(StardewValley.Object.healthRecoveredOnConsumption)
    )]
    internal static class SeasoningHealthPatch
    {
        private static void Prefix(out bool __state)
        {
            __state = SeasoningRestorationHelper.IsCalculatingHealth;
            SeasoningRestorationHelper.IsCalculatingHealth = true;
        }

        private static void Postfix(
            StardewValley.Object __instance,
            ref int __result,
            bool __state)
        {
            __result += SeasoningRestorationHelper
                .GetEffects(__instance)
                .Where(effect => effect.Type == SpiceEffectType.Health)
                .Sum(effect => effect.Value);

            SeasoningRestorationHelper.IsCalculatingHealth = __state;
        }
    }
}
