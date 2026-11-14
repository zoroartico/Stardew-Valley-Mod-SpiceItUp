namespace SpiceItUpMod.Models
{
    internal enum SpiceEffectType
    {
        // Restoration
        Energy,
        Health,

        // Skills
        Farming,
        Fishing,
        Mining,
        Foraging,
        Luck,

        // Player stats
        MaxStamina,
        MagneticRadius,
        Speed,
        Attack, 
        Defense,
        Immunity,

        // Combat multipliers
        AttackMultiplier,
        KnockbackMultiplier,
        WeaponSpeedMultiplier,
        CriticalChanceMultiplier,
        CriticalPowerMultiplier,
        WeaponPrecisionMultiplier
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