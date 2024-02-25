using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEverywhere : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
