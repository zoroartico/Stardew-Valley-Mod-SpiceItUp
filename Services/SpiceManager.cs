
using System;
using System.Collections.Generic;
using SpiceItUpMod.Models;
using StardewValley;
using StardewValley.GameData.Objects;

namespace SpiceItUpMod.Services
{
    internal sealed class SpiceManager
    {
        private readonly string modId;

        private readonly Dictionary<string, SpiceDefinition> spices =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, SpiceRecipe> recipes =
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

            this.RegisterRecipe(new SpiceRecipe(
                id: "Cinnamon",
                spiceId: "Cinnamon",
                ingredients: new Dictionary<string, int>
                {
                    ["(O)396"] = 1, // Spice Berry
                    ["(O)16"] = 1   // Wild Horseradish
                }
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

        private void RegisterRecipe(SpiceRecipe recipe)
        {
            if (!this.spices.ContainsKey(recipe.SpiceId))
            {
                throw new InvalidOperationException(
                    $"Spice '{recipe.SpiceId}' is not registered."
                );
            }

            if (!this.recipes.TryAdd(recipe.Id, recipe))
            {
                throw new InvalidOperationException(
                    $"A recipe with ID '{recipe.Id}' is already registered."
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

        public IEnumerable<SpiceRecipe> GetRecipes()
        {
            return this.recipes.Values;
        }

        public bool HasIngredients(Farmer player, SpiceRecipe recipe)
        {
            foreach (var ingredient in recipe.Ingredients)
            {
                int available = player.Items
                    .OfType<Item>()
                    .Where(item => item.QualifiedItemId == ingredient.Key)
                    .Sum(item => item.Stack);

                if (available < ingredient.Value)
                    return false;
            }

            return true;
        }

        public bool Craft(Farmer player, string recipeId)
        {
            if (!this.recipes.TryGetValue(recipeId, out SpiceRecipe? recipe))
                return false;

            if (!this.HasIngredients(player, recipe))
                return false;

            // output the crafted spice
            Item output = ItemRegistry.Create(
                this.GetQualifiedItemId(recipe.SpiceId),
                recipe.OutputCount
            );

            // this prevents losing ingredients if inventory is full.
            if (!player.addItemToInventoryBool(output))
                return false;

            // consume ingredients.
            foreach (var ingredient in recipe.Ingredients)
            {
                int remaining = ingredient.Value;

                foreach (Item item in player.Items.OfType<Item>().ToList())
                {
                    if (item.QualifiedItemId != ingredient.Key)
                        continue;

                    int amountToRemove = Math.Min(item.Stack, remaining);

                    item.Stack -= amountToRemove;
                    remaining -= amountToRemove;

                    if (item.Stack <= 0)
                        player.Items.Remove(item);

                    if (remaining <= 0)
                        break;
                }
            }

            return true;
        }
    }
}
