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

            // Two columns, three rows. Will change later to better accomodate stacking, just testing idea rn
            const float iconSize = 10f;
            const float iconScale = 1.2f;
            const float spacing = 1f;

            float totalWidth = (iconSize * iconScale * 2) + spacing;
            float startX = location.X + 64f * scaleSize - totalWidth;
            float startY = location.Y + 64f * scaleSize - 25f;

            for (int i = 0; i < uniqueEffects.Count; i++)
            {
                int column = i % 2;
                int row = i / 2;

                Vector2 position = new(
                    startX + column * (iconSize * iconScale + spacing),
                    startY + row * (iconSize * iconScale + spacing)
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
