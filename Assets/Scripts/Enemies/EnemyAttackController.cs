using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackAnimationDirection
{
    public Vector2 difference;
    public bool yAxis = false;

    public AttackAnimationDirection(Vector2 diff, bool axis) 
    {
        difference= diff;
        yAxis = axis;
    }
}

public class EnemyAttackController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float damage = -1f;
    public float tickRate = 1.5f;
    public float attackRange = 2f;
    public float attackDuration = 0.45f;
    public float attackStartup = 0.13f;

    public bool canMultihit = false;
    public float attacktickRate = 0f;
    public bool hasHit = false;

    public LayerMask playerLayerMask;

    [SerializeField]
    private bool isAvailable = true;

    [SerializeField]
    private bool isInRange = false;

    [SerializeField]
    private bool isInAttack = false;

    public GameObject prefabHitbox;
    private GameObject hitbox;

    Vector3 closestTargetPosition;
    // Start is called before the first frame update
    void Start()
    {
        isAvailable = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isInAttack)
        {
            CheckTargetInRange();
        }
    }

    void CheckTargetInRange()
    {
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayerMask);

        if (hitObjects.Length > 0)
        {
            if (hitObjects[0].CompareTag("Player"))
            {
                closestTargetPosition = hitObjects[0].transform.position;
                isInRange = true;
            }
        }
        else
        {
            isInRange = false;
        }
    }

    public AttackAnimationDirection GetAnimAttackingDirection()
    {

        bool yAxis = false;
        Vector2 difference = (closestTargetPosition - transform.position);

        // Get the angle of the direction
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        Debug.Log(angle);
        if (angle > 25f && angle <= 130f)
        {
            yAxis = true;
        }
        else if (angle < -40f && angle >= -130f)
        {
            yAxis = true;
        }

        return new AttackAnimationDirection(difference.normalized,
            yAxis);

    }

    public bool IsAttacking() 
    {
        return isInAttack;
    }


    public bool CanAttack() 
    {
        if (isInRange && isAvailable)
        {
            return true;
        }

        return false;
    }

    public void StartEnemyAttack() 
    {
        hasHit = false;
        StartCoroutine(StartAttack(attackDuration));
        StartCoroutine(StartTickCooldown());
    }


    private IEnumerator StartTickCooldown()
    {
        isAvailable = false;
        yield return new WaitForSeconds(tickRate);
        isAvailable = true;
    }

    private IEnumerator StartAttack(float animDuration)
    {
        isInAttack = true;
        StartCoroutine(SpawnHitBox());
        yield return new WaitForSeconds(animDuration);
        isInAttack = false;
        Destroy(hitbox);
    }

    //DrawAttackRange when selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private IEnumerator SpawnHitBox() 
    {
        yield return new WaitForSeconds(attackStartup);
        hitbox = Instantiate(prefabHitbox, transform.position, Quaternion.identity, gameObject.transform);
        AttackAnimationDirection animDirection = GetAnimAttackingDirection();
        if (animDirection.yAxis)
        {
            hitbox.GetComponent<Animator>().SetFloat("DirectionY", GetAnimAttackingDirection().difference.y);
            hitbox.GetComponent<Animator>().SetFloat("DirectionX", 0);
        }
        else
        {
            hitbox.GetComponent<Animator>().SetFloat("DirectionX", GetAnimAttackingDirection().difference.x);
            hitbox.GetComponent<Animator>().SetFloat("DirectionY", 0);
        }
    }

    public void OnHit(Collider2D collision)
    {
        IHealth player = collision.GetComponentInParent<IHealth>();
        if (player.CurrentHealth() > 0 && !hasHit)
        {
            player.ChangeHealth(damage);
        }

        hasHit = true;
    }
}
