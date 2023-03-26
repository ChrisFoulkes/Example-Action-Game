using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffTracker : MonoBehaviour
{
    private float duration;
    private bool initialized = false;
    private TextMeshProUGUI durationText;

    public void Initialize(float duration)
    {
        this.duration = duration;
        initialized = true;
        durationText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void RefreshDuration(float newDuration)
    {
        duration = newDuration;
    }

    public void UpdateDuration(float newDuration)
    {
        // Update the TextMeshPro text to display the current duration rounded to seconds
        durationText.text = Mathf.Round(newDuration).ToString();
    }
}