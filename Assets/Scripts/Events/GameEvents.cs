using System.Numerics;

namespace EventCallbacks
{
    public class GamePauseEvent : GameEvent
    {
        public bool isPaused;
    }

    public class WaveCompleteEvent : GameEvent
    {
        public int completedWave;
    }
    public class HealthChangedEvent : GameEvent
    {
        public float ChangeValue;
    }
    public class DamageEvent : GameEvent
    {
        public DamageInfo DamageInfo;
        public CasterContext Caster;
        public TargetContext Target;
    }

    public class HealEvent : GameEvent
    {
        public HealInfo HealInfo;
        public TargetContext Target;
        //public AbilityContext Caster;
    }
}