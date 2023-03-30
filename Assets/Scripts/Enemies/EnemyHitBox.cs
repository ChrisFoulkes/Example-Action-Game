using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        ;
        if (collision.CompareTag("Player"))
        {
            GetComponentInParent<EnemyAttackController>().OnHit(collision);
        }
    }
}
