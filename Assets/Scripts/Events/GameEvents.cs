using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class GamePauseEvent : Event<GamePauseEvent>
    {
        public bool isPaused;
    }
}