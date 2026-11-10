using System.Text.Json;
using HarmonyLib;
using StardewValley;
using StardewValley.Buffs;
using SpiceItUpMod.Models;

namespace SpiceItUpMod.Patches
{
    [HarmonyPatch(
        typeof(StardewValley.Object),
        nameof(StardewValley.Object.GetFoodOrDrinkBuffs)
    )]
    internal static class SeasoningBuffPatch
    {
        private const string SeasoningDataKey =
            "SpiceItUpMod/SeasoningEffects";

        private static void Postfix(
            StardewValley.Object __instance,
            ref IEnumerable<Buff> __result)
        {
            if (!__instance.modData.TryGetValue(
                SeasoningDataKey,
                out string data))
            {
                return;
            }

            List<SpiceEffect> effects;

            try
            {
                effects = JsonSerializer.Deserialize<List<SpiceEffect>>(data)
                    ?? new List<SpiceEffect>();
            }
            catch (JsonException)
            {
                return;
            }

            List<Buff> seasoningBuffs = new();

            for (int i = 0; i < effects.Count; i++)
            {
                SpiceEffect effect = effects[i];

                BuffEffects buffEffects = new();

                switch (effect.Type)
                {
                    case SpiceEffectType.Speed:
                        buffEffects.Speed.Value = effect.Value;
                        break;

                    case SpiceEffectType.Attack:
                        buffEffects.Attack.Value = effect.Value;
                        break;

                    case SpiceEffectType.Defense:
                        buffEffects.Defense.Value = effect.Value;
                        break;

                    case SpiceEffectType.Luck:
                        buffEffects.LuckLevel.Value = effect.Value;
                        break;

                    default:
                        continue;
                }

                int duration =
                    effect.Duration * Game1.realMilliSecondsPerGameMinute;

                seasoningBuffs.Add(new Buff(
                    id: $"{ModEntry.ModId}_Seasoning_{effect.Type}_{i}",
                    displayName: $"Seasoned {__instance.DisplayName}",
                    iconTexture: null,
                    iconSheetIndex: -1,
                    duration: duration,
                    effects: buffEffects
                ));
            }

            __result = __result.Concat(seasoningBuffs);
        }
    }
}
