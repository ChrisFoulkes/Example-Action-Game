using EventCallbacks;
using TMPro;
using UnityEngine;

public class RoundTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float startTime;
    private bool isRunning = false;

    public static RoundTimer Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        EventManager.AddGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnDisable()
    {

        EventManager.RemoveGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    public void Start()
    {
        StartTimer();
    }

    private void Update()
    {
        if (isRunning)
        {
            float elapsedTime = Time.time - startTime;
            int hours = Mathf.FloorToInt(elapsedTime / 3600);
            int minutes = Mathf.FloorToInt((elapsedTime % 3600) / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);

            timerText.text = string.Format("Time: {0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
    }

    public void StartTimer()
    {
        startTime = Time.time;
        isRunning = true;
    }

    public void ResetTimer()
    {
        startTime = Time.time;
        timerText.text = "00:00:000";
    }



    private void OnPlayerDeath(PlayerDeathEvent deathEvent)
    {
        isRunning = false;
    }
}