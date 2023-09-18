using UnityEngine;

//Eventually will need to extend this to add support for more data collection or bonus effects on certain targets
public class TargetContext
{
    public Transform Transform { get; }
public IMovement MovementController { get; }
    public IDamage DamageController { get; }
    public StatusEffectController StatusController { get; } // should be an interface
    public BuffController BuffController { get; } // should be an interface

    public TargetContext(Transform target)
    {
        MovementController = target.GetComponent<IMovement>();
        BuffController = target.GetComponent<BuffController>();
        StatusController = target.GetComponent<StatusEffectController>();
        DamageController = target.GetComponent <IDamage>();
        Transform = target;
    }
}
