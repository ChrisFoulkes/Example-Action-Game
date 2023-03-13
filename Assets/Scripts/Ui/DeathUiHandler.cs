using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class DeathUiHandler : MonoBehaviour
{
    public GameObject DisplayParent;
    private void Start()
    {
        DisplayDeathUiEvent.RegisterListener(DisplayUI);

    }

    void OnDestroy()
    {
        DisplayDeathUiEvent.UnregisterListener(DisplayUI);
    }


    void DisplayUI(DisplayDeathUiEvent eventData)
    {
        DisplayParent.SetActive(true);
    }
}
