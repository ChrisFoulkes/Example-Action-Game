using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class CombatText : MonoBehaviour
{
    public float destroyTime;
    public float moveSpeed;
    public TextMeshPro textMesh;
  

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
    }

    public void init(float duration, float speed) 
    {
        destroyTime = duration;
        moveSpeed = speed;
    }

    public void SetText(string text, Color color)
    {
        textMesh.SetText(text);
        textMesh.color = color;        // Start fading out text when it has reached 80% of its lifespan
        float fadeOutTime = destroyTime * 0.4f;
        Invoke("FadeOut", destroyTime - fadeOutTime);
    }

    private void Update()
    {
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float duration = destroyTime * 0.5f;
        float startTime = Time.time;
        Color startColor = textMesh.color;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            textMesh.color = Color.Lerp(startColor, new Color(startColor.r, startColor.g, startColor.b, 0), t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
