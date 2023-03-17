using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.UI;

public class XpBarController : MonoBehaviour
{
    public float maxValue;
    public float currentValue;

    public Image imageSprite;


    private void Start()
    { 
        PlayerExperienceEvent.RegisterListener(OnExpEvent);
    }

    private void OnDestroy()
    {
        PlayerExperienceEvent.UnregisterListener(OnExpEvent);
    }



    void OnExpEvent(PlayerExperienceEvent pXPEvent)
    {
        currentValue = pXPEvent.currentExperience;
        maxValue = pXPEvent.RequiredExperience;
        UpdateXPBar();
    }

    void UpdateXPBar()
    {
        float fillAmount = Mathf.Clamp(currentValue / maxValue, 0, 1);
        Vector3 xpScale = imageSprite.transform.localScale;
        imageSprite.transform.localScale = new Vector3(fillAmount, xpScale.y, xpScale.z);
    }

}
