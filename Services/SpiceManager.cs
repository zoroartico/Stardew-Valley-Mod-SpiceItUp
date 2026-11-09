
using System;
using System.Collections.Generic;
using SpiceItUpMod.Models;
using StardewValley.GameData.Objects;

namespace SpiceItUpMod.Services
{
    internal sealed class SpiceManager
    {
        private readonly string modId;

        private readonly Dictionary<string, SpiceDefinition> spices =
            new(StringComparer.OrdinalIgnoreCase);

        public SpiceManager(string modId)
        {
            this.modId = modId;

            this.Register(new SpiceDefinition(
                id: "Cinnamon",
                displayName: "Cinnamon",
                description: "A warm, aromatic spice.",
                price: 20,
                edibility: -300,
                spriteIndex: 246
            ));
        }

        private void Register(SpiceDefinition spice)
        {
            if (!this.spices.TryAdd(spice.Id, spice))
            {
                throw new InvalidOperationException(
                    $"A spice with ID '{spice.Id}' is already registered."
                );
            }
        }

        public void AddToObjectData(IDictionary<string, ObjectData> objects)
        {
            foreach (SpiceDefinition spice in this.spices.Values)
            {
                string itemId = $"{this.modId}_{spice.Id}";

                objects[itemId] = new ObjectData
                {
                    Name = itemId,
                    DisplayName = spice.DisplayName,
                    Description = spice.Description,
                    Type = "Basic",
                    Category = -17,
                    Price = spice.Price,
                    Edibility = spice.Edibility,
                    Texture = "Maps/springobjects",
                    SpriteIndex = spice.SpriteIndex
                };
            }
        }

        public string GetQualifiedItemId(string spiceId)
        {
            if (!this.spices.ContainsKey(spiceId))
            {
                throw new KeyNotFoundException(
                    $"Spice '{spiceId}' is not registered."
                );
            }

            return $"(O){this.modId}_{spiceId}";
        }
    }
}
