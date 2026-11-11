using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace SpiceItUpMod.Patches
{
    [HarmonyPatch(
        typeof(StardewValley.Object),
        nameof(StardewValley.Object.drawInMenu)
    )]
    internal static class SeasoningIconPatch
    {
        private static readonly Dictionary<Models.SpiceEffectType, Rectangle>
            IconSources = new()
            {
                [Models.SpiceEffectType.Energy] =
                    new Rectangle(0, 428, 10, 10),

                [Models.SpiceEffectType.Health] =
                    new Rectangle(0, 438, 10, 10),

                [Models.SpiceEffectType.Luck] =
                    new Rectangle(50, 428, 10, 10),

                [Models.SpiceEffectType.Speed] =
                    new Rectangle(100, 428, 10, 10),

                [Models.SpiceEffectType.Defense] =
                    new Rectangle(110, 428, 10, 10),

                [Models.SpiceEffectType.Attack] =
                    new Rectangle(120, 428, 10, 10)
            };

        private static void Postfix(
            StardewValley.Object __instance,
            SpriteBatch spriteBatch,
            Vector2 location,
            float scaleSize,
            float transparency,
            float layerDepth)
        {
            List<Models.SpiceEffect> effects =
                SeasoningRestorationHelper.GetEffects(__instance);

            if (effects.Count == 0)
                return;

            // Remove duplicate icons.
            var uniqueEffects = new List<Models.SpiceEffectType>();

            foreach (var effect in effects)
            {
                if (!uniqueEffects.Contains(effect.Type)
                    && IconSources.ContainsKey(effect.Type))
                {
                    uniqueEffects.Add(effect.Type);
                }
            }

            // Three columns, rows grow upward from the bottom-left.
            const float iconSize = 10f;
            const float iconScale = 1.2f;
            const float spacing = 1f;
            const int columns = 3;

            float iconSpacing = iconSize * iconScale + spacing;

            float startX = location.X + 2f;
            float startY = location.Y + 64f * scaleSize
                - iconSpacing * ((uniqueEffects.Count + columns - 1) / columns);

            for (int i = 0; i < uniqueEffects.Count; i++)
            {
                int column = i % columns;
                int row = i / columns;

                Vector2 position = new(
                    startX + column * iconSpacing,
                    startY + ((uniqueEffects.Count + columns - 1) / columns - 1 - row) * iconSpacing
                );

                spriteBatch.Draw(
                    Game1.mouseCursors,
                    position,
                    IconSources[uniqueEffects[i]],
                    Color.White * transparency,
                    0f,
                    Vector2.Zero,
                    iconScale,
                    SpriteEffects.None,
                    layerDepth + 0.00001f
                );
            }
        }
    }
}
