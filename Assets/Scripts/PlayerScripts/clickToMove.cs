using EventCallbacks;
using UnityEngine;

public class clickToMove : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        ResetPlayerDestination();

    }

    void OnEnable()
    {
        EventManager.AddGlobalListener<GamePauseEvent>(OnPauseEvent);
    }
    void OnDisable()
    {
        EventManager.RemoveGlobalListener<GamePauseEvent>(OnPauseEvent);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (Input.GetMouseButtonDown(0))
            {
                gameObject.transform.position = mousePosition;
            }


            // need to update the click to move to better support cancelling movement 

            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                ResetPlayerDestination();
            }
        }
    }

    void ResetPlayerDestination()
    {
        gameObject.transform.position = (player.position);
    }

    void OnPauseEvent(GamePauseEvent pauseEvent)
    {
        if (pauseEvent.isPaused)
        {
            ResetPlayerDestination();
        }
    }

}
