using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffTracker : MonoBehaviour
{
    private TextMeshProUGUI durationText;

    public void Initialize(float duration)
    {
        durationText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void RefreshDuration(float newDuration)
    {
    }

    public void UpdateDuration(float newDuration)
    {
        // Update the TextMeshPro text to display the current duration rounded to seconds
        durationText.text = Mathf.Round(newDuration).ToString();
    }
}