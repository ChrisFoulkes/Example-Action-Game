using System.Collections;
using UnityEngine;

public class TargetAttack : Attack
{
    private Rigidbody2D rb2d;
    MouseTargetAbility ability;

    protected override void Awake()
    {
        base.Awake();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(MouseTargetAbility ability, Vector3 targetPosition)
    {
        this.ability = ability;

        //Needs Adjusting
        StartCoroutine(MoveAndScale(targetPosition, 0.5f, 2f, 0.7f));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (disableRepeatedHits)
        {
            if (!hitTimeDict.ContainsKey(collision))
            {
                PerformHit(collision);
                hitTimeDict[collision] = Time.time;
            }
        }
        else
        {
            if (!hitTimeDict.ContainsKey(collision) || Time.time - hitTimeDict[collision] > hitCooldown)
            {
                PerformHit(collision);
                hitTimeDict[collision] = Time.time;
            }
        }
    }

    protected override void OnHit(Collider2D collision)
    {
        ability.OnHit(collision, this);
    }

    private IEnumerator MoveAndScale(Vector3 targetPosition, float moveDuration, float scaleFactor, float scaleDuration)
    {

        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = initialScale * scaleFactor;

        // Calculate the adjusted target position based on the change in scale
        Vector3 adjustedTargetPosition = targetPosition - (targetScale - initialScale) * 0.5f;

        while (Time.time - startTime < moveDuration)
        {
            float t = (Time.time - startTime) / moveDuration;

            // Update position
            transform.position = Vector3.Lerp(startPosition, adjustedTargetPosition, t);

            // Update scale if within scale duration
            if (Time.time - startTime < scaleDuration)
            {
                transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            }

            yield return null;
        }

        // Set the position and scale to the target values to make sure they are exactly at the targets
        transform.position = adjustedTargetPosition;

        if (scaleDuration >= moveDuration)
        {
            transform.localScale = targetScale;
        }
    }
    public void DestoryEffect()
    {
        GameObject.Destroy(gameObject);
    }
}
