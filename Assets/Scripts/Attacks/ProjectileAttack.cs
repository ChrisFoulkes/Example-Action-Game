using System.Collections;
using UnityEngine;
public class ProjectileAttack : Attack
{
    private float projectileSpeed;
    private float projectileLifetime;
    private Rigidbody2D rb2d;
    ProjectileAbility ability;

    protected override void Awake()
    {
        base.Awake();
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Initialize(ProjectileAbility ability)
    {
        projectileSpeed = ability.ProjData.projectileSpeed;
        projectileLifetime = ability.ProjData.projectileLifetime;

        this.ability = ability;

        rb2d.velocity = transform.up * projectileSpeed;
        StartCoroutine(Lifespan(projectileLifetime));
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

    IEnumerator Lifespan(float delay)
    {
        yield return new WaitForSeconds(delay);


        animator.SetBool("OnHit", true);
    }
}