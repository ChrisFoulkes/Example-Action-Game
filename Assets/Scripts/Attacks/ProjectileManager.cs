using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class ProjectileManager : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    private bool IsAvailable = true;
    public float duration = 0.4f;
    public float cooldown = 0.5f;
    private PlayerStopMovementEvent stopMovementEvent;
    private SetPlayerFacingDirectionEvent setDirectionEvent;
    public int numProjectiles = 3;
    public float firingArc = 10f;
    bool isDead = false;
    // Start is called before the first frame update
    private void Start()
    {
        PlayerDeathEvent.RegisterListener(OnPlayerDeath);

    }

    void OnDestroy()
    {
        PlayerDeathEvent.UnregisterListener(OnPlayerDeath);
    }
        // Update is called once per frame
        void Update()
    {
        if (!GameManager.Instance.isPaused && !isDead)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (IsAvailable)
                {
                    FireProjectile(numProjectiles, firingArc);
                    StartCoroutine(StartTickCooldown());
                }
            }
        }
    }

    public void IncreaseProjectileCount() 
    {
        numProjectiles++;
    }
    public void DecreaseCooldown() 
    {
        cooldown -= 0.1f;
    }

    void FireProjectile(int numProjectiles, float firingArc)
    {
        for (int i = 0; i < numProjectiles; i++)
        {
            
            //ignore firing arc for singular projectiles 
            if (numProjectiles == 1) 
            {
                firingArc = 0;
            }

            SetTheFiringRotation(firingArc, getIncrement(i, firingArc, numProjectiles));
            //GameObject projectile = Instantiate(projectilePrefab, GetClosestPoint(), projectileSpawnPoint.rotation);

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0f, 0f, 0f));
        }

        stopMovementEvent = new PlayerStopMovementEvent();
        stopMovementEvent.duration = duration;
        stopMovementEvent.FireEvent();

        setDirectionEvent = new SetPlayerFacingDirectionEvent(getFacingDirection(), duration);
        setDirectionEvent.FireEvent();
    }

    float getIncrement(float index, float arc, int count)
    {
        if (count == 1)
            return 0;

        return index * arc / (count - 1);

    }
    void SetTheFiringRotation(float arc, float increment) 
    {
        Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float lookAngle = (Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg) - (arc/2);

        projectileSpawnPoint.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f + increment);
    }

    Vector2 getFacingDirection() 
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position;

        // Get the direction from the player to the mouse
        Vector2 direction = mousePosition - playerPosition;

        // Get the angle of the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Get the facing direction based on the angle
        Vector2 facingDirection;
        if (angle > -45f && angle <= 45f)
        {
            return facingDirection = Vector2.right; // Right
        }
        else if (angle > 45f && angle <= 135f)
        {
            return facingDirection = Vector2.up; // Up
        }
        else if (angle > 135f || angle <= -135f)
        {
            return facingDirection = Vector2.left; // Left
        }
        else
        {
            return facingDirection = Vector2.down; // Down
        }
    }

    Vector3 GetClosestPoint()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        Vector3 closestPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * 0.01f;
        closestPoint += projectileSpawnPoint.position;
        
        return closestPoint;
    }

    void OnPlayerDeath(PlayerDeathEvent death)
    {
        isDead = true;
    }
    public IEnumerator StartTickCooldown()
    {
        IsAvailable = false;
        yield return new WaitForSeconds(cooldown);
        IsAvailable = true;
    }
}
