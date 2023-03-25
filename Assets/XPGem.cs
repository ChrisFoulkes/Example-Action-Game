using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPGem : MonoBehaviour
{
    public GameObject player;
    public float attractSpeed = 2f;
    public float speedIncreaseMultiplier = 1.5f;
    private bool attractToPlayer = false;
    private bool alreadyAttracting = false;
    public float initialDelay = 3f;

    public float minDestroyDistance = 0.1f;
    public float maxAttractSpeed = 30f;

    // ... other methods ...

    private void FixedUpdate()
    {
        if (player != null && Vector3.Distance(transform.position, player.transform.position) <= minDestroyDistance)
        {
            player.GetComponentInParent<PlayerExperienceController>().AddExperience(2);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(ActivateGlobe(initialDelay));
    }

    private IEnumerator ActivateGlobe(float delay)
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(delay);
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPickup") && !alreadyAttracting)
        {
            player = collision.gameObject;
            attractToPlayer = true;
            alreadyAttracting = true;
            StartCoroutine(AttractGlobeToPlayer());
        }
    }
    private IEnumerator AttractGlobeToPlayer()
    {
        float currentTime = 0f;
        Vector3 startPoint = transform.position;
        Vector3 controlPoint = startPoint + new Vector3(0, 2, 0);

        while (attractToPlayer)
        {
            currentTime += Time.deltaTime * attractSpeed;
            Vector3 targetPosition = player.transform.position;

            // Clamp the t value between 0 and 1
            float t = Mathf.Clamp01(currentTime);

            transform.position = CalculateBezierPoint(t, startPoint, controlPoint, targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) <= minDestroyDistance)
            {
                attractToPlayer = false;
            }
            else
            {
                attractSpeed += currentTime * speedIncreaseMultiplier * Time.deltaTime;
                if (attractSpeed > maxAttractSpeed) { attractSpeed = maxAttractSpeed; }
            }

            yield return null;
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = (uu * p0) + (2 * u * t * p1) + (tt * p2);

        return point;
    }
}