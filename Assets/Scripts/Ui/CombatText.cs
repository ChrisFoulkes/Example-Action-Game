using System.Collections;
using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    private float duration;
    private float destroyTime;
    private float moveSpeed;
    private float scalingFactor;

    private TextMeshPro textMesh;
    private Vector3 initialScale;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        initialScale = transform.localScale;
    }

    public void Init(float duration, float speed, float scalingFactor)
    {
        this.duration = duration;
        destroyTime = duration;
        moveSpeed = speed;
        this.scalingFactor = Mathf.Clamp(scalingFactor, 0f, 1f);
    }

    public void SetText(string text, FloatingCombatTextColour colourType)
    {
        if (colourType.useGradient)
        {
            textMesh.color = Color.white;
            textMesh.colorGradient = new VertexGradient(colourType.topColor, colourType.topColor, colourType.bottomColor, colourType.bottomColor);
        }
        else
        {
            textMesh.color = colourType.color;
        }

        textMesh.text = text;
    }

    private void Update()
    {
        MoveUpwards();
        UpdateDestroyTime();
        ScaleText();
    }

    private void MoveUpwards()
    {
        if (transform != null)
        {
            transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;
        }
    }

    private void UpdateDestroyTime()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0)
        {
            Destroy(gameObject);
        }
        else if (destroyTime <= duration * 0.5f)
        {
            FadeOut();
        }
    }

    private void ScaleText()
    {
        float t = 1 - (destroyTime / duration);
        float scaleFactor = 1.0f;

        if (t <= 1f / 3f) // Scaling up
        {
            scaleFactor = 1 + scalingFactor * Mathf.Sin(t * Mathf.PI * 1.5f);
        }
        else if (t >= 2.5f / 3f) // Scaling down
        {
            float tScaled = (t - 2f / 3f) * 3f;
            scaleFactor = 1 + scalingFactor * Mathf.Sin((1 - tScaled) * Mathf.PI * 1.5f);
        }
        else // Maximum scale
        {
            scaleFactor = 1 + scalingFactor;
        }

        transform.localScale = initialScale * scaleFactor;
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
    }
}