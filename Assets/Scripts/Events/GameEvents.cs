using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}