using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileLifetime;
    public int projectileDamage;

    public Animator animator;
    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb2d.velocity = transform.up * projectileSpeed;
        StartCoroutine(Lifespan(projectileLifetime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IHealth enemy = collision.GetComponent<IHealth>();
            if (enemy.CurrentHealth() > 0){
                enemy.ChangeHealth(projectileDamage);
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
