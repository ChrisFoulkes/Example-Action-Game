using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
        MeleeAbility ability;

        public Animator animator;
        private Rigidbody2D rb2d;

        private void Awake()
        {
            rb2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

        }

        public void Initialize(MeleeAbility ability)
        {
            this.ability = ability;

            animator.SetFloat("AnimMulti", ability.meleeData.originalCastTime / ability.meleeData.castTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                IHealth enemy = collision.GetComponentInParent<IHealth>();
                if (enemy.CurrentHealth() > 0)
                {
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
    }

