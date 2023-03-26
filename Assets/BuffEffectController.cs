using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class ActiveBuff
{
    public int buffID;
    public float duration;
}

public class BuffEffectController : MonoBehaviour
{
    public GameObject trackerPrefab;
    public Transform buffCanvas;
    public Dictionary<ActiveBuff, BuffTracker> effectDisplay = new Dictionary<ActiveBuff, BuffTracker>();

    public CharacterStatsController characterStatsController;

    // Start is called before the first frame update
    void Start()
    {
        characterStatsController = GetComponent<CharacterStatsController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ApplyBuff(ActiveBuffData buffData)
    { 
        // Check if the buff is already active
        ActiveBuff activeBuff = effectDisplay.Keys.FirstOrDefault(buff => buff.buffID == buffData.BuffID);

        if (activeBuff != null)
        {
            // Refresh the buff duration
            activeBuff.duration = buffData.buffDuration;
        }
        else
        {
            // If the buff is not active, apply the buff
            StartCoroutine(HandleBuff(buffData));
        }
    }

    private IEnumerator HandleBuff(ActiveBuffData buffData)
    {
        // Apply the buff
        BuffTracker buffTracker = Instantiate(trackerPrefab, buffCanvas).GetComponent<BuffTracker>();
        buffTracker.Initialize(buffData.buffDuration);


        ActiveBuff activeBuff = new ActiveBuff { buffID = buffData.BuffID, duration = buffData.buffDuration };
        effectDisplay.Add(activeBuff, buffTracker);

        foreach (AffectStat affectedStat in buffData.affectStats)
        {
            characterStatsController.AlterStat(affectedStat.stat.ID, affectedStat.amount);
        }

        
        while (activeBuff.duration > 0)
        {
            // Update the buff tracker display
            buffTracker.UpdateDuration(activeBuff.duration);

            activeBuff.duration -= 0.1f;

            // Wait for a short period of time before checking again
            yield return new WaitForSeconds(0.1f);
        }

        // Remove the buff
        foreach (AffectStat affectedStat in buffData.affectStats)
        {
            characterStatsController.AlterStat(affectedStat.stat.ID, -affectedStat.amount);
        }

        // Remove the buff tracker from the effect display and destroy the tracker
        effectDisplay.Remove(activeBuff);
        Destroy(buffTracker.gameObject);
    }
}