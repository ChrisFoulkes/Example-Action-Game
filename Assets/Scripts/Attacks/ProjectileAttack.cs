using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    private float projectileSpeed;
    private float projectileLifetime;
    ProjectileAbility ability;

    public Animator animator;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    public void Initialize(ProjectileAbility ability) 
    {
        projectileSpeed = ability.projData.projectileSpeed;
        projectileLifetime = ability.projData.projectileLifetime;

        this.ability = ability;

        rb2d.velocity = transform.up * projectileSpeed;
        StartCoroutine(Lifespan(projectileLifetime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IHealth enemy = collision.GetComponentInParent<IHealth>();
            if (enemy.CurrentHealth() > 0){
                ability.OnHit(collision, this);
                animator.SetBool("OnHit", true);
            }
        }
        else 
        {
            if (collision.CompareTag("Background"))
            {
                animator.SetBool("OnHit", true);
            }
        }

    }

    public void HitAnimationComplete() 
    {
        Destroy(gameObject);
    }

    IEnumerator Lifespan(float delay)
    {
        yield return new WaitForSeconds(delay);


        animator.SetBool("OnHit", true);
    }
}
