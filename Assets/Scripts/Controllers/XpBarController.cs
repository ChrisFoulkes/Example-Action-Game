using EventCallbacks;
using UnityEngine;
using UnityEngine.UI;

public class XpBarController : MonoBehaviour
{
    public float maxValue;
    public float currentValue;

    public Image imageSprite;

    void OnEnable()
    {
        EventManager.AddGlobalListener<PlayerExperienceEvent>(OnExpEvent);
    }

    void OnDisable()
    {
        EventManager.RemoveGlobalListener<PlayerExperienceEvent>(OnExpEvent);
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
