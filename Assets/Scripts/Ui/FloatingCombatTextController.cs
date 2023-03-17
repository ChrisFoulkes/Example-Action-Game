using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class FloatingCombatTextController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0); // Adjust the offset as needed
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float bufferTime = 0.02f;

    [Header("References")]
    [SerializeField] private GameObject textPrefab;

    private float lastTime = 0f;
    private int numOfInstances = 0;

    // Create a new floating combat text object and set the text, color, and position
    public void CreateFloatingCombatText(string text, UnityEngine.Color color, bool shouldAttachToSelf = false, float destroyTime = 0.5f)
    {
        float timeSinceLastText = Time.time - lastTime;
        if (timeSinceLastText > bufferTime)
        {
            StartCoroutine(SpawnText(text, color, 0, destroyTime, shouldAttachToSelf));
            numOfInstances = 0;
        }
        else
        {
            StartCoroutine(SpawnText(text, color, (bufferTime - timeSinceLastText) + (bufferTime * numOfInstances), destroyTime, shouldAttachToSelf));
            numOfInstances++;
        }

        lastTime = Time.time;
    }

    private IEnumerator SpawnText(string text, UnityEngine.Color color, float bufferTime, float destroyTime, bool shouldAttachToSelf)
    {
        yield return new WaitForSeconds(bufferTime);

        Vector3 randomizedOffset = new Vector3(
            Random.Range(-0.25f, 0.25f),
            offset.y + Random.Range(0, 0.5f),
            0
        );

        Vector3 spawnPosition = transform.position + randomizedOffset;
        Quaternion spawnRotation = Quaternion.identity;
        Transform parentTransform = shouldAttachToSelf ? transform : null;

        GameObject textObj = Instantiate(textPrefab, spawnPosition, spawnRotation, parentTransform);
        CombatText combatText = textObj.GetComponent<CombatText>();
        combatText.Init(destroyTime, moveSpeed);
        combatText.SetText(text, color);
    }
}
