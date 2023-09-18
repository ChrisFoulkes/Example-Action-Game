using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public Animator animator;
    protected Dictionary<Collider2D, float> hitTimeDict;
    public float hitCooldown = 1f;
    public bool disableRepeatedHits = false;

    protected virtual void Awake()
    {

        animator = GetComponent<Animator>();
        hitTimeDict = new Dictionary<Collider2D, float>();
    }

    protected virtual void PerformHit(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IHealth enemy = collision.GetComponentInParent<IHealth>();
            if (enemy.CurrentHealth() > 0)
            {
                OnHit(collision);
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

    protected abstract void OnHit(Collider2D collision);

    public void HitAnimationComplete()
    {
        Destroy(gameObject);
    }
}