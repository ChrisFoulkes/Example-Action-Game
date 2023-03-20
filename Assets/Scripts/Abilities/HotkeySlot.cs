using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotkeySlot : MonoBehaviour
{
   private bool isOnCooldown = false;

    private float cooldownDuration = 0f;
    private float currentCooldown = 0f;

    public Image cooldownImage;
    public GameObject KeytextObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isOnCooldown) 
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                isOnCooldown = false;
                //KeytextObj.SetActive(true);
                cooldownImage.fillAmount = 0;
            }
            else
            {
                cooldownImage.fillAmount = (currentCooldown/ cooldownDuration);
            }

        }
    }

    public void StartCoolDown(float duration) 
    {
        //KeytextObj.SetActive(false);
        cooldownDuration = duration;
        currentCooldown = duration;
        isOnCooldown = true; 
    }
}
