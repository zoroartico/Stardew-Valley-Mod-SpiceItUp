
namespace SpiceItUpMod.Models
{
    internal class SpiceDefinition
    {
        public string Id { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public int Price { get; }
        public int Edibility { get; }
        public int SpriteIndex { get; }

        public SpiceDefinition(
            string id,
            string displayName,
            string description,
            int price,
            int edibility,
            int spriteIndex)
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            Price = price;
            Edibility = edibility;
            SpriteIndex = spriteIndex;
        }
    }
}
