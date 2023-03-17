using EventCallbacks;
using UnityEngine;

public class Enemy : MonoBehaviour, IDeath
{
    [Header("Enemy Components")]
    public CircleCollider2D physCollider;
    public CircleCollider2D hurtBox;

    private bool isDead = false;

    private void Awake()
    {
    }

    private void Start()
    {
        ToggleColliders(true);
    }

    private void LateUpdate()
    {
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void StartDeath()
    {
        isDead = true;
        EnemyKilledEvent killedEvent = new EnemyKilledEvent();
        killedEvent.FireEvent();

        ToggleColliders(false);
    }

    public void ToggleColliders(bool toggle)
    {
        physCollider.enabled = toggle;
        hurtBox.enabled = toggle;
    }
}
