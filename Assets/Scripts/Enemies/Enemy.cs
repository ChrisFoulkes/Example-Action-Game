using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDeath
{
    public Animator animator;
    public HpBarController HpBar;
    public bool deathComplete = false;

    public bool IsAvailable = true;
    public float damage = -1f;
    public float tickRate = 1f;
    private Pathfinding.AIPath aiPath;
    private BoxCollider2D boxCollider2D;
    private FloatingCombatTextController combatText;

    public void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        aiPath = GetComponent<Pathfinding.AIPath>();
        combatText = GetComponent<FloatingCombatTextController>();
    }

    public void Start()
    {
        boxCollider2D.enabled = true;
    }


    public void StartDeath()
    {
        EnemyKilledEvent killedEvent = new EnemyKilledEvent();
        killedEvent.FireEvent();

        boxCollider2D.enabled = false;
        aiPath.canMove = false;
        animator.SetBool("IsDead", true);
    }

    public void CompleteDeath()
    {
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (IsAvailable)
            {
                IHealth healthController = collision.gameObject.GetComponent<IHealth>();
                healthController.ChangeHealth(damage);
                StartCoroutine(StartTickCooldown());
            }
        }
    }
    private IEnumerator StartTickCooldown()
    {
        IsAvailable = false;
        yield return new WaitForSeconds(tickRate);
        IsAvailable = true;
    }
}
