using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FloatingCombatTextController : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 0.5f, 0); // Adjust the offset as needed
    private float moveSpeed = 1f;
    public GameObject textPrefab;
    private float lastTime = 0f;
    private float bufferTime = 0.02f;
    private float numOfInstances = 0;


    // Create a new floating combat text object and set the text, color, and position
    public void CreateFloatingCombatText(string text, UnityEngine.Color color, float destroyTime = 0.5f)
    {
        float timeSinceLastText = Time.time - lastTime;
        if (timeSinceLastText > bufferTime)
        {
            StartCoroutine(SpawnText(text, color, 0, destroyTime));
            numOfInstances = 0;
        }
        else 
        {

            StartCoroutine(SpawnText(text, color, (bufferTime - timeSinceLastText) + (bufferTime * numOfInstances), destroyTime));
            numOfInstances++;
        }

        lastTime = Time.time;
    }


    private IEnumerator SpawnText(string text, UnityEngine.Color color , float bufferTime, float destroyTime)
    {
        yield return new WaitForSeconds(bufferTime);
        Vector3 randomisedOffset = new Vector3(Random.Range(-0.25f,0.25f), offset.y + Random.Range(0, 0.5f), 0); 
        var textObj = Instantiate(textPrefab, transform.position + randomisedOffset, Quaternion.identity);
        CombatText combatText = textObj.GetComponent<CombatText>();
        combatText.init(destroyTime, moveSpeed);
        combatText.SetText(text, color);
    }
}
