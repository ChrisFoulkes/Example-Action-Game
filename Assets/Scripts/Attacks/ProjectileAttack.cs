using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    public float projectileSpeed;
    public float projectileLifetime;
    public int projectileDamage;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();


        rb2d.velocity = transform.up * projectileSpeed;
        Destroy(gameObject, projectileLifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IHealth enemy = collision.GetComponent<IHealth>();
            if (enemy.CurrentHealth() > 0){
                enemy.ChangeHealth(projectileDamage);
                Destroy(gameObject);
            }
        }
        else 
        {
            if (collision.CompareTag("Background")) 
            {
                Destroy(gameObject);
            }
        }

    }
}
