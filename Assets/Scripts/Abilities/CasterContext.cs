using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CasterContext
{
    public Transform transform { get; }


    public CasterContext(Transform caster)
    {
        transform = caster;
    }
}