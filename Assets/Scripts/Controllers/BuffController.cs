using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveBuff
{
    public int BuffID { get; private set; }
    public float Duration;
    public List<AffectedStat> AffectedStats;
    public bool IsStackable;
    public float MaxStacks;
    public float StackCount = 0;

    public ActiveBuff(int id, float duration, List<AffectedStat> affectedStats, bool isStackable, float maxStacks)
    {
        this.BuffID = id;
        this.Duration = duration;
        this.AffectedStats = affectedStats;
        this.IsStackable = isStackable;
        this.MaxStacks = maxStacks;

    }
}

public class BuffController : MonoBehaviour
{
    [SerializeField] private GameObject _trackerPrefab;
    [SerializeField] private Transform _buffCanvas;
    private Dictionary<ActiveBuff, BuffTracker> _effectDisplay = new Dictionary<ActiveBuff, BuffTracker>();

    private CharacterStatsController _characterStatsController;

    private void Start()
    {
        _characterStatsController = GetComponent<CharacterStatsController>();
    }

    public void ApplyBuff(ActiveBuffData buffData)
    {
        ActiveBuff activeBuff = _effectDisplay.Keys.FirstOrDefault(buff => buff.BuffID == buffData.BuffID);

        if (activeBuff != null)
        {
            activeBuff.Duration = buffData.BuffDuration;

            if (activeBuff.IsStackable && activeBuff.StackCount < activeBuff.MaxStacks)
            {
                foreach (AffectedStat affectedStat in buffData.AffectedStats)
                {
                    Debug.Log("STACK: " + activeBuff.StackCount);
                    _characterStatsController.AlterStat(affectedStat.Stat.ID, -affectedStat.Amount * activeBuff.StackCount);
                }

                activeBuff.StackCount++; 
                
                foreach (AffectedStat affectedStat in buffData.AffectedStats)
                {
                    Debug.Log("STACK: " + activeBuff.StackCount);
                    _characterStatsController.AlterStat(affectedStat.Stat.ID, affectedStat.Amount * activeBuff.StackCount);
                }
            }
        }
        else
        {
            ActiveBuff newActiveBuff = new ActiveBuff(buffData.BuffID, buffData.BuffDuration, buffData.AffectedStats, buffData.IsStackable, buffData.MaxStacks);
            newActiveBuff.StackCount++;
            StartCoroutine(HandleBuff(newActiveBuff, buffData));

            
        }
    }

    private IEnumerator HandleBuff(ActiveBuff activeBuff, ActiveBuffData buffData)
    {
        BuffTracker buffTracker = Instantiate(_trackerPrefab, _buffCanvas).GetComponent<BuffTracker>();
        buffTracker.Initialize(buffData.BuffDuration);

        _effectDisplay.Add(activeBuff, buffTracker);

        foreach (AffectedStat affectedStat in buffData.AffectedStats)
        {
            Debug.Log("STACK: " + activeBuff.StackCount);
            _characterStatsController.AlterStat(affectedStat.Stat.ID, affectedStat.Amount);
        }

        while (activeBuff.Duration > 0)
        {
            buffTracker.UpdateDuration(activeBuff.Duration);

            activeBuff.Duration -= 0.3f;

            //buff tick rate on duration
            yield return new WaitForSeconds(0.3f);
        }

        RemoveActiveBuff(activeBuff);
    }

    public void RemoveBuff(int buffID)
    {
        ActiveBuff buffToRemove = _effectDisplay.Keys.FirstOrDefault(buff => buff.BuffID == buffID);

        if (buffToRemove != null)
        {
            RemoveActiveBuff(buffToRemove);
            
        }
        else
        {
            Debug.LogWarning("Attempting to remove missing buff: ID " + buffID + " not Found ");
        }
    }

    private void RemoveActiveBuff(ActiveBuff activeBuff)
    {
        foreach (AffectedStat affectedStat in activeBuff.AffectedStats)
        {
            _characterStatsController.AlterStat(affectedStat.Stat.ID, -affectedStat.Amount * activeBuff.StackCount);
        }

        BuffTracker buffTracker = _effectDisplay[activeBuff];
        _effectDisplay.Remove(activeBuff);
        Destroy(buffTracker.gameObject);
        activeBuff.StackCount = 0;
    }
}