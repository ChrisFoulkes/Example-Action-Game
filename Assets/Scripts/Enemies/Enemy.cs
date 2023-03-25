using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDeath
{
    [Header("Enemy Components")]
    public CircleCollider2D physCollider;
    public CircleCollider2D hurtBox;

    public GameObject xpGem;
    private int minGems = 5;
    private int maxGems = 10;
    public float gemSpawnRadius = 1f;


    [SerializeField] private Vector3 targetScale = new Vector3(5f, 5f, 1);
    [SerializeField] private float scaleDuration = 0.5f;

    public Transform scaleParentTransform;

    private bool isDead = false;

    private EnemyKilledEvent killedEvent = new EnemyKilledEvent();


    private void Start()
    {
        ToggleColliders(true);
    }

    public bool IsDead()
    {
        return isDead;
    }
    public void AddListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        killedEvent.AddLocalListener(listener);
    }

    public void RemoveListener(GameEvent.EventDelegate<GameEvent> listener)
    {
        killedEvent.RemoveLocalListener(listener);
    }

    public void StartDeath()
    {
        StartCoroutine(ScaleCoroutine());
        isDead = true;
        EventManager.Raise(killedEvent);

        SpawnXpGemFountain();

        ToggleColliders(false);
    }

    private void SpawnXpGemFountain()
    {
        int gemCount = Random.Range(minGems, maxGems + 1);

        for (int i = 0; i < gemCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * gemSpawnRadius;
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            GameObject createdXpGem = Instantiate(xpGem, spawnPosition, Quaternion.identity);
        }
    }

    public void CompleteDeath() 
    {
        GameObject.Destroy(transform.parent.gameObject);
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
