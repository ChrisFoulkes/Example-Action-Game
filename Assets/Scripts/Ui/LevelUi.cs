using EventCallbacks;
using TMPro;
using UnityEngine;

public class LevelUi : MonoBehaviour
{
    TextMeshProUGUI textObj;
    private void Awake()
    {
        textObj = GetComponent<TextMeshProUGUI>();
    }
    void OnEnable()
    {
        EventManager.AddGlobalListener<PlayerExperienceEvent>(OnEXP);
    }

    void OnDisable()
    {
        EventManager.RemoveGlobalListener<PlayerExperienceEvent>(OnEXP);
    }


    public void OnEXP(PlayerExperienceEvent XPEvent)
    {
        if (XPEvent.isLevelUP)
        {
            textObj.text = "Level: " + XPEvent.currentLevel;
        }
    }
}
