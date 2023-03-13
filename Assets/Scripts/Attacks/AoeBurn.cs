using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeBurn : MonoBehaviour
{
    public bool IsAvailable = true;
    public float tickRate = 1f;
    public float hitTickRate = 1f;
    public float damage;

    //The list of colliders currently inside the trigger
    public List<Collider2D> TriggerList = new List<Collider2D>();

    //called when somet$$anonymous$$ng enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
        {
            //if the object is not already in the list
            if (!TriggerList.Contains(other))
            {
                //add the object to the list
                TriggerList.Add(other);
            }
        }

    //called when somet$$anonymous$$ng exits the trigger
    private void OnTriggerExit2D(Collider2D other)
        {
            //if the object is in the list
            if (TriggerList.Contains(other))
            {
                //remove it from the list
                TriggerList.Remove(other);
            }
        }

    public void FixedUpdate()
    {
        if (IsAvailable) 
        {
            for (int i = 0; i < TriggerList.Count; ++i)
            {
                if (TriggerList[i] != null) {
                    if (TriggerList[i].CompareTag("Enemy"))
                    {
                        HealthController enemy = TriggerList[i].GetComponent<HealthController>();
                        enemy.ChangeHealth(damage);
                    }
                }
            }

            StartCoroutine(StartTickCooldown());
        }              
    }

    public IEnumerator StartTickCooldown()
    {
        IsAvailable = false;
        yield return new WaitForSeconds(tickRate);
        IsAvailable = true;
    }
}
