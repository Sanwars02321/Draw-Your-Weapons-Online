using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSingleton<T> : MonoBehaviour where T : class
{
    public static T Instance { get; set; }

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;

        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
    }
}
