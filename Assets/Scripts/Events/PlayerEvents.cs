using UnityEngine;
using UnityEngine.UIElements;

namespace EventCallbacks
{
    public class PlayerDeathEvent : GameEvent
    {
        /*
        Info about cause of death, our killer, etc...
        */
    }

    public class DisplayDeathUiEvent : GameEvent
    {
    }

    public class PlayerStopMovementEvent : GameEvent
    {
        public float duration = 0.2f;
    }

    public class SetPlayerFacingDirectionEvent : GameEvent
    {
        public Vector2 direction;
        public float duration;
        public SetPlayerFacingDirectionEvent(Vector2 direction, float time)
        {
            this.direction = direction;
            duration = time;
        }
    }

    public class PlayerExperienceEvent : GameEvent
    {
        public float currentExperience;
        public int currentLevel;
        public float RequiredExperience;
        public bool isLevelUP;

        public PlayerExperienceEvent(bool isLevel, float reqXP, float xp, int Level)
        {
            currentLevel = Level;
            isLevelUP = isLevel;
            RequiredExperience = reqXP;
            currentExperience = xp;
        }
    }

    public class PlayerDamageEvent : DamageEvent
    {
    }

    public class PlayerHealEvent : HealEvent
    {
    }
}