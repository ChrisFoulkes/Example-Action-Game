using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace EventCallbacks
{
    public class PlayerDeathEvent : Event<PlayerDeathEvent>
    {
        /*
        Info about cause of death, our killer, etc...
        */
    }

    public class DisplayDeathUiEvent : Event<DisplayDeathUiEvent>
    { 
    }

    public class PlayerStopMovementEvent : Event<PlayerStopMovementEvent>
    {
        public float duration = 0.2f;
    }

    public class SetPlayerFacingDirectionEvent : Event<SetPlayerFacingDirectionEvent>
    {
        public Vector2 direction;

        public SetPlayerFacingDirectionEvent(Vector2 direction)
        {
            this.direction = direction;
        }   
    }

    public class EnemyKilledEvent : Event<EnemyKilledEvent>
    {
        public float xpValue = 1;
    }

    public class PlayerExperienceEvent : Event<PlayerExperienceEvent>
    {
        public float currentExperience;
        public float RequiredExperience;
        public bool isLevelUP;

        public PlayerExperienceEvent(bool isLevel, float reqXP, float xp)
        {
            isLevelUP = isLevel;
            RequiredExperience = reqXP;
            currentExperience = xp;
        }
    }
}