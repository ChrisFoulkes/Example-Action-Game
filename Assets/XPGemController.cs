using System.Collections;
using UnityEngine;

/// <summary>
/// The XPGemController class manages the behavior of experience gem objects
/// This includes attraction towards the player, enabling the gem's collider after a delay, and
/// awarding experience points to the player when the gem reaches them. The attraction is
/// implemented using a quadratic Bezier curve for a smooth path.
/// </summary>
/// 
public class XPGemController : MonoBehaviour
{
    [SerializeField] private GameObject _targetPlayer;
    [SerializeField] private float _initialAttractSpeed = 2f;
    [SerializeField] private float _attractSpeedIncreaseFactor = 1.5f;
    [SerializeField] private float _initialDelay = 3f;
    [SerializeField] private float _minDestroyDistance = 0.1f;
    [SerializeField] private float _maxAttractSpeed = 30f;

    [SerializeField] private Vector3 _archDistance = new Vector3(0, 2, 0);

    private bool _attractToPlayer = false;
    private bool _isAttracting = false;

    private void FixedUpdate()
    {
        if (_targetPlayer != null && Vector3.Distance(transform.position, _targetPlayer.transform.position) <= _minDestroyDistance)
        {
            _targetPlayer.GetComponentInParent<PlayerExperienceController>().AddExperience(2);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(ColliderActivationDelay(_initialDelay));
    }

    private IEnumerator ColliderActivationDelay(float delay)
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(delay);
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerPickup") && !_isAttracting)
        {
            _targetPlayer = collision.gameObject;
            _attractToPlayer = true;
            _isAttracting = true;
            StartCoroutine(AttractGemToPlayer());
        }
    }

    private IEnumerator AttractGemToPlayer()
    {
        float currentTime = 0f;
        Vector3 startPoint = transform.position;
        Vector3 controlPoint = startPoint + _archDistance;

        while (_attractToPlayer)
        {
            currentTime += Time.deltaTime * _initialAttractSpeed;
            Vector3 targetPosition = _targetPlayer.transform.position;

            float t = Mathf.Clamp01(currentTime);

            transform.position = CalculateQuadraticBezierPoint(t, startPoint, controlPoint, targetPosition);

            if (Vector3.Distance(transform.position, targetPosition) <= _minDestroyDistance)
            {
                _attractToPlayer = false;
            }
            else
            {
                _initialAttractSpeed += currentTime * _attractSpeedIncreaseFactor * Time.deltaTime;
                _initialAttractSpeed = Mathf.Min(_initialAttractSpeed, _maxAttractSpeed);
            }

            yield return null;
        }
    }

    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}