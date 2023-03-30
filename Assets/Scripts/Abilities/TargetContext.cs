using UnityEngine;

//Eventually will need to extend this to add support for more data collection or bonus effects on certain targets
public class TargetContext
{
    public Transform Transform;

    public TargetContext(Transform transform)
    {
        Transform = transform;
    }
}
