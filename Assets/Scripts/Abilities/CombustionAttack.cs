using UnityEngine;

public class CombustionAttack : Attack
{
    CombustionAbility ability;

    public void Initialize(CombustionAbility ability)
    {
        this.ability = ability;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (disableRepeatedHits)
        {
            if (!hitTimeDict.ContainsKey(collision))
            {
                PerformHit(collision);
                hitTimeDict[collision] = Time.time;
            }
        }
        else
        {
            if (!hitTimeDict.ContainsKey(collision) || Time.time - hitTimeDict[collision] > hitCooldown)
            {
                PerformHit(collision);
                hitTimeDict[collision] = Time.time;
            }
        }
    }

    protected override void OnHit(Collider2D collision)
    {
        IDamage damageController = collision.GetComponentInParent<IDamage>();

        /*
         * non functional needs further support
        damageController.ApplyDamage(new DamageInfo(-4, false, FloatingColourType.Ignite) _caster);
        */
    }
}
