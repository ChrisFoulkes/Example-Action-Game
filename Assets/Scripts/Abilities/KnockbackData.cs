using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KnockbackData
{
    public bool shouldKnockback = false;
    public bool knockbackFromEffect = false;
    public Vector2 Force;
    public float Duration;
}