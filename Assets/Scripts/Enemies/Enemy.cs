using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDeath
{
    [Header("Enemy Components")]
    public CircleCollider2D physCollider;
    public CircleCollider2D hurtBox;

    [SerializeField] private Vector3 targetScale = new Vector3(5f, 5f, 1);
    [SerializeField] private float scaleDuration = 0.5f;

    public Transform scaleParentTransform;

    private bool isDead = false;

    private void Start()
    {
        ToggleColliders(true);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void StartDeath()
    {
        StartCoroutine(ScaleCoroutine());
        isDead = true;

        EnemyKilledEvent killedEvent = new EnemyKilledEvent();
        killedEvent.xpValue = 10;
        killedEvent.FireEvent();

        ToggleColliders(false);
    }

    public void ToggleColliders(bool toggle)
    {
        physCollider.enabled = toggle;
        hurtBox.enabled = toggle;
    }


    //DeathScaling Effect
    private IEnumerator ScaleCoroutine()
    {
        Vector3 initialScale = scaleParentTransform.localScale;
        float elapsedTime = 0;

        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / scaleDuration;
            scaleParentTransform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
            yield return null;
        }

        scaleParentTransform.localScale = targetScale;
    }
}
