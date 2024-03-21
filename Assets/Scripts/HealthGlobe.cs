using System.Collections;
using UnityEngine;

//Quick and dirty heal globe
public class HealthGlobe : MonoBehaviour
{
    public GameObject player;
    public float attractSpeed = 2f;
    public float speedIncreaseMultiplier = 1.5f;
    private bool attractToPlayer = false;
    private bool alreadyAttracting = false;
    public float initialDelay = 3f;

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
        if (collision.CompareTag("PlayerPickupDestroy"))
        {
            if (player == null)
            {
                player = collision.gameObject;
            }


            //Non functional test - fix
            //player.GetComponentInParent<HealthController>().ChangeHealth(5, false, FloatingColourType.Heal);

            Destroy(gameObject);
        }
    }

    private IEnumerator AttractGlobeToPlayer()
    {
        float currentTime = 0f;

        while (attractToPlayer)
        {
            currentTime += Time.deltaTime;

            Vector3 targetPosition = player.transform.position;
            float step = attractSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            float distance = Vector3.Distance(transform.position, targetPosition);
            if (distance <= 0.1f)
            {
                break;
            }

            attractSpeed += currentTime * speedIncreaseMultiplier * Time.deltaTime;

            yield return null;
        }
    }
}