using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UI.Image;


[System.Serializable]
public class FloatingCombatTextColour
{
    public Color color;
    public bool useGradient = false;
    public Color bottomColor;
    public Color topColor;
}

[System.Serializable]
public enum FloatingColourType
{
    Heal,
    Generic,
    Ignite,
    levelUp
}



// TO DO add a callback message to confirm all combat texts are complete to so deleting the transform is allowed for enemys dying whilst combat text active 


[System.Serializable] public class DamageColourTypes : UnitySerializedDictionary<FloatingColourType, FloatingCombatTextColour> { }
public class FloatingCombatTextController : MonoBehaviour
{
    public static FloatingCombatTextController Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0.5f, 0); // Adjust the offset as needed
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float bufferTime = 0.02f;

    [Header("References")]
    [SerializeField] private GameObject textPrefab;
    [SerializeField] public DamageColourTypes damageColourTypes;

    private Dictionary<Transform, float> lastTimeByOrigin = new Dictionary<Transform, float>();
    private int numOfInstances = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("MULTIPLE FLOATING COMBAT TEXT CONTROLLERS!!!");
            Destroy(gameObject);
        }
    }

    public void CreateFloatingCombatText(string text, Transform origin, FloatingColourType colourType, bool shouldAttachToSelf = false, float destroyTime = 0.9f, float scaleValue = 0.3f)
    {
        if (!lastTimeByOrigin.ContainsKey(origin))
        {
            lastTimeByOrigin[origin] = Time.time;
            StartCoroutine(SpawnText(text, origin, GetDamageTypeColour(colourType), 0, destroyTime, shouldAttachToSelf, 0, scaleValue));
            numOfInstances = 0;
        }
        else
        {
            numOfInstances++;
            lastTimeByOrigin[origin] = Time.time + (bufferTime * numOfInstances);
            StartCoroutine(SpawnText(text, origin, GetDamageTypeColour(colourType),(bufferTime * numOfInstances), destroyTime, shouldAttachToSelf, numOfInstances, scaleValue));
         
        }

        StartCoroutine(RemoveOriginAfterBuffer(origin, bufferTime));
    }

    private FloatingCombatTextColour GetDamageTypeColour(FloatingColourType colourType)
    {

        if (damageColourTypes.TryGetValue(colourType, out FloatingCombatTextColour damageColor))
        {
            return damageColor;
        }
        else
        {
            return damageColourTypes[FloatingColourType.Generic];
        }
    }

    private IEnumerator SpawnText(string text, Transform origin, FloatingCombatTextColour colourType, float bufferTime, float destroyTime, bool shouldAttachToSelf, float instance = 0, float scaleValue = 0.3f)
    {
        yield return new WaitForSeconds(bufferTime);

        if (origin == null)
        {
            Debug.LogWarning("Created text on a dead object");
        }
        else
        {
            Vector3 randomizedOffset = new Vector3(
                Random.Range(-0.25f, 0.25f),
                offset.y + Random.Range(0, 0.5f),
                0
            );

            Vector3 spawnPosition = origin.position + randomizedOffset;

            Transform parentTransform = shouldAttachToSelf ? origin : null;
     

            Quaternion spawnRotation = Quaternion.identity;

            GameObject textObj = Instantiate(textPrefab, spawnPosition, spawnRotation, parentTransform);
            CombatText combatText = textObj.GetComponent<CombatText>();

            textObj.GetComponent<TextMeshPro>().sortingOrder = Mathf.RoundToInt(10f + (10f * scaleValue));

            combatText.Init(destroyTime, moveSpeed, scaleValue);
            combatText.SetText(text, colourType);

        }
    }

    private IEnumerator RemoveOriginAfterBuffer(Transform origin, float bufferTime)
    {
        yield return new WaitForSeconds(bufferTime);

        if (lastTimeByOrigin.ContainsKey(origin) && Time.time - lastTimeByOrigin[origin] >= bufferTime)
        {
            lastTimeByOrigin.Remove(origin);
        }
    }
}
