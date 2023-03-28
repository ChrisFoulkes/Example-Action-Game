using Pathfinding;
using System.Collections;
using UnityEngine;

public class MeleeAttack : Attack
{
    MeleeAbility ability;
    public Vector2 knockbackForce = new Vector2(10f, 10f);

    public bool shouldKnockback = false;

    public  void Initialize(MeleeAbility ability)
    {
        this.ability = ability;
        animator.SetFloat("AnimMulti", ability.meleeData.originalCastTime / ability.castTime);
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
        ability.OnHit(collision, this);
    }
}