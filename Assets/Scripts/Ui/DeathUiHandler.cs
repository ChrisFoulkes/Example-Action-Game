using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class DeathUiHandler : MonoBehaviour
{
    public GameObject DisplayParent;
    private void OnEnable()
    {
        EventManager.AddGlobalListener<DisplayDeathUiEvent>(DisplayUI);
    }

    void OnDisable()
    {
        EventManager.RemoveGlobalListener<DisplayDeathUiEvent>(DisplayUI);
    }


    void DisplayUI(DisplayDeathUiEvent eventData)
    {
        DisplayParent.SetActive(true);
    }
}
