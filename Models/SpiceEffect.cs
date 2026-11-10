namespace SpiceItUpMod.Models
{
    internal enum SpiceEffectType
    {
        Energy,
        Health,
        Speed,
        Attack,
        Defense,
        Luck
    }

    internal class SpiceEffect
    {
        public SpiceEffectType Type { get; }
        public int Value { get; }
        public int Duration { get; }

        public SpiceEffect(
            SpiceEffectType type,
            int value,
            int duration = 0)
        {
            Type = type;
            Value = value;
            Duration = duration;
        }
    }
}