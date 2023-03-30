using EventCallbacks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isPaused { get; private set; } = false;
    // Start is called before the first frame update
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void PauseGameToggle()
    {
        isPaused = !isPaused;
        GamePauseEvent pauseEvent = new GamePauseEvent();
        pauseEvent.isPaused = isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        EventManager.Raise(pauseEvent);
    }
}
