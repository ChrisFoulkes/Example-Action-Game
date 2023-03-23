using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MeleeAttack : Attack
{
    MeleeAbility ability;
    public Vector2 knockbackForce = new Vector2(300f, 300f);

    public bool shouldKnockback = false;

    public  void Initialize(MeleeAbility ability)
    {
        this.ability = ability;
        animator.SetFloat("AnimMulti", ability.meleeData.originalCastTime / ability.meleeData.castTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

        if (shouldKnockback)
        {
            StartCoroutine(KnockbackDelay(collision, 0.2f));
        }
    
    }

    // massive bug this needs to be shifted into the movecontroller on the mob 
    // if the attack speed is high enough the attack will be deleted before movement is re-enaabled 
    IEnumerator KnockbackDelay(Collider2D collision, float delay)
    {

        collision.GetComponentInParent<AIPath>().canMove = false; Rigidbody2D targetRb = collision.GetComponentInParent<Rigidbody2D>();
        if (targetRb != null)
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            targetRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(delay);
        collision.GetComponentInParent<AIPath>().canMove = true;
    }
}