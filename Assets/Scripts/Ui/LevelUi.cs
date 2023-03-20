using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUi : MonoBehaviour
{
    TextMeshProUGUI textObj;
    private void Awake()
    {
        textObj = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        PlayerExperienceEvent.RegisterListener(OnEXP);
    }

    void OnDestroy()
    {
        PlayerExperienceEvent.UnregisterListener(OnEXP);
    }


    public void OnEXP(PlayerExperienceEvent XPEvent)
    {
        if (XPEvent.isLevelUP)
        {
            textObj.text = "Level: " + XPEvent.currentLevel;
        }
    }
}
