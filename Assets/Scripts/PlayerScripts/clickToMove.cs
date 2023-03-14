using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class clickToMove : MonoBehaviour
{
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        ResetPlayerDestination();
        GamePauseEvent.RegisterListener(OnPauseEvent);

    }

    void OnDestroy()
    {
        GamePauseEvent.UnregisterListener(OnPauseEvent);
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

            if (Input.GetButtonDown("Fire1"))
            {
                ResetPlayerDestination();
            }
        }
    }

    void ResetPlayerDestination() 
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
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
