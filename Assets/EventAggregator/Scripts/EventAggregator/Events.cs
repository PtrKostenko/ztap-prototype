namespace EventAggregation
{
    public class SimpleEvent : IEventBase { }

    public class IntEvent : IEventBase { public int Value { get; set; } }

    public class CharacterDamaged : IEventBase { public float Value { get; set; } }

    public class ZombieKilledByPlayersCharacter : IEventBase { public int Exp { get; set; } }

    public class ZombieKilledByPlayer : IEventBase { public int Exp { get; set; } }

    public class CharacterLevelUp : IEventBase
    {
        public int Level { get; set; }
        public float MaxHp { get; set; }
        public float Damage { get; set; }
        public float Radius { get; set; }
        public int ExpToNextLvl { get; set; }
    }

    public class CharacterLevelReached : IEventBase { }

    public class CharacterDeath : IEventBase { }

}