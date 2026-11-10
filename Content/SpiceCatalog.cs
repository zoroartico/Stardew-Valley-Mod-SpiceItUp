
using SpiceItUpMod.Models;

namespace SpiceItUpMod.Content
{
    internal static class SpiceCatalog
    {
        public static IEnumerable<SpiceDefinition> GetSpices()
        {
            return new List<SpiceDefinition>
            {
                new SpiceDefinition(
                    id: "Cinnamon",
                    displayName: "Cinnamon",
                    description: "A warm, aromatic spice. Not to be challenged.",
                    price: 20,
                    edibility: -300,
                    spriteIndex: 246,
                    effects: new List<SpiceEffect>
                    {
                        new(SpiceEffectType.Energy, 50),
                        new(SpiceEffectType.Health, 25),
                        new(SpiceEffectType.Speed, 1, 120),
                        new(SpiceEffectType.Attack, 2, 300),
                        new(SpiceEffectType.Defense, 2, 300),
                        new(SpiceEffectType.Luck, 1, 900)
                    }
                ),

                new SpiceDefinition(
                    id: "GroundGinger",
                    displayName: "Ground Ginger",
                    description: "Unexpectedly sweet when cooked.",
                    price: 25,
                    edibility: -300,
                    spriteIndex: 248,
                    effects: new List<SpiceEffect>
                    {
                        new(SpiceEffectType.Energy, 50),
                        new(SpiceEffectType.Health, 25),
                        new(SpiceEffectType.Speed, 1, 120),
                        new(SpiceEffectType.Attack, 2, 300),
                        new(SpiceEffectType.Defense, 2, 300),
                        new(SpiceEffectType.Luck, 1, 900)
                    }
                ),
                new SpiceDefinition(
                    id: "ChiliPowder",
                    displayName: "Chili Powder",
                    description: "A fiery, spicy blend that adds heat to any dish.",
                    price: 30,
                    edibility: -300,
                    spriteIndex: 249,
                    effects: new List<SpiceEffect>
                    {
                        new(SpiceEffectType.Energy, 50),
                        new(SpiceEffectType.Health, 25),
                        new(SpiceEffectType.Speed, 1, 120),
                        new(SpiceEffectType.Attack, 2, 300),
                        new(SpiceEffectType.Defense, 2, 300),
                        new(SpiceEffectType.Luck, 1, 900)
                    }
                ),
                new SpiceDefinition(
                    id: "GarlicPowder",
                    displayName: "Garlic Powder",
                    description: "The best spice ever created, the apex of natural flavour enhancement.",
                    price: 50,
                    edibility: -300,
                    spriteIndex: 250,
                    effects: new List<SpiceEffect>
                    {
                        new(SpiceEffectType.Energy, 50),
                        new(SpiceEffectType.Health, 25),
                        new(SpiceEffectType.Speed, 1, 120),
                        new(SpiceEffectType.Attack, 2, 300),
                        new(SpiceEffectType.Defense, 2, 300),
                        new(SpiceEffectType.Luck, 1, 900)
                    }
                ),
            };
        }

        public static IEnumerable<SpiceRecipe> GetRecipes()
        {
            return new List<SpiceRecipe>
            {
                new SpiceRecipe(
                    id: "Cinnamon",
                    spiceId: "Cinnamon",
                    ingredients: new Dictionary<string, int>
                    {
                        ["(O)396"] = 1 // Spice Berry
                    }
                ),

                new SpiceRecipe(
                    id: "GroundGinger",
                    spiceId: "GroundGinger",
                    ingredients: new Dictionary<string, int>
                    {
                        ["(O)829"] = 1 // Ginger
                    }
                ),
                new SpiceRecipe(
                    id: "ChiliPowder",
                    spiceId: "ChiliPowder",
                    ingredients: new Dictionary<string, int>
                    {
                        ["(O)260"] = 1 // Hot Peppers
                    }
                ),
                new SpiceRecipe(
                    id: "GarlicPowder",
                    spiceId: "GarlicPowder",
                    ingredients: new Dictionary<string, int>
                    {
                        ["(O)248"] = 1 // Garlic
                    }
                ),
            };
        }
    }
}
