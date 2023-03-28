using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : MonoBehaviour
{
    Animator _animator;
    // Start is called before the first frame update
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(int Count, bool bonusEffect)
    {
        if (bonusEffect) 
        {
            _animator.SetBool("BlueFlame", true);
        }
    }

    public void DestoryEffect()
    {
       GameObject.Destroy(gameObject);
    }
}
