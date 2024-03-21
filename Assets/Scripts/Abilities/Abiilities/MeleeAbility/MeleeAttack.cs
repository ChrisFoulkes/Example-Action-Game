using UnityEngine;

public class MeleeAttack : Attack
{
    MeleeAbility ability;

    public void Initialize(MeleeAbility ability)
    {
        this.ability = ability;
        animator.SetFloat("AnimMulti", ability.MeleeData.originalCastTime / ability.CastTime);
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
        IDamage damageController  = collision.GetComponentInParent<IDamage>();

        TargetContext target = damageController.GetContext();
        ability.OnHit(target, this);

        SpawnHitEffect(target, collision);
    }

    public void SpawnHitEffect(TargetContext target, Collider2D collision)
    {
        Transform enemyTransform = target.Transform;
        Vector2 collisionPoint = collision.ClosestPoint(enemyTransform.position);
        HitEffectSpawner.Instance.SpawnHitEffect(collisionPoint, enemyTransform);
    }
}