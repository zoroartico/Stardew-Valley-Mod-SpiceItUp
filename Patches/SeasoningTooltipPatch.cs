using System.Globalization;
using System.Reflection;
using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using SpiceItUpMod.Models;

namespace SpiceItUpMod.Patches
{
    [HarmonyPatch]
    internal static class SeasoningTooltipPatch
    {
        private static MethodBase TargetMethod()
        {
            return AccessTools.GetDeclaredMethods(typeof(IClickableMenu))
                .First(method =>
                {
                    ParameterInfo[] parameters = method.GetParameters();

                    return method.Name == nameof(IClickableMenu.drawHoverText)
                        && parameters.Length == 25
                        && parameters[1].ParameterType == typeof(string);
                });
        }

        private static void Prefix(
            ref string[] buffIconsToDisplay,
            Item hoveredItem)
        {
            if (hoveredItem is not StardewValley.Object obj)
            {
                return;
            }

            var effects = SeasoningRestorationHelper.GetEffects(obj);

            if (effects.Count == 0)
            {
                return;
            }

            string[] icons = buffIconsToDisplay ?? new string[13];

            if (icons.Length < 13)
            {
                Array.Resize(ref icons, 13);
            }

            for (int i = 0; i < 12; i++)
            {
                icons[i] ??= "0";
            }

            icons[12] ??= "";

            foreach (SpiceEffect effect in effects)
            {
                int index = effect.Type switch
                {
                    SpiceEffectType.Luck => 4,
                    SpiceEffectType.Speed => 9,
                    SpiceEffectType.Defense => 10,
                    SpiceEffectType.Attack => 11,
                    _ => -1
                };

                if (index == -1)
                {
                    continue;
                }

                double currentValue = double.TryParse(
                    icons[index],
                    NumberStyles.Float,
                    CultureInfo.CurrentCulture,
                    out double parsedValue)
                        ? parsedValue
                        : 0;

                icons[index] = (currentValue + effect.Value)
                    .ToString(CultureInfo.CurrentCulture);
            }

            buffIconsToDisplay = icons;
        }
    }
}
