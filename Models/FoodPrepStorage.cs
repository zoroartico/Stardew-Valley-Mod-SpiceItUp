
using System.Text.Json;
using StardewValley;

namespace SpiceItUpMod.Models
{
    internal static class FoodPrepStorage
    {
        private const string FoodKey = "SpiceItUpMod/StoredFood";
        private const string SeasoningKey = "SpiceItUpMod/StoredSeasoning";

        private class StoredItem
        {
            public string QualifiedItemId { get; set; } = "";
            public int Stack { get; set; }
            public int Quality { get; set; }
            public Dictionary<string, string> ModData { get; set; } = new();
        }

        public static Item? Load(
            StardewValley.Object station,
            string key)
        {
            if (!station.modData.TryGetValue(key, out string data))
                return null;

            try
            {
                StoredItem? stored =
                    JsonSerializer.Deserialize<StoredItem>(data);

                if (stored == null)
                    return null;

                Item item = ItemRegistry.Create(
                    stored.QualifiedItemId,
                    stored.Stack,
                    stored.Quality
                );

                foreach (var entry in stored.ModData)
                    item.modData[entry.Key] = entry.Value;

                return item;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Save(
            StardewValley.Object station,
            string key,
            Item? item)
        {
            if (item == null)
            {
                station.modData.Remove(key);
                return;
            }

            StoredItem stored = new()
            {
                QualifiedItemId = item.QualifiedItemId,
                Stack = item.Stack,
                Quality = item.Quality,
                ModData = item.modData.Pairs.ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value
                )
            };

            station.modData[key] = JsonSerializer.Serialize(stored);
        }

        public static bool HasItems(StardewValley.Object station)
        {
            return station.modData.ContainsKey(FoodKey)
                || station.modData.ContainsKey(SeasoningKey);
        }

        public const string StoredFoodKey = FoodKey;
        public const string StoredSeasoningKey = SeasoningKey;
    }
}
