using System.Collections.Generic;
using UnityEngine;

public class DynamicHitBox : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider;

    private Sprite previousSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update()
    {
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        if (previousSprite != spriteRenderer.sprite)
        {
            polygonCollider.pathCount = spriteRenderer.sprite.GetPhysicsShapeCount();

            for (int i = 0; i < polygonCollider.pathCount; i++)
            {
                List<Vector2> path = new List<Vector2>();
                spriteRenderer.sprite.GetPhysicsShape(i, path);
                polygonCollider.SetPath(i, path);
            }

            previousSprite = spriteRenderer.sprite;
        }
    }
}
