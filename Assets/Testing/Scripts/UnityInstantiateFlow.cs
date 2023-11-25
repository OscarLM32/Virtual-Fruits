using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityInstantiateFlow : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Calling AWAKE");
    }

    private void OnEnable()
    {
        Debug.Log("Calling ON_ENABLE");
    }

    void Start()
    {
        Debug.Log("Calling START");
    }
}
