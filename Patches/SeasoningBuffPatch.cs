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

            List<SpiceEffect> seasoningEffects;

            try
            {
                seasoningEffects =
                    JsonSerializer.Deserialize<List<SpiceEffect>>(data)
                    ?? new List<SpiceEffect>();
            }
            catch (JsonException)
            {
                return;
            }

            // Collect the food's existing effects and durations.
            Dictionary<SpiceEffectType, (float Value, int Duration)>
                combinedEffects = new();

            List<Buff> buffs = __result.ToList();

            foreach (Buff buff in buffs)
            {
                AddEffect(
                    combinedEffects,
                    SpiceEffectType.Speed,
                    buff.effects.Speed.Value,
                    buff.millisecondsDuration
                );

                AddEffect(
                    combinedEffects,
                    SpiceEffectType.Attack,
                    buff.effects.Attack.Value,
                    buff.millisecondsDuration
                );

                AddEffect(
                    combinedEffects,
                    SpiceEffectType.Defense,
                    buff.effects.Defense.Value,
                    buff.millisecondsDuration
                );

                AddEffect(
                    combinedEffects,
                    SpiceEffectType.Luck,
                    buff.effects.LuckLevel.Value,
                    buff.millisecondsDuration
                );

                // Remove these effects from the original buff.
                // Other vanilla effects remain untouched.
                buff.effects.Speed.Value = 0;
                buff.effects.Attack.Value = 0;
                buff.effects.Defense.Value = 0;
                buff.effects.LuckLevel.Value = 0;
            }

            // Remove original buffs that no longer contain effects.
            // Keep buffs that still have actions to apply.
            buffs.RemoveAll(buff =>
                !buff.HasAnyEffects()
                && (buff.actionsOnApply == null
                    || buff.actionsOnApply.Length == 0)
            );

            // Add the seasoning effects to the collected food effects.
            foreach (SpiceEffect effect in seasoningEffects)
            {
                if (effect.Type != SpiceEffectType.Speed
                    && effect.Type != SpiceEffectType.Attack
                    && effect.Type != SpiceEffectType.Defense
                    && effect.Type != SpiceEffectType.Luck)
                {
                    continue;
                }

                int duration =
                    effect.Duration * Game1.realMilliSecondsPerGameMinute;

                AddEffect(
                    combinedEffects,
                    effect.Type,
                    effect.Value,
                    duration
                );
            }

            // Create one buff per effect type.
            foreach (var entry in combinedEffects)
            {
                BuffEffects buffEffects = new();

                switch (entry.Key)
                {
                    case SpiceEffectType.Speed:
                        buffEffects.Speed.Value = entry.Value.Value;
                        break;

                    case SpiceEffectType.Attack:
                        buffEffects.Attack.Value = entry.Value.Value;
                        break;

                    case SpiceEffectType.Defense:
                        buffEffects.Defense.Value = entry.Value.Value;
                        break;

                    case SpiceEffectType.Luck:
                        buffEffects.LuckLevel.Value = entry.Value.Value;
                        break;
                }

                buffs.Add(new Buff(
                    id: $"{ModEntry.ModId}_SeasonedFood_{entry.Key}",
                    displayName: $"Seasoned {__instance.DisplayName}",
                    duration: entry.Value.Duration,
                    effects: buffEffects
                ));
            }

            __result = buffs;
        }

        private static void AddEffect(
            Dictionary<SpiceEffectType, (float Value, int Duration)> effects,
            SpiceEffectType type,
            float value,
            int duration)
        {
            if (value == 0)
                return;

            if (effects.TryGetValue(type, out var current))
            {
                int combinedDuration;

                if (current.Duration == Buff.ENDLESS
                    || duration == Buff.ENDLESS)
                {
                    combinedDuration = Buff.ENDLESS;
                }
                else
                {
                    combinedDuration =
                        Math.Max(0, current.Duration)
                        + Math.Max(0, duration);
                }

                effects[type] = (
                    current.Value + value,
                    combinedDuration
                );
            }
            else
            {
                effects[type] = (value, duration);
            }
        }
    }
}
