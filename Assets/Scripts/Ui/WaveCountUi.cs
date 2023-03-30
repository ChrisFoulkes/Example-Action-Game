using EventCallbacks;
using TMPro;
using UnityEngine;

public class WaveCountUi : MonoBehaviour
{
    TextMeshProUGUI textObj;
    private void Awake()
    {
        textObj = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        EventManager.AddGlobalListener<WaveCompleteEvent>(OnWaveComplete);
    }

    void OnDisable()
    {
        EventManager.RemoveGlobalListener<WaveCompleteEvent>(OnWaveComplete);
    }


    public void OnWaveComplete(WaveCompleteEvent waveEvent)
    {
        textObj.text = "Wave: " + (waveEvent.completedWave);
    }
}
