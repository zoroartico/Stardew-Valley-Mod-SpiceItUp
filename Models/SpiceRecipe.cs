
using System.Collections.Generic;

namespace SpiceItUpMod.Models
{
    internal class SpiceRecipe
    {
        public string Id { get; }
        public string SpiceId { get; }
        public Dictionary<string, int> Ingredients { get; }
        public int OutputCount { get; }

        public SpiceRecipe(
            string id,
            string spiceId,
            Dictionary<string, int> ingredients,
            int outputCount = 1)
        {
            Id = id;
            SpiceId = spiceId;
            Ingredients = ingredients;
            OutputCount = outputCount;
        }
    }
}
