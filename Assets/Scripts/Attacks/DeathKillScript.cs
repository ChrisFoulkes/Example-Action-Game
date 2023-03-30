using UnityEngine;

public class DeathKillScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HealthController enemy = other.GetComponentInParent<HealthController>();
            enemy.Kill();
        }
    }
}
