namespace EventCallbacks
{
    public class EnemyDamageEvent : DamageEvent
    {
    }

    public class EnemyHealEvent : HealEvent
    {
        //public AbilityContext Caster;
    }
    public class EnemyKilledEvent : GameEvent
    {
        public float xpValue = 1; 
        public Enemy killedEnemy;
    }
}