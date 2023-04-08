using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Very quick and dirty visual time graph of upcoming boss spawns
public class TimeTrackController : MonoBehaviour
{
    public Image TimeTrackBar;
    public GameObject SkullIconPrefab;
    public RectTransform EventIconContainer;

    public WaveManager WaveManager;

    private bool _isPlayerAlive = true;
    public float totalTrackedTime = 300f;

    private float endX = 4f;

    private List<GameObject> activeIcons = new List<GameObject>();

    private void Start()
    {
        CreateInitialTrack();
    }

    private void FixedUpdate()
    {
        if (!_isPlayerAlive) return;

        float elapsedTime = Time.fixedDeltaTime;
        UpdateSkullIconPosition(elapsedTime);

        //this is horrible and i hate it 
        CheckForNewSpawn();
    }

    private void OnEnable()
    {
        EventManager.AddGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    private void OnDisable()
    {
        EventManager.RemoveGlobalListener<PlayerDeathEvent>(OnPlayerDeath);
    }

    void UpdateSkullIconPosition(float elapsedTime)
    {
        for (int i = activeIcons.Count - 1; i >= 0; i--)
        {
            GameObject activeIcon = activeIcons[i];
            float currentTime = Mathf.Max(0, GetElapsedTime(activeIcon) - elapsedTime);
            SetElapsedTime(activeIcon, currentTime);

            if (currentTime == 0)
            {
                activeIcons.RemoveAt(i);
                Destroy(activeIcon);
            }
            else
            {
                activeIcon.transform.localPosition = new Vector3(GetPointOnLine(currentTime), 0, 0);
            }
        }
    }

    void CheckForNewSpawn() 
    {
        float currentLastValue = GetElapsedTime(activeIcons[activeIcons.Count - 1]);
        if (currentLastValue + WaveManager.GetBossSpawnRate() < totalTrackedTime) 
        {
            float skullSpawnTime = currentLastValue + WaveManager.GetBossSpawnRate();

            GameObject skullIcon = Instantiate(SkullIconPrefab, EventIconContainer);
            activeIcons.Add(skullIcon);
            skullIcon.transform.localPosition = new Vector3(GetPointOnLine(skullSpawnTime), 0, 0);
            skullIcon.GetComponent<IconData>().spawnTime = (skullSpawnTime);
        }
    }

    void CreateInitialTrack()
    {

        float bossSpawnTime = WaveManager.GetBossSpawnRate();
        float spawnMultiplier = 1;

        while (bossSpawnTime * spawnMultiplier < totalTrackedTime)
        {
            float skullSpawnTime = bossSpawnTime * spawnMultiplier;

            GameObject skullIcon = Instantiate(SkullIconPrefab, EventIconContainer);
            activeIcons.Add(skullIcon);
            skullIcon.transform.localPosition = new Vector3(GetPointOnLine((totalTrackedTime/2) + skullSpawnTime ), 0, 0);
            skullIcon.GetComponent<IconData>().spawnTime = (totalTrackedTime / 2) + skullSpawnTime;

            spawnMultiplier++;
        }
    }

    float GetPointOnLine(float currentTime)
    {
        float t = (currentTime) / (totalTrackedTime);
        return Mathf.Lerp(-endX, endX, t);
    }

    float GetElapsedTime(GameObject activeIcon)
    {
        return activeIcon.GetComponent<IconData>().spawnTime;
    }

    void SetElapsedTime(GameObject activeIcon, float value)
    {
        activeIcon.GetComponent<IconData>().spawnTime = value;
    }

    private void OnPlayerDeath(PlayerDeathEvent deathEvent)
    {
        _isPlayerAlive = false;
        ClearActiveIcons();
    }

    private void ClearActiveIcons()
    {
        foreach (GameObject icon in activeIcons)
        {
            Destroy(icon);
        }
        activeIcons.Clear();
    }
}


