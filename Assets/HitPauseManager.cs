using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPauseManager : MonoBehaviour
{
    public static HitPauseManager Instance { get; private set; }

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
    }

    public void RequestHitPause(float duration)
    {
        StartCoroutine(HitPauseCoroutine(duration));
    }

    private IEnumerator HitPauseCoroutine(float duration)
    {
        Time.timeScale = 0.0f;
        float pauseEndTime = Time.realtimeSinceStartup + duration;

        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return null;
        }

        Time.timeScale = 1.0f;
    }
}