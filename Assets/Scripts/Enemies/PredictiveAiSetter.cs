
using Pathfinding;
using UnityEngine;

public class PredictiveAiSetter : AIDestinationSetter
{
    public float predictionTime = 1f;
    public float switchRange = 5f;
    public float offsetDistance = 1f;
    private Transform playerTransform;
    private Rigidbody2D playerRigidbody;
    private Transform targetTransform;

    // Start is called before the first frame update
    new void Awake()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerRigidbody = player.GetComponent<Rigidbody2D>();
        }

        // Create a new temporary target transform and set it as the target for the AIDestinationSetter
        targetTransform = new GameObject("PredictedTarget").transform;
        target = targetTransform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate the distance between the AI and the player
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // Calculate the direction from the AI to the player
        Vector2 aiToPlayerDirection = (playerTransform.position - transform.position).normalized;

        // Calculate the dot product between the direction from the AI to the player and the player's velocity
        float dotProduct = Vector2.Dot(aiToPlayerDirection, playerRigidbody.velocity.normalized);

        if (distanceToPlayer <= switchRange && dotProduct > 0)
        {
            // If the player is within the switch range and moving towards the AI, set the target to the player's position
            target.position = playerTransform.position;
        }
        else
        {
            // Calculate the predicted player position based on their velocity and predictionTime
            Vector2 playerVelocity = playerRigidbody.velocity;
            Vector2 predictedPlayerPosition = playerTransform.position + (Vector3)(playerVelocity * predictionTime);

            // If the AI is within the switch range, set the target to a position in front of the player
            if (distanceToPlayer <= switchRange)
            {
                Vector2 playerDirection = playerRigidbody.velocity.normalized;
                Vector2 targetPosition = (Vector2)playerTransform.position + playerDirection * offsetDistance;
                target.position = targetPosition;
            }
            else
            {
                // Update the target position and set the target to the temporary target transform
                targetTransform.position = predictedPlayerPosition;
                target = targetTransform;
            }
        }
    }
}