using UnityEngine;

public class EnemyContext
{
    public Transform Transform { get; }

    public EnemyContext(Transform transform)
    {
        this.Transform = transform;
    }
}
