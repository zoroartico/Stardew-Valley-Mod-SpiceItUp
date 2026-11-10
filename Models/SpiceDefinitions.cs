
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
        public List<SpiceEffect> Effects { get; }

        public SpiceDefinition(
            string id,
            string displayName,
            string description,
            int price,
            int edibility,
            int spriteIndex,
            List<SpiceEffect> effects
            )
        {
            Id = id;
            DisplayName = displayName;
            Description = description;
            Price = price;
            Edibility = edibility;
            SpriteIndex = spriteIndex;
            Effects = effects;
        }
    }
}