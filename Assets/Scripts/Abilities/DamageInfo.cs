using UnityEngine;

public class DamageInfo
{
    public float Amount;
    public bool IsCriticalHit;
    public FloatingColourType ColourType;
    public float HitStun = 0f;
    public KnockbackData Knockback { get; }
    public Vector3 EffectPosition;

    public DamageInfo(float amount, bool isCriticalHit, FloatingColourType colourType, float hitStun = 0f, KnockbackData knockback = null)
    {
        Amount = amount;
        IsCriticalHit = isCriticalHit;
        ColourType = colourType;
        HitStun = hitStun;
        Knockback = knockback;
    }
    public DamageInfo(float amount, bool isCriticalHit, FloatingColourType colourType, float hitStun, Vector3 effectPosition, KnockbackData knockback)
    {
        Amount = amount;
        IsCriticalHit = isCriticalHit;
        ColourType = colourType;
        HitStun = hitStun;
        Knockback = knockback;
        EffectPosition = effectPosition;
    }
}
